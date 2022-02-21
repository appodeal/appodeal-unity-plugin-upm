using System.IO;
using System.Linq;
using UnityEditor;
using AppodealStack.UnityEditor.Utils;
using AppodealStack.UnityEditor.InternalResources;

namespace AppodealStack.UnityEditor.AssetExtractors
{
    static class NetworkConfigsInstaller
    {
        [InitializeOnLoadMethod]
        static void InstallNetworkConfigs()
        {
            if (PluginPreferences.Instance.AreNetworkConfigsImported) return;
            ImportConfigsFromPackage();
        }

        private static void ImportConfigsFromPackage() {
            var info = new DirectoryInfo(Path.Combine(AppodealEditorConstants.PackagePath, AppodealEditorConstants.NetworkDepsPath));
            var fileInfo = info.GetFiles();
            fileInfo = fileInfo.Length <= 0 ? null : fileInfo.Where(val => !val.Name.Contains("meta") && val.Name.Contains("Dependencies")).ToArray();

            if (!Directory.Exists(Path.Combine(AppodealEditorConstants.PluginPath, AppodealEditorConstants.NetworkDepsPath)))
            {
                Directory.CreateDirectory(Path.Combine(AppodealEditorConstants.PluginPath, AppodealEditorConstants.NetworkDepsPath) ?? string.Empty);
            }
            fileInfo.ToList().ForEach(file => FileUtil.ReplaceFile(file.FullName, Path.Combine(AppodealEditorConstants.PluginPath, AppodealEditorConstants.NetworkDepsPath, file.Name.Replace(".txt", ".xml"))));

            PluginPreferences.Instance.AreNetworkConfigsImported = true;
            PluginPreferences.Instance.SaveAsync();
            AssetDatabase.Refresh();
        }
    }
}
