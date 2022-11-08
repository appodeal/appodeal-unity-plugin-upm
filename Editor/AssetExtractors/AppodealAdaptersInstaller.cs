using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using AppodealStack.UnityEditor.Utils;
using AppodealStack.UnityEditor.InternalResources;

// ReSharper Disable CheckNamespace
namespace AppodealStack.UnityEditor.AssetExtractors
{
    internal static class AppodealAdaptersInstaller
    {
        public static bool InstallAdapters()
        {
            return !PluginPreferences.Instance.AreAdaptersImported && CopyAdaptersFromPackage();
        }

        private static bool CopyAdaptersFromPackage()
        {
            var depsDir = new DirectoryInfo($"{AppodealEditorConstants.PackagePath}/{AppodealEditorConstants.DependenciesPath}");
            if (!depsDir.Exists)
            {
                Debug.LogError($"[Appodeal] Directory not found: '{depsDir}'. Please, contact support@apppodeal.com about this issue.");
                return false;
            }

            var deps = depsDir.GetFiles("*Dependencies.txt", SearchOption.AllDirectories);
            if (deps.Length < 1)
            {
                Debug.LogError($"[Appodeal] No Adapters were found on path '{depsDir}'. Please, contact support@apppodeal.com about this issue.");
                return false;
            }

            string outputDir = $"{AppodealEditorConstants.PluginPath}/{AppodealEditorConstants.DependenciesPath}";

            FileUtil.DeleteFileOrDirectory(outputDir);
            Directory.CreateDirectory(outputDir);

            deps.ToList().ForEach(dep => FileUtil.ReplaceFile(dep.FullName, $"{outputDir}/{dep.Name.Replace(".txt", ".xml")}"));

            PluginPreferences.Instance.AreAdaptersImported = true;
            PluginPreferences.SaveAsync();

            return true;
        }
    }
}
