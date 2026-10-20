using AppodealInc.Mediation.Shared.Editor;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    public static partial class DmConstants
    {
        internal static class Plugin
        {
            public const string PackageGitUrl = "https://github.com/appodeal/appodeal-unity-plugin-upm.git";
            public const string BundledDependenciesFilePath = PluginPaths.PackageDir + "/Editor/DependencyManager/DefaultDependencies/AppodealDependencies.txt";
            public const string PostUpdatePendingKey = "ApdDmPostUpdatePending";
            public const string UpdateCheckPerformedKey = "ApdDmAutoCheckForUpdatesPerformed";
        }
    }
}
