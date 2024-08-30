// ReSharper disable CheckNamespace

using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal static partial class DmUIElements
    {
        private static VisualTreeAsset _sdkCardVta;

        private static VisualTreeAsset GetSdkCardAsset()
        {
            return _sdkCardVta ??= AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(DmConstants.SdkCardUxmlPath);
        }

        private static VisualElement CreateSdkCardElement(List<Sdk> sdks, VisualElement tooltip)
        {
            SdkInfoScriptableObject sdkInfo = null;
            var sdkInfoRequest = SdkInfoScriptableObject.Get(sdks[0].status);
            if (sdkInfoRequest.IsSuccess) sdkInfo = sdkInfoRequest.Value;

            VisualElement sdkCard = GetSdkCardAsset().Instantiate();
            SetCommonSdkCardValues(sdkCard, tooltip, sdks, sdkInfo);

            if (!sdkInfoRequest.IsSuccess) return sdkCard;

            sdkCard.Q<VisualElement>(DmConstants.Uxml.SdkCard.SdkIcon).style.backgroundImage = new StyleBackground(sdkInfoRequest.Value.Texture);

            return sdkCard;
        }
    }
}
