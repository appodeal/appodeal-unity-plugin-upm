using System;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine.UIElements;
using AppodealInc.Mediation.Analytics.Editor;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal class DmSettingsProvider : SettingsProvider
    {
        private CancellationTokenSource _cts;

        private WizardScreenController _wizardController;
        private LoadingScreenView _loadingView;
        private ErrorScreenController _errorController;

        private VisualElement _rootElement;

        private DmSettingsProvider(string path, SettingsScope scope = SettingsScope.User) : base(path, scope) { }

        public override async void OnActivate(string searchContext, VisualElement rootElement)
        {
            try
            {
                AnalyticsService.TrackClickEvent(ActionType.OpenDependencyManager);

                if (DmChoicesScriptableObject.Instance == null)
                {
                    LogHelper.LogError("Dependency Manager window cannot be displayed as its data asset failed to load");
                    return;
                }

                _rootElement = rootElement;
                _rootElement?.Clear();

                _cts?.Cancel();
                _cts?.Dispose();
                _cts = new CancellationTokenSource();
                var cancellationToken = _cts.Token;

                _loadingView = new LoadingScreenView();
                if (!_loadingView.TryLoadFromTemplate())
                {
                    LogHelper.LogError("Dependency Manager window cannot be displayed: Loading screen failed to load");
                    return;
                }
                _rootElement?.Add(_loadingView?.Root);

                _wizardController = new WizardScreenController(cancellationToken);
                bool success = await _wizardController.TryInitializeAsync();

                if (cancellationToken.IsCancellationRequested)
                {
                    _wizardController?.Dispose();
                    _wizardController = null;
                    _loadingView?.Dispose();
                    _loadingView = null;
                    _rootElement?.Clear();
                    return;
                }

                if (!success)
                {
                    LogHelper.LogError("Dependency Manager window cannot be displayed: failed to create UI");
                    _wizardController?.Dispose();
                    _wizardController = null;

                    _errorController = new ErrorScreenController();
                    if (_errorController.TryInitialize())
                    {
                        _rootElement?.Add(_errorController?.Root);
                    }
                    return;
                }

                _rootElement?.Add(_wizardController?.Root);

                WizardScrollViewHelper.Initialize(_wizardController?.Root);
                DropdownOverlay.Initialize(_rootElement);
                SdkTooltipOverlay.Initialize(_rootElement);
            }
            catch (Exception ex)
            {
                LogHelper.LogError($"Dependency Manager window initialization failed with exception: {ex.Message}");
                LogHelper.LogException(ex);

                if (!_cts?.Token.IsCancellationRequested ?? false)
                {
                    _wizardController?.Dispose();
                    _wizardController = null;

                    _errorController = new ErrorScreenController();
                    if (_errorController.TryInitialize())
                    {
                        _rootElement?.Add(_errorController?.Root);
                    }
                }
            }
            finally
            {
                _rootElement?.Remove(_loadingView?.Root);
                _loadingView?.Dispose();
                _loadingView = null;
            }
        }

        public override void OnDeactivate()
        {
            AnalyticsService.TrackClickEvent(ActionType.CloseDependencyManager);

            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;

            _wizardController?.Dispose();
            _wizardController = null;

            _errorController?.Dispose();
            _errorController = null;

            _loadingView?.Dispose();
            _loadingView = null;

            _rootElement?.Clear();
            _rootElement = null;

            SdkTooltipOverlay.Cleanup();
            DropdownOverlay.Cleanup();
            WizardScrollViewHelper.Cleanup();
        }

        [SettingsProvider]
        public static SettingsProvider CreateDmSettingsProvider()
        {
            var provider = new DmSettingsProvider($"Project/{DmConstants.UI.WindowName}", SettingsScope.Project)
            {
                label = "Appodeal DM",
                keywords = new HashSet<string>(new[] { "Appodeal", "Dependency", "Manager", "DM" })
            };
            return provider;
        }
    }
}
