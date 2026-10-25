using System;
using AppodealInc.Mediation.PluginSettings.Editor;
using AppodealInc.Mediation.Shared.Editor;

namespace AppodealInc.Mediation.PreProcess.Editor
{
    internal static class FacebookConfigurator
    {
        private static readonly AppodealLogger Log = PreProcessLogging.Log;

        private const string ApplicationId = "com.facebook.sdk.ApplicationId";
        private const string ClientToken = "com.facebook.sdk.ClientToken";
        private const string AutoLogAppEventsEnabled = "com.facebook.sdk.AutoLogAppEventsEnabled";
        private const string AdvertiserIDCollectionEnabled = "com.facebook.sdk.AdvertiserIDCollectionEnabled";

        public static void Configure(AndroidManifest manifest, AppodealSettings settings)
        {
            if (!settings.FacebookAutoConfiguration) return;

            string appId = settings.FacebookAndroidAppId;
            string clientToken = settings.FacebookAndroidClientToken;

            if (String.IsNullOrEmpty(appId) || String.IsNullOrEmpty(clientToken))
            {
                Log.Warning("Facebook auto configuration is enabled in 'Appodeal > Appodeal Settings', but the required App ID or Client Token is not set. The SDK will likely fail to initialize.");
                return;
            }

            manifest.AddMetaData(ApplicationId, "fb" + appId);
            manifest.AddMetaData(ClientToken, clientToken);
            manifest.AddMetaData(AutoLogAppEventsEnabled, settings.FacebookAutoLogAppEvents.ToString().ToLower());
            manifest.AddMetaData(AdvertiserIDCollectionEnabled, settings.FacebookAdvertiserIDCollection.ToString().ToLower());
        }
    }
}
