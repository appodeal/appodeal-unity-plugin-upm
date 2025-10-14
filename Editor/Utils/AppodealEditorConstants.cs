// ReSharper disable CheckNamespace

namespace AppodealInc.Mediation.Utils.Editor
{
    public static class AppodealEditorConstants
    {
        public const string GitRepoAddress = "https://github.com/appodeal/appodeal-unity-plugin-upm.git";

        public const string PackageName = "com.appodeal.mediation";
        public const string PackageDir = "Packages/" + PackageName;

        private const string PluginName = "Appodeal";
        private const string PluginDir = "Assets/" + PluginName;
        public const string PluginEditorDir = PluginDir + "/Editor";

        public const string DependenciesDir = PluginEditorDir + "/Dependencies";
        private const string DependenciesFileName = PluginName + "Dependencies";
        public const string DependenciesFilePath = DependenciesDir + "/" + DependenciesFileName + ".xml";
        public const string BundledDependenciesFilePath = PackageDir + "/Editor/DependencyManager/DefaultDependencies/" + DependenciesFileName + ".txt";

        public const string ResourcesDirName = "Resources";
        public const string EditorResourcesRootDir = PluginEditorDir + "/" + ResourcesDirName;
        public const string ResourcesSubPath = PluginName;
        public const string EditorResourcesDir = EditorResourcesRootDir + "/" + ResourcesSubPath;

        private const string AppodealSettingsResourceName = PluginName + "Settings";
        private const string AppodealSettingsFileName = AppodealSettingsResourceName + ".asset";
        public const string AppodealSettingsFilePath = EditorResourcesDir + "/" + AppodealSettingsFileName;
        public const string AppodealSettingsResourcePath = ResourcesSubPath + "/" + AppodealSettingsResourceName;

        public const string EditorAdPrefabsDir = PackageDir + "/Editor/EditorAds/Prefabs";
        public const string RemoveListFilePath = PackageDir + "/Editor/PluginRemover/remove_list.xml";

        public const string DependencyManagerWindowName = PluginName + "DependencyManager";

        public const string AdMobAppIdPlaceholder = "ca-app-pub-xxxxxxxxxxxxxxxx~yyyyyyyyyy";

        public const string FacebookApplicationId = "com.facebook.sdk.ApplicationId";
        public const string FacebookClientToken = "com.facebook.sdk.ClientToken";
        public const string FacebookAutoLogAppEventsEnabled = "com.facebook.sdk.AutoLogAppEventsEnabled";
        public const string FacebookAdvertiserIDCollectionEnabled = "com.facebook.sdk.AdvertiserIDCollectionEnabled";

        public const string AppodealAndroidLibDir = "Assets/Plugins/Android/appodeal.androidlib";
        public const string AndroidManifestFile = "AndroidManifest.xml";
        public const string FirebaseAndroidConfigPath = "res/values";
        public const string FirebaseAndroidConfigFile = "google-services.xml";
        public const string FirebaseAndroidJsonFile = "google-services.json";
    }
}
