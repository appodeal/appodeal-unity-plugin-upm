using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEditor;
using AppodealStack.UnityEditor.Utils;

// ReSharper Disable once CheckNamespace
namespace AppodealStack.UnityEditor.AssetExtractors
{
    internal class ApdAssetPostprocessor : AssetPostprocessor
    {
        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        [SuppressMessage("ReSharper", "Unity.IncorrectMethodSignature")]
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
        {
#if APPODEAL_DEV
            return;
#endif
            if (deletedAssets.Any(asset => asset.Contains(AppodealEditorConstants.PackageDir))) return;

            if (AndroidLibraryInstaller.InstallAndroidLibrary() | AppodealAdaptersInstaller.InstallAdapters())
            {
                AssetDatabase.Refresh();
            }
        }
    }
}
