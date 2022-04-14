using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using AppodealStack.UnityEditor.Utils;
using AppodealStack.UnityEditor.InternalResources;

// ReSharper Disable CheckNamespace
namespace AppodealStack.UnityEditor.AssetExtractors
{
    static class ExternalDependencyManagerInstaller
    {
        [InitializeOnLoadMethod]
        static void InstallPluginIfUserAgrees()
        {
            if (IsPluginInstalled() || PluginPreferences.Instance.ShouldIgnoreEdmInstallation) return;

            if (PluginInstallationRequest())
            {
                InstallPlugin();
            }
        }

        private static bool IsPluginInstalled()
        {
            try
            {
                return Type.GetType("Google.VersionHandler, Google.VersionHandler") != null;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return false;
            }
        }

        private static bool PluginInstallationRequest()
        {
            int decision = EditorUtility.DisplayDialogComplex("External Dependency Manager Required",
                "Appodeal uses External Dependency Manager to resolve dependencies.\n\nWould you like to import the package?",
                "Import", "Cancel", "Ignore - Do not ask anymore");

            switch (decision)
            {
                case 0:
                    return true;
                case 1:
                    return false;
                case 2:
                    PluginPreferences.Instance.ShouldIgnoreEdmInstallation = true;
                    PluginPreferences.Instance.SaveAsync();
                    return false;
                default:
                    return false;
            }
        }

        static void InstallPlugin()
        {
            string path = Path.Combine(AppodealEditorConstants.PackagePath, AppodealEditorConstants.EdmPackagePath);

            var fileInfo = new DirectoryInfo(path).GetFiles("*.unitypackage");
            
            if (fileInfo.Length > 0)
            {
                AssetDatabase.ImportPackage(fileInfo[0].FullName, false);
            }
        }
    }
}
