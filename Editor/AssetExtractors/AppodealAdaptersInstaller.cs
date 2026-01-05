// ReSharper disable CheckNamespace

using System;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using AppodealInc.Mediation.DependencyManager.Editor;
using AppodealInc.Mediation.Utils.Editor;

namespace AppodealInc.Mediation.AssetExtractors.Editor
{
    internal static class AppodealAdaptersInstaller
    {
        internal static async Task<bool> InstallAdapters()
        {
            try
            {
                if (File.Exists(AppodealEditorConstants.DependenciesFilePath))
                {
                    if (AppodealUnityUtils.IsDevModeEnabled) return false;

                    bool versionMatches = await VersionComparer.IsLocalDependenciesVersionMatchingPackageVersionAsync();
                    if (versionMatches) return false;
                }

                var depsFileInfo = new FileInfo(AppodealEditorConstants.BundledDependenciesFilePath);
                if (!depsFileInfo.Exists)
                {
                    Debug.LogError($"[Appodeal] File was not found: '{depsFileInfo.FullName}'. Please, contact support@appodeal.com about this issue");
                    return false;
                }

                Directory.CreateDirectory(AppodealEditorConstants.DependenciesDir);
                FileUtil.ReplaceFile(depsFileInfo.FullName, AppodealEditorConstants.DependenciesFilePath);

                return true;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return false;
            }
        }
    }
}
