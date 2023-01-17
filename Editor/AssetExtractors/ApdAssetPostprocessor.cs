using System.IO;
using UnityEditor;
using AppodealStack.UnityEditor.InternalResources;

// ReSharper Disable CheckNamespace
namespace AppodealStack.UnityEditor.AssetExtractors
{
    internal class ApdAssetPostprocessor : AssetPostprocessor
    {
#if UNITY_2021_3_OR_NEWER
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
        {
#else
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            const string prefsPath = "Assets/Appodeal/Editor/InternalResources/PluginPreferences.asset";
            if (File.Exists(prefsPath) && AssetDatabase.LoadAssetAtPath<PluginPreferences>(prefsPath) == null) return;
#endif
            if (AndroidLibraryInstaller.InstallAndroidLibrary() | AppodealAdaptersInstaller.InstallAdapters() | ExternalDependencyManagerInstaller.InstallPluginIfUserAgrees())
            {
                AssetDatabase.Refresh();
            }
#if UNITY_2020_3_16_OR_NEWER
            AssetDatabase.SaveAssetIfDirty(PluginPreferences.Instance);
#endif
        }
    }
}
