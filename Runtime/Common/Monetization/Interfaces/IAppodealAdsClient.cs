using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AppodealStack.ConsentManagement.Common;

// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Common
{
    /// <summary>
    /// Interface containing all <see langword="Appodeal"/> API methods' signatures.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public interface IAppodealAdsClient
    {
        void Initialize(string appKey, int adTypes, IAppodealInitializationListener listener);
        bool IsInitialized(int adType);
        void UpdateConsent(IConsent consent);
        void UpdateGdprConsent(GdprUserConsent consent);
        void UpdateCcpaConsent(CcpaUserConsent consent);
        bool IsAutoCacheEnabled(int adType);
        void SetInterstitialCallbacks(IInterstitialAdListener listener);
        void SetRewardedVideoCallbacks(IRewardedVideoAdListener listener);
        void SetBannerCallbacks(IBannerAdListener listener);
        void SetMrecCallbacks(IMrecAdListener listener);
        void SetAdRevenueCallback(IAdRevenueListener listener);
        void Cache(int adType);
        bool Show(int adType);
        bool Show(int adType, string placement);
        bool ShowBannerView(int yAxis, int xGravity, string placement);
        bool ShowMrecView(int yAxis, int xGravity, string placement);
        void Hide(int adType);
        void HideBannerView();
        void HideMrecView();
        void SetAutoCache(int adTypes, bool autoCache);
        void SetTriggerOnLoadedOnPrecache(int adTypes, bool onLoadedTriggerBoth);
        bool IsLoaded(int adType);
        bool IsPrecache(int adType);
        void SetSmartBanners(bool value);
        bool IsSmartBannersEnabled();
        void SetTabletBanners(bool value);
        void SetBannerAnimation(bool value);
        void SetBannerRotation(int leftBannerRotation, int rightBannerRotation);
        void SetUseSafeArea(bool value);
        void TrackInAppPurchase(double amount, string currency);
        List<string> GetNetworks(int adType);
        void DisableNetwork(string network);
        void DisableNetwork(string network, int adType);
        void SetLocationTracking(bool value);
        void SetUserId(string id);
        string GetUserId();
        string GetVersion();
        long GetSegmentId();
        void SetTesting(bool test);
        void SetLogLevel(AppodealLogLevel level);
        void SetCustomFilter(string name, bool value);
        void SetCustomFilter(string name, int value);
        void SetCustomFilter(string name, double value);
        void SetCustomFilter(string name, string value);
        void ResetCustomFilter(string name);
        bool CanShow(int adType);
        bool CanShow(int adType, string placement);
        AppodealReward GetReward(string placement);
        KeyValuePair<string, double> GetRewardParameters();
        KeyValuePair<string, double> GetRewardParameters(string placement);
        void MuteVideosIfCallsMuted(bool value);
        void ShowTestScreen();
        void SetChildDirectedTreatment(bool value);
        void Destroy(int adType);
        void SetExtraData(string key, bool value);
        void SetExtraData(string key, int value);
        void SetExtraData(string key, double value);
        void SetExtraData(string key, string value);
        void ResetExtraData(string key);
        double GetPredictedEcpm(int adType);
        void LogEvent(string eventName, Dictionary<string, object> eventParams);
        void ValidatePlayStoreInAppPurchase(IPlayStoreInAppPurchase purchase, IInAppPurchaseValidationListener listener);
        void ValidateAppStoreInAppPurchase(IAppStoreInAppPurchase purchase, IInAppPurchaseValidationListener listener);

        #region Deprecated signatures

        void initialize(string appKey, int adTypes);
        void initialize(string appKey, int adTypes, bool hasConsent);
        void initialize(string appKey, int adTypes, IConsent consent);
        void updateConsent(bool value);
        void setSharedAdsInstanceAcrossActivities(bool value);
        void disableLocationPermissionCheck();
        void setUserGender(AppodealUserGender gender);
        void setUserAge(int age);

        #endregion

    }
}
