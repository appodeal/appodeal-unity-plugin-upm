using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal class WizardScreenController : IDisposable
    {
        private readonly WizardScreenView _view = new();
        private readonly SdkSelectionModeManager _selectionManager = new();

        private readonly List<GenericSdkCardController> _mediationControllers = new();
        private readonly List<AdNetworkSdkCardController> _adNetworkControllers = new();
        private readonly List<GenericSdkCardController> _serviceControllers = new();

        private readonly CancellationToken _cancellationToken;

        private string _pluginVersion;

        private WizardStage _currentStage = WizardStage.Mediations;
        private bool _isDirty = true;

        private bool _disposed;

        public VisualElement Root => _view.Root;

        public WizardScreenController(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            UnsubscribeFromViewEvents();
            UnsubscribeFromSelectionManagerEvents();

            ClearMediations();
            ClearComponents();

            _view?.Dispose();
        }

        public async Task<bool> TryInitializeAsync()
        {
            var packageVersionLookupOutcome = await PackageVersionProvider.TryLookupVersionAsync(_cancellationToken);
            if (_cancellationToken.IsCancellationRequested) return false;

            if (!packageVersionLookupOutcome.IsSuccess)
            {
                LogHelper.LogError($"Failed to get plugin version: {packageVersionLookupOutcome.Failure.Message}");
                return false;
            }

            var availabilityOutcome = await VersionComparer.TryCheckVersionAvailabilityAsync(packageVersionLookupOutcome.Value, _cancellationToken);
            if (_cancellationToken.IsCancellationRequested) return false;

            if (!availabilityOutcome.IsSuccess)
            {
                LogHelper.LogError($"Failed to check version availability: {availabilityOutcome.Failure.Message}");
                return false;
            }
            if (!availabilityOutcome.Value)
            {
                LogHelper.LogError($"Plugin version '{packageVersionLookupOutcome.Value}' is not available");
                return false;
            }

            _pluginVersion = packageVersionLookupOutcome.Value;

            if (!_view.TryLoadFromTemplate()) return false;

            _view.SetSelectionMode(_selectionManager.CurrentMode);

            SubscribeToViewEvents();
            SubscribeToSelectionManagerEvents();

            SetStage(WizardStage.Mediations);

            return await TryLoadMediationsAsync();
        }

        private async Task<bool> TryLoadMediationsAsync()
        {
            var loadOutcome = await MediationsLoader.TryLoadAsync(_pluginVersion, _cancellationToken);
            if (_cancellationToken.IsCancellationRequested) return false;

            if (!loadOutcome.IsSuccess) return false;

            foreach (var controller in loadOutcome.Value)
            {
                SubscribeToCardEvents(controller);
                _view.MediationsContainer.Add(controller.Root);
                _mediationControllers.Add(controller);
            }

            _selectionManager.ApplyModeToControllers(_mediationControllers);
            return true;
        }

        private async Task<bool> TryLoadComponentsAsync()
        {
            var request = WizardRequestBuilder.BuildComponentsRequest(_mediationControllers);
            var loadOutcome = await ComponentsLoader.TryLoadAsync(request, _pluginVersion, _cancellationToken);
            if (_cancellationToken.IsCancellationRequested) return false;

            if (!loadOutcome.IsSuccess) return false;

            var (adNetworks, services) = loadOutcome.Value;

            foreach (var controller in adNetworks)
            {
                SubscribeToCardEvents(controller);
                _view.AdNetworksContainer.Add(controller.Root);
                _adNetworkControllers.Add(controller);
            }

            foreach (var controller in services)
            {
                SubscribeToCardEvents(controller);
                _view.ServicesContainer.Add(controller.Root);
                _serviceControllers.Add(controller);
            }

            _selectionManager.ApplyModeToControllers(_adNetworkControllers);
            _selectionManager.ApplyModeToControllers(_serviceControllers);

            return true;
        }

        private void ClearMediations()
        {
            foreach (var controller in _mediationControllers)
            {
                UnsubscribeFromCardEvents(controller);
                controller.Dispose();
            }
            _mediationControllers.Clear();

            _view?.MediationsContainer?.Clear();
        }

        private void ClearComponents()
        {
            foreach (var controller in _adNetworkControllers)
            {
                UnsubscribeFromCardEvents(controller);
                controller.Dispose();
            }
            _adNetworkControllers.Clear();

            foreach (var controller in _serviceControllers)
            {
                UnsubscribeFromCardEvents(controller);
                controller.Dispose();
            }
            _serviceControllers.Clear();

            _view?.ClearComponentsContainers();
        }

        private void SetStage(WizardStage stage)
        {
            _currentStage = stage;
            _view.SetStage(stage);
            UpdateButtonStates();
        }

        private void SetDirty(bool dirty)
        {
            _isDirty = dirty;
            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            _view.SetConfirmMediationsButtonEnabled(_currentStage == WizardStage.Mediations);
            _view.SetGenerateButtonEnabled(_currentStage == WizardStage.Components && _isDirty);
        }

        private async Task<bool> TryGenerateDependenciesAsync()
        {
            _view.HideStatusPopups();

            var request = WizardRequestBuilder.BuildDependenciesRequest(_mediationControllers, _adNetworkControllers, _serviceControllers);
            var fetchOutcome = await DataLoader.FetchDependenciesAsync(request, _pluginVersion, _cancellationToken);
            if (_cancellationToken.IsCancellationRequested) return false;

            if (!fetchOutcome.IsSuccess)
            {
                LogHelper.LogError($"Failed to generate dependencies: {fetchOutcome.Failure.Message}");
                _view.ShowFailPopup();
                return false;
            }

            var deserializationOutcome = XmlHandler.TryDeserializeDependencies(fetchOutcome.Value);
            if (!deserializationOutcome.IsSuccess)
            {
                LogHelper.LogError($"Failed to parse dependencies XML: {deserializationOutcome.Failure.Message}");
                _view.ShowFailPopup();
                return false;
            }

            var depsReadOutcome = DataLoader.TryReadLocalDependencies();
            var localDeps = depsReadOutcome.IsSuccess ? depsReadOutcome.Value : null;

            if (!XmlHandler.TryUpdateDependencies(deserializationOutcome.Value))
            {
                LogHelper.LogError("Failed to save dependencies to file");
                _view.ShowFailPopup();
                return false;
            }

            var diff = DependenciesDiff.Get(localDeps, deserializationOutcome.Value);
            string message = $"{LogHelper.Colorize(Color.green, "Successfully")} updated dependencies.";
            LogHelper.LogDepsUpdate(message, diff);

            SaveCustomSelections();

            _view.ShowSuccessPopup();
            return true;
        }

        private void SaveCustomSelections()
        {
            if (_selectionManager.CurrentMode != SdkSelectionMode.Custom) return;

            var choices = DmChoicesScriptableObject.Instance;
            choices.CustomSelections = WizardRequestBuilder.BuildSelectionStates(_mediationControllers, _adNetworkControllers, _serviceControllers);
            DmChoicesScriptableObject.SaveToDisk();
        }

        private void SubscribeToViewEvents()
        {
            _view.ConfirmMediationsClicked += OnConfirmMediationsClickedHandler;
            _view.GenerateClicked += OnGenerateClickedHandler;

            _view.SelectDefaultButton.clicked += OnSelectDefaultClicked;
            _view.SelectCustomButton.clicked += OnSelectCustomClicked;
            _view.SelectAllButton.clicked += OnSelectAllClicked;
            _view.SelectMinimalButton.clicked += OnSelectMinimalClicked;
        }

        private void UnsubscribeFromViewEvents()
        {
            _view.ConfirmMediationsClicked -= OnConfirmMediationsClickedHandler;
            _view.GenerateClicked -= OnGenerateClickedHandler;

            if (_view.SelectDefaultButton != null) _view.SelectDefaultButton.clicked -= OnSelectDefaultClicked;
            if (_view.SelectCustomButton != null) _view.SelectCustomButton.clicked -= OnSelectCustomClicked;
            if (_view.SelectAllButton != null) _view.SelectAllButton.clicked -= OnSelectAllClicked;
            if (_view.SelectMinimalButton != null) _view.SelectMinimalButton.clicked -= OnSelectMinimalClicked;
        }

        private void SubscribeToSelectionManagerEvents()
        {
            _selectionManager.ModeChanged += OnSelectionModeChanged;
        }

        private void UnsubscribeFromSelectionManagerEvents()
        {
            _selectionManager.ModeChanged -= OnSelectionModeChanged;
        }

        private void SubscribeToCardEvents(ISdkCardController controller)
        {
            controller.SelectionChanged += OnCardSelectionChanged;
            controller.AndroidVersionChanged += OnCardVersionChanged;
            controller.IosVersionChanged += OnCardVersionChanged;
        }

        private void UnsubscribeFromCardEvents(ISdkCardController controller)
        {
            controller.SelectionChanged -= OnCardSelectionChanged;
            controller.AndroidVersionChanged -= OnCardVersionChanged;
            controller.IosVersionChanged -= OnCardVersionChanged;
        }

        private async void OnConfirmMediationsClickedHandler(object sender, EventArgs e)
        {
            try
            {
                if (_currentStage == WizardStage.Components) return;

                _view.SetConfirmMediationsButtonEnabled(false);
                if (await TryLoadComponentsAsync()) SetStage(WizardStage.Components);
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex);
            }
            finally
            {
                UpdateButtonStates();
            }
        }

        private async void OnGenerateClickedHandler(object sender, EventArgs e)
        {
            try
            {
                if (_currentStage != WizardStage.Components || !_isDirty) return;

                _view.SetGenerateButtonEnabled(false);
                if (await TryGenerateDependenciesAsync()) SetDirty(false);
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex);
            }
            finally
            {
                UpdateButtonStates();
            }
        }

        private void OnSelectDefaultClicked() => OnSelectionButtonClicked(SdkSelectionMode.Default);
        private void OnSelectCustomClicked() => OnSelectionButtonClicked(SdkSelectionMode.Custom);
        private void OnSelectAllClicked() => OnSelectionButtonClicked(SdkSelectionMode.All);
        private void OnSelectMinimalClicked() => OnSelectionButtonClicked(SdkSelectionMode.Minimal);

        private void OnSelectionButtonClicked(SdkSelectionMode mode) => _selectionManager.SetMode(mode);

        private void OnSelectionModeChanged(object sender, SdkSelectionModeChangedEventArgs e)
        {
            _view.SetSelectionMode(e.NewMode);
            _view.HideStatusPopups();

            bool mediationsChanged = _selectionManager.ApplyModeToControllers(_mediationControllers);
            _ = _selectionManager.ApplyModeToControllers(_adNetworkControllers);
            _ = _selectionManager.ApplyModeToControllers(_serviceControllers);

            if (mediationsChanged && _currentStage == WizardStage.Components)
            {
                ClearComponents();
                SetStage(WizardStage.Mediations);
            }

            SetDirty(true);
        }

        private void OnCardSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ISdkCardController { HasAnyVersions: false }) return;

            _selectionManager.SetMode(SdkSelectionMode.Custom, notify: false);
            HandleCardStateChange(sender);
        }

        private void OnCardVersionChanged(object sender, VersionChangedEventArgs e) => HandleCardStateChange(sender);

        private void HandleCardStateChange(object sender)
        {
            bool isMediationCard = sender is GenericSdkCardController ctrl && _mediationControllers.Contains(ctrl);

            if (isMediationCard && _currentStage == WizardStage.Components)
            {
                ClearComponents();
                SetStage(WizardStage.Mediations);
            }

            _view.HideStatusPopups();
            SetDirty(true);
        }
    }
}
