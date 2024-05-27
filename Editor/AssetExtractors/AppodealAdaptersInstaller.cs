using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using AppodealStack.UnityEditor.Utils;

// ReSharper Disable once CheckNamespace
namespace AppodealStack.UnityEditor.AssetExtractors
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
