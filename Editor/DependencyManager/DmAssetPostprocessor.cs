using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal class DmAssetPostprocessor : AssetPostprocessor
    {
        private static readonly object Gate = new();
        private static bool _isProcessing;

        private static async void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
        {
            bool acquired = false;

            try
            {
                if (deletedAssets.Any(asset => asset.Contains($"Packages/{DmConstants.Plugin.PackageName}"))) return;
                if (movedAssets.Any(asset => asset.Contains(DmConstants.Choices.FileName))) return;

                lock (Gate)
                {
                    if (_isProcessing) return;
                    _isProcessing = true;
                    acquired = true;
                }

                if (ShouldCheckPluginUpdates())
                {
                    SessionState.SetBool(DmConstants.Plugin.UpdateCheckPerformedKey, true);
                    bool isPluginUpdated = await PluginUpdateService.CheckAndUpdateAsync();
                    if (isPluginUpdated) return;
                }

                bool depsUpdated = await DependenciesInstaller.EnsureDependenciesXmlFileAsync();
                bool androidLibUpdated = AndroidLibraryInstaller.EnsureAndroidLibraryDirectory(forceReinstall: depsUpdated);

                if (ShouldValidateDependencies())
                {
                    bool validationPerformed = await DependencyValidationService.ValidateAsync(isManual: false);
                    if (validationPerformed) SessionState.SetBool(DmConstants.Validation.AutoValidatePerformedKey, true);
                }

                if (depsUpdated || androidLibUpdated) AssetDatabase.Refresh();
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex);
            }
            finally
            {
                if (acquired)
                {
                    lock (Gate)
                    {
                        _isProcessing = false;
                    }
                }
            }
        }

        private static bool ShouldCheckPluginUpdates()
        {
            if (Application.isBatchMode) return false;
            bool isEnabled = DmChoicesScriptableObject.Instance?.CheckPeriodicallyForPluginUpdates ?? false;
            bool notPerformedYet = !SessionState.GetBool(DmConstants.Plugin.UpdateCheckPerformedKey, false);
            return isEnabled && notPerformedYet;
        }

        private static bool ShouldValidateDependencies()
        {
            if (Application.isBatchMode) return false;
            bool isEnabled = DmChoicesScriptableObject.Instance?.ValidateDependenciesPeriodically ?? false;
            bool notPerformedYet = !SessionState.GetBool(DmConstants.Validation.AutoValidatePerformedKey, false);
            return isEnabled && notPerformedYet;
        }
    }
}
