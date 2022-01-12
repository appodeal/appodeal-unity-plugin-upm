using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using AppodealAds.Unity.Editor.AppodealManager;
using AppodealAds.Unity.Editor.InternalResources;

namespace ExternalDependencyManager.Unity.Editor.Installer
{
    static class ExternalDependencyManagerInstaller
    {
        [InitializeOnLoadMethod]
        static void InstallPluginIfUserAgreed()
        {
            if (IsPluginInstalled() || AppodealPreferences.Instance.ShouldIgnoreEDMInstallation)
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
                    AppodealPreferences.Instance.ShouldIgnoreEDMInstallation = true;
                    AppodealPreferences.Instance.SaveAsync();
                    return false;
                default:
                    return false;
            }
        }

        static void InstallPlugin()
        {
            var path = Path.Combine(AppodealDependencyUtils.Package_path,
                AppodealDependencyUtils.EDMPackagePath, AppodealDependencyUtils.EDMPackageName);
            var fullPath = Path.GetFullPath(path);
            AssetDatabase.ImportPackage(fullPath, false);
        }
    }
}
