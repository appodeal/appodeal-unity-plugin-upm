using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal class DmAssetPostprocessor : AssetPostprocessor
    {
        private static async void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
        {
            try
            {
                if (Application.isBatchMode)
                {
                    LogHelper.Log("[DmAssetPostprocessor] Skipping auto check flow in batch mode");
                    return;
                }

                if (deletedAssets.Any(asset => asset.Contains($"Packages/{DmConstants.Plugin.PackageName}"))) return;
                if (movedAssets.Any(asset => asset.Contains(DmConstants.Choices.FileName))) return;

                if (DmChoicesScriptableObject.Instance == null) return;

                if (ShouldCheckPluginUpdates())
                {
                    SessionState.SetBool(DmConstants.Plugin.UpdateCheckPerformedKey, true);
                    bool isPluginUpdated = await PluginUpdateService.CheckAndUpdateAsync();
                    if (isPluginUpdated) return;
                }

                if (ShouldValidateDependencies())
                {
                    bool wasValidationPerformed = await DependencyValidationService.ValidateAsync(isManual: false);
                    if (wasValidationPerformed) SessionState.SetBool(DmConstants.Validation.AutoValidatePerformedKey, true);
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError($"[DmAssetPostprocessor] Unexpected error: {ex.Message}");
            }
        }

        private static bool ShouldCheckPluginUpdates()
        {
            bool isEnabled = DmChoicesScriptableObject.Instance.CheckPeriodicallyForPluginUpdates;
            bool notPerformedYet = !SessionState.GetBool(DmConstants.Plugin.UpdateCheckPerformedKey, false);
            return isEnabled && notPerformedYet;
        }

        private static bool ShouldValidateDependencies()
        {
            bool isEnabled = DmChoicesScriptableObject.Instance.ValidateDependenciesPeriodically;
            bool notPerformedYet = !SessionState.GetBool(DmConstants.Validation.AutoValidatePerformedKey, false);
            return isEnabled && notPerformedYet;
        }
    }
}
