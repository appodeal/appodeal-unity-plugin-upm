// ReSharper disable CheckNamespace

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using AppodealInc.Mediation.Analytics.Editor;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal class DmAutoUpdateHandler : AssetPostprocessor
    {
        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        [SuppressMessage("ReSharper", "Unity.IncorrectMethodSignature")]
        private static async void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
        {
            if (deletedAssets.Any(asset => asset.Contains($"Packages/{DmConstants.AppodealPackageName}"))) return;
            if (movedAssets.Any(asset => asset.Contains(DmConstants.DmChoicesFileName))) return;

            if (DmChoicesScriptableObject.Instance == null) return;

            if (!DmChoicesScriptableObject.Instance.CheckPeriodicallyForPluginUpdates
                && !DmChoicesScriptableObject.Instance.CheckPeriodicallyForAdapterUpdates) return;

            if (SessionState.GetBool(DmConstants.AutoCheckForUpdatesPerformedKey, false)) return;
            SessionState.SetBool(DmConstants.AutoCheckForUpdatesPerformedKey, true);

            var (_, configRequest) = await DataLoader.GetConfigAsync(Guid.Empty);
            if (!configRequest.IsSuccess)
            {
                LogHelper.LogError($"Cannot check for the updates. Reason - '{configRequest.Error.Message}'.");
                return;
            }

            bool isPluginUpdated = await HandlePluginUpdate(configRequest.Value.Plugins);
            if (isPluginUpdated) return;

            await HandleAdaptersUpdate(configRequest.Value);
        }

        private static async Task<bool> HandlePluginUpdate(List<Plugin> availableVersions)
        {
            LogHelper.Log($"{nameof(HandlePluginUpdate)}() method is called");

#if !UNITY_2022_1_OR_NEWER && !APPODEAL_DEV
            return false;
#endif

            if (!DmChoicesScriptableObject.Instance.CheckPeriodicallyForPluginUpdates) return false;

            var currentPluginVersionRequest = await VersionHelper.GetCurrentVersionForPackage(DmConstants.AppodealPackageName);
            if (!currentPluginVersionRequest.IsSuccess) return false;

            var latestPluginVersionRequest = VersionHelper.GetLatestPlugin(availableVersions, DmChoicesScriptableObject.Instance.IncludePluginBetaVersions);
            if (!latestPluginVersionRequest.IsSuccess) return false;

            if (latestPluginVersionRequest.Value.CompareTo(currentPluginVersionRequest.Value) != ComparisonResult.Subsequent) return false;

            bool shouldUpdate = ShowUpdateDialog($"Appodeal Unity Plugin v{latestPluginVersionRequest.Value.version} is available. Do you want to update now?");
            if (!shouldUpdate) return false;

            AnalyticsService.TrackClickEvent(ActionType.UpdatePlugin);

            var (_, configRequest) = await DataLoader.GetConfigAsync(Guid.Empty, latestPluginVersionRequest.Value.version);
            if (!configRequest.IsSuccess) return false;

            var urlRequest = await UrlBuilder.GetLocalDependenciesUrlAsync(configRequest.Value, latestPluginVersionRequest.Value.version);
            if (!urlRequest.IsSuccess) return false;

            var remoteDepsRequest = await DataLoader.GetRemoteDependenciesAsync(urlRequest.Value);
            if (!remoteDepsRequest.IsSuccess) return false;

            var xmlRequest = XmlHandler.DeserializeDependencies(remoteDepsRequest.Value);
            if (!xmlRequest.IsSuccess) return false;

            bool areAdaptersUpdated = XmlHandler.UpdateDependencies(xmlRequest.Value);
            if (!areAdaptersUpdated) return false;

            // $"file:/Users/fdn/Downloads/Appodeal/com.appodeal.mediation-{latestPluginVersionRequest.Value.version}.tgz"
            var result = Client.Add($"{DmConstants.AppodealPackageGitUrl}#v{latestPluginVersionRequest.Value.version}");
            while (!result.IsCompleted) await Task.Yield();
            if (result.Status == StatusCode.Failure)
            {
                LogHelper.LogError($"Plugin Update Failed. Reason - '{result.Error.message}'.");
                if (configRequest.Value.LocalDeps != null && XmlHandler.UpdateDependencies(configRequest.Value.LocalDeps)) return false;
                if (File.Exists(DmConstants.DependenciesFilePath)) FileUtil.DeleteFileOrDirectory(DmConstants.DependenciesFilePath);
                return false;
            }

            if (Directory.Exists(DmConstants.AndroidLibDir))
            {
                FileUtil.DeleteFileOrDirectory(DmConstants.AndroidLibDir);
                FileUtil.DeleteFileOrDirectory($"{DmConstants.AndroidLibDir}.meta");
            }

            var diff = DependenciesDiff.Get(configRequest.Value.LocalDeps, xmlRequest.Value);
            string message = $"{LogHelper.Colorize(Color.green, "Successfully")} updated Appodeal Unity Plugin.";
            LogHelper.LogDepsUpdate(message, diff);

            return true;
        }

        private static async Task HandleAdaptersUpdate(DmConfigDto config)
        {
            LogHelper.Log($"{nameof(HandleAdaptersUpdate)}() method is called");

            if (!DmChoicesScriptableObject.Instance.CheckPeriodicallyForAdapterUpdates) return;
            if (!File.Exists(DmConstants.DependenciesFilePath) || config.LocalDeps == null) return;

            var urlRequest = await UrlBuilder.GetLocalDependenciesUrlAsync(config);
            if (!urlRequest.IsSuccess) return;

            var remoteDepsRequest = await DataLoader.GetRemoteDependenciesAsync(urlRequest.Value);
            if (!remoteDepsRequest.IsSuccess) return;

            var xmlRequest = XmlHandler.DeserializeDependencies(remoteDepsRequest.Value);
            if (!xmlRequest.IsSuccess) return;

            if (!config.LocalDeps.IsDifferentFromRemote(xmlRequest.Value)) return;

            if (DmChoicesScriptableObject.Instance.UpdateAdaptersAutomatically)
            {
                bool areAdaptersUpdated = XmlHandler.UpdateDependencies(xmlRequest.Value);
                if (areAdaptersUpdated)
                {
                    var diff = DependenciesDiff.Get(config.LocalDeps, xmlRequest.Value);
                    string message = $"{LogHelper.Colorize(Color.green, "Successfully")} updated dependencies.";
                    LogHelper.LogDepsUpdate(message, diff);
                }
            }
            else
            {
                bool shouldUpdateAdapters = ShowUpdateDialog("Some SDK adapters are outdated. Do you want to update them now?");
                if (!shouldUpdateAdapters) return;

                AnalyticsService.TrackClickEvent(ActionType.UpdateDependencies);

                bool areAdaptersUpdated = XmlHandler.UpdateDependencies(xmlRequest.Value);
                if (areAdaptersUpdated)
                {
                    var diff = DependenciesDiff.Get(config.LocalDeps, xmlRequest.Value);
                    string message = $"{LogHelper.Colorize(Color.green, "Successfully")} updated dependencies.";
                    LogHelper.LogDepsUpdate(message, diff);
                }
            }
        }

        private static bool ShowUpdateDialog(string message)
        {
            bool shouldUpdate = EditorUtility.DisplayDialog("Appodeal Dependency Manager", message, "Update", "Cancel");
            LogHelper.Log($"{nameof(ShowUpdateDialog)}({nameof(message)}: {message}) => {shouldUpdate}");
            return shouldUpdate;
        }
    }
}
