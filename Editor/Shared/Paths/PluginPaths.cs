namespace AppodealInc.Mediation.Shared.Editor
{
    internal static class PluginPaths
    {
        public const string PackageName = "com.appodeal.mediation";
        public const string PackageDir = "Packages/" + PackageName;

        private const string EditorDir = "Assets/Appodeal/Editor";

        public const string DependenciesDir = EditorDir + "/Dependencies";
        public const string DependenciesFile = DependenciesDir + "/AppodealDependencies.xml";

        public const string ResourcesSubDir = "Appodeal";
        public const string EditorResourcesDir = EditorDir + "/Resources/" + ResourcesSubDir;

        public const string AndroidPluginsDir = "Assets/Plugins/Android";
        public const string AndroidLibDir = AndroidPluginsDir + "/appodeal.androidlib";
    }
}
