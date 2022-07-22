using UnityEditor;
#if UNITY_2020_3_OR_NEWER
using AppodealStack.UnityEditor.InternalResources;
#endif

// ReSharper Disable CheckNamespace
namespace AppodealStack.UnityEditor.AssetExtractors
{
    internal class ApdAssetPostprocessor : AssetPostprocessor
    {
#if UNITY_2021_3_OR_NEWER
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
#else
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
#endif
        {
            if (AndroidLibraryInstaller.InstallAndroidLibrary() | AppodealAdaptersInstaller.InstallAdapters() | ExternalDependencyManagerInstaller.InstallPluginIfUserAgrees())
            {
                AssetDatabase.Refresh();
            }
#if UNITY_2020_3_OR_NEWER
            AssetDatabase.SaveAssetIfDirty(PluginPreferences.Instance);
#endif
        }
    }
}
