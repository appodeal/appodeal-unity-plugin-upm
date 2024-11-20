// ReSharper disable CheckNamespace

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal static partial class DmUIElements
    {
        private static VisualTreeAsset _adNetworkSdkCardVta;

        private static VisualTreeAsset GetAdNetworkSdkCardAsset()
        {
            return _adNetworkSdkCardVta ??= AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(DmConstants.AdNetworkSdkCardUxmlPath);
        }

        private static VisualElement CreateAdNetworkSdkCardElement(List<Sdk> sdks, List<AdType> adTypes, List<Sdk> mediations, VisualElement tooltip)
        {
            SdkInfoScriptableObject sdkInfo = null;
            var sdkInfoRequest = SdkInfoScriptableObject.Get(sdks[0].status);
            if (sdkInfoRequest.IsSuccess) sdkInfo = sdkInfoRequest.Value;

            VisualElement adNetworkSdkCard = GetAdNetworkSdkCardAsset().Instantiate();
            SetCommonSdkCardValues(adNetworkSdkCard, tooltip, sdks, sdkInfo);

            adTypes?.ForEach(adType => adNetworkSdkCard.Q<VisualElement>(GetVisualElementNameByAdType(adType)).style.display = DisplayStyle.Flex);

            var mediationsContainer = adNetworkSdkCard.Q<VisualElement>(DmConstants.Uxml.SdkCard.SupportedMediationsContainer);
            mediations?.ForEach(mediation => mediationsContainer.Add(CreateMediationIcon(mediation)));

            return adNetworkSdkCard;
        }

        private static VisualElement CreateMediationIcon(Sdk mediation)
        {
            var infoRequest = SdkInfoScriptableObject.Get(mediation.status);

            var icon = new VisualElement
            {
                tooltip = mediation.name,
                name = $"{mediation.name} Icon"
            };
            icon.AddToClassList(DmConstants.Uss.MediationIconClassName);

            if (infoRequest.IsSuccess) icon.style.backgroundImage = new StyleBackground(infoRequest.Value.Texture);
            icon.style.display = DisplayStyle.Flex;

            return icon;
        }

        private static string GetVisualElementNameByAdType(AdType adType)
        {
            return adType switch
            {
                AdType.Banner => DmConstants.Uxml.SdkCard.BannerAdTypeIcon,
                AdType.Interstitial => DmConstants.Uxml.SdkCard.InterstitialAdTypeIcon,
                AdType.Mrec => DmConstants.Uxml.SdkCard.MrecAdTypeIcon,
                AdType.Rewarded => DmConstants.Uxml.SdkCard.RewardedAdTypeIcon,
                _ => String.Empty
            };
        }
    }
}
