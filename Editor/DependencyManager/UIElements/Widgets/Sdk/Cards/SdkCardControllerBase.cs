using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal abstract class SdkCardControllerBase<TView> : ISdkCardController where TView : ISdkCardView
    {
        private const string ImmutableSdkId = "appodeal";

        private bool _disposed;

        protected readonly TView View;
        private SdkCardShell Shell => View?.Shell;

        public Sdk AndroidModel { get; }
        public Sdk IosModel { get; }

        public VersionInfo SelectedAndroidVersion => Shell?.View.AndroidDropdown?.SelectedVersion;
        public VersionInfo SelectedIosVersion => Shell?.View.IosDropdown?.SelectedVersion;

        public VisualElement Root => View.Root;

        public string SdkId => AndroidModel?.Name ?? IosModel?.Name;

        public string SdkName
        {
            get => Shell?.View.SdkName;
            set { if (Shell?.View != null) Shell.View.SdkName = value; }
        }

        public bool IsSelected => Shell?.View.IsSelected ?? false;
        public bool HasAnyVersions => (AndroidModel?.HasVersions ?? false) || (IosModel?.HasVersions ?? false);

        public event EventHandler<SelectionChangedEventArgs> SelectionChanged;
        public event EventHandler<VersionChangedEventArgs> AndroidVersionChanged;
        public event EventHandler<VersionChangedEventArgs> IosVersionChanged;

        protected SdkCardControllerBase(TView view, Sdk androidModel, Sdk iosModel)
        {
            if (androidModel == null && iosModel == null)
            {
                throw new ArgumentNullException(nameof(androidModel), "At least one platform model must be provided");
            }

            View = view;
            AndroidModel = androidModel;
            IosModel = iosModel;
        }

        public bool TryInitialize()
        {
            if (!View.TryLoadFromTemplate())
            {
                return false;
            }

            ApplyModel();
            SubscribeToShellEvents();
            return true;
        }

        public void SetSelectedWithoutNotify(bool value)
        {
            Shell?.View.SetSelectedWithoutNotify(value);
            Shell?.View.SetDeselectedOverlayVisible(!value);
        }

        public virtual void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            UnsubscribeFromShellEvents();
            Shell?.View?.Dispose();
        }

        protected virtual void ApplyModel()
        {
            string displayName = AndroidModel?.DisplayName ?? IosModel?.DisplayName;
            if (!String.IsNullOrEmpty(displayName))
            {
                SdkName = displayName;
            }

            var requirement = AndroidModel.GetStrictestRequirement(IosModel);
            SetRequirement(requirement);

            Shell?.SetPlatformDisabled(Platform.Android, AndroidModel == null);
            Shell?.SetPlatformDisabled(Platform.Ios, IosModel == null);

            ApplyVersions(Shell?.View.AndroidDropdown, AndroidModel);
            ApplyVersions(Shell?.View.IosDropdown, IosModel);

            if (SdkId == ImmutableSdkId)
            {
                Shell?.View.SetDropdownsLocked(true);
            }
        }

        private void SetRequirement(Requirement requirement)
        {
            if (Shell?.View == null) return;

            switch (requirement)
            {
                case Requirement.Required:
                    Shell.View.SetSelectedWithoutNotify(true);
                    Shell.View.SetSelectionEnabled(false);
                    Shell.View.SetDeselectedOverlayVisible(false);
                    break;
                case Requirement.Default:
                    Shell.View.SetSelectedWithoutNotify(true);
                    Shell.View.SetSelectionEnabled(true);
                    Shell.View.SetDeselectedOverlayVisible(false);
                    break;
                case Requirement.Optional:
                default:
                    Shell.View.SetSelectedWithoutNotify(false);
                    Shell.View.SetSelectionEnabled(true);
                    Shell.View.SetDeselectedOverlayVisible(true);
                    break;
            }
        }

        private static void ApplyVersions(SdkVersionDropdown dropdown, Sdk model)
        {
            if (dropdown == null) return;

            var versions = model?.Versions?.ToList() ?? new List<VersionInfo>();
            if (model == null || model.Requirement != Requirement.Required)
            {
                versions.Add(null);
            }

            dropdown.SetVersions(versions);

            bool hasVersions = model?.HasVersions ?? false;
            bool hasWarning = model?.HasWarning ?? false;

            dropdown.IsButtonEnabled = hasVersions;
            dropdown.SetTooltip(model?.Warning);

            dropdown.SetWarningStyle(hasVersions && hasWarning);
        }

        private void SubscribeToShellEvents()
        {
            if (Shell?.View == null) return;

            Shell.View.SelectionChanged += OnSelectionChanged;
            Shell.View.InfoIconHoverEnter += OnInfoIconHoverEnter;
            Shell.View.InfoIconHoverLeave += OnInfoIconHoverLeave;
            Shell.View.AndroidVersionChanged += OnAndroidVersionChanged;
            Shell.View.IosVersionChanged += OnIosVersionChanged;
        }

        private void UnsubscribeFromShellEvents()
        {
            if (Shell?.View == null) return;

            Shell.View.SelectionChanged -= OnSelectionChanged;
            Shell.View.InfoIconHoverEnter -= OnInfoIconHoverEnter;
            Shell.View.InfoIconHoverLeave -= OnInfoIconHoverLeave;
            Shell.View.AndroidVersionChanged -= OnAndroidVersionChanged;
            Shell.View.IosVersionChanged -= OnIosVersionChanged;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Shell?.View.SetDeselectedOverlayVisible(!e.IsSelected);
            SelectionChanged?.Invoke(this, e);
        }

        private void OnInfoIconHoverEnter(object sender, EventArgs e)
        {
            var requirement = AndroidModel.GetStrictestRequirement(IosModel);
            var outcome = SdkInfoScriptableObject.TryGetById(SdkId);
            string description = outcome.IsSuccess ? outcome.Value.Description : String.Empty;
            SdkTooltipOverlay.Show(Root, SdkName, requirement, description);
        }

        private static void OnInfoIconHoverLeave(object sender, EventArgs e) => SdkTooltipOverlay.Hide();

        private void OnAndroidVersionChanged(object sender, VersionChangedEventArgs e)
        {
            OnVersionChanged(Platform.Android, e.Version);
            AndroidVersionChanged?.Invoke(this, e);
        }

        private void OnIosVersionChanged(object sender, VersionChangedEventArgs e)
        {
            OnVersionChanged(Platform.Ios, e.Version);
            IosVersionChanged?.Invoke(this, e);
        }

        protected virtual void OnVersionChanged(Platform platform, VersionInfo version) { } // Override in derived classes to handle version changes
    }
}
