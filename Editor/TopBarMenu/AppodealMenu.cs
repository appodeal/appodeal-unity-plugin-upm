// ReSharper disable CheckNamespace

using System;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using AppodealInc.Mediation.Analytics.Editor;
using AppodealInc.Mediation.DependencyManager.Editor;
using AppodealInc.Mediation.PluginRemover.Editor;
using AppodealInc.Mediation.SettingsWindow.Editor;
using AppodealInc.Mediation.Utils.Editor;

namespace AppodealInc.Mediation.TopBarMenu.Editor
{
    public class AppodealMenu : ScriptableObject
    {
#if APPODEAL_DEV
        [MenuItem("Appodeal/Create Tarball", false, 0)]
        public static void CreateTarball()
        {
            Client.Pack("Packages/com.appodeal.mediation", "~/Downloads/Appodeal/");
        }
#endif

        [MenuItem("Appodeal/Plugin Documentation", false, 1)]
        public static void OpenDocumentation()
        {
            AnalyticsService.TrackClickEvent(ActionType.OpenDocumentation);
            Application.OpenURL("https://docs.appodeal.com/unity/get-started");
        }

        [MenuItem("Appodeal/Appodeal Homepage", false, 2)]
        public static void OpenAppodealHomepage()
        {
            Application.OpenURL("https://appodeal.com/");
        }

        [MenuItem("Appodeal/Dependency Manager/Open &d", false, 13)]
        public static void AppodealDependencyManager()
        {
            SettingsService.OpenProjectSettings($"Project/{AppodealEditorConstants.DependencyManagerWindowName}");
        }

        [MenuItem("Appodeal/Dependency Manager/Validate Dependencies &v", false, 14)]
        public static async void ValidateDependencies()
        {
            try
            {
                await DependencyValidationService.ValidateAsync(isManual: true);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Appodeal] Failed to validate dependencies: {ex.Message}");
                EditorUtility.DisplayDialog(DmConstants.UI.DialogTitle, $"An unexpected error occurred:\n{ex.Message}", "OK");
            }
        }

        [MenuItem("Appodeal/Dependency Manager/Settings", false, 15)]
        public static void DependencyManagerSettings()
        {
            SettingsService.OpenProjectSettings($"Project/{AppodealEditorConstants.DependencyManagerWindowName}/Settings");
        }

        [MenuItem("Appodeal/Appodeal Settings &s", false, 16)]
        public static void AppodealSettings()
        {
            AppodealSettingsWindow.ShowAppodealSettingsWindow();
        }

        [MenuItem("Appodeal/Remove Plugin", false, 27)]
        public static void RemoveAppodealPlugin()
        {
            RemoveHelper.RemovePlugin();
        }
    }
}
