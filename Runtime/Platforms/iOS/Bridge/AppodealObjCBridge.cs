using System.Runtime.InteropServices;

// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Platforms.Ios
{
    internal delegate void AppodealInitializationCallback();

    internal delegate void AppodealAdRevenueCallback(string adType, string networkName, string adUnitName, string demandSource, string placement, double revenue, string currency, string revenuePrecision);

    internal delegate void AppodealInterstitialCallbacks();

    internal delegate void AppodealInterstitialDidLoadCallback(bool isPrecache);

    internal delegate void AppodealBannerCallbacks();

    internal delegate void AppodealBannerDidLoadCallback(int height, bool isPrecache);

    internal delegate void AppodealBannerViewCallbacks();

    internal delegate void AppodealBannerViewDidLoadCallback(int height, bool isPrecache);

    internal delegate void AppodealMrecViewCallbacks();

    internal delegate void AppodealMrecViewDidLoadCallback(bool isPrecache);

    internal delegate void AppodealRewardedVideoCallbacks();

    internal delegate void AppodealRewardedVideoDidLoadCallback(bool isPrecache);

    internal delegate void AppodealRewardedVideoDidDismissCallback(bool isFinished);

    internal delegate void AppodealRewardedVideoDidFinishCallback(double amount, string name);

    internal delegate void InAppPurchaseValidationSucceededCallback(string json);

    internal delegate void InAppPurchaseValidationFailedCallback(string error);

    internal static class AppodealObjCBridge
    {
        [DllImport("__Internal")]
        internal static extern void AppodealInitialize(string apiKey, int types, string pluginVer, string engineVersion);

        [DllImport("__Internal")]
        internal static extern bool AppodealIsInitialized(int type);

        [DllImport("__Internal")]
        internal static extern bool AppodealShowAd(int style);

        [DllImport("__Internal")]
        internal static extern bool AppodealShowAdForPlacement(int style, string placement);

        [DllImport("__Internal")]
        internal static extern bool AppodealShowBannerAdViewForPlacement(int style, int gravity, string placement);

        [DllImport("__Internal")]
        internal static extern bool AppodealShowMrecAdViewForPlacement(int style, int gravity, string placement);

        [DllImport("__Internal")]
        internal static extern bool AppodealIsReadyWithStyle(int style);

        [DllImport("__Internal")]
        internal static extern void AppodealHideBanner();

        [DllImport("__Internal")]
        internal static extern void AppodealHideBannerView();

        [DllImport("__Internal")]
        internal static extern void AppodealHideMrecView();

        [DllImport("__Internal")]
        internal static extern void AppodealCacheAd(int types);

        [DllImport("__Internal")]
        internal static extern void AppodealSetAutoCache(bool autoCache, int types);

        [DllImport("__Internal")]
        internal static extern void AppodealSetSmartBanners(bool value);

        [DllImport("__Internal")]
        internal static extern bool AppodealIsSmartBannersEnabled();

        [DllImport("__Internal")]
        internal static extern void AppodealSetTabletBanners(bool value);

        [DllImport("__Internal")]
        internal static extern void AppodealSetBannerAnimation(bool value);

        [DllImport("__Internal")]
        internal static extern void AppodealSetBannerRotation(int leftBannerRotation, int rightBannerRotation);

        [DllImport("__Internal")]
        internal static extern void AppodealSetLogLevel(int loglevel);

        [DllImport("__Internal")]
        internal static extern void AppodealSetTestingEnabled(bool value);

        [DllImport("__Internal")]
        internal static extern void AppodealSetChildDirectedTreatment(bool value);

        [DllImport("__Internal")]
        internal static extern string AppodealGetNetworks(int types);

        [DllImport("__Internal")]
        internal static extern void AppodealDisableNetwork(string name);

        [DllImport("__Internal")]
        internal static extern void AppodealSetTriggerPrecacheCallbacks(int types, bool value);

        [DllImport("__Internal")]
        internal static extern void AppodealDisableNetworkForAdTypes(string name, int type);

        [DllImport("__Internal")]
        internal static extern void AppodealSetLocationTracking(bool value);

        [DllImport("__Internal")]
        internal static extern string AppodealGetVersion();

        [DllImport("__Internal")]
        internal static extern long AppodealGetSegmentId();

        [DllImport("__Internal")]
        internal static extern bool AppodealCanShow(int style);

        [DllImport("__Internal")]
        internal static extern bool AppodealCanShowWithPlacement(int style, string placement);

        [DllImport("__Internal")]
        internal static extern string AppodealGetRewardCurrency(string placement);

        [DllImport("__Internal")]
        internal static extern double AppodealGetRewardAmount(string placement);

        [DllImport("__Internal")]
        internal static extern double AppodealGetPredictedEcpm(int adType);

		[DllImport("__Internal")]
		internal static extern double AppodealGetPredictedEcpmForPlacement(int adType, string placement);

        [DllImport("__Internal")]
        internal static extern void AppodealSetCustomFilterBool(string name, bool value);

        [DllImport("__Internal")]
        internal static extern void AppodealSetCustomFilterString(string name, string value);

        [DllImport("__Internal")]
        internal static extern void AppodealSetCustomFilterDouble(string name, double value);

        [DllImport("__Internal")]
        internal static extern void AppodealSetCustomFilterInt(string name, int value);

        [DllImport("__Internal")]
        internal static extern void AppodealResetCustomFilter(string name);

        [DllImport("__Internal")]
        internal static extern void AppodealSetExtraDataBool(string name, bool value);

        [DllImport("__Internal")]
        internal static extern void AppodealSetExtraDataInt(string name, int value);

        [DllImport("__Internal")]
        internal static extern void AppodealSetExtraDataDouble(string name, double value);

        [DllImport("__Internal")]
        internal static extern void AppodealSetExtraDataString(string name, string value);

        [DllImport("__Internal")]
        internal static extern void AppodealResetExtraData(string name);

        [DllImport("__Internal")]
        internal static extern void AppodealTrackInAppPurchase(double amount, string currency);

        [DllImport("__Internal")]
        internal static extern void AppodealSetUserId(string id);

        [DllImport("__Internal")]
        internal static extern string AppodealGetUserId();

        [DllImport("__Internal")]
        internal static extern bool AppodealIsPrecacheAd(int adType);

        [DllImport("__Internal")]
        internal static extern bool AppodealIsAutoCacheEnabled(int adType);

        [DllImport("__Internal")]
        internal static extern void AppodealLogEvent(string eventName, string eventParams);

        [DllImport("__Internal")]
        internal static extern void AppodealValidateInAppPurchase(
            string productIdentifier,
            string price,
            string currency,
            string transactionId,
            string additionalParams,
            int type,
            InAppPurchaseValidationSucceededCallback success,
            InAppPurchaseValidationFailedCallback failure);

        [DllImport("__Internal")]
        internal static extern void AppodealSetInitializationDelegate(
            AppodealInitializationCallback appodealSDKDidInitialize
        );

        [DllImport("__Internal")]
        internal static extern void AppodealSetAdRevenueDelegate(
            AppodealAdRevenueCallback didReceiveRevenue
        );

        [DllImport("__Internal")]
        internal static extern void AppodealSetInterstitialDelegate(
            AppodealInterstitialDidLoadCallback interstitialDidLoadAd,
            AppodealInterstitialCallbacks interstitialDidFailToLoadAd,
            AppodealInterstitialCallbacks interstitialDidFailToPresent,
            AppodealInterstitialCallbacks interstitialWillPresent,
            AppodealInterstitialCallbacks interstitialDidDismiss,
            AppodealInterstitialCallbacks interstitialDidClick,
            AppodealInterstitialCallbacks interstitialDidExpired
        );

        [DllImport("__Internal")]
        internal static extern void AppodealSetRewardedVideoDelegate(
            AppodealRewardedVideoDidLoadCallback rewardedVideoDidLoadAd,
            AppodealRewardedVideoCallbacks rewardedVideoDidFailToLoadAd,
            AppodealRewardedVideoCallbacks rewardedVideoDidFailToPresentWithError,
            AppodealRewardedVideoDidDismissCallback rewardedVideoWillDismiss,
            AppodealRewardedVideoDidFinishCallback rewardedVideoDidFinish,
            AppodealRewardedVideoCallbacks rewardedVideoDidPresent,
            AppodealRewardedVideoCallbacks rewardedVideoDidExpired,
            AppodealRewardedVideoCallbacks rewardedVideoDidReceiveTap
        );

        [DllImport("__Internal")]
        internal static extern void AppodealSetBannerDelegate(
            AppodealBannerDidLoadCallback bannerDidLoadAd,
            AppodealBannerCallbacks bannerDidFailToLoadAd,
            AppodealBannerCallbacks bannerDidClick,
            AppodealBannerCallbacks bannerDidExpired,
            AppodealBannerCallbacks bannerDidShow,
            AppodealBannerCallbacks bannerDidFailToPresent
        );

        [DllImport("__Internal")]
        internal static extern void AppodealSetBannerViewDelegate(
            AppodealBannerViewDidLoadCallback bannerViewDidLoadAd,
            AppodealBannerViewCallbacks bannerViewDidFailToLoadAd,
            AppodealBannerViewCallbacks bannerViewDidClick,
            AppodealBannerViewCallbacks bannerViewDidShow,
            AppodealBannerViewCallbacks bannerViewDidFailToPresent,
            AppodealBannerViewCallbacks bannerViewDidExpired
        );

        [DllImport("__Internal")]
        internal static extern void AppodealSetMrecViewDelegate(
            AppodealMrecViewDidLoadCallback mrecDidLoadAd,
            AppodealMrecViewCallbacks mrecDidFailToLoadAd,
            AppodealMrecViewCallbacks mrecDidClick,
            AppodealMrecViewCallbacks mrecDidShow,
            AppodealMrecViewCallbacks mrecViewDidFailToPresent,
            AppodealMrecViewCallbacks mrecDidExpired
        );
    }
}
