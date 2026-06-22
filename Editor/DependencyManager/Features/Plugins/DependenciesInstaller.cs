// ReSharper disable CheckNamespace

using System;
using System.IO;
using UnityEditor;
using AppodealInc.Mediation.Utils.Editor;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal static class DependenciesInstaller
    {
        internal static bool EnsureDependenciesXmlFile()
        {
            try
            {
                if (File.Exists(AppodealEditorConstants.DependenciesFilePath))
                {
                    bool versionMatches = VersionComparer.IsLocalDependenciesVersionMatchingPackageVersion();
                    if (versionMatches) return false;
                }

                var depsFileInfo = new FileInfo(AppodealEditorConstants.BundledDependenciesFilePath);
                if (!depsFileInfo.Exists)
                {
                    LogHelper.LogError($"File was not found: '{depsFileInfo.FullName}'. Please, contact support@appodeal.com about this issue");
                    return false;
                }

                Directory.CreateDirectory(AppodealEditorConstants.DependenciesDir);
                FileUtil.ReplaceFile(depsFileInfo.FullName, AppodealEditorConstants.DependenciesFilePath);

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex);
                return false;
            }
        }
    }
}
