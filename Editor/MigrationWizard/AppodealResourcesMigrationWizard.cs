// ReSharper disable CheckNamespace

using UnityEditor;
using AppodealInc.Mediation.Utils.Editor;

namespace AppodealInc.Mediation.MigrationWizard.Editor
{
    internal class AppodealResourcesMigrationWizard : AssetPostprocessor
    {
        private const string OldDirPath = "Assets/Appodeal/Editor/InternalResources";
        private const string NewDirPath = AppodealEditorConstants.EditorResourcesDir;

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
        {
            if (!AssetDatabase.IsValidFolder(OldDirPath)) return;

            try
            {
                AssetDatabase.StartAssetEditing();

                if (AssetDatabase.IsValidFolder(NewDirPath))
                {
                    AssetDatabase.DeleteAsset(NewDirPath);
                }

                if (!AssetDatabase.IsValidFolder(AppodealEditorConstants.EditorResourcesRootDir))
                {
                    AssetDatabase.CreateFolder(AppodealEditorConstants.PluginEditorDir, AppodealEditorConstants.ResourcesDirName);
                }

                AssetDatabase.MoveAsset(OldDirPath, NewDirPath);
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
                AssetDatabase.Refresh();
            }
        }
    }
}
