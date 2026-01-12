using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using AppodealInc.Mediation.Analytics.Editor;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal static class PluginUpdateService
    {
        public static async Task<bool> CheckAndUpdateAsync()
        {
            if (!IsPluginAutoUpdateSupported()) return false;

            var pluginsFetchOutcome = await DataLoader.FetchAvailablePluginsAsync();
            if (!pluginsFetchOutcome.IsSuccess)
            {
                LogHelper.LogError($"Cannot check for plugin updates. Reason - '{pluginsFetchOutcome.Failure.Message}'");
                return false;
            }

            return await TryUpdatePluginAsync(pluginsFetchOutcome.Value);
        }

        private static async Task<bool> TryUpdatePluginAsync(List<PluginDto> availablePlugins)
        {
            if (!DmChoicesScriptableObject.Instance.CheckPeriodicallyForPluginUpdates) return false;

            var packageVersionLookupOutcome = await PackageVersionProvider.TryLookupVersionAsync();
            if (!packageVersionLookupOutcome.IsSuccess) return false;

            var latestPluginSelectOutcome = VersionComparer.TrySelectLatestPlugin(availablePlugins, DmChoicesScriptableObject.Instance.IncludePluginBetaVersions);
            if (!latestPluginSelectOutcome.IsSuccess) return false;

            if (latestPluginSelectOutcome.Value.CompareTo(packageVersionLookupOutcome.Value) != VersionComparisonResult.Subsequent) return false;

            bool shouldUpdate = ShowUpdateDialog($"Appodeal Unity Plugin v{latestPluginSelectOutcome.Value.version} is available. Do you want to update now?");
            if (!shouldUpdate) return false;

            AnalyticsService.TrackClickEvent(ActionType.UpdatePlugin);

            // string homePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile);
            // var request = Client.Add($"file:{homePath}/Downloads/Appodeal/com.appodeal.mediation-{latestPluginSelectOutcome.Value.version}.tgz");
            var request = Client.Add($"{DmConstants.Plugin.PackageGitUrl}#v{latestPluginSelectOutcome.Value.version}");
            while (!request.IsCompleted) await Task.Yield();
            if (request.Status == StatusCode.Failure)
            {
                LogHelper.LogError($"Plugin Update Failed. Reason - '{request.Error.message}'");
                return false;
            }

            SessionState.SetBool(DmConstants.Plugin.PostUpdatePendingKey, true);

            string message = $"{LogHelper.Colorize(Color.green, "Successfully")} updated Appodeal Unity Plugin to v{latestPluginSelectOutcome.Value.version}";
            LogHelper.Log(message);

            EditorUtility.RequestScriptReload();
            return true;
        }

        private static bool ShowUpdateDialog(string message)
        {
            bool shouldUpdate = EditorUtility.DisplayDialog(DmConstants.UI.DialogTitle, message, "Update", "Cancel");
            LogHelper.Log($"{nameof(ShowUpdateDialog)}({nameof(message)}: {message}) => {shouldUpdate}");
            return shouldUpdate;
        }

        private static bool IsPluginAutoUpdateSupported()
        {
#if UNITY_2022_1_OR_NEWER
            return true;
#else
            LogHelper.LogWarning("Plugin auto-update is disabled on Unity v2021 due to Package Manager instability. Please update manually via Package Manager");
            return false;
#endif
        }
    }
}
