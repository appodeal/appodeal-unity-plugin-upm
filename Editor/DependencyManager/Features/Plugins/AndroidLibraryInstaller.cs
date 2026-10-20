using System.IO;
using UnityEditor;
using AppodealInc.Mediation.Shared.Editor;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal static class AndroidLibraryInstaller
    {
        private static readonly AppodealLogger Log = DmLogging.Log;

        public static bool EnsureAndroidLibraryDirectory(bool forceReinstall = false)
        {
            if (Directory.Exists(PluginPaths.AndroidLibDir))
            {
                if (!forceReinstall) return false;
                FileUtil.DeleteFileOrDirectory(PluginPaths.AndroidLibDir);
            }

            string source = $"{PluginPaths.PackageDir}/Runtime/Plugins/Android/appodeal.androidlib~";
            if (!Directory.Exists(source))
            {
                Log.Error($"Directory was not found: '{source}'. Please, contact support@appodeal.com about this issue");
                return false;
            }

            Directory.CreateDirectory(PluginPaths.AndroidPluginsDir);
            FileUtil.CopyFileOrDirectory(source, PluginPaths.AndroidLibDir);

            return true;
        }
    }
}
