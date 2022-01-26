using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using AppodealAds.Unity.Editor.Utils;
using AppodealAds.Unity.Editor.InternalResources;

namespace AppodealAds.Unity.Editor.AssetsExtractor
{
    static class ExternalDependencyManagerInstaller
    {
        [InitializeOnLoadMethod]
        static void InstallPluginIfUserAgreed()
        {
            if (IsPluginInstalled() || PluginPreferences.Instance.ShouldIgnoreEDMInstallation)
                return;

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
            var decision = EditorUtility.DisplayDialogComplex("External Dependency Manager required",
                "Appodeal requires External Dependency Manager to resolve dependencies.\n" +
                " Would you like to import the package?",
                "Import", "Cancel", "Ignore - Do not ask anymore");

            switch (decision)
            {
                case 0:
                    return true;
                case 1:
                    return false;
                case 2:
                    PluginPreferences.Instance.ShouldIgnoreEDMInstallation = true;
                    PluginPreferences.Instance.SaveAsync();
                    return false;
                default:
                    return false;
            }
        }

        static void InstallPlugin()
        {
            var path = Path.Combine(AppodealEditorConstants.PackagePath,
                AppodealEditorConstants.EDMPackagePath, AppodealEditorConstants.EDMPackageName);
            var fullPath = Path.GetFullPath(path);
            AssetDatabase.ImportPackage(fullPath, false);
        }
    }
}
