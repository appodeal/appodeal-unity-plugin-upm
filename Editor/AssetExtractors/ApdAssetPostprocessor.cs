using UnityEditor;
using AppodealStack.UnityEditor.InternalResources;

// ReSharper Disable CheckNamespace
namespace AppodealStack.UnityEditor.AssetExtractors
{
    internal class ApdAssetPostprocessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
        {
            if (AndroidLibraryInstaller.InstallAndroidLibrary() | AppodealAdaptersInstaller.InstallAdapters())
            {
                AssetDatabase.Refresh();
            }

            AssetDatabase.SaveAssetIfDirty(PluginPreferences.Instance);
        }
    }
}
