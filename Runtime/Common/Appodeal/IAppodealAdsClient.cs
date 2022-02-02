using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using AppodealCM.Unity.Platforms;

namespace AppodealAds.Unity.Common
{
    /// <summary>
    /// Interface containing all API methods' signatures.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedParameter.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public interface IAppodealAdsClient
    {
        // void initialize(string appkey, int type, IAppodealInitializeListener listener);
        void initialize(string appKey, int type);
        void initialize(string appKey, int type, bool hasConsent);
        void initialize(string appKey, int type, Consent hasConsent);
        bool isInitialized(int adType);
        void updateConsent(bool value);
        void updateConsent(Consent value);
        bool isAutoCacheEnabled(int adType);
        void setInterstitialCallbacks(IInterstitialAdListener listener);
        void setRewardedVideoCallbacks(IRewardedVideoAdListener listener);
        void setNonSkippableVideoCallbacks(INonSkippableVideoAdListener listener);
        void setBannerCallbacks(IBannerAdListener listener);
        void setMrecCallbacks(IMrecAdListener listener);
        void cache(int adTypes);
        bool show(int adTypes);
        bool show(int adTypes, string placement);
        bool showBannerView(int YAxis, int XGravity, string placement);
        bool showMrecView(int YAxis, int XGravity, string placement);
        void hide(int adTypes);
        void hideBannerView();
        void hideMrecView();
        void setAutoCache(int adTypes, bool autoCache);
        void setTriggerOnLoadedOnPrecache(int adTypes, bool onLoadedTriggerBoth);
        void setSharedAdsInstanceAcrossActivities(bool value);
        bool isLoaded(int adTypes);
        bool isPrecache(int adTypes);
        void setSmartBanners(bool value);
        void setTabletBanners(bool value);
        void setBannerAnimation(bool value);
        void setBannerRotation(int leftBannerRotation, int rightBannerRotation);
        void setUseSafeArea(bool value);
        void trackInAppPurchase(double amount, string currency);
        void disableNetwork(string network);
        void disableNetwork(string network, int type);
        void disableLocationPermissionCheck();
        void setLocationTracking(bool value);
        void getUserSettings();
        void setUserId(string id);
        void setUserGender(AppodealUserGender gender);
        void setUserAge(int age);
        string getVersion();
        void setTesting(bool test);
        void setLogLevel(AppodealLogLevel level);
        void setCustomFilter(string name, bool value);
        void setCustomFilter(string name, int value);
        void setCustomFilter(string name, double value);
        void setCustomFilter(string name, string value);
        bool canShow(int adTypes);
        bool canShow(int adTypes, string placement);
        KeyValuePair<string, double> getRewardParameters();
        KeyValuePair<string, double> getRewardParameters(string placement);
        void muteVideosIfCallsMuted(bool value);
        void showTestScreen();
        void setChildDirectedTreatment(bool value);
        void destroy(int adTypes);
        void setExtraData(string key, bool value);
        void setExtraData(string key, int value);
        void setExtraData(string key, double value);
        void setExtraData(string key, string value);
        double getPredictedEcpm(int adTypes);
        // void validateInAppPurchase();
    }
}
