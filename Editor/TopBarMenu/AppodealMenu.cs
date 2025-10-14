// ReSharper disable CheckNamespace

using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using AppodealInc.Mediation.Analytics.Editor;
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
            Client.Pack($"{Application.dataPath}/com.appodeal.mediation", "/Users/fdn/Downloads/Appodeal/");
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

        [MenuItem("Appodeal/Dependency Manager &d", false, 13)]
        public static void AppodealDependencyManager()
        {
            SettingsService.OpenProjectSettings($"Project/{AppodealEditorConstants.DependencyManagerWindowName}");
        }

        [MenuItem("Appodeal/Appodeal Settings &s", false, 14)]
        public static void AppodealSettings()
        {
            AppodealSettingsWindow.ShowAppodealSettingsWindow();
        }

        [MenuItem("Appodeal/Remove Plugin", false, 25)]
        public static void RemoveAppodealPlugin()
        {
            RemoveHelper.RemovePlugin();
        }
    }
}
