using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal class ErrorScreenController : IDisposable
    {
        private readonly ErrorScreenView _view = new();

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

            SubscribeToViewEvents();

            return true;
        }

        private void SubscribeToViewEvents()
        {
            _view.ReloadClicked += OnReloadClicked;
            _view.ContactSupportClicked += OnContactSupportClicked;
        }

        private void UnsubscribeFromViewEvents()
        {
            _view.ReloadClicked -= OnReloadClicked;
            _view.ContactSupportClicked -= OnContactSupportClicked;
        }

        private static void OnReloadClicked(object sender, EventArgs e)
        {
            SettingsService.OpenProjectSettings($"Project/{DmConstants.UI.WindowName}");
        }

        private static void OnContactSupportClicked(object sender, EventArgs e)
        {
            Application.OpenURL(DmConstants.UI.ContactSupportUrl);
        }
    }
}
