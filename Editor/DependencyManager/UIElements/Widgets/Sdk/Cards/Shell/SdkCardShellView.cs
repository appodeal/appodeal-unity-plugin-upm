using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal class SdkCardShellView : IDisposable
    {
        private const string TemplatePath = "Appodeal/DependencyManager/SdkCards/SdkCardShell";

        private Toggle _isSelectedToggle;
        private Label _sdkNameLabel;
        private VisualElement _infoIcon;
        private VisualElement _deselectedOverlay;

        public event EventHandler<SelectionChangedEventArgs> SelectionChanged;
        public event EventHandler InfoIconHoverEnter;
        public event EventHandler InfoIconHoverLeave;
        public event EventHandler<VersionChangedEventArgs> AndroidVersionChanged;
        public event EventHandler<VersionChangedEventArgs> IosVersionChanged;

        public VisualElement Root { get; private set; }

        public VisualElement Body { get; private set; }

        public SdkVersionDropdown AndroidDropdown { get; private set; }

        public SdkVersionDropdown IosDropdown { get; private set; }

        public bool IsSelected => _isSelectedToggle?.value ?? false;

        public string SdkName
        {
            get => _sdkNameLabel?.text;
            set { if (_sdkNameLabel != null) _sdkNameLabel.text = value; }
        }

        public void Dispose() => UnsubscribeFromEvents();

        public bool TryLoadFromTemplate()
        {
            var template = Resources.Load<VisualTreeAsset>(TemplatePath);
            if (template == null)
            {
                LogHelper.LogError($"[SdkCardShellView] Failed to load template from Resources/{TemplatePath}");
                return false;
            }

            Root = template.Instantiate();

            _isSelectedToggle = Root.Q<Toggle>(DmConstants.Uxml.SdkCards.Shell.IsCardSelected);
            _sdkNameLabel = Root.Q<Label>(DmConstants.Uxml.SdkCards.Shell.SdkName);
            _infoIcon = Root.Q<VisualElement>(DmConstants.Uxml.SdkCards.Shell.InfoIcon);
            Body = Root.Q<VisualElement>(DmConstants.Uxml.SdkCards.Shell.Body);
            AndroidDropdown = Root.Q<SdkVersionDropdown>(DmConstants.Uxml.SdkCards.Shell.AndroidVersionDropdown);
            IosDropdown = Root.Q<SdkVersionDropdown>(DmConstants.Uxml.SdkCards.Shell.IosVersionDropdown);
            _deselectedOverlay = Root.Q<VisualElement>(DmConstants.Uxml.SdkCards.Shell.DeselectedOverlay);

            if (_isSelectedToggle == null || _sdkNameLabel == null || Body == null || AndroidDropdown == null || IosDropdown == null)
            {
                LogHelper.LogError("[SdkCardShellView] Failed to find required elements in template");
                return false;
            }

            SubscribeToEvents();
            return true;
        }

        public void SetSelectionEnabled(bool enabled)
        {
            _isSelectedToggle?.SetEnabled(enabled);
        }

        public void SetDropdownsLocked(bool locked)
        {
            AndroidDropdown?.SetLocked(locked);
            IosDropdown?.SetLocked(locked);
        }

        public void SetSelectedWithoutNotify(bool value)
        {
            _isSelectedToggle?.SetValueWithoutNotify(value);
        }

        public void SetDeselectedOverlayVisible(bool visible)
        {
            _deselectedOverlay?.EnableInClassList(DmConstants.Uss.SdkCards.Shell.DeselectedOverlayVisibleModifier, visible);
        }

        private void SubscribeToEvents()
        {
            _isSelectedToggle.RegisterCallback<ChangeEvent<bool>>(OnSelectionChanged);
            _infoIcon?.RegisterCallback<PointerEnterEvent>(OnInfoIconHoverEnter);
            _infoIcon?.RegisterCallback<PointerOutEvent>(OnInfoIconHoverLeave);

            AndroidDropdown.VersionChanged += OnAndroidVersionChanged;
            IosDropdown.VersionChanged += OnIosVersionChanged;
        }

        private void UnsubscribeFromEvents()
        {
            _isSelectedToggle?.UnregisterCallback<ChangeEvent<bool>>(OnSelectionChanged);
            _infoIcon?.UnregisterCallback<PointerEnterEvent>(OnInfoIconHoverEnter);
            _infoIcon?.UnregisterCallback<PointerOutEvent>(OnInfoIconHoverLeave);

            if (AndroidDropdown != null)
            {
                AndroidDropdown.VersionChanged -= OnAndroidVersionChanged;
                AndroidDropdown.Dispose();
            }

            if (IosDropdown != null)
            {
                IosDropdown.VersionChanged -= OnIosVersionChanged;
                IosDropdown.Dispose();
            }
        }

        private void OnSelectionChanged(ChangeEvent<bool> evt) => SelectionChanged?.Invoke(this, new SelectionChangedEventArgs(evt.newValue));
        private void OnInfoIconHoverEnter(PointerEnterEvent evt) => InfoIconHoverEnter?.Invoke(this, EventArgs.Empty);
        private void OnInfoIconHoverLeave(PointerOutEvent evt) => InfoIconHoverLeave?.Invoke(this, EventArgs.Empty);
        private void OnAndroidVersionChanged(object sender, VersionChangedEventArgs e) => AndroidVersionChanged?.Invoke(this, e);
        private void OnIosVersionChanged(object sender, VersionChangedEventArgs e) => IosVersionChanged?.Invoke(this, e);
    }
}
