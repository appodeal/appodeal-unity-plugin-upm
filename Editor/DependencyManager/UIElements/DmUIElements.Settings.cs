using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

// ReSharper disable once CheckNamespace
namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal static partial class DmUIElements
    {
        private static VisualTreeAsset _settingsVta;

        public static bool IsSettingsAssetLoadable() => GetSettingsAsset() != null;

        public static VisualElement CreateSettingsUI()
        {
            VisualElement settings = GetSettingsAsset().Instantiate();

            var checkForAdapterUpdatesToggle = settings.Q<Toggle>(DmConstants.Uxml.Settings.CheckForAdapterUpdatesToggle);
            checkForAdapterUpdatesToggle.value = DmChoicesScriptableObject.Instance.CheckPeriodicallyForAdapterUpdates;
            checkForAdapterUpdatesToggle.RegisterValueChangedCallback(evt => { DmChoicesScriptableObject.Instance.CheckPeriodicallyForAdapterUpdates = evt.newValue; });

            var updateAdaptersToggle = settings.Q<Toggle>(DmConstants.Uxml.Settings.UpdateAdaptersAutomaticallyToggle);
            updateAdaptersToggle.value = DmChoicesScriptableObject.Instance.UpdateAdaptersAutomatically;
            updateAdaptersToggle.RegisterValueChangedCallback(evt => { DmChoicesScriptableObject.Instance.UpdateAdaptersAutomatically = evt.newValue; });

            var checkForPluginUpdatesToggle = settings.Q<Toggle>(DmConstants.Uxml.Settings.CheckForPluginUpdatesToggle);
            checkForPluginUpdatesToggle.value = DmChoicesScriptableObject.Instance.CheckPeriodicallyForPluginUpdates;
            checkForPluginUpdatesToggle.RegisterValueChangedCallback(evt => { DmChoicesScriptableObject.Instance.CheckPeriodicallyForPluginUpdates = evt.newValue; });

            var includePluginBetVersionsToggle = settings.Q<Toggle>(DmConstants.Uxml.Settings.IncludePluginBetaVersionsToggle);
            includePluginBetVersionsToggle.value = DmChoicesScriptableObject.Instance.IncludePluginBetaVersions;
            includePluginBetVersionsToggle.RegisterValueChangedCallback(evt => { DmChoicesScriptableObject.Instance.IncludePluginBetaVersions = evt.newValue; });

            var enableLoggingToggle = settings.Q<Toggle>(DmConstants.Uxml.Settings.EnableLoggingToggle);
            enableLoggingToggle.value = DmChoicesScriptableObject.Instance.EnableVerboseLogging;
            enableLoggingToggle.RegisterValueChangedCallback(evt => { DmChoicesScriptableObject.Instance.EnableVerboseLogging = evt.newValue; });

            settings.Q<Button>(DmConstants.Uxml.Settings.OpenDocumentationButton).clicked += () => { Application.OpenURL(DmConstants.DocumentationUrl); };

            return settings;
        }

        private static VisualTreeAsset GetSettingsAsset()
        {
            return _settingsVta ??= AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(DmConstants.SettingsUxmlPath);
        }
    }
}
