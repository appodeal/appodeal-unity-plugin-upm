using UnityEngine;
using UnityEngine.UIElements;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal static class SdkTooltipOverlay
    {
        private const float GapBelowAnchor = 5f;
        private const float GapAboveAnchor = 5f;
        private const float EditorWindowBorder = 2f;

        private static VisualElement _parentRoot;
        private static SdkTooltipOverlayView _overlayView;
        private static SdkTooltipPopupView _currentTooltip;
        private static VisualElement _currentAnchor;

        public static void Initialize(VisualElement root)
        {
            if (_overlayView != null) Cleanup();

            _parentRoot = root;

            _overlayView = new SdkTooltipOverlayView();
            if (!_overlayView.TryLoadFromTemplate())
            {
                LogHelper.LogError("[SdkTooltipOverlay] Failed to load template");
                _overlayView = null;
                return;
            }

            _parentRoot.Add(_overlayView.Root);
        }

        public static void Cleanup()
        {
            Hide();

            if (_overlayView != null)
            {
                _overlayView.Root.RemoveFromHierarchy();
                _overlayView = null;
            }

            _parentRoot = null;
        }

        public static void Show(VisualElement anchorElement, string sdkName, Requirement requirement, string description)
        {
            if (_overlayView == null || _parentRoot == null) return;

            Hide();

            _currentAnchor = anchorElement;

            _currentTooltip = new SdkTooltipPopupView(sdkName, requirement.ToString(), description);

            if (!_currentTooltip.TryLoadFromTemplate())
            {
                LogHelper.LogError("[SdkTooltipOverlay] Failed to create tooltip view");
                _currentTooltip = null;
                return;
            }

            _currentTooltip.Root.RegisterCallback<GeometryChangedEvent>(OnTooltipGeometryChanged);
            _overlayView.AddTooltip(_currentTooltip.Root);
            _overlayView.Root.BringToFront();
        }

        public static void Hide()
        {
            if (_currentTooltip != null)
            {
                _currentTooltip.Root.UnregisterCallback<GeometryChangedEvent>(OnTooltipGeometryChanged);
                _overlayView?.ClearTooltip();
                _currentTooltip = null;
            }

            _currentAnchor = null;
        }

        private static Vector2 CalculatePosition(Rect anchorRect, float tooltipWidth, float tooltipHeight)
        {
            var panelRect = _parentRoot.panel.visualTree.worldBound;

            float spaceBelow = panelRect.yMax - anchorRect.yMax;
            float spaceAbove = anchorRect.yMin - panelRect.yMin;
            bool showBelow = spaceBelow >= tooltipHeight + GapBelowAnchor || spaceBelow >= spaceAbove;

            float y = showBelow
                ? anchorRect.yMax + GapBelowAnchor
                : anchorRect.yMin - tooltipHeight - GapAboveAnchor;

            float rightEdgeOffset = Mathf.Max(WizardScrollViewHelper.VerticalScrollbarWidth, EditorWindowBorder);
            float x = anchorRect.center.x - tooltipWidth / 2;

            if (x + tooltipWidth > panelRect.xMax - rightEdgeOffset)
            {
                x = panelRect.xMax - tooltipWidth - rightEdgeOffset;
            }

            float leftEdge = WizardScrollViewHelper.ViewportXMin;
            if (x < leftEdge)
            {
                x = leftEdge;
            }

            return new Vector2(x, y);
        }

        private static void OnTooltipGeometryChanged(GeometryChangedEvent evt)
        {
            if (_currentTooltip == null || _parentRoot == null || _currentAnchor == null) return;

            var tooltipSize = _currentTooltip.Root.resolvedStyle;
            if (Mathf.Approximately(tooltipSize.width, 0f)) return;

            var infoIcon = _currentAnchor.Q<VisualElement>(DmConstants.Uxml.SdkCards.Shell.InfoIcon);
            if (infoIcon == null) return;

            var position = CalculatePosition(infoIcon.worldBound, tooltipSize.width, tooltipSize.height);
            var localPosition = _parentRoot.WorldToLocal(position);

            _currentTooltip.SetPosition(localPosition.x, localPosition.y);
        }
    }
}
