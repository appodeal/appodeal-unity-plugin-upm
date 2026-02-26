using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
#if UNITY_6000_0_OR_NEWER
    [UxmlElement]
#else
    // ReSharper disable once PartialTypeWithSinglePart
#endif
    internal partial class SdkVersionDropdown : VisualElement, IDisposable
    {
#if !UNITY_6000_0_OR_NEWER
        public new class UxmlFactory : UxmlFactory<SdkVersionDropdown> { }
#endif

        private readonly DropdownButtonController _buttonController;
        private readonly DropdownPopupController _popupController;

        private List<VersionInfo> _versions = new();
        private VersionInfo _selectedVersion;

        public event EventHandler<VersionChangedEventArgs> VersionChanged;

        public VersionInfo SelectedVersion
        {
            get => _selectedVersion;
            set => SetSelectedVersion(value);
        }

        public bool IsButtonEnabled
        {
            get => _buttonController.IsEnabled;
            set => _buttonController.IsEnabled = value;
        }

        private string Value => _selectedVersion?.Name ?? "none";

        public SdkVersionDropdown()
        {
            _buttonController = new DropdownButtonController();
            _popupController = new DropdownPopupController();

            if (!_buttonController.TryInitialize())
            {
                LogHelper.LogError("[SdkVersionDropdown] Failed to initialize button controller");
                return;
            }

            if (!_popupController.TryInitialize())
            {
                LogHelper.LogError("[SdkVersionDropdown] Failed to initialize popup controller");
                return;
            }

            Add(_buttonController.Root);

            SubscribeToEvents();
        }

        public void Dispose()
        {
            UnsubscribeFromEvents();
            ClosePopup();
        }

        public void SetVersions(List<VersionInfo> versions)
        {
            _versions = versions ?? new List<VersionInfo>();
            SelectRecommended();
            SyncButtonLabel();
        }

        public void SetWarningStyle(bool enabled)
        {
            _buttonController?.SetWarningStyle(enabled);
        }

        public void SetLocked(bool locked)
        {
            IsButtonEnabled = !locked;
            _buttonController?.SetLockedStyle(locked);
        }

        public void SetTooltip(string message)
        {
            _buttonController?.SetTooltip(message);
        }

        private void ShowDropdownMenu()
        {
            _popupController.PopulateItems(_versions, SelectedVersion);

            if (!DropdownOverlay.Show(_popupController, _buttonController.Root))
            {
                return;
            }

            _buttonController.IsExpanded = true;
        }

        private void ClosePopup()
        {
            if (!_buttonController.IsExpanded) return;

            DropdownOverlay.Hide();
            _buttonController.IsExpanded = false;
        }

        private void SelectRecommended()
        {
            _selectedVersion = _versions.FirstOrDefault(v => v?.IsRecommended == true) ?? _versions.FirstOrDefault(v => v != null);
        }

        private void SetSelectedVersion(VersionInfo version)
        {
            if (_selectedVersion == version) return;

            _selectedVersion = version;
            SyncButtonLabel();
            VersionChanged?.Invoke(this, new VersionChangedEventArgs(_selectedVersion));
        }

        private void SyncButtonLabel()
        {
            _buttonController.SetLabelText(Value);
        }

        private void SubscribeToEvents()
        {
            _buttonController.Clicked += OnButtonClicked;
            _popupController.ItemSelected += OnItemSelected;

            RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
        }

        private void UnsubscribeFromEvents()
        {
            UnregisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            UnregisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);

            if (_buttonController != null)
            {
                _buttonController.Clicked -= OnButtonClicked;
            }

            if (_popupController != null)
            {
                _popupController.ItemSelected -= OnItemSelected;
            }

            DropdownOverlay.ClickedOutside -= OnClickedOutside;
        }

        private void OnAttachToPanel(AttachToPanelEvent evt)
        {
            DropdownOverlay.ClickedOutside += OnClickedOutside;
        }

        private void OnDetachFromPanel(DetachFromPanelEvent evt)
        {
            DropdownOverlay.ClickedOutside -= OnClickedOutside;
        }

        private void OnButtonClicked(object sender, EventArgs e)
        {
            if (!IsButtonEnabled) return;

            ShowDropdownMenu();
        }

        private void OnItemSelected(object sender, DropdownItemSelectedEventArgs args)
        {
            SetSelectedVersion(args.Model);
            ClosePopup();
        }

        private void OnClickedOutside(object sender, DropdownOverlayEventArgs args)
        {
            if (args.Controller != _popupController) return;

            ClosePopup();
        }
    }
}
