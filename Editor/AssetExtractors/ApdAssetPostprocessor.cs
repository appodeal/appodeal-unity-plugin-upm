// ReSharper disable CheckNamespace

using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using AppodealInc.Mediation.Utils.Editor;

namespace AppodealInc.Mediation.AssetExtractors.Editor
{
    internal class ApdAssetPostprocessor : AssetPostprocessor
    {
        private static readonly object InstallLock = new();
        private static bool _isInstalling;

        private static async void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
        {
            try
            {
                if (deletedAssets.Any(asset => asset.Contains(AppodealEditorConstants.PackageDir))) return;

                lock (InstallLock)
                {
                    if (_isInstalling) return;
                    _isInstalling = true;
                }

                bool adaptersInstalled = await AppodealAdaptersInstaller.InstallAdapters();
                bool androidLibInstalled = AndroidLibraryInstaller.InstallAndroidLibrary(forceReinstall: adaptersInstalled);

                if (androidLibInstalled || adaptersInstalled)
                {
                    AssetDatabase.Refresh();
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            finally
            {
                lock (InstallLock)
                {
                    _isInstalling = false;
                }
            }
        }
    }
}
