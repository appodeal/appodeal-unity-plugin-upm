using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal class DropdownItemView
    {
        private const string TemplatePath = "Appodeal/DependencyManager/SdkVersionDropdown/SdkVersionDropdownItem";

        private VisualElement _item;
        private VisualElement _selectionToggle;
        private Label _versionLabel;
        private VisualElement _badgeNew;
        private VisualElement _badgeRecommended;
        private VisualElement _badgeDeprecated;
        private VisualElement _badgeUnstable;

        public event EventHandler Clicked;

        public VisualElement Root { get; private set; }

        public bool TryLoadFromTemplate()
        {
            var template = Resources.Load<VisualTreeAsset>(TemplatePath);
            if (template == null)
            {
                LogHelper.LogError($"[DropdownItemView] Failed to load template from Resources/{TemplatePath}");
                return false;
            }

            Root = template.Instantiate();

            _item = Root.Q<VisualElement>(DmConstants.Uxml.SdkVersionDropdown.Item.ContentContainer);
            if (_item == null)
            {
                LogHelper.LogError("[DropdownItemView] Failed to find DropdownItem element in template");
                return false;
            }

            _selectionToggle = _item.Q<VisualElement>(DmConstants.Uxml.SdkVersionDropdown.Item.SelectionToggle);
            _versionLabel = _item.Q<Label>(DmConstants.Uxml.SdkVersionDropdown.Item.VersionLabel);
            _badgeNew = _item.Q<VisualElement>(DmConstants.Uxml.SdkVersionDropdown.Item.BadgeNew);
            _badgeRecommended = _item.Q<VisualElement>(DmConstants.Uxml.SdkVersionDropdown.Item.BadgeRecommended);
            _badgeDeprecated = _item.Q<VisualElement>(DmConstants.Uxml.SdkVersionDropdown.Item.BadgeDeprecated);
            _badgeUnstable = _item.Q<VisualElement>(DmConstants.Uxml.SdkVersionDropdown.Item.BadgeUnstable);

            _item.RegisterCallback<MouseDownEvent>(OnItemClicked);

            return true;
        }

        public void SetVersionText(string versionText)
        {
            if (_versionLabel != null) _versionLabel.text = versionText;
        }

        public void SetSelected(bool isSelected)
        {
            if (_selectionToggle != null) _selectionToggle.style.visibility = isSelected ? Visibility.Visible : Visibility.Hidden;
        }

        public void SetTooltip(string tooltip)
        {
            if (_item != null) _item.tooltip = tooltip;
        }

        public void SetBadgeNew(bool visible, string tooltip = null)
        {
            SetBadgeVisibility(_badgeNew, visible, tooltip);
        }

        public void SetBadgeRecommended(bool visible, string tooltip = null)
        {
            SetBadgeVisibility(_badgeRecommended, visible, tooltip);
        }

        public void SetBadgeDeprecated(bool visible, string tooltip = null)
        {
            SetBadgeVisibility(_badgeDeprecated, visible, tooltip);
        }

        public void SetBadgeUnstable(bool visible, string tooltip = null)
        {
            SetBadgeVisibility(_badgeUnstable, visible, tooltip);
        }

        public void HideAllBadges()
        {
            SetBadgeVisibility(_badgeNew, false);
            SetBadgeVisibility(_badgeRecommended, false);
            SetBadgeVisibility(_badgeDeprecated, false);
            SetBadgeVisibility(_badgeUnstable, false);
        }

        private static void SetBadgeVisibility(VisualElement badge, bool visible, string tooltip = null)
        {
            if (badge == null) return;
            badge.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
            badge.tooltip = tooltip;
        }

        private void OnItemClicked(MouseDownEvent evt)
        {
            Clicked?.Invoke(this, EventArgs.Empty);
            evt?.StopPropagation();
        }
    }
}
