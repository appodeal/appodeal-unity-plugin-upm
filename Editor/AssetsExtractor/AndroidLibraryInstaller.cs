using System.IO;
using UnityEditor;
using AppodealAds.Unity.Editor.Utils;
using AppodealAds.Unity.Editor.InternalResources;

namespace AppodealAds.Unity.Editor.Utils
{
    static class AndroidLibraryInstaller
    {
        [InitializeOnLoadMethod]
        static void InstallAndroidLibrary()
        {
            if (PluginPreferences.Instance.IsAndroidLibraryImported) return;
            ImportAndroidLibraryFromPackage();
        }

        private static void ImportAndroidLibraryFromPackage() {
            var source = Path.Combine(AppodealEditorConstants.PackagePath, "Runtime/Plugins/Android/appodeal.androidlib~");
            var destination = Path.Combine("Assets/Plugins/Android", "appodeal.androidlib");

            if (Directory.Exists(destination)) return;
            
            FileUtil.CopyFileOrDirectory(source, destination);

            PluginPreferences.Instance.IsAndroidLibraryImported = true;
            PluginPreferences.Instance.SaveAsync();
            AssetDatabase.Refresh();
        }
    }
}
