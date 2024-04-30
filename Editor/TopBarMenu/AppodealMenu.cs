using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager;
using AppodealStack.UnityEditor.SDKManager;
using AppodealStack.UnityEditor.PluginRemover;
using AppodealStack.UnityEditor.SettingsWindow;

// ReSharper Disable CheckNamespace
namespace AppodealStack.UnityEditor.TopBarMenu
{
    public class AppodealMenu : ScriptableObject
    {
        [MenuItem("Appodeal/Plugin Documentation")]
        public static void OpenDocumentation()
        {
            Application.OpenURL("https://docs.appodeal.com/unity/get-started");
        }

        [MenuItem("Appodeal/Appodeal Homepage")]
        public static void OpenAppodealHome()
        {
            Application.OpenURL("https://appodeal.com/");
        }

        // [MenuItem("Appodeal/Plugin Configuration")]
        public static void AppodealSdkManager()
        {
            AppodealAdapterManager.ShowSdkManager();
        }

        [MenuItem("Appodeal/Appodeal Settings")]
        public static void AppodealSettings()
        {
            AppodealSettingsWindow.ShowAppodealSettingsWindow();
        }

        [MenuItem("Appodeal/Remove Plugin")]
        public static void RemoveAppodealPlugin()
        {
            bool decision = RemoveHelper.RemovePlugin();
            if (decision)
            {
                Client.Remove("com.appodeal.mediation");
            }
        }
    }
}
