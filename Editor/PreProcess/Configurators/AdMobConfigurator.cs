using System;
using System.IO;
using AppodealInc.Mediation.PluginSettings.Editor;
using AppodealInc.Mediation.Shared.Editor;

namespace AppodealInc.Mediation.PreProcess.Editor
{
    internal static class AdMobConfigurator
    {
        private static readonly AppodealLogger Log = PreProcessLogging.Log;

        private const string ApplicationId = "com.google.android.gms.ads.APPLICATION_ID";

        public static void Configure(AndroidManifest manifest, AppodealSettings settings)
        {
            const string depsFilePath = PluginPaths.DependenciesFile;

            if (!File.Exists(depsFilePath) || !CheckContainsAdmobAdapter(depsFilePath))
            {
                Log.Info($"Dependencies file '{depsFilePath}' has no AdMob adapter. The App ID isn't added.");
                return;
            }

            string admobAppId = settings.AdMobAndroidAppId;

            if (String.IsNullOrEmpty(admobAppId))
            {
                Log.Warning("AdMob App ID is not set in 'Appodeal > Appodeal Settings'. The app will likely crash on startup.");
                return;
            }

            // No return here because a false alarm stays harmless and an invalid ID crashes the app either way
            if (!admobAppId.StartsWith("ca-app-pub-") || admobAppId == AppodealSettings.AdMobAppIdPlaceholder)
            {
                Log.Warning($"AdMob App ID '{admobAppId}' in 'Appodeal > Appodeal Settings' is invalid. The app will likely crash on startup.");
            }

            manifest.AddMetaData(ApplicationId, admobAppId);
        }

        private static bool CheckContainsAdmobAdapter(string depsFilePath)
        {
            return File.ReadAllText(depsFilePath).Contains("com.appodeal.ads.sdk.adapters:admob");
        }
    }
}
