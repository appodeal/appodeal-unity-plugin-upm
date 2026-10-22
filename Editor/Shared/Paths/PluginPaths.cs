namespace AppodealInc.Mediation.Shared.Editor
{
    internal static class PluginPaths
    {
        public const string PackageName = "com.appodeal.mediation";
        public const string PackageDir = "Packages/" + PackageName;

        public const string AssetsDir = "Assets";

        private const string EditorDir = AssetsDir + "/Appodeal/Editor";

        public const string DependenciesDir = EditorDir + "/Dependencies";
        public const string DependenciesFile = DependenciesDir + "/AppodealDependencies.xml";

        public const string ResourcesSubDir = "Appodeal";
        public const string EditorResourcesDir = EditorDir + "/Resources/" + ResourcesSubDir;

        public const string AndroidPluginsDir = AssetsDir + "/Plugins/Android";
    }
}
