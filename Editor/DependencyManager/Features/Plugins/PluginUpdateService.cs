using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using AppodealInc.Mediation.Analytics.Editor;
using AppodealInc.Mediation.Shared.Editor;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal static class PluginUpdateService
    {
        private static readonly AppodealLogger Log = DmLogging.Log;

        public static async Task<bool> CheckAndUpdateAsync()
        {
            var pluginsFetchOutcome = await DataLoader.FetchAvailablePluginsAsync();
            if (!pluginsFetchOutcome.IsSuccess)
            {
                Log.Error($"Cannot check for plugin updates. Reason - '{pluginsFetchOutcome.Failure.Message}'");
                return false;
            }

            return await TryUpdatePluginAsync(pluginsFetchOutcome.Value);
        }

        private static async Task<bool> TryUpdatePluginAsync(List<PluginDto> availablePlugins)
        {
            if (!DmChoicesScriptableObject.Instance.CheckPeriodicallyForPluginUpdates) return false;

            var packageVersionLookupOutcome = PackageVersionProvider.TryLookupVersion();
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
                Log.Error($"Plugin Update Failed. Reason - '{request.Error.message}'");
                return false;
            }

            SessionState.SetBool(DmConstants.Plugin.PostUpdatePendingKey, true);

            string message = $"{AppodealLogger.Colorize(Color.green, "Successfully")} updated Appodeal Unity Plugin to v{latestPluginSelectOutcome.Value.version}";
            Log.Verbose(message);

            EditorUtility.RequestScriptReload();
            return true;
        }

        private static bool ShowUpdateDialog(string message)
        {
            bool shouldUpdate = EditorUtility.DisplayDialog(DmConstants.UI.DialogTitle, message, "Update", "Cancel");
            Log.Verbose($"{nameof(ShowUpdateDialog)}({nameof(message)}: {message}) => {shouldUpdate}");
            return shouldUpdate;
        }
    }
}
