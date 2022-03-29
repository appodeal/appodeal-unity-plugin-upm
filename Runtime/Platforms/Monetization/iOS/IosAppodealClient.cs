using AOT;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AppodealStack.Monetization.Common;
using AppodealStack.ConsentManagement.Api;

// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Platforms.Ios
{
    /// <summary>
    /// iOS implementation of <see langword="IAppodealAdsClient"/> interface.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class IosAppodealClient : IAppodealAdsClient
    {
        private const int AppodealAdTypeInterstitial = 1 << 0;
        private const int AppodealAdTypeBanner = 1 << 2;
        private const int AppodealAdTypeRewardedVideo = 1 << 4;
        private const int AppodealAdTypeMrec = 1 << 5;

        private const int AppodealShowStyleInterstitial = 1 << 0;
        private const int AppodealShowStyleBannerTop = 1 << 2;
        private const int AppodealShowStyleBannerBottom = 1 << 3;
        private const int AppodealShowStyleRewardedVideo = 1 << 4;
        private const int AppodealShowStyleBannerLeft = 1 << 6;
        private const int AppodealShowStyleBannerRight = 1 << 7;

        private static IMrecAdListener                      _mrecListener;
        private static IBannerAdListener                    _bannerListener;
        private static IInterstitialAdListener              _interstitialListener;
        private static IRewardedVideoAdListener             _rewardedVideoListener;
        private static IAppodealInitializationListener      _initializationListener;
        private static IInAppPurchaseValidationListener     _inAppPurchaseValidationListener;


        #region AppodealInitialization delegate

        [MonoPInvokeCallback(typeof(AppodealInitializationCallback))]
        private static void AppodealSdkDidInitialize()
        {
            _initializationListener?.OnInitializationFinished(null);
        }

        private void SetAppodealInitializationCallback(IAppodealInitializationListener listener)
        {
            _initializationListener = listener;

            AppodealObjCBridge.AppodealSetInitializationDelegate(AppodealSdkDidInitialize);
        }

        #endregion

        #region Interstitial Delegate

        [MonoPInvokeCallback(typeof(AppodealInterstitialDidLoadCallback))]
        private static void InterstitialDidLoad(bool isPrecache)
        {
            _interstitialListener?.OnInterstitialLoaded(isPrecache);
        }

        [MonoPInvokeCallback(typeof(AppodealInterstitialCallbacks))]
        private static void InterstitialDidFailToLoad()
        {
            _interstitialListener?.OnInterstitialFailedToLoad();
        }

        [MonoPInvokeCallback(typeof(AppodealInterstitialCallbacks))]
        private static void InterstitialDidFailToPresent()
        {
            _interstitialListener?.OnInterstitialShowFailed();
        }

        [MonoPInvokeCallback(typeof(AppodealInterstitialCallbacks))]
        private static void InterstitialDidClick()
        {
            _interstitialListener?.OnInterstitialClicked();
        }

        [MonoPInvokeCallback(typeof(AppodealInterstitialCallbacks))]
        public static void InterstitialDidDismiss()
        {
            _interstitialListener?.OnInterstitialClosed();
        }

        [MonoPInvokeCallback(typeof(AppodealInterstitialCallbacks))]
        private static void InterstitialWillPresent()
        {
            _interstitialListener?.OnInterstitialShown();
        }

        [MonoPInvokeCallback(typeof(AppodealInterstitialCallbacks))]
        private static void InterstitialDidExpired()
        {
            _interstitialListener?.OnInterstitialExpired();
        }

        public void SetInterstitialCallbacks(IInterstitialAdListener listener)
        {
            _interstitialListener = listener;

            AppodealObjCBridge.AppodealSetInterstitialDelegate(
                InterstitialDidLoad,
                InterstitialDidFailToLoad,
                InterstitialDidFailToPresent,
                InterstitialWillPresent,
                InterstitialDidDismiss,
                InterstitialDidClick,
                InterstitialDidExpired
            );
        }

        #endregion

        #region Rewarded Video Delegate

        [MonoPInvokeCallback(typeof(AppodealRewardedVideoDidLoadCallback))]
        private static void RewardedVideoDidLoadAd(bool isPrecache)
        {
            _rewardedVideoListener?.OnRewardedVideoLoaded(isPrecache);
        }

        [MonoPInvokeCallback(typeof(AppodealRewardedVideoCallbacks))]
        private static void RewardedVideoDidFailToLoadAd()
        {
            _rewardedVideoListener?.OnRewardedVideoFailedToLoad();
        }

        [MonoPInvokeCallback(typeof(AppodealRewardedVideoCallbacks))]
        private static void RewardedVideoDidFailToPresentWithError()
        {
            _rewardedVideoListener?.OnRewardedVideoShowFailed();
        }

        [MonoPInvokeCallback(typeof(AppodealRewardedVideoDidDismissCallback))]
        private static void RewardedVideoWillDismiss(bool isFinished)
        {
            _rewardedVideoListener?.OnRewardedVideoClosed(isFinished);
        }

        [MonoPInvokeCallback(typeof(AppodealRewardedVideoDidFinishCallback))]
        private static void RewardedVideoDidFinish(double amount, string name)
        {
            _rewardedVideoListener?.OnRewardedVideoFinished(amount, name);
        }

        [MonoPInvokeCallback(typeof(AppodealRewardedVideoCallbacks))]
        private static void RewardedVideoDidPresent()
        {
            _rewardedVideoListener?.OnRewardedVideoShown();
        }

        [MonoPInvokeCallback(typeof(AppodealRewardedVideoCallbacks))]
        private static void RewardedVideoDidExpired()
        {
            _rewardedVideoListener?.OnRewardedVideoExpired();
        }

        [MonoPInvokeCallback(typeof(AppodealRewardedVideoCallbacks))]
        private static void RewardedVideoDidReceiveTap()
        {
            _rewardedVideoListener?.OnRewardedVideoClicked();
        }

        public void SetRewardedVideoCallbacks(IRewardedVideoAdListener listener)
        {
            _rewardedVideoListener = listener;

            AppodealObjCBridge.AppodealSetRewardedVideoDelegate(
                RewardedVideoDidLoadAd,
                RewardedVideoDidFailToLoadAd,
                RewardedVideoDidFailToPresentWithError,
                RewardedVideoWillDismiss,
                RewardedVideoDidFinish,
                RewardedVideoDidPresent,
                RewardedVideoDidExpired,
                RewardedVideoDidReceiveTap
            );
        }

        #endregion

        #region Banner Delegate

        [MonoPInvokeCallback(typeof(AppodealBannerDidLoadCallback))]
        private static void BannerDidLoadAd(int height, bool isPrecache)
        {
            _bannerListener?.OnBannerLoaded(height, isPrecache);
        }

        [MonoPInvokeCallback(typeof(AppodealBannerCallbacks))]
        private static void BannerDidFailToLoadAd()
        {
            _bannerListener?.OnBannerFailedToLoad();
        }

        [MonoPInvokeCallback(typeof(AppodealBannerCallbacks))]
        private static void BannerDidClick()
        {
            _bannerListener?.OnBannerClicked();
        }

        [MonoPInvokeCallback(typeof(AppodealBannerCallbacks))]
        private static void BannerDidExpired()
        {
            _bannerListener?.OnBannerExpired();
        }

        [MonoPInvokeCallback(typeof(AppodealBannerCallbacks))]
        private static void BannerDidShow()
        {
            _bannerListener?.OnBannerShown();
        }

        [MonoPInvokeCallback(typeof(AppodealBannerCallbacks))]
        private static void BannerDidFailToPresent()
        {
            _bannerListener?.OnBannerShowFailed();
        }



        [MonoPInvokeCallback(typeof(AppodealBannerViewDidLoadCallback))]
        private static void BannerViewDidLoadAd(int height, bool isPrecache)
        {
            _bannerListener?.OnBannerLoaded(height, isPrecache);
        }

        [MonoPInvokeCallback(typeof(AppodealBannerViewCallbacks))]
        private static void BannerViewDidFailToLoadAd()
        {
            _bannerListener?.OnBannerFailedToLoad();
        }

        [MonoPInvokeCallback(typeof(AppodealBannerViewCallbacks))]
        private static void BannerViewDidClick()
        {
            _bannerListener?.OnBannerClicked();
        }

        [MonoPInvokeCallback(typeof(AppodealBannerViewCallbacks))]
        private static void BannerViewDidShow()
        {
            _bannerListener?.OnBannerShown();
        }

        [MonoPInvokeCallback(typeof(AppodealBannerViewCallbacks))]
        private static void BannerViewDidFailToPresent()
        {
            _bannerListener?.OnBannerShowFailed();
        }

        [MonoPInvokeCallback(typeof(AppodealBannerViewCallbacks))]
        private static void BannerViewDidExpired()
        {
            _bannerListener?.OnBannerExpired();
        }

        public void SetBannerCallbacks(IBannerAdListener listener)
        {
            _bannerListener = listener;

            AppodealObjCBridge.AppodealSetBannerDelegate(
                BannerDidLoadAd,
                BannerDidFailToLoadAd,
                BannerDidClick,
                BannerDidExpired,
                BannerDidShow,
                BannerDidFailToPresent);

            AppodealObjCBridge.AppodealSetBannerViewDelegate(
                BannerViewDidLoadAd,
                BannerViewDidFailToLoadAd,
                BannerViewDidClick,
                BannerViewDidShow,
                BannerViewDidFailToPresent,
                BannerViewDidExpired);
        }

        #endregion

        #region Mrec Delegate

        [MonoPInvokeCallback(typeof(AppodealMrecViewDidLoadCallback))]
        private static void MrecViewDidLoadAd(bool isPrecache)
        {
            _mrecListener?.OnMrecLoaded(isPrecache);
        }

        [MonoPInvokeCallback(typeof(AppodealMrecViewCallbacks))]
        private static void MrecViewDidFailToLoadAd()
        {
            _mrecListener?.OnMrecFailedToLoad();
        }

        [MonoPInvokeCallback(typeof(AppodealMrecViewCallbacks))]
        private static void MrecViewDidClick()
        {
            _mrecListener?.OnMrecClicked();
        }

        [MonoPInvokeCallback(typeof(AppodealMrecViewCallbacks))]
        private static void MrecViewDidShow()
        {
            _mrecListener?.OnMrecShown();
        }

        [MonoPInvokeCallback(typeof(AppodealMrecViewCallbacks))]
        private static void MrecViewDidFailToPresent()
        {
            _mrecListener?.OnMrecShowFailed();
        }

        [MonoPInvokeCallback(typeof(AppodealMrecViewCallbacks))]
        private static void MrecViewDidExpired()
        {
            _mrecListener?.OnMrecExpired();
        }

        public void SetMrecCallbacks(IMrecAdListener listener)
        {
            _mrecListener = listener;

            AppodealObjCBridge.AppodealSetMrecViewDelegate(
                MrecViewDidLoadAd,
                MrecViewDidFailToLoadAd,
                MrecViewDidClick,
                MrecViewDidShow,
                MrecViewDidFailToPresent,
                MrecViewDidExpired);
        }

        #endregion

        #region In-App Purchase Validation delegate

        [MonoPInvokeCallback(typeof(InAppPurchaseValidationSucceededCallback))]
        private static void inAppPurchaseValidationSucceeded(string json)
        {
            _inAppPurchaseValidationListener?.OnInAppPurchaseValidationSucceeded(json);
        }

        [MonoPInvokeCallback(typeof(InAppPurchaseValidationFailedCallback))]
        private static void inAppPurchaseValidationFailed(string error)
        {
            _inAppPurchaseValidationListener?.OnInAppPurchaseValidationFailed(error);
        }

        #endregion

        private static int NativeAdTypesForType(int adTypes)
        {
            var nativeAdTypes = 0;

            if ((adTypes & AppodealAdType.Interstitial) > 0)
            {
                nativeAdTypes |= AppodealAdTypeInterstitial;
            }

            if ((adTypes & AppodealAdType.Banner) > 0)
            {
                nativeAdTypes |= AppodealAdTypeBanner;
            }

            if ((adTypes & AppodealAdType.Mrec) > 0)
            {
                nativeAdTypes |= AppodealAdTypeMrec;
            }

            if ((adTypes & AppodealAdType.RewardedVideo) > 0)
            {
                nativeAdTypes |= AppodealAdTypeRewardedVideo;
            }

            return nativeAdTypes;
        }

        private static int NativeShowStyleForType(int adTypes)
        {
            if ((adTypes & AppodealShowStyle.Interstitial) > 0)
            {
                return AppodealShowStyleInterstitial;
            }

            if ((adTypes & AppodealShowStyle.BannerTop) > 0)
            {
                return AppodealShowStyleBannerTop;
            }

            if ((adTypes & AppodealShowStyle.BannerBottom) > 0)
            {
                return AppodealShowStyleBannerBottom;
            }

            if ((adTypes & AppodealShowStyle.BannerLeft) > 0)
            {
                return AppodealShowStyleBannerLeft;
            }

            if ((adTypes & AppodealShowStyle.BannerRight) > 0)
            {
                return AppodealShowStyleBannerRight;
            }

            if ((adTypes & AppodealShowStyle.RewardedVideo) > 0)
            {
                return AppodealShowStyleRewardedVideo;
            }

            return 0;
        }

        private static int NativeStyleForIsReady(int adTypes)
        {
            if ((adTypes & AppodealAdType.Interstitial) > 0)
            {
                return AppodealShowStyleInterstitial;
            }

            if ((adTypes & AppodealAdType.Banner) > 0)
            {
                return AppodealShowStyleBannerBottom;
            }

            if ((adTypes & AppodealAdType.RewardedVideo) > 0)
            {
                return AppodealShowStyleRewardedVideo;
            }

            return 0;
        }

        private static string DictionaryToString(Dictionary <string, object> dictionary) {  
            var dictionaryString = dictionary.Aggregate("", (current, keyValues)
                => current + (keyValues.Key + "=" + keyValues.Value.GetType() + ":" + keyValues.Value + ","));
            return dictionaryString.TrimEnd(',');
        }

        public void Initialize(string appKey, int adTypes, IAppodealInitializationListener listener)
        {
            if (listener != null) SetAppodealInitializationCallback(listener);

            AppodealObjCBridge.AppodealInitialize(appKey, NativeAdTypesForType(adTypes),
                $"{AppodealVersions.GetPluginVersion()}-upm", AppodealVersions.GetUnityVersion());
        }

        public void initialize(string appKey, int adTypes)
        {
            initialize(appKey, adTypes, true);
        }

        public void initialize(string appKey, int adTypes, bool hasConsent)
        {
            AppodealObjCBridge.AppodealInitialize(appKey, NativeAdTypesForType(adTypes), hasConsent,
                $"{AppodealVersions.GetPluginVersion()}-upm", AppodealVersions.GetUnityVersion());
        }

        public void initialize(string appKey, int adTypes, Consent consent)
        {
            AppodealObjCBridge.AppodealInitializeWithConsent(appKey, NativeAdTypesForType(adTypes),
                $"{AppodealVersions.GetPluginVersion()}-upm", AppodealVersions.GetUnityVersion());
        }

        public bool IsInitialized(int adType)
        {
            return AppodealObjCBridge.AppodealIsInitialized(NativeAdTypesForType(adType));
        }

        public bool Show(int adTypes)
        {
            return AppodealObjCBridge.AppodealShowAd(NativeShowStyleForType(adTypes));
        }

        public bool Show(int adTypes, string placement)
        {
            return AppodealObjCBridge.AppodealShowAdForPlacement(NativeShowStyleForType(adTypes), placement);
        }

        public bool ShowBannerView(int yAxis, int xGravity, string placement)
        {
            return AppodealObjCBridge.AppodealShowBannerAdViewForPlacement(yAxis, xGravity, placement);
        }

        public bool ShowMrecView(int YAxis, int XGravity, string Placement)
        {
            return AppodealObjCBridge.AppodealShowMrecAdViewForPlacement(YAxis, XGravity, Placement);
        }

        public bool IsLoaded(int adTypes)
        {
            return AppodealObjCBridge.AppodealIsReadyWithStyle(NativeStyleForIsReady(adTypes));
        }

        public void Cache(int adTypes)
        {
            AppodealObjCBridge.AppodealCacheAd(NativeAdTypesForType(adTypes));
        }

        public void SetAutoCache(int adTypes, bool autoCache)
        {
            AppodealObjCBridge.AppodealSetAutoCache(autoCache, NativeAdTypesForType(adTypes));
        }

        public void Hide(int adTypes)
        {
            if ((NativeAdTypesForType(adTypes) & AppodealAdTypeBanner) > 0)
            {
                AppodealObjCBridge.AppodealHideBanner();
            }
        }

        public void HideBannerView()
        {
            AppodealObjCBridge.AppodealHideBannerView();
        }

        public void HideMrecView()
        {
            AppodealObjCBridge.AppodealHideMrecView();
        }

        public bool IsPrecache(int adTypes)
        {
            return AppodealObjCBridge.AppodealIsPrecacheAd(NativeAdTypesForType(adTypes));
        }

        public void SetSmartBanners(bool value)
        {
            AppodealObjCBridge.AppodealSetSmartBanners(value);
        }

        public bool IsSmartBannersEnabled()
        {
            return AppodealObjCBridge.AppodealIsSmartBannersEnabled();
        }

        public void SetBannerAnimation(bool value)
        {
            AppodealObjCBridge.AppodealSetBannerAnimation(value);
        }

        public void SetTabletBanners(bool value)
        {
            AppodealObjCBridge.AppodealSetTabletBanners(value);
        }

        public void SetBannerRotation(int leftBannerRotation, int rightBannerRotation)
        {
            AppodealObjCBridge.AppodealSetBannerRotation(leftBannerRotation, rightBannerRotation);
        }

        public void SetTesting(bool test)
        {
            AppodealObjCBridge.AppodealSetTestingEnabled(test);
        }

        public void SetLogLevel(AppodealLogLevel level)
        {
            switch (level)
            {
                case AppodealLogLevel.None:
                {
                    AppodealObjCBridge.AppodealSetLogLevel(1);
                    break;
                }
                case AppodealLogLevel.Debug:
                {
                    AppodealObjCBridge.AppodealSetLogLevel(2);
                    break;
                }
                case AppodealLogLevel.Verbose:
                {
                    AppodealObjCBridge.AppodealSetLogLevel(3);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }
        }

        public void SetChildDirectedTreatment(bool value)
        {
            AppodealObjCBridge.AppodealSetChildDirectedTreatment(value);
        }

        public void updateConsent(bool value)
        {
            AppodealObjCBridge.AppodealUpdateConsent(value);
        }

        public void UpdateConsent(Consent consent)
        {
            AppodealObjCBridge.AppodealUpdateConsentReport();
        }

        public void UpdateConsentGdpr(GdprUserConsent consent)
        {
            switch (consent)
            {
                case GdprUserConsent.Unknown:
                {
                    AppodealObjCBridge.AppodealUpdateConsentGDPR(0);
                    break;
                }
                case GdprUserConsent.Personalized:
                {
                    AppodealObjCBridge.AppodealUpdateConsentGDPR(1);
                    break;
                }
                case GdprUserConsent.NonPersonalized:
                {
                    AppodealObjCBridge.AppodealUpdateConsentGDPR(2);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(consent), consent, null);
            }
        }

        public void UpdateConsentCcpa(CcpaUserConsent consent)
        {
            switch (consent)
            {
                case CcpaUserConsent.Unknown:
                {
                    AppodealObjCBridge.AppodealUpdateConsentCCPA(0);
                    break;
                }
                case CcpaUserConsent.OptIn:
                {
                    AppodealObjCBridge.AppodealUpdateConsentCCPA(1);
                    break;
                }
                case CcpaUserConsent.OptOut:
                {
                    AppodealObjCBridge.AppodealUpdateConsentCCPA(2);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(consent), consent, null);
            }
        }

        public List<string> GetNetworks(int adTypes)
        {
            var networksList = new List<string>();
            string [] splitNetworkStrings = AppodealObjCBridge.AppodealGetNetworks(NativeAdTypesForType(adTypes)).Split(',');
            splitNetworkStrings.ToList().ForEach(network => networksList.Add(network));
            return networksList;
        }

        public void DisableNetwork(string network)
        {
            AppodealObjCBridge.AppodealDisableNetwork(network);
        }

        public void DisableNetwork(string network, int adTypes)
        {
            AppodealObjCBridge.AppodealDisableNetworkForAdTypes(network, NativeAdTypesForType(adTypes));
        }

        public void SetLocationTracking(bool value)
        {
            AppodealObjCBridge.AppodealSetLocationTracking(value);
        }

        public void disableLocationPermissionCheck()
        {
            AppodealObjCBridge.AppodealDisableLocationPermissionCheck();
        }

        public string GetVersion()
        {
            return AppodealObjCBridge.AppodealGetVersion();
        }

        public long GetSegmentId()
        {
            return AppodealObjCBridge.AppodealGetSegmentId();
        }

        public bool CanShow(int adTypes, string placement)
        {
            return AppodealObjCBridge.AppodealCanShowWithPlacement(NativeAdTypesForType(adTypes), placement);
        }

        public bool CanShow(int adTypes)
        {
            return AppodealObjCBridge.AppodealCanShow(NativeAdTypesForType(adTypes));
        }

        public KeyValuePair<string, double> GetRewardParameters()
        {
            string currency = AppodealObjCBridge.AppodealGetRewardCurrency("default");
            double amount = AppodealObjCBridge.AppodealGetRewardAmount("default");
            return new KeyValuePair<string, double>(currency, amount);
        }

        public KeyValuePair<string, double> GetRewardParameters(string placement)
        {
            string currency = AppodealObjCBridge.AppodealGetRewardCurrency(placement);
            double amount = AppodealObjCBridge.AppodealGetRewardAmount(placement);
            return new KeyValuePair<string, double>(currency, amount);
        }

        public double GetPredictedEcpm(int adType)
        {
            return AppodealObjCBridge.AppodealGetPredictedEcpm(NativeAdTypesForType(adType));
        }

        public void SetCustomFilter(string name, bool value)
        {
            AppodealObjCBridge.AppodealSetCustomFilterBool(name, value);
        }

        public void SetCustomFilter(string name, int value)
        {
            AppodealObjCBridge.AppodealSetCustomFilterInt(name, value);
        }

        public void SetCustomFilter(string name, double value)
        {
            AppodealObjCBridge.AppodealSetCustomFilterDouble(name, value);
        }

        public void SetCustomFilter(string name, string value)
        {
            AppodealObjCBridge.AppodealSetCustomFilterString(name, value);
        }

        public void SetCustomFilter(Dictionary<string, object> filters)
        {
            var data = new Dictionary<string, object>();

            filters.Keys.Where(key => filters[key] is System.Int32 || filters[key] is System.Boolean || filters[key] is System.String || filters[key] is System.Double)
                .ToList().ForEach(key => data.Add(key, filters[key]));

            AppodealObjCBridge.AppodealSetCustomFilter(DictionaryToString(data));
        }

        public void SetExtraData(string key, bool value)
        {
            AppodealObjCBridge.AppodealSetExtraDataBool(key, value);
        }

        public void SetExtraData(string key, int value)
        {
            AppodealObjCBridge.AppodealSetExtraDataInt(key, value);
        }

        public void SetExtraData(string key, double value)
        {
            AppodealObjCBridge.AppodealSetExtraDataDouble(key, value);
        }

        public void SetExtraData(string key, string value)
        {
            AppodealObjCBridge.AppodealSetExtraDataString(key, value);
        }

        public void SetExtraData(Dictionary<string, object> extraData)
        {
            var data = new Dictionary<string, object>();

            extraData.Keys.Where(key => extraData[key] is System.Int32 || extraData[key] is System.Boolean || extraData[key] is System.String || extraData[key] is System.Double)
                .ToList().ForEach(key => data.Add(key, extraData[key]));

            AppodealObjCBridge.AppodealSetExtraData(DictionaryToString(data));
        }

        public void SetTriggerOnLoadedOnPrecache(int adTypes, bool onLoadedTriggerBoth)
        {
            AppodealObjCBridge.AppodealSetTriggerPrecacheCallbacks(onLoadedTriggerBoth);
        }

        public bool IsAutoCacheEnabled(int adType)
        {
            return AppodealObjCBridge.AppodealIsAutoCacheEnabled(NativeAdTypesForType(adType));
        }

        public void TrackInAppPurchase(double amount, string currency)
        {
            AppodealObjCBridge.AppodealTrackInAppPurchase(amount, currency);
        }

        public void LogEvent(string eventName, Dictionary<string, object> eventParams)
        {
            var paramsFiltered = new Dictionary<string, object>();

            eventParams.Keys.Where(key => eventParams[key] is System.Int32 || eventParams[key] is System.Double || eventParams[key] is System.String)
                .ToList().ForEach(key => paramsFiltered.Add(key, eventParams[key]));

            AppodealObjCBridge.AppodealLogEvent(eventName, DictionaryToString(paramsFiltered));
        }

        public void ValidateInAppPurchaseIos(string productIdentifier, string price, string currency, string transactionId ,string additionalParams, IosPurchaseType type, IInAppPurchaseValidationListener listener)
        {
            _inAppPurchaseValidationListener = listener;
            AppodealObjCBridge.AppodealValidateInAppPurchase(productIdentifier, price, currency, transactionId, additionalParams, (int) type, inAppPurchaseValidationSucceeded, inAppPurchaseValidationFailed);
        }

        public void Destroy(int adType)
        {
            Debug.Log("Not Supported by iOS SDK");
        }

        public void setSharedAdsInstanceAcrossActivities(bool value)
        {
            Debug.Log("Not Supported by iOS SDK");
        }

        public void SetUseSafeArea(bool value)
        {
            Debug.Log("Not Supported by iOS SDK");
        }

        public void ValidateInAppPurchaseAndroid(IInAppPurchase purchase, IInAppPurchaseValidationListener listener)
        {
            Debug.Log("Not Supported by iOS SDK");
        }

        public void MuteVideosIfCallsMuted(bool value)
        {
            // Not supported for iOS SDK
        }

        public void ShowTestScreen()
        {
            // Not supported for iOS SDK
        }

        #region User Settings

        public void SetUserId(string id)
        {
            AppodealObjCBridge.AppodealSetUserId(id);
        }

        public string GetUserId()
        {
            return AppodealObjCBridge.AppodealGetUserId();
        }

        public void setUserAge(int age)
        {
            AppodealObjCBridge.AppodealSetUserAge(age);
        }

        public void setUserGender(AppodealUserGender gender)
        {
            switch (gender)
            {
                case AppodealUserGender.Other:
                {
                    AppodealObjCBridge.AppodealSetUserGender(0);
                    break;
                }
                case AppodealUserGender.Male:
                {
                    AppodealObjCBridge.AppodealSetUserGender(1);
                    break;
                }
                case AppodealUserGender.Female:
                {
                    AppodealObjCBridge.AppodealSetUserGender(2);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(gender), gender, null);
            }
        }

        #endregion

    }
}