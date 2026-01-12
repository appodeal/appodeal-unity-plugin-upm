using System;
using UnityEngine.UIElements;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal class DropdownButtonController
    {
        private readonly DropdownButtonView _view = new();
        private bool _isExpanded;

        public event EventHandler Clicked;

        public VisualElement Root => _view.Root;

        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                _isExpanded = value;
                _view.SetExpanded(value);
            }
        }

        public bool IsEnabled
        {
            get => _view.IsEnabled;
            set => _view.IsEnabled = value;
        }

        public bool TryInitialize()
        {
            if (!_view.TryLoadFromTemplate())
            {
                return false;
            }

            _view.Clicked += OnViewClicked;
            return true;
        }

        public void SetLabelText(string labelText)
        {
            _view?.SetLabelText(labelText);
        }

        public void SetWarningStyle(bool enabled)
        {
            _view?.SetWarningStyle(enabled);
        }

        public void SetLockedStyle(bool locked)
        {
            _view?.SetLockedStyle(locked);
        }

        public void SetTooltip(string message)
        {
            _view?.SetTooltip(message);
        }

        private void OnViewClicked(object sender, EventArgs e)
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
