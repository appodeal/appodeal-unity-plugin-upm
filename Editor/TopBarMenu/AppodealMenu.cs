using UnityEditor;
using UnityEngine;
using AppodealInc.Mediation.Analytics.Editor;
using AppodealInc.Mediation.DependencyManager.Editor;
using AppodealInc.Mediation.PluginRemover.Editor;
using AppodealInc.Mediation.SettingsWindow.Editor;

namespace AppodealInc.Mediation.TopBarMenu.Editor
{
    internal static class AppodealMenu
    {
        private const string Root = "Appodeal/";
        private const string DependencyManagerRoot = Root + "Dependency Manager/";

        // Unity draws a separator between items whose priorities differ by more than 10
        private const int LinksGroup = 1;
        private const int ToolsGroup = 13;
        private const int RemovalGroup = 27;

        [MenuItem(Root + "Plugin Documentation", false, LinksGroup)]
        public static void OpenDocumentation()
        {
            AnalyticsService.TrackClickEvent(ActionType.OpenDocumentation);
            Application.OpenURL("https://docs.appodeal.com/unity/get-started");
        }

        [MenuItem(Root + "Appodeal Homepage", false, LinksGroup + 1)]
        public static void OpenAppodealHomepage()
        {
            Application.OpenURL("https://appodeal.com/");
        }

        [MenuItem(DependencyManagerRoot + "Open &d", false, ToolsGroup)]
        public static void AppodealDependencyManager()
        {
            DmSettingsProvider.Open();
        }

        [MenuItem(DependencyManagerRoot + "Validate Dependencies &v", false, ToolsGroup + 1)]
        public static void ValidateDependencies()
        {
            DependencyValidationService.ValidateFromMenu();
        }

        [MenuItem(DependencyManagerRoot + "Settings", false, ToolsGroup + 2)]
        public static void DependencyManagerSettings()
        {
            DmChoicesSettingsProvider.Open();
        }

        [MenuItem(Root + "Appodeal Settings &s", false, ToolsGroup + 3)]
        public static void AppodealSettings()
        {
            AppodealSettingsWindow.ShowAppodealSettingsWindow();
        }

        [MenuItem(Root + "Remove Plugin", false, RemovalGroup)]
        public static void RemoveAppodealPlugin()
        {
            RemoveHelper.RemovePlugin();
        }
    }
}
