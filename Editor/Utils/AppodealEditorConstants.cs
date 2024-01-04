using System.Diagnostics.CodeAnalysis;

// ReSharper Disable CheckNamespace
namespace AppodealStack.UnityEditor.Utils
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static class AppodealEditorConstants
    {
        public const string PluginRequest = "https://mw-backend.appodeal.com/v2.1/unity";
        public const string AdaptersRequest = "https://mw-backend.appodeal.com/v2.1/unity/config/";
        public const string GitRepoAddress = "https://github.com/appodeal/appodeal-unity-plugin-upm.git";
        public const string PackagePath = "Packages/com.appodeal.mediation";
        public const string PluginPath = "Assets/Appodeal";
        public const string DependenciesPath = "Editor/Dependencies";
        public const string EditorAdPrefabsPath = "Editor/EditorAds/Prefabs";
        public const string ReplaceNetworkDepValue = "com.appodeal.ads.sdk.networks:";
        public const string ReplaceServiceDepValue = "com.appodeal.ads.sdk.services:";
        public const string ReplaceAdmobDepValue = "com.appodeal.ads.sdk.networks:admob";
        public const string ReplaceDepCore = "com.appodeal.ads.sdk:core:";
        public const string PackageName = "Name";
        public const string CurrentVersionHeader = "Current Version";
        public const string LatestVersionHeader = "Latest Version";
        public const string ActionHeader = "Action";
        public const string BoxStyle = "box";
        public const string ActionUpdate = "Update";
        public const string ActionImport = "Import";
        public const string ActionReimport = "Reimport";
        public const string ActionRemove = "Remove";
        public const string EmptyCurrentVersion = "    -  ";
        public const string AppodealUnityPlugin = "Appodeal Unity Plugin";
        public const string AppodealSdkManager = "Appodeal SDK Manager";
        public const string Appodeal = "Appodeal";
        public const string Loading = "Loading...";
        public const string ProgressBarCancelled = "Progress bar canceled by the user";
        public const string AppodealCoreDependencies = "Appodeal Core Dependencies";
        public const string Ios = "iOS";
        public const string Android = "Android";
        public const string AppodealNetworkDependencies = "Appodeal Network Dependencies";
        public const string AppodealServiceDependencies = "Appodeal Service Dependencies";
        public const string SpecOpenDependencies = "<dependencies>\n";
        public const string SpecCloseDependencies = "</dependencies>";
        public const string XmlFileExtension = ".xml";
        public const string TwitterMoPub = "TwitterMoPub";
        public const string GoogleAdMob = "GoogleAdMob";
        public const string ApdAppodealAdExchangeAdapter = "APDAppodealAdExchangeAdapter";
        public const string Dependencies = "Dependencies";

        public const string AdMobAppIdPlaceholder = "ca-app-pub-xxxxxxxxxxxxxxxx~yyyyyyyyyy";

        public const string FacebookApplicationId = "com.facebook.sdk.ApplicationId";
        public const string FacebookClientToken = "com.facebook.sdk.ClientToken";
        public const string FacebookAutoLogAppEventsEnabled = "com.facebook.sdk.AutoLogAppEventsEnabled";
        public const string FacebookAdvertiserIDCollectionEnabled = "com.facebook.sdk.AdvertiserIDCollectionEnabled";

        public const string AppodealAndroidLibPath = "Plugins/Android/appodeal.androidlib";
        public const string AndroidManifestFile = "AndroidManifest.xml";
        public const string FirebaseAndroidConfigPath = "res/values";
        public const string FirebaseAndroidConfigFile = "google-services.xml";
        public const string FirebaseAndroidJsonFile = "google-services.json";
    }
}
