using System;
using UnityEngine.UIElements;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal static class WizardScrollViewHelper
    {
        private const string ScrollViewName = "WizardScrollView";
        private const float FallbackScrollbarWidth = 13f;

        private static ScrollView _cachedScrollView;

        public static void Initialize(VisualElement root)
        {
            _cachedScrollView = root?.Q<ScrollView>(ScrollViewName);
        }

        public static void Cleanup()
        {
            _cachedScrollView = null;
        }

        public static float VerticalScrollbarWidth
        {
            get
            {
                if (!HasVerticalScrollbar) return 0f;
                float width = _cachedScrollView?.verticalScroller?.resolvedStyle.width ?? FallbackScrollbarWidth;
                return Single.IsNaN(width) ? FallbackScrollbarWidth : width;
            }
        }

        public static float ViewportXMin => _cachedScrollView?.contentViewport.worldBound.xMin ?? 0f;

        private static bool HasVerticalScrollbar
        {
            get
            {
                if (_cachedScrollView == null) return false;
                return _cachedScrollView.verticalScroller?.resolvedStyle.display == DisplayStyle.Flex;
            }
        }
    }
}
