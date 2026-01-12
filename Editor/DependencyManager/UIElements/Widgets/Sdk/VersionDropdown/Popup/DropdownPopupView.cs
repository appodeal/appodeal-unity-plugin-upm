using UnityEngine;
using UnityEngine.UIElements;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal class DropdownPopupView
    {
        private const string TemplatePath = "Appodeal/DependencyManager/SdkVersionDropdown/SdkVersionDropdownPopup";

        private ScrollView _scrollView;
        private VisualElement _itemsContainer;

        public VisualElement Root { get; private set; }

        public bool TryLoadFromTemplate()
        {
            var template = Resources.Load<VisualTreeAsset>(TemplatePath);
            if (template == null)
            {
                LogHelper.LogError($"[DropdownPopupView] Failed to load template from Resources/{TemplatePath}");
                return false;
            }

            Root = template.Instantiate();

            _scrollView = Root.Q<ScrollView>(DmConstants.Uxml.SdkVersionDropdown.Popup.PopupScrollView);
            _itemsContainer = _scrollView.Q<VisualElement>(DmConstants.Uxml.SdkVersionDropdown.Popup.ContentContainer);

            if (_scrollView == null || _itemsContainer == null)
            {
                LogHelper.LogError("[DropdownPopupView] Failed to find required elements in template");
                return false;
            }

            return true;
        }

        public void ClearItems()
        {
            _itemsContainer?.Clear();
        }

        public void AddItem(VisualElement item)
        {
            _itemsContainer?.Add(item);
        }

        public void SetNoScrollbar(bool noScrollbar)
        {
            _scrollView?.EnableInClassList(DmConstants.Uss.SdkVersionDropdown.Popup.ScrollViewNoScrollbarModifier, noScrollbar);
        }
    }
}
