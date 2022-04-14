using System.IO;
using System.Linq;
using UnityEditor;
using AppodealStack.UnityEditor.Utils;
using AppodealStack.UnityEditor.InternalResources;

// ReSharper Disable CheckNamespace
namespace AppodealStack.UnityEditor.AssetExtractors
{
    static class AppodealConfigsInstaller
    {
        [InitializeOnLoadMethod]
        static void InstallAppodealConfigs()
        {
            if (PluginPreferences.Instance.AreNetworkConfigsImported) return;
            ImportConfigsFromPackage();
        }

        private static void ImportConfigsFromPackage()
        {
            var info = new DirectoryInfo(Path.Combine(AppodealEditorConstants.PackagePath, AppodealEditorConstants.DependenciesPath));
            var fileInfo = info.GetFiles("*Dependencies.txt", SearchOption.AllDirectories);

            if (!Directory.Exists(Path.Combine(AppodealEditorConstants.PluginPath, AppodealEditorConstants.DependenciesPath)))
            {
                Directory.CreateDirectory(Path.Combine(AppodealEditorConstants.PluginPath, AppodealEditorConstants.DependenciesPath));
            }
            fileInfo.ToList().ForEach(file => FileUtil.ReplaceFile(file.FullName, Path.Combine(AppodealEditorConstants.PluginPath, AppodealEditorConstants.DependenciesPath, file.Name.Replace(".txt", ".xml"))));

            PluginPreferences.Instance.AreNetworkConfigsImported = true;
            PluginPreferences.Instance.SaveAsync();
            AssetDatabase.Refresh();
        }
    }
}
