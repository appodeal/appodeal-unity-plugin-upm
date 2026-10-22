using System.IO;
using System.Linq;
using UnityEditor;
using AppodealInc.Mediation.Shared.Editor;

namespace AppodealInc.Mediation.LegacyCleanup.Editor
{
    // Plugin versions before 4.5.0 copied the androidlib into Assets. It now ships in the package.
    // Delete this assembly once the data show no plugin versions before 4.5.0, and review PluginPaths with it.
    internal class LegacyAndroidLibraryRemover : AssetPostprocessor
    {
        private static readonly AppodealLogger Log = LegacyCleanupLogging.Log;

        private const string PluginsDir = PluginPaths.AssetsDir + "/Plugins";
        private const string AndroidLibDir = PluginPaths.AndroidPluginsDir + "/appodeal.androidlib";

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
        {
            if (!Directory.Exists(AndroidLibDir)) return;

            DeleteWithMeta(AndroidLibDir);
            DeleteIfEmpty(PluginPaths.AndroidPluginsDir);
            DeleteIfEmpty(PluginsDir);
            AssetDatabase.Refresh();

            Log.Warning($"Removed '{AndroidLibDir}': the plugin now applies its Android settings in the generated Gradle project. If you changed files in that folder, move the changes to your own Android library.");
        }

        private static void DeleteIfEmpty(string dir)
        {
            if (Directory.Exists(dir) && !Directory.EnumerateFileSystemEntries(dir).Any()) DeleteWithMeta(dir);
        }

        private static void DeleteWithMeta(string path)
        {
            FileUtil.DeleteFileOrDirectory(path);
            FileUtil.DeleteFileOrDirectory($"{path}.meta");
        }
    }
}
