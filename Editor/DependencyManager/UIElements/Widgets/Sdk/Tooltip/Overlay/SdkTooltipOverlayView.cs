using UnityEngine;
using UnityEngine.UIElements;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal class SdkTooltipOverlayView
    {
        private const string TemplatePath = "Appodeal/DependencyManager/SdkTooltip/SdkTooltipOverlay";

        private VisualElement _overlay;

        public VisualElement Root { get; private set; }

        public bool TryLoadFromTemplate()
        {
            var template = Resources.Load<VisualTreeAsset>(TemplatePath);
            if (template == null)
            {
                LogHelper.LogError($"[SdkTooltipOverlayView] Failed to load template from Resources/{TemplatePath}");
                return false;
            }

            Root = template.Instantiate();

            _overlay = Root.Q<VisualElement>(DmConstants.Uxml.SdkTooltip.Overlay.ContentContainer);

            if (_overlay == null)
            {
                LogHelper.LogError("[SdkTooltipOverlayView] Failed to find required elements in template");
                return false;
            }

            Root.AddToClassList(DmConstants.Uss.SdkTooltip.Overlay.Root);
            Root.pickingMode = PickingMode.Ignore;
            _overlay.pickingMode = PickingMode.Ignore;

            return true;
        }

        public void AddTooltip(VisualElement tooltip)
        {
            _overlay?.Add(tooltip);
        }

        public void ClearTooltip()
        {
            _overlay?.Clear();
        }
    }
}
