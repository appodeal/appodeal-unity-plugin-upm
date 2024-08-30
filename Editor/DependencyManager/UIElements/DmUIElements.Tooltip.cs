// ReSharper disable CheckNamespace

using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal static partial class DmUIElements
    {
        private static VisualTreeAsset _tooltipVta;

        public static VisualElement CreateTooltipUI(VisualElement rootElement)
        {
            var tooltip = GetTooltipAsset().Instantiate();

            var tooltipRoot = tooltip.Q<VisualElement>(DmConstants.Uxml.Tooltip.Root);

            tooltipRoot.RegisterCallback<GeometryChangedEvent>(_ =>
            {
                if (Mathf.Approximately(tooltipRoot.resolvedStyle.width, 0f)) return;

                var sdkCard = rootElement.Query(className: DmConstants.Uss.SdkClassName)
                    .Where(el => el.Q<Label>(DmConstants.Uxml.SdkCard.SdkId).text == tooltipRoot.Q<Label>(DmConstants.Uxml.Tooltip.SdkId).text)
                    .First();
                var infoIcon = sdkCard.Q<VisualElement>(DmConstants.Uxml.SdkCard.InfoIcon);

                var tooltipSize = tooltipRoot.resolvedStyle;

                float desiredXAxisOffset = sdkCard.worldBound.xMax - rootElement.worldBound.xMin - tooltipSize.width / 2;
                float maxXAxisOffset = rootElement.worldBound.xMax - rootElement.worldBound.xMin - tooltipSize.width;
                if (desiredXAxisOffset > maxXAxisOffset) desiredXAxisOffset = maxXAxisOffset - 15;
                tooltip.style.left = desiredXAxisOffset;

                float desiredYAxisOffset = infoIcon.worldBound.yMax - rootElement.worldBound.yMin + 5;
                float maxYAxisOffset = rootElement.worldBound.yMax - rootElement.worldBound.yMin - tooltipSize.height;
                if (desiredYAxisOffset > maxYAxisOffset) desiredYAxisOffset = infoIcon.worldBound.yMin - tooltipSize.height - 50;
                tooltip.style.top = desiredYAxisOffset;
            });

            return tooltip;
        }

        private static VisualTreeAsset GetTooltipAsset()
        {
            return _tooltipVta ??= AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(DmConstants.TooltipUxmlPath);
        }
    }
}
