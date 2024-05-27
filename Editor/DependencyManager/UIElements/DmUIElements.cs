using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;

// ReSharper disable once CheckNamespace
namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal static partial class DmUIElements
    {
        public static bool AreAllDmAssetsLoadable()
        {
            return GetErrorAsset() != null &&
                   GetLoadingAsset() != null &&
                   GetDependencyManagerAsset() != null &&
                   GetSdkCardAsset() != null &&
                   GetAdNetworkSdkCardAsset() != null &&
                   GetTooltipAsset() != null;
        }

        private static void SetCommonSdkCardValues(VisualElement sdkCard, VisualElement tooltip, List<Sdk> sdks, SdkInfoScriptableObject sdkInfo)
        {
            sdkCard.Q<Label>(DmConstants.Uxml.SdkCard.SdkId).text = sdks[0].status;
            sdkCard.Q<Label>(DmConstants.Uxml.SdkCard.SdkName).text = sdks[0].name;

            var toggle = sdkCard.Q<Toggle>(DmConstants.Uxml.SdkCard.IsSdkCardSelected);
            if (sdks[0].requirement == Requirement.Required)
            {
                // toggle.style.display = DisplayStyle.None;
                toggle.value = true;
                toggle.SetEnabled(false);
            }
            else
            {
                if (DmChoicesScriptableObject.IsSdkSelectionStateKnown(sdks[0].status, out var status))
                {
                    toggle.value = status.isSelected;
                }
                else
                {
                    toggle.value = sdks[0].requirement == Requirement.Default;
                }
            }

            var androidSdk = sdks.FirstOrDefault(sdk => sdk.platform == Platform.Android);
            if (androidSdk != null)
            {
                sdkCard.Q<Label>(DmConstants.Uxml.SdkCard.AndroidSdkVersion).text = androidSdk.version;
            }
            else
            {
                sdkCard.Q<VisualElement>(DmConstants.Uxml.SdkCard.AndroidSdk).style.display = DisplayStyle.None;
            }

            var iosSdk = sdks.FirstOrDefault(sdk => sdk.platform == Platform.Ios);
            if (iosSdk != null)
            {
                sdkCard.Q<Label>(DmConstants.Uxml.SdkCard.IosSdkVersion).text = iosSdk.version;
            }
            else
            {
                sdkCard.Q<VisualElement>(DmConstants.Uxml.SdkCard.IosSdk).style.display = DisplayStyle.None;
            }

            var infoIcon = sdkCard.Q<VisualElement>(DmConstants.Uxml.SdkCard.InfoIcon);

            infoIcon.RegisterCallback<PointerEnterEvent>(_ =>
            {
                if (tooltip.Q<Label>(DmConstants.Uxml.Tooltip.SdkId).text != sdks[0].status)
                {
                    tooltip.Q<Label>(DmConstants.Uxml.Tooltip.SdkId).text = sdks[0].status;
                    tooltip.Q<Label>(DmConstants.Uxml.Tooltip.SdkName).text = sdks[0].name;
                    tooltip.Q<Label>(DmConstants.Uxml.Tooltip.SdkRequirementStatus).text = sdks[0].requirement.ToString();
                    tooltip.Q<Label>(DmConstants.Uxml.Tooltip.SdkDescription).text = sdkInfo?.Description;
                }

                var tooltipColor = EditorGUIUtility.isProSkin ? DmConstants.DarkTooltipBgColor : DmConstants.LightTooltipBgColor;
                tooltip.Q<VisualElement>(DmConstants.Uxml.Tooltip.Root).style.backgroundColor = tooltipColor;

                tooltip.BringToFront();
                tooltip.style.display = DisplayStyle.Flex;
            });

            infoIcon.RegisterCallback<PointerOutEvent>(_ =>
            {
                tooltip.style.display = DisplayStyle.None;
            });
        }
    }
}
