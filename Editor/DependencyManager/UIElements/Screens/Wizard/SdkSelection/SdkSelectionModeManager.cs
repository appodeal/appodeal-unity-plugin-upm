using System;
using System.Collections.Generic;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal class SdkSelectionModeManager
    {
        public SdkSelectionMode CurrentMode { get; private set; } = DmChoicesScriptableObject.Instance?.SelectionMode ?? SdkSelectionMode.Default;

        public event EventHandler<SdkSelectionModeChangedEventArgs> ModeChanged;

        public void SetMode(SdkSelectionMode mode, bool notify = true)
        {
            if (CurrentMode == mode) return;

            CurrentMode = mode;

            DmChoicesScriptableObject.Instance.SelectionMode = mode;
            DmChoicesScriptableObject.SaveToDisk();

            if (notify) ModeChanged?.Invoke(this, new SdkSelectionModeChangedEventArgs(mode));
        }

        public bool ApplyModeToControllers(IEnumerable<ISdkCardController> controllers)
        {
            bool anyChanged = false;
            foreach (var ctrl in controllers)
            {
                anyChanged |= ApplyModeToController(ctrl);
            }
            return anyChanged;
        }

        private bool ApplyModeToController(ISdkCardController ctrl)
        {
            var requirement = ctrl.AndroidModel.GetStrictestRequirement(ctrl.IosModel);

            if (requirement == Requirement.Required) return false;

            bool targetSelected = GetTargetSelection(ctrl, requirement);

            if (ctrl.IsSelected == targetSelected) return false;

            ctrl.SetSelectedWithoutNotify(targetSelected);
            return ctrl.HasAnyVersions;
        }

        private bool GetTargetSelection(ISdkCardController ctrl, Requirement requirement)
        {
            switch (CurrentMode)
            {
                case SdkSelectionMode.All:
                    return true;
                case SdkSelectionMode.Minimal:
                    return false;
                case SdkSelectionMode.Default:
                    return requirement is Requirement.Required or Requirement.Default;
                case SdkSelectionMode.Custom:
                    if (DmChoicesScriptableObject.IsSdkSelectionStateKnown(ctrl.SdkId, out var state)) return state.isSelected;
                    return requirement is Requirement.Required or Requirement.Default;
                default:
                    return requirement is Requirement.Required or Requirement.Default;
            }
        }
    }
}
