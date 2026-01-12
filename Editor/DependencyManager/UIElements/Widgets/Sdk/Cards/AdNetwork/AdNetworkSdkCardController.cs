using System.Collections.Generic;
using UnityEngine;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal class AdNetworkSdkCardController : SdkCardControllerBase<AdNetworkSdkCardView>
    {
        public AdNetworkSdkCardController(Sdk androidModel, Sdk iosModel) : base(new AdNetworkSdkCardView(), androidModel, iosModel) { }

        protected override void ApplyModel()
        {
            base.ApplyModel();

            var adTypes = AndroidModel?.AdTypes.Combine(IosModel?.AdTypes);
            if (adTypes.HasValue)
            {
                SetAdTypes(adTypes);
            }

            InitializeMediationsDisplay();
        }

        protected override void OnVersionChanged(Platform platform, VersionInfo version)
        {
            if (platform == Platform.Android)
            {
                SetAndroidMediations(version?.Mediations);
            }
            else
            {
                SetIosMediations(version?.Mediations);
            }
        }

        private void SetAdTypes(AdTypes? adTypes)
        {
            if (!adTypes.HasValue) return;

            var flags = adTypes.Value;
            if (flags.HasFlag(AdTypes.Banner)) View.ShowBannerIcon();
            if (flags.HasFlag(AdTypes.Mrec)) View.ShowMrecIcon();
            if (flags.HasFlag(AdTypes.Interstitial)) View.ShowInterstitialIcon();
            if (flags.HasFlag(AdTypes.Rewarded)) View.ShowRewardedIcon();
        }

        private void InitializeMediationsDisplay()
        {
            SetAndroidMediations(SelectedAndroidVersion?.Mediations);
            SetIosMediations(SelectedIosVersion?.Mediations);
        }

        private void SetAndroidMediations(List<string> mediationIds)
        {
            View.ClearAndroidMediations();
            if (mediationIds == null) return;

            foreach (string mediationId in mediationIds)
            {
                var texture = GetMediationTexture(mediationId);
                View.AddAndroidMediationIcon(mediationId, texture);
            }
        }

        private void SetIosMediations(List<string> mediationIds)
        {
            View.ClearIosMediations();
            if (mediationIds == null) return;

            foreach (string mediationId in mediationIds)
            {
                var texture = GetMediationTexture(mediationId);
                View.AddIosMediationIcon(mediationId, texture);
            }
        }

        private static Texture2D GetMediationTexture(string mediationId)
        {
            var outcome = SdkInfoScriptableObject.TryGetById(mediationId);
            return outcome.IsSuccess ? outcome.Value.Texture : null;
        }
    }
}
