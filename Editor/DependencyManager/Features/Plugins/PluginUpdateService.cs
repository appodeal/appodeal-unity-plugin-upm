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
#if !UNITY_2022_1_OR_NEWER && !APPODEAL_DEV
            return false;
#endif

            if (!DmChoicesScriptableObject.Instance.CheckPeriodicallyForPluginUpdates) return false;

            var packageVersionLookupOutcome = await PackageVersionProvider.TryLookupVersionAsync();
            if (!packageVersionLookupOutcome.IsSuccess) return false;

            var latestPluginSelectOutcome = VersionComparer.TrySelectLatestPlugin(availablePlugins, DmChoicesScriptableObject.Instance.IncludePluginBetaVersions);
            if (!latestPluginSelectOutcome.IsSuccess) return false;

            if (latestPluginSelectOutcome.Value.CompareTo(packageVersionLookupOutcome.Value) != VersionComparisonResult.Subsequent) return false;

            bool shouldUpdate = ShowUpdateDialog($"Appodeal Unity Plugin v{latestPluginSelectOutcome.Value.version} is available. Do you want to update now?");
            if (!shouldUpdate) return false;

            AnalyticsService.TrackClickEvent(ActionType.UpdatePlugin);

            var request = Client.Add($"{DmConstants.Plugin.PackageGitUrl}#v{latestPluginSelectOutcome.Value.version}");
            while (!request.IsCompleted) await Task.Yield();
            if (request.Status == StatusCode.Failure)
            {
                LogHelper.LogError($"Plugin Update Failed. Reason - '{request.Error.message}'");
                return false;
            }

            string message = $"{LogHelper.Colorize(Color.green, "Successfully")} updated Appodeal Unity Plugin to v{latestPluginSelectOutcome.Value.version}";
            LogHelper.Log(message);

            return true;
        }

        private static bool ShowUpdateDialog(string message)
        {
            bool shouldUpdate = EditorUtility.DisplayDialog(DmConstants.UI.DialogTitle, message, "Update", "Cancel");
            LogHelper.Log($"{nameof(ShowUpdateDialog)}({nameof(message)}: {message}) => {shouldUpdate}");
            return shouldUpdate;
        }
    }
}
