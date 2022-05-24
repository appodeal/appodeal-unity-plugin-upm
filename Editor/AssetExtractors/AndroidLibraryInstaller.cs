using System.IO;
using UnityEditor;
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
            string source = Path.Combine(AppodealEditorConstants.PackagePath, "Runtime/Plugins/Android/appodeal.androidlib~");
            string destination = Path.Combine("Assets/Plugins/Android", "appodeal.androidlib");
            string valuesDir = Path.Combine(destination, AppodealEditorConstants.FirebaseAndroidConfigPath);

            if (Directory.Exists(destination)) return;

            if (!Directory.Exists("Assets/Plugins/Android"))
            {
                Directory.CreateDirectory("Assets/Plugins/Android");
            }

            FileUtil.CopyFileOrDirectory(source, destination);

            if (!Directory.Exists(valuesDir))
            {
                Directory.CreateDirectory(valuesDir);
            }

            PluginPreferences.Instance.IsAndroidLibraryImported = true;
            PluginPreferences.Instance.SaveAsync();
            AssetDatabase.Refresh();
        }
    }
}
