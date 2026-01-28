using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal class SettingsScreenView : IDisposable
    {
        private const string TemplatePath = "Appodeal/DependencyManager/Screens/SettingsScreen";

        private Toggle _validateDependenciesPeriodicallyToggle;
        private Toggle _checkForPluginUpdatesToggle;
        private Toggle _includePluginBetaVersionsToggle;
        private Toggle _enableLoggingToggle;
        private Button _openDocumentationButton;

        public event EventHandler<ToggleChangedEventArgs> ValidateDependenciesPeriodicallyChanged;
        public event EventHandler<ToggleChangedEventArgs> CheckForPluginUpdatesChanged;
        public event EventHandler<ToggleChangedEventArgs> IncludePluginBetaVersionsChanged;
        public event EventHandler<ToggleChangedEventArgs> EnableLoggingChanged;
        public event EventHandler OpenDocumentationClicked;

        public VisualElement Root { get; private set; }

        public void Dispose() => UnsubscribeFromEvents();

        public bool TryLoadFromTemplate()
        {
            var template = Resources.Load<VisualTreeAsset>(TemplatePath);
            if (template == null)
            {
                LogHelper.LogError($"[SettingsScreenView] Failed to load template from Resources/{TemplatePath}");
                return false;
            }

            Root = template.Instantiate();

            _validateDependenciesPeriodicallyToggle = Root.Q<Toggle>(DmConstants.Uxml.SettingsScreen.ValidateDependenciesPeriodicallyToggle);
            _checkForPluginUpdatesToggle = Root.Q<Toggle>(DmConstants.Uxml.SettingsScreen.CheckForPluginUpdatesToggle);
            _includePluginBetaVersionsToggle = Root.Q<Toggle>(DmConstants.Uxml.SettingsScreen.IncludePluginBetaVersionsToggle);
            _enableLoggingToggle = Root.Q<Toggle>(DmConstants.Uxml.SettingsScreen.EnableLoggingToggle);
            _openDocumentationButton = Root.Q<Button>(DmConstants.Uxml.SettingsScreen.OpenDocumentationButton);

            if (_validateDependenciesPeriodicallyToggle == null ||
                _checkForPluginUpdatesToggle == null || _includePluginBetaVersionsToggle == null ||
                _enableLoggingToggle == null || _openDocumentationButton == null)
            {
                LogHelper.LogError("[SettingsScreenView] Failed to find required elements in template");
                return false;
            }

            SubscribeToEvents();

            return true;
        }

        public void InitValidateDependenciesPeriodicallyOption(bool value) => _validateDependenciesPeriodicallyToggle?.SetValueWithoutNotify(value);

        public void InitCheckForPluginUpdatesOption(bool value) => _checkForPluginUpdatesToggle?.SetValueWithoutNotify(value);

        public void InitIncludePluginBetaVersionsOption(bool value) => _includePluginBetaVersionsToggle?.SetValueWithoutNotify(value);

        public void InitEnableLoggingOption(bool value) => _enableLoggingToggle?.SetValueWithoutNotify(value);

        private void SubscribeToEvents()
        {
            _validateDependenciesPeriodicallyToggle.RegisterCallback<ChangeEvent<bool>>(OnValidateDependenciesPeriodicallyChanged);
            _checkForPluginUpdatesToggle.RegisterCallback<ChangeEvent<bool>>(OnCheckForPluginUpdatesChanged);
            _includePluginBetaVersionsToggle.RegisterCallback<ChangeEvent<bool>>(OnIncludePluginBetaVersionsChanged);
            _enableLoggingToggle.RegisterCallback<ChangeEvent<bool>>(OnEnableLoggingChanged);
            _openDocumentationButton.clicked += OnOpenDocumentationClicked;
        }

        private void UnsubscribeFromEvents()
        {
            _validateDependenciesPeriodicallyToggle?.UnregisterCallback<ChangeEvent<bool>>(OnValidateDependenciesPeriodicallyChanged);
            _checkForPluginUpdatesToggle?.UnregisterCallback<ChangeEvent<bool>>(OnCheckForPluginUpdatesChanged);
            _includePluginBetaVersionsToggle?.UnregisterCallback<ChangeEvent<bool>>(OnIncludePluginBetaVersionsChanged);
            _enableLoggingToggle?.UnregisterCallback<ChangeEvent<bool>>(OnEnableLoggingChanged);
            if (_openDocumentationButton != null) _openDocumentationButton.clicked -= OnOpenDocumentationClicked;
        }

        private void OnValidateDependenciesPeriodicallyChanged(ChangeEvent<bool> evt)
        {
            ValidateDependenciesPeriodicallyChanged?.Invoke(this, new ToggleChangedEventArgs(evt.newValue));
        }

        private void OnCheckForPluginUpdatesChanged(ChangeEvent<bool> evt)
        {
            CheckForPluginUpdatesChanged?.Invoke(this, new ToggleChangedEventArgs(evt.newValue));
        }

        private void OnIncludePluginBetaVersionsChanged(ChangeEvent<bool> evt)
        {
            IncludePluginBetaVersionsChanged?.Invoke(this, new ToggleChangedEventArgs(evt.newValue));
        }

        private void OnEnableLoggingChanged(ChangeEvent<bool> evt)
        {
            EnableLoggingChanged?.Invoke(this, new ToggleChangedEventArgs(evt.newValue));
        }

        private void OnOpenDocumentationClicked()
        {
            OpenDocumentationClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
