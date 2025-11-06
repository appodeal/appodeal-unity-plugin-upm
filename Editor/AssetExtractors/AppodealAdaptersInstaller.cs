// ReSharper disable CheckNamespace

using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using AppodealInc.Mediation.Utils.Editor;

namespace AppodealInc.Mediation.AssetExtractors.Editor
{
    internal static class AppodealAdaptersInstaller
    {
        public static bool InstallAdapters()
        {
            if (File.Exists(AppodealEditorConstants.DependenciesFilePath)) return false;

            try
            {
                var depsFileInfo = new FileInfo(AppodealEditorConstants.BundledDependenciesFilePath);
                if (!depsFileInfo.Exists)
                {
                    Debug.LogError($"[Appodeal] File was not found: '{depsFileInfo.FullName}'. Please, contact support@apppodeal.com about this issue.");
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
