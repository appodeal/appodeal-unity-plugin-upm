using System.IO;
using UnityEditor;
using UnityEngine;
using AppodealStack.UnityEditor.Utils;
using AppodealStack.UnityEditor.InternalResources;

// ReSharper Disable CheckNamespace
namespace AppodealStack.UnityEditor.AssetExtractors
{
    internal static class AndroidLibraryInstaller
    {
        public static bool InstallAndroidLibrary()
        {
            return !PluginPreferences.Instance.IsAndroidLibraryImported && CopyAndroidLibraryFromPackage();
        }

        private static bool CopyAndroidLibraryFromPackage()
        {
            string source = $"{AppodealEditorConstants.PackagePath}/Runtime/Plugins/Android/appodeal.androidlib~";
            string destination = $"Assets/{AppodealEditorConstants.AppodealAndroidLibPath}";

            if (!Directory.Exists(source))
            {
                Debug.LogError($"[Appodeal] Directory not found: '{source}'. Please, contact support@apppodeal.com about this issue.");
                return false;
            }

            Directory.CreateDirectory("Assets/Plugins/Android");

            if (Directory.Exists(destination))
            {
                FileUtil.DeleteFileOrDirectory(destination);
                FileUtil.DeleteFileOrDirectory($"{destination}.meta");
            }

            FileUtil.CopyFileOrDirectory(source, destination);

            PluginPreferences.Instance.IsAndroidLibraryImported = true;
            PluginPreferences.SaveAsync();

            return true;
        }
    }
}
