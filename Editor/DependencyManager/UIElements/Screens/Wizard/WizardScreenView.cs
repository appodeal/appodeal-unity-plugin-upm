using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal class WizardScreenView : IDisposable
    {
        private const string TemplatePath = "Appodeal/DependencyManager/Screens/WizardScreen";
        private const int PopupAutoHideDelayMs = 5000;

        private VisualElement _wizardContainer;

        private Button _confirmMediationsButton;
        private Button _generateButton;

        private IVisualElementScheduledItem _popupHideSchedule;
        private VisualElement _successPopup;
        private VisualElement _failPopup;

        public event EventHandler ConfirmMediationsClicked;
        public event EventHandler GenerateClicked;

        public VisualElement Root { get; private set; }

        public VisualElement MediationsContainer { get; private set; }

        public VisualElement AdNetworksContainer { get; private set; }

        public VisualElement ServicesContainer { get; private set; }

        public Button SelectAllButton { get; private set; }
        public Button SelectMinimalButton { get; private set; }
        public Button SelectDefaultButton { get; private set; }
        public Button SelectCustomButton { get; private set; }

        public void Dispose()
        {
            UnsubscribeFromEvents();
            CancelPopupHideSchedule();
        }

        public bool TryLoadFromTemplate()
        {
            var template = Resources.Load<VisualTreeAsset>(TemplatePath);
            if (template == null)
            {
                LogHelper.LogError($"[WizardScreenView] Failed to load template from Resources/{TemplatePath}");
                return false;
            }

            Root = template.Instantiate();

            if (!BindElements())
            {
                LogHelper.LogError("[WizardScreenView] Failed to bind required elements");
                return false;
            }

            SubscribeToEvents();
            return true;
        }

        private bool BindElements()
        {
            _wizardContainer = Root.Q<VisualElement>(DmConstants.Uxml.WizardScreen.ContentContainer);
            if (_wizardContainer == null) return false;

            MediationsContainer = _wizardContainer.Q<VisualElement>(DmConstants.Uxml.WizardScreen.MediationsContainer);
            AdNetworksContainer = _wizardContainer.Q<VisualElement>(DmConstants.Uxml.WizardScreen.AdNetworksContainer);
            ServicesContainer = _wizardContainer.Q<VisualElement>(DmConstants.Uxml.WizardScreen.ServicesContainer);
            SelectAllButton = _wizardContainer.Q<Button>(DmConstants.Uxml.WizardScreen.SelectAllButton);
            SelectMinimalButton = _wizardContainer.Q<Button>(DmConstants.Uxml.WizardScreen.SelectMinimalButton);
            SelectDefaultButton = _wizardContainer.Q<Button>(DmConstants.Uxml.WizardScreen.SelectDefaultButton);
            SelectCustomButton = _wizardContainer.Q<Button>(DmConstants.Uxml.WizardScreen.SelectCustomButton);
            _confirmMediationsButton = _wizardContainer.Q<Button>(DmConstants.Uxml.WizardScreen.ConfirmMediationsButton);
            _generateButton = _wizardContainer.Q<Button>(DmConstants.Uxml.WizardScreen.GenerateButton);
            _successPopup = _wizardContainer.Q<VisualElement>(DmConstants.Uxml.WizardScreen.SuccessStatusPopup);
            _failPopup = _wizardContainer.Q<VisualElement>(DmConstants.Uxml.WizardScreen.FailStatusPopup);

            return MediationsContainer != null && AdNetworksContainer != null && ServicesContainer != null &&
                   SelectAllButton != null && SelectMinimalButton != null && SelectDefaultButton != null && SelectCustomButton != null &&
                   _confirmMediationsButton != null && _generateButton != null;
        }

        public void SetStage(WizardStage stage)
        {
            _wizardContainer?.EnableInClassList(DmConstants.Uss.WizardScreen.MediationsStageModifier, stage == WizardStage.Mediations);
        }

        public void SetSelectionMode(SdkSelectionMode mode)
        {
            SelectDefaultButton?.EnableInClassList(DmConstants.Uss.WizardScreen.SelectionButtonSelected, mode == SdkSelectionMode.Default);
            SelectCustomButton?.EnableInClassList(DmConstants.Uss.WizardScreen.SelectionButtonSelected, mode == SdkSelectionMode.Custom);
            SelectAllButton?.EnableInClassList(DmConstants.Uss.WizardScreen.SelectionButtonSelected, mode == SdkSelectionMode.All);
            SelectMinimalButton?.EnableInClassList(DmConstants.Uss.WizardScreen.SelectionButtonSelected, mode == SdkSelectionMode.Minimal);
        }

        public void ClearComponentsContainers()
        {
            AdNetworksContainer?.Clear();
            ServicesContainer?.Clear();
        }

        public void SetConfirmMediationsButtonEnabled(bool enabled) => _confirmMediationsButton?.SetEnabled(enabled);

        public void SetGenerateButtonEnabled(bool enabled) => _generateButton?.SetEnabled(enabled);

        public void ShowSuccessPopup()
        {
            HideStatusPopups();
            if (_successPopup != null) _successPopup.style.display = DisplayStyle.Flex;
            SchedulePopupHide();
        }

        public void ShowFailPopup()
        {
            HideStatusPopups();
            if (_failPopup != null) _failPopup.style.display = DisplayStyle.Flex;
            SchedulePopupHide();
        }

        public void HideStatusPopups()
        {
            CancelPopupHideSchedule();
            if (_successPopup != null) _successPopup.style.display = DisplayStyle.None;
            if (_failPopup != null) _failPopup.style.display = DisplayStyle.None;
        }

        private void SchedulePopupHide()
        {
            CancelPopupHideSchedule();
            _popupHideSchedule = Root?.schedule.Execute(HideStatusPopups).StartingIn(PopupAutoHideDelayMs);
        }

        private void CancelPopupHideSchedule()
        {
            _popupHideSchedule?.Pause();
            _popupHideSchedule = null;
        }

        private void SubscribeToEvents()
        {
            _confirmMediationsButton.clicked += OnConfirmMediationsClicked;
            _generateButton.clicked += OnGenerateClicked;
        }

        private void UnsubscribeFromEvents()
        {
            if (_confirmMediationsButton != null) _confirmMediationsButton.clicked -= OnConfirmMediationsClicked;
            if (_generateButton != null) _generateButton.clicked -= OnGenerateClicked;
        }

        private void OnConfirmMediationsClicked()
        {
            ConfirmMediationsClicked?.Invoke(this, EventArgs.Empty);
        }

        private void OnGenerateClicked()
        {
            GenerateClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
