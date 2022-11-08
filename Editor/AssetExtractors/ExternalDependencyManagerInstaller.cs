using System.IO;
using UnityEditor;
using UnityEngine;
using AppodealStack.UnityEditor.Utils;
using AppodealStack.UnityEditor.InternalResources;

// ReSharper Disable CheckNamespace
namespace AppodealStack.UnityEditor.AssetExtractors
{
    internal static class ExternalDependencyManagerInstaller
    {
        private const string EdmInstallCancelledKey = "EDMInstallCancelled";

        public static bool InstallPluginIfUserAgrees()
        {
            if (PluginPreferences.Instance.IsEdmImported || PluginPreferences.Instance.ShouldIgnoreEdmInstallation || SessionState.GetBool(EdmInstallCancelledKey, false)) return false;

            return PluginInstallationRequest() && InstallPlugin();
        }

        private static bool PluginInstallationRequest()
        {
            int decision = EditorUtility.DisplayDialogComplex("EDM Installation Request",
                "Appodeal recommends to use an\nExternal Dependency Manager\nto resolve dependencies.\n\nWould you like to import this package?",
                "Import", "Cancel", "Ignore - Do not ask anymore");

            switch (decision)
            {
                case 0:
                    return true;
                case 1:
                    SessionState.SetBool(EdmInstallCancelledKey, true);
                    return false;
                case 2:
                    PluginPreferences.Instance.ShouldIgnoreEdmInstallation = true;
                    PluginPreferences.SaveAsync();
                    SessionState.SetBool(EdmInstallCancelledKey, true);
                    return false;
                default:
                    return false;
            }
        }

        private static bool InstallPlugin()
        {
            var edmDir = new DirectoryInfo($"{AppodealEditorConstants.PackagePath}/{AppodealEditorConstants.EdmPackagePath}");

            if (!edmDir.Exists)
            {
                Debug.LogError($"[Appodeal] Directory not found: '{edmDir}'. Please, contact support@apppodeal.com about this issue.");
                return false;
            }

            var edmPackages = edmDir.GetFiles("*.unitypackage");

            if (edmPackages.Length < 1)
            {
                Debug.LogError($"[Appodeal] No EDM Packages were found on path: '{edmDir}'. Please, contact support@apppodeal.com about this issue.");
                return false;
            }

            AssetDatabase.ImportPackage(edmPackages[0].FullName, false);

            PluginPreferences.Instance.IsEdmImported = true;
            PluginPreferences.SaveAsync();

            return true;
        }
    }
}
