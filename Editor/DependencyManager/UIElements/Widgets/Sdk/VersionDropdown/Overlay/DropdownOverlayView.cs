using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal class DropdownOverlayView
    {
        private const string TemplatePath = "Appodeal/DependencyManager/SdkVersionDropdown/SdkVersionDropdownOverlay";

        private VisualElement _overlay;
        private VisualElement _clickCatcher;
        private VisualElement _popupContainer;

        public VisualElement Root { get; private set; }

        public event EventHandler ClickCatcherPointerDown;

        public bool TryLoadFromTemplate()
        {
            var template = Resources.Load<VisualTreeAsset>(TemplatePath);
            if (template == null)
            {
                LogHelper.LogError($"[DropdownOverlayView] Failed to load template from Resources/{TemplatePath}");
                return false;
            }

            Root = template.Instantiate();

            _overlay = Root.Q<VisualElement>(DmConstants.Uxml.SdkVersionDropdown.Overlay.ContentContainer);
            _clickCatcher = _overlay?.Q<VisualElement>(DmConstants.Uxml.SdkVersionDropdown.Overlay.ClickCatcher);
            _popupContainer = _overlay?.Q<VisualElement>(DmConstants.Uxml.SdkVersionDropdown.Overlay.PopupContainer);

            if (_overlay == null || _clickCatcher == null || _popupContainer == null)
            {
                LogHelper.LogError("[DropdownOverlayView] Failed to find required elements in template");
                return false;
            }

            Root.AddToClassList(DmConstants.Uss.SdkVersionDropdown.Overlay.Root);
            Root.pickingMode = PickingMode.Ignore;

            _clickCatcher.RegisterCallback<PointerDownEvent>(OnClickCatcherPointerDown);

            return true;
        }

        public void SetVisible(bool visible)
        {
            _overlay?.EnableInClassList(DmConstants.Uss.SdkVersionDropdown.Overlay.VisibleModifier, visible);
        }

        public void SetPopupPosition(float x, float y)
        {
            if (_popupContainer == null) return;

            _popupContainer.style.left = x;
            _popupContainer.style.top = y;
        }

        public void AddPopup(VisualElement popup)
        {
            _popupContainer?.Add(popup);
        }

        public void ClearPopup()
        {
            _popupContainer?.Clear();
        }

        private void OnClickCatcherPointerDown(PointerDownEvent evt)
        {
            ClickCatcherPointerDown?.Invoke(this, EventArgs.Empty);
        }
    }
}
