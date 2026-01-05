using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal class SettingsScreenController : IDisposable
    {
        private readonly SettingsScreenView _view = new();

        public VisualElement Root => _view.Root;

        public void Dispose()
        {
            UnsubscribeFromViewEvents();
            _view?.Dispose();
        }

        public bool TryInitialize()
        {
            if (!_view.TryLoadFromTemplate())
            {
                return false;
            }

            ApplyModel();
            SubscribeToViewEvents();

            return true;
        }

        private void ApplyModel()
        {
            _view.InitValidateDependenciesPeriodicallyOption(DmChoicesScriptableObject.Instance.ValidateDependenciesPeriodically);
            _view.InitCheckForPluginUpdatesOption(DmChoicesScriptableObject.Instance.CheckPeriodicallyForPluginUpdates);
            _view.InitIncludePluginBetaVersionsOption(DmChoicesScriptableObject.Instance.IncludePluginBetaVersions);
            _view.InitEnableLoggingOption(DmChoicesScriptableObject.Instance.EnableVerboseLogging);
        }

        private void SubscribeToViewEvents()
        {
            _view.ValidateDependenciesPeriodicallyChanged += OnValidateDependenciesPeriodicallyChanged;
            _view.CheckForPluginUpdatesChanged += OnCheckForPluginUpdatesChanged;
            _view.IncludePluginBetaVersionsChanged += OnIncludePluginBetaVersionsChanged;
            _view.EnableLoggingChanged += OnEnableLoggingChanged;
            _view.OpenDocumentationClicked += OnOpenDocumentationClicked;
        }

        private void UnsubscribeFromViewEvents()
        {
            _view.ValidateDependenciesPeriodicallyChanged -= OnValidateDependenciesPeriodicallyChanged;
            _view.CheckForPluginUpdatesChanged -= OnCheckForPluginUpdatesChanged;
            _view.IncludePluginBetaVersionsChanged -= OnIncludePluginBetaVersionsChanged;
            _view.EnableLoggingChanged -= OnEnableLoggingChanged;
            _view.OpenDocumentationClicked -= OnOpenDocumentationClicked;
        }

        private static void OnValidateDependenciesPeriodicallyChanged(object sender, ToggleChangedEventArgs e)
        {
            DmChoicesScriptableObject.Instance.ValidateDependenciesPeriodically = e.NewValue;
        }

        private static void OnCheckForPluginUpdatesChanged(object sender, ToggleChangedEventArgs e)
        {
            DmChoicesScriptableObject.Instance.CheckPeriodicallyForPluginUpdates = e.NewValue;
        }

        private static void OnIncludePluginBetaVersionsChanged(object sender, ToggleChangedEventArgs e)
        {
            DmChoicesScriptableObject.Instance.IncludePluginBetaVersions = e.NewValue;
        }

        private static void OnEnableLoggingChanged(object sender, ToggleChangedEventArgs e)
        {
            DmChoicesScriptableObject.Instance.EnableVerboseLogging = e.NewValue;
        }

        private static void OnOpenDocumentationClicked(object sender, EventArgs e)
        {
            Application.OpenURL(DmConstants.UI.DocumentationUrl);
        }
    }
}
