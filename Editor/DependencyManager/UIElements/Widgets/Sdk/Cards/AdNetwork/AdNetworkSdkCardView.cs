using UnityEngine;
using UnityEngine.UIElements;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal class AdNetworkSdkCardView : ISdkCardView
    {
        private const string TemplatePath = "Appodeal/DependencyManager/SdkCards/AdNetworkSdkCardTemplate";

        private VisualElement _bannerIcon;
        private VisualElement _mrecIcon;
        private VisualElement _interstitialIcon;
        private VisualElement _rewardedIcon;
        private VisualElement _androidMediationsContainer;
        private VisualElement _iosMediationsContainer;

        public VisualElement Root { get; private set; }

        public SdkCardShell Shell { get; private set; }

        public bool TryLoadFromTemplate()
        {
            var template = Resources.Load<VisualTreeAsset>(TemplatePath);
            if (template == null)
            {
                LogHelper.LogError($"[AdNetworkSdkCardView] Failed to load template from Resources/{TemplatePath}");
                return false;
            }

            Root = template.Instantiate();

            Shell = Root.Q<SdkCardShell>(DmConstants.Uxml.SdkCards.AdNetwork.ShellName);
            if (Shell == null)
            {
                LogHelper.LogError("[AdNetworkSdkCardView] SdkCardShell not found in template");
                return false;
            }

            _bannerIcon = Root.Q<VisualElement>(DmConstants.Uxml.SdkCards.AdNetwork.BannerIcon);
            _mrecIcon = Root.Q<VisualElement>(DmConstants.Uxml.SdkCards.AdNetwork.MrecIcon);
            _interstitialIcon = Root.Q<VisualElement>(DmConstants.Uxml.SdkCards.AdNetwork.InterstitialIcon);
            _rewardedIcon = Root.Q<VisualElement>(DmConstants.Uxml.SdkCards.AdNetwork.RewardedIcon);
            _androidMediationsContainer = Root.Q<VisualElement>(DmConstants.Uxml.SdkCards.AdNetwork.AndroidMediationsContainer);
            _iosMediationsContainer = Root.Q<VisualElement>(DmConstants.Uxml.SdkCards.AdNetwork.IosMediationsContainer);

            if (_androidMediationsContainer == null || _iosMediationsContainer == null)
            {
                LogHelper.LogError("[AdNetworkSdkCardView] Failed to find required elements in template");
                return false;
            }

            return true;
        }

        public void ShowBannerIcon() => Show(_bannerIcon);
        public void ShowMrecIcon() => Show(_mrecIcon);
        public void ShowInterstitialIcon() => Show(_interstitialIcon);
        public void ShowRewardedIcon() => Show(_rewardedIcon);

        private static void Show(VisualElement element)
        {
            if (element != null) element.style.display = DisplayStyle.Flex;
        }

        public void ClearAndroidMediations() => _androidMediationsContainer?.Clear();
        public void ClearIosMediations() => _iosMediationsContainer?.Clear();

        public void AddAndroidMediationIcon(string id, Texture2D texture)
        {
            var icon = CreateMediationIcon(id, texture);
            _androidMediationsContainer?.Add(icon);
        }

        public void AddIosMediationIcon(string id, Texture2D texture)
        {
            var icon = CreateMediationIcon(id, texture);
            _iosMediationsContainer?.Add(icon);
        }

        private static VisualElement CreateMediationIcon(string id, Texture2D texture)
        {
            var icon = new VisualElement
            {
                name = $"{id}Icon",
                tooltip = id
            };
            icon.AddToClassList(DmConstants.Uss.SdkCards.AdNetwork.MediationIcon);

            if (texture != null) icon.style.backgroundImage = new StyleBackground(texture);

            return icon;
        }
    }
}
