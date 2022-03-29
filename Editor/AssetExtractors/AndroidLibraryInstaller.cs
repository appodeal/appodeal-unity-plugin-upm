using UnityEditor;
using System.IO;
using AppodealStack.UnityEditor.Utils;
using AppodealStack.UnityEditor.InternalResources;

// ReSharper Disable CheckNamespace
namespace AppodealStack.UnityEditor.AssetExtractors
{
    static class AndroidLibraryInstaller
    {
        [InitializeOnLoadMethod]
        static void InstallAndroidLibrary()
        {
            if (PluginPreferences.Instance.IsAndroidLibraryImported) return;
            ImportAndroidLibraryFromPackage();
        }

        private static void ImportAndroidLibraryFromPackage()
        {
            var source = Path.Combine(AppodealEditorConstants.PackagePath, "Runtime/Plugins/Android/appodeal.androidlib~");
            var destination = Path.Combine("Assets/Plugins/Android", "appodeal.androidlib");

            if (Directory.Exists(destination)) return;

            if (!Directory.Exists("Assets/Plugins/Android"))
            {
                Directory.CreateDirectory("Assets/Plugins/Android");
            }

            FileUtil.CopyFileOrDirectory(source, destination);

            PluginPreferences.Instance.IsAndroidLibraryImported = true;
            PluginPreferences.Instance.SaveAsync();
            AssetDatabase.Refresh();
        }
    }
}
