using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal class DropdownButtonView
    {
        private const string TemplatePath = "Appodeal/DependencyManager/SdkVersionDropdown/SdkVersionDropdownButton";

        private Button _button;
        private Label _label;
        private VisualElement _icon;

        public event EventHandler Clicked;

        public VisualElement Root { get; private set; }

        public bool IsEnabled
        {
            get => _button?.enabledSelf ?? false;
            set => _button?.SetEnabled(value);
        }

        public bool TryLoadFromTemplate()
        {
            var template = Resources.Load<VisualTreeAsset>(TemplatePath);
            if (template == null)
            {
                LogHelper.LogError($"[DropdownButtonView] Failed to load template from Resources/{TemplatePath}");
                return false;
            }

            Root = template.Instantiate();

            _button = Root.Q<Button>(DmConstants.Uxml.SdkVersionDropdown.Button.ContentContainer);
            _label = _button?.Q<Label>(DmConstants.Uxml.SdkVersionDropdown.Button.Label);
            _icon = _button?.Q<VisualElement>(DmConstants.Uxml.SdkVersionDropdown.Button.Icon);

            if (_button == null || _label == null || _icon == null)
            {
                LogHelper.LogError("[DropdownButtonView] Failed to find required elements in template");
                return false;
            }

            _button.clicked += OnButtonClicked;

            return true;
        }

        public void SetLabelText(string labelText)
        {
            if (_label != null) _label.text = labelText;
        }

        public void SetExpanded(bool expanded)
        {
            _icon?.EnableInClassList(DmConstants.Uss.SdkVersionDropdown.Button.IconExpandedModifier, expanded);
        }

        public void SetWarningStyle(bool enabled)
        {
            _button?.EnableInClassList(DmConstants.Uss.SdkVersionDropdown.Button.WarningModifier, enabled);
        }

        public void SetLockedStyle(bool locked)
        {
            _button?.EnableInClassList(DmConstants.Uss.SdkVersionDropdown.Button.LockedModifier, locked);
        }

        public void SetTooltip(string message)
        {
            if (_button != null)
            {
                _button.tooltip = message;
            }
        }

        private void OnButtonClicked()
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
