using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal static class DropdownOverlay
    {
        private const float PopupWidth = 188f;
        private const float ContentMaxHeight = 100f;
        private const float ItemHeight = 25f;
        private const float HorizontalPadding = 4f;
        private const float VerticalPadding = 4f;
        private const float BorderWidth = 2f;
        private const float PopupGapBelow = 2f;
        private const float PopupGapAbove = 2f;
        private const float EditorWindowBorder = 2f;

        private static VisualElement _parentRoot;
        private static DropdownOverlayView _view;
        private static DropdownPopupController _activeController;

        public static event EventHandler<DropdownOverlayEventArgs> ClickedOutside;

        public static void Initialize(VisualElement root)
        {
            if (_view != null) Cleanup();

            _parentRoot = root;

            _view = new DropdownOverlayView();
            if (!_view.TryLoadFromTemplate())
            {
                LogHelper.LogError("[DropdownOverlay] Failed to load template");
                _view = null;
                return;
            }

            _view.ClickCatcherPointerDown += OnClickOutside;
            _parentRoot.Add(_view.Root);
        }

        public static void Cleanup()
        {
            Hide();

            if (_view != null)
            {
                _view.ClickCatcherPointerDown -= OnClickOutside;
                _view.Root.RemoveFromHierarchy();
                _view = null;
            }

            _parentRoot = null;
        }

        public static bool Show(DropdownPopupController controller, VisualElement anchorElement)
        {
            if (_view == null) return false;
            if (_activeController != null) return false;

            _activeController = controller;

            int itemCount = controller.ItemCount;
            bool hasScrollbar = HasScrollbar(itemCount);
            controller.SetNoScrollbar(!hasScrollbar);

            var position = CalculatePopupPosition(anchorElement.worldBound, itemCount, hasScrollbar);
            var localPosition = _parentRoot.WorldToLocal(position);

            _view.SetPopupPosition(localPosition.x, localPosition.y);
            _view.AddPopup(controller.Root);
            _view.SetVisible(true);

            return true;
        }

        public static void Hide()
        {
            if (_view == null) return;

            _view.SetVisible(false);
            _view.ClearPopup();
            _activeController = null;
        }

        private static bool HasScrollbar(int itemCount)
        {
            float contentHeight = itemCount * ItemHeight;
            return contentHeight > ContentMaxHeight;
        }

        private static Vector2 CalculatePopupPosition(Rect buttonRect, int itemCount, bool hasScrollbar)
        {
            var panelRect = _parentRoot.panel.visualTree.worldBound;

            float contentHeight = itemCount * ItemHeight;
            float scrollbarWidth = WizardScrollViewHelper.VerticalScrollbarWidth;
            float horizontalPaddingTotal = hasScrollbar ? HorizontalPadding : HorizontalPadding * 2;
            float totalPopupWidth = PopupWidth + horizontalPaddingTotal + BorderWidth * 2 + (hasScrollbar ? scrollbarWidth : 0f);
            float totalPopupHeight = Mathf.Min(ContentMaxHeight, contentHeight) + VerticalPadding * 2 + BorderWidth * 2;

            float spaceBelow = panelRect.yMax - buttonRect.yMax;
            float spaceAbove = buttonRect.yMin - panelRect.yMin;
            bool showBelow = spaceBelow >= totalPopupHeight || spaceBelow >= spaceAbove;

            float y = showBelow
                ? buttonRect.yMax + PopupGapBelow
                : buttonRect.yMin - totalPopupHeight - PopupGapAbove;

            float rightEdgeOffset = Mathf.Max(WizardScrollViewHelper.VerticalScrollbarWidth, EditorWindowBorder);
            float x = buttonRect.xMin;

            if (x + totalPopupWidth > panelRect.xMax - rightEdgeOffset)
            {
                x = buttonRect.xMax - totalPopupWidth;
            }

            if (x + totalPopupWidth > panelRect.xMax - rightEdgeOffset)
            {
                x = panelRect.xMax - totalPopupWidth - rightEdgeOffset;
            }

            float leftEdge = WizardScrollViewHelper.ViewportXMin;
            if (x < leftEdge)
            {
                x = leftEdge;
            }

            return new Vector2(x, y);
        }

        private static void OnClickOutside(object sender, EventArgs e)
        {
            var args = new DropdownOverlayEventArgs(_activeController);
            ClickedOutside?.Invoke(sender, args);
        }
    }
}
