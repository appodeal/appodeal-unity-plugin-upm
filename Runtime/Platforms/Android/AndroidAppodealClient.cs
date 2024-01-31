using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using AppodealStack.Monetization.Common;

// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Platforms.Android
{
    /// <summary>
    /// Android implementation of <see langword="IAppodealAdsClient"/> interface.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class AndroidAppodealClient : IAppodealAdsClient
    {
        private const int AppodealAdTypeInterstitial = 3;
        private const int AppodealAdTypeBanner = 4;
        private const int AppodealAdTypeRewardedVideo = 128;
        private const int AppodealAdTypeMrec = 256;

        private const int AppodealShowStyleInterstitial = 3;
        private const int AppodealShowStyleRewardedVideo = 128;
        private const int AppodealShowStyleBannerBottom = 8;
        private const int AppodealShowStyleBannerTop = 16;
        private const int AppodealShowStyleBannerLeft = 1024;
        private const int AppodealShowStyleBannerRight = 2048;

        private AndroidJavaClass _appodealClass;
        private AndroidJavaClass _appodealUnityClass;

        private AndroidJavaObject _activity;
        private AndroidJavaObject _appodealBannerInstance;

        private static int NativeYAxisPosForUnityViewPos(int viewPos)
        {
            if (viewPos == AppodealViewPosition.VerticalBottom) return AppodealShowStyleBannerBottom;

            if (viewPos == AppodealViewPosition.VerticalTop) return AppodealShowStyleBannerTop;

            return viewPos;
        }

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

        private AndroidJavaClass GetAppodealClass()
        {
            return _appodealClass ??= new AndroidJavaClass("com.appodeal.ads.Appodeal");
        }

        public AndroidJavaClass GetAppodealUnityClass()
        {
            return _appodealUnityClass ??= new AndroidJavaClass("com.appodeal.unity.AppodealUnity");
        }

        private AndroidJavaObject GetAppodealBannerInstance()
        {
            return _appodealBannerInstance ??= new AndroidJavaClass("com.appodeal.ads.AppodealUnityBannerView").CallStatic<AndroidJavaObject>("getInstance");
        }

        private AndroidJavaObject GetActivity()
        {
            return _activity ??= new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        }

        private void SetCallbacks()
        {
            GetAppodealClass().CallStatic("setMrecCallbacks", new AppodealMrecCallbacks(AppodealCallbacks.Mrec.Instance.MrecAdEventsImpl));
            GetAppodealClass().CallStatic("setBannerCallbacks", new AppodealBannerCallbacks(AppodealCallbacks.Banner.Instance.BannerAdEventsImpl));
            GetAppodealClass().CallStatic("setInterstitialCallbacks", new AppodealInterstitialCallbacks(AppodealCallbacks.Interstitial.Instance.InterstitialAdEventsImpl));
            GetAppodealClass().CallStatic("setRewardedVideoCallbacks", new AppodealRewardedVideoCallbacks(AppodealCallbacks.RewardedVideo.Instance.RewardedVideoAdEventsImpl));
            GetAppodealClass().CallStatic("setAdRevenueCallbacks", new AppodealAdRevenueCallback(AppodealCallbacks.AdRevenue.Instance.AdRevenueEventsImpl));
        }

        private AppodealInitializationCallback GetInitCallback(IAppodealInitializationListener listener)
        {
            AppodealCallbacks.Sdk.Instance.SdkEventsImpl.InitListener = listener;
            return new AppodealInitializationCallback(AppodealCallbacks.Sdk.Instance.SdkEventsImpl);
        }

        private InAppPurchaseValidationCallbacks GetPurchaseCallback(IInAppPurchaseValidationListener listener)
        {
            AppodealCallbacks.InAppPurchase.Instance.PurchaseEventsImpl.Listener = listener;
            return new InAppPurchaseValidationCallbacks(AppodealCallbacks.InAppPurchase.Instance.PurchaseEventsImpl);
        }

        public void Initialize(string appKey, int adTypes, IAppodealInitializationListener listener)
        {
            SetCallbacks();

            GetAppodealClass().CallStatic("setFramework", "unity", $"{AppodealVersions.GetPluginVersion()}-upm", AppodealVersions.GetUnityVersion());
            GetAppodealClass().CallStatic("initialize", GetActivity(), appKey, NativeAdTypesForType(adTypes), GetInitCallback(listener));
        }

        public bool IsInitialized(int adType)
        {
            return GetAppodealClass().CallStatic<bool>("isInitialized", NativeAdTypesForType(adType));
        }

        public bool Show(int adTypes)
        {
            return GetAppodealClass().CallStatic<bool>("show", GetActivity(), NativeShowStyleForType(adTypes));
        }

        public bool Show(int adTypes, string placement)
        {
            return GetAppodealClass().CallStatic<bool>("show", GetActivity(), NativeShowStyleForType(adTypes), placement);
        }

        public bool ShowBannerView(int yAxis, int xAxis, string placement)
        {
            return GetAppodealBannerInstance().Call<bool>("showBannerView", GetActivity(), xAxis, NativeYAxisPosForUnityViewPos(yAxis), placement);
        }

        public bool ShowMrecView(int yAxis, int xAxis, string placement)
        {
            return GetAppodealBannerInstance().Call<bool>("showMrecView", GetActivity(), xAxis, NativeYAxisPosForUnityViewPos(yAxis), placement);
        }

        public bool IsLoaded(int adTypes)
        {
            return GetAppodealClass().CallStatic<bool>("isLoaded", NativeAdTypesForType(adTypes));
        }

        public void Cache(int adTypes)
        {
            GetAppodealClass().CallStatic("cache", GetActivity(), NativeAdTypesForType(adTypes));
        }

        public void Hide(int adTypes)
        {
            GetAppodealClass().CallStatic("hide", GetActivity(), NativeAdTypesForType(adTypes));
        }

        public void HideBannerView()
        {
            GetAppodealBannerInstance().Call("hideBannerView", GetActivity());
        }

        public void HideMrecView()
        {
            GetAppodealBannerInstance().Call("hideMrecView", GetActivity());
        }

        public bool IsPrecache(int adTypes)
        {
            return GetAppodealClass().CallStatic<bool>("isPrecache", NativeAdTypesForType(adTypes));
        }

        public void SetAutoCache(int adTypes, bool autoCache)
        {
            GetAppodealClass().CallStatic("setAutoCache", NativeAdTypesForType(adTypes), autoCache);
        }

        public void SetSmartBanners(bool value)
        {
            GetAppodealClass().CallStatic("setSmartBanners", value);
        }

        public bool IsSmartBannersEnabled()
        {
            return GetAppodealClass().CallStatic<bool>("isSmartBannersEnabled");
        }

        public void SetBannerAnimation(bool value)
        {
            GetAppodealClass().CallStatic("setBannerAnimation", value);
        }

        public void SetTabletBanners(bool value)
        {
            GetAppodealClass().CallStatic("set728x90Banners", value);
        }

        public void SetBannerRotation(int leftBannerRotation, int rightBannerRotation)
        {
            GetAppodealClass().CallStatic("setBannerRotation", leftBannerRotation, rightBannerRotation);
        }

        public void SetTesting(bool test)
        {
            GetAppodealClass().CallStatic("setTesting", test);
        }

        public void SetLogLevel(AppodealLogLevel logging)
        {
            var logLevel = new AndroidJavaClass("com.appodeal.ads.utils.Log$LogLevel");
            switch (logging)
            {
                case AppodealLogLevel.None:
                {
                    GetAppodealClass().CallStatic("setLogLevel", logLevel.CallStatic<AndroidJavaObject>("valueOf", "none"));
                    break;
                }
                case AppodealLogLevel.Debug:
                {
                    GetAppodealClass().CallStatic("setLogLevel", logLevel.CallStatic<AndroidJavaObject>("valueOf", "debug"));
                    break;
                }
                case AppodealLogLevel.Verbose:
                {
                    GetAppodealClass().CallStatic("setLogLevel", logLevel.CallStatic<AndroidJavaObject>("valueOf", "verbose"));
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(logging), logging, null);
            }
        }

        public void SetChildDirectedTreatment(bool value)
        {
            GetAppodealClass().CallStatic("setChildDirectedTreatment", Helper.GetJavaObject(value));
        }

        public void DisableNetwork(string network)
        {
            GetAppodealClass().CallStatic("disableNetwork", network);
        }

        public void DisableNetwork(string network, int adTypes)
        {
            GetAppodealClass().CallStatic("disableNetwork", network, NativeAdTypesForType(adTypes));
        }

        public void SetTriggerOnLoadedOnPrecache(int adTypes, bool onLoadedTriggerBoth)
        {
            GetAppodealClass().CallStatic("setTriggerOnLoadedOnPrecache", NativeAdTypesForType(adTypes), onLoadedTriggerBoth);
        }

        public void MuteVideosIfCallsMuted(bool value)
        {
            GetAppodealClass().CallStatic("muteVideosIfCallsMuted", value);
        }

        public void ShowTestScreen()
        {
            GetAppodealClass().CallStatic("startTestActivity", GetActivity());
        }

        public string GetVersion()
        {
            return GetAppodealClass().CallStatic<string>("getVersion");
        }

        public long GetSegmentId()
        {
            return GetAppodealClass().CallStatic<long>("getSegmentId");
        }

        public bool CanShow(int adTypes)
        {
            return GetAppodealClass().CallStatic<bool>("canShow", NativeAdTypesForType(adTypes));
        }

        public bool CanShow(int adTypes, string placement)
        {
            return GetAppodealClass().CallStatic<bool>("canShow", NativeAdTypesForType(adTypes), placement);
        }

        public void SetCustomFilter(string name, bool value)
        {
            GetAppodealClass().CallStatic("setCustomFilter", name, value);
        }

        public void SetCustomFilter(string name, int value)
        {
            GetAppodealClass().CallStatic("setCustomFilter", name, value);
        }

        public void SetCustomFilter(string name, double value)
        {
            GetAppodealClass().CallStatic("setCustomFilter", name, value);
        }

        public void SetCustomFilter(string name, string value)
        {
            GetAppodealClass().CallStatic("setCustomFilter", name, value);
        }

        public void ResetCustomFilter(string name)
        {
            GetAppodealClass().CallStatic("setCustomFilter", name, null);
        }

        public void SetExtraData(string key, bool value)
        {
            GetAppodealClass().CallStatic("setExtraData", key, value);
        }

        public void SetExtraData(string key, int value)
        {
            GetAppodealClass().CallStatic("setExtraData", key, value);
        }

        public void SetExtraData(string key, double value)
        {
            GetAppodealClass().CallStatic("setExtraData", key, value);
        }

        public void SetExtraData(string key, string value)
        {
            GetAppodealClass().CallStatic("setExtraData", key, value);
        }

        public void ResetExtraData(string key)
        {
            GetAppodealClass().CallStatic("setExtraData", key, null);
        }

        public void TrackInAppPurchase(double amount, string currency)
        {
            GetAppodealClass().CallStatic("trackInAppPurchase", GetActivity(), amount, currency);
        }

        public List<string> GetNetworks(int adTypes)
        {
            var networks = GetAppodealClass().CallStatic<AndroidJavaObject>("getNetworks", NativeAdTypesForType(adTypes));
            int countOfNetworks = networks.Call<int>("size");
            var networksList = new List<string>();
            for(int i = 0; i < countOfNetworks; i++)
            {
                networksList.Add(networks.Call<string>("get", i));
            }
            return networksList;
        }

        public AppodealReward GetReward(string placement)
        {
            var reward = String.IsNullOrEmpty(placement)
                ? GetAppodealClass().CallStatic<AndroidJavaObject>("getReward")
                : GetAppodealClass().CallStatic<AndroidJavaObject>("getReward", placement);

            return new AppodealReward()
            {
                Amount = reward.Call<double>("getAmount"),
                Currency = reward.Call<string>("getCurrency")
            };
        }

        public double GetPredictedEcpm(int adType)
        {
            return GetAppodealClass().CallStatic<double>("getPredictedEcpm", NativeAdTypesForType(adType));
        }

        public double GetPredictedEcpmForPlacement(int adType, string placement)
        {
            return String.IsNullOrEmpty(placement)
                ? GetAppodealClass().CallStatic<double>("getPredictedEcpmByPlacement", NativeAdTypesForType(adType))
                : GetAppodealClass().CallStatic<double>("getPredictedEcpmByPlacement", NativeAdTypesForType(adType), placement);
        }

        public void Destroy(int adTypes)
        {
            GetAppodealClass().CallStatic("destroy", NativeAdTypesForType(adTypes));
        }

        public void SetUserId(string id)
        {
            GetAppodealClass().CallStatic("setUserId", id);
        }

        public string GetUserId()
        {
            return GetAppodealClass().CallStatic<string>("getUserId");
        }

        public void SetInterstitialCallbacks(IInterstitialAdListener listener)
        {
            AppodealCallbacks.Interstitial.Instance.InterstitialAdEventsImpl.Listener = listener;
        }

        public void SetRewardedVideoCallbacks(IRewardedVideoAdListener listener)
        {
            AppodealCallbacks.RewardedVideo.Instance.RewardedVideoAdEventsImpl.Listener = listener;
        }

        public void SetBannerCallbacks(IBannerAdListener listener)
        {
            AppodealCallbacks.Banner.Instance.BannerAdEventsImpl.Listener = listener;
        }

        public void SetMrecCallbacks(IMrecAdListener listener)
        {
            AppodealCallbacks.Mrec.Instance.MrecAdEventsImpl.Listener = listener;
        }

        public void SetAdRevenueCallback(IAdRevenueListener listener)
        {
            AppodealCallbacks.AdRevenue.Instance.AdRevenueEventsImpl.Listener = listener;
        }

        public void setSharedAdsInstanceAcrossActivities(bool value)
        {
            GetAppodealClass().CallStatic("setSharedAdsInstanceAcrossActivities", value);
        }

        public void SetUseSafeArea(bool value)
        {
            GetAppodealClass().CallStatic("setUseSafeArea", value);
        }

        public bool IsAutoCacheEnabled(int adType)
        {
            return GetAppodealClass().CallStatic<bool>("isAutoCacheEnabled", NativeAdTypesForType(adType));
        }

        public void LogEvent(string eventName, Dictionary<string, object> eventParams)
        {
            if (eventParams == null)
            {
                GetAppodealClass().CallStatic("logEvent", eventName, null);
            }
            else
            {
                var paramsFiltered = new Dictionary<string, object>();

                eventParams.Keys.Where(key => eventParams[key] is int || eventParams[key] is double || eventParams[key] is string)
                    .ToList().ForEach(key => paramsFiltered.Add(key, eventParams[key]));

                var map = new AndroidJavaObject("java.util.HashMap");

                foreach (var entry in paramsFiltered)
                {
                    map.Call<AndroidJavaObject>("put", entry.Key, Helper.GetJavaObject(entry.Value));
                }

                GetAppodealClass().CallStatic("logEvent", eventName, map);
            }
        }

        public void ValidatePlayStoreInAppPurchase(IPlayStoreInAppPurchase purchase, IInAppPurchaseValidationListener listener)
        {
            var androidPurchase = purchase.NativeInAppPurchase as AndroidPlayStoreInAppPurchase;
            if (androidPurchase == null) return;
            GetAppodealClass().CallStatic("validateInAppPurchase", GetActivity(), androidPurchase.GetInAppPurchase(), GetPurchaseCallback(listener));
        }

        public void SetLocationTracking(bool value)
        {
            Debug.Log("Not supported on Android platform");
        }

        public void ValidateAppStoreInAppPurchase(IAppStoreInAppPurchase purchase, IInAppPurchaseValidationListener listener)
        {
            Debug.Log("Not supported on Android platform");
        }
    }

    public static class Helper
    {
        public static object GetJavaObject(object value)
        {
            if (!(value is bool) && !(value is char) && !(value is int) && !(value is long) && !(value is float) && !(value is double) && !(value is string))
            {
                Debug.LogError($"[Appodeal Unity Plugin] Conversion of {value.GetType()} type to java is not implemented");
            }

            return value switch
            {
                bool _ => new AndroidJavaObject("java.lang.Boolean", value),
                char _ => new AndroidJavaObject("java.lang.Character", value),
                int _ => new AndroidJavaObject("java.lang.Integer", value),
                long _ => new AndroidJavaObject("java.lang.Long", value),
                float _ => new AndroidJavaObject("java.lang.Float", value),
                double _ => new AndroidJavaObject("java.lang.Double", value),
                string _ => value,
                _ => null
            };
        }
    }
}
