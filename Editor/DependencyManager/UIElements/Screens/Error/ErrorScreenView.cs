using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal class ErrorScreenView : IDisposable
    {
        private const string TemplatePath = "Appodeal/DependencyManager/Screens/ErrorScreen";

        private Button _reloadButton;
        private Button _contactSupportButton;

        public event EventHandler ReloadClicked;
        public event EventHandler ContactSupportClicked;

        public VisualElement Root { get; private set; }

        public void Dispose() => UnsubscribeFromEvents();

        public bool TryLoadFromTemplate()
        {
            var template = Resources.Load<VisualTreeAsset>(TemplatePath);
            if (template == null)
            {
                LogHelper.LogError($"[ErrorScreenView] Failed to load template from Resources/{TemplatePath}");
                return false;
            }

            Root = template.Instantiate();

            _reloadButton = Root.Q<Button>(DmConstants.Uxml.ErrorScreen.ReloadButton);
            _contactSupportButton = Root.Q<Button>(DmConstants.Uxml.ErrorScreen.ContactSupportButton);

            if (_reloadButton == null || _contactSupportButton == null)
            {
                LogHelper.LogError("[ErrorScreenView] Failed to find required elements in template");
                return false;
            }

            SubscribeToEvents();

            return true;
        }

        private void SubscribeToEvents()
        {
            _reloadButton.clicked += OnReloadClicked;
            _contactSupportButton.clicked += OnContactSupportClicked;
        }

        private void UnsubscribeFromEvents()
        {
            if (_reloadButton != null) _reloadButton.clicked -= OnReloadClicked;
            if (_contactSupportButton != null) _contactSupportButton.clicked -= OnContactSupportClicked;
        }

        private void OnReloadClicked()
        {
            ReloadClicked?.Invoke(this, EventArgs.Empty);
        }

        private void OnContactSupportClicked()
        {
            ContactSupportClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
