// ReSharper disable CheckNamespace

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;
using AppodealStack.Monetization.Common;

namespace AppodealStack.Monetization.Platforms.Android
{
    /// <summary>
    /// Android implementation of the <see cref="AppodealStack.Monetization.Common.IAppodealAdsClient"/> interface.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    internal class AndroidAppodealClient : IAppodealAdsClient
    {
        private AndroidJavaClass _appodealJavaClass;

        private AndroidJavaObject _unityActivityJavaObject;
        private AndroidJavaObject _appodealBannerJavaObject;

        private AndroidJavaClass AppodealJavaClass
        {
            get
            {
                if (_appodealJavaClass != null) return _appodealJavaClass;

                try
                {
                    _appodealJavaClass = new AndroidJavaClass(AndroidConstants.JavaClassName.Appodeal);
                }
                catch (Exception e)
                {
                    AndroidAppodealHelper.LogIntegrationError(e.Message);
                    _appodealJavaClass = null;
                }

                return _appodealJavaClass;
            }
        }

        private AndroidJavaObject UnityActivityJavaObject
        {
            get
            {
                if (_unityActivityJavaObject != null) return _unityActivityJavaObject;

                try
                {
                    using var unityPlayerJavaClass = new AndroidJavaClass(AndroidConstants.JavaClassName.UnityPlayer);
                    _unityActivityJavaObject = unityPlayerJavaClass.GetStatic<AndroidJavaObject>(AndroidConstants.JavaFieldName.UnityPlayerCurrentActivity);
                }
                catch (Exception e)
                {
                    AndroidAppodealHelper.LogIntegrationError(e.Message);
                    _unityActivityJavaObject = null;
                }

                return _unityActivityJavaObject;
            }
        }

        private AndroidJavaObject AppodealBannerJavaObject
        {
            get
            {
                if (_appodealBannerJavaObject != null) return _appodealBannerJavaObject;

                try
                {
                    using var appodealBannerViewJavaClass = new AndroidJavaClass(AndroidConstants.JavaClassName.AppodealBannerView);
                    _appodealBannerJavaObject = appodealBannerViewJavaClass.CallStatic<AndroidJavaObject>(AndroidConstants.JavaMethodName.AppodealBannerView.GetInstance);
                }
                catch (Exception e)
                {
                    AndroidAppodealHelper.LogIntegrationError(e.Message);
                    _appodealBannerJavaObject = null;
                }

                return _appodealBannerJavaObject;
            }
        }

        public void Initialize(string appKey, int adTypes, IAppodealInitializationListener listener)
        {
            if (AppodealJavaClass == null || UnityActivityJavaObject == null) return;

            AppodealCallbacks.Sdk.Instance.SdkEventsImpl.InitListener = listener;

            AppodealJavaClass.CallStatic(AndroidConstants.JavaMethodName.Appodeal.SetFramework, AndroidConstants.FrameworkName, $"{AppodealVersions.GetPluginVersion()}-upm", AppodealVersions.GetUnityVersion());

            AppodealJavaClass.CallStatic(AndroidConstants.JavaMethodName.Appodeal.SetMrecCallbacks, new AppodealMrecCallbacks(AppodealCallbacks.Mrec.Instance.MrecAdEventsImpl));
            AppodealJavaClass.CallStatic(AndroidConstants.JavaMethodName.Appodeal.SetBannerCallbacks, new AppodealBannerCallbacks(AppodealCallbacks.Banner.Instance.BannerAdEventsImpl));
            AppodealJavaClass.CallStatic(AndroidConstants.JavaMethodName.Appodeal.SetInterstitialCallbacks, new AppodealInterstitialCallbacks(AppodealCallbacks.Interstitial.Instance.InterstitialAdEventsImpl));
            AppodealJavaClass.CallStatic(AndroidConstants.JavaMethodName.Appodeal.SetRewardedVideoCallbacks, new AppodealRewardedVideoCallbacks(AppodealCallbacks.RewardedVideo.Instance.RewardedVideoAdEventsImpl));
            AppodealJavaClass.CallStatic(AndroidConstants.JavaMethodName.Appodeal.SetAdRevenueCallbacks, new AppodealAdRevenueCallback(AppodealCallbacks.AdRevenue.Instance.AdRevenueEventsImpl));
            AppodealJavaClass.CallStatic(AndroidConstants.JavaMethodName.Appodeal.SetPurchaseListener, new AppodealPurchaseCallbacks(AppodealCallbacks.Purchase.Instance.PurchaseEventsImpl));

            int javaAdTypes = AndroidAppodealHelper.GetJavaAdTypes(adTypes);
            var initCallback = new AppodealInitializationCallback(AppodealCallbacks.Sdk.Instance.SdkEventsImpl);
            AppodealJavaClass.CallStatic(AndroidConstants.JavaMethodName.Appodeal.Initialize, UnityActivityJavaObject, appKey, javaAdTypes, initCallback);
        }

        public bool IsInitialized(int adType)
        {
            return AppodealJavaClass?.CallStatic<bool>(AndroidConstants.JavaMethodName.Appodeal.IsInitialized, AndroidAppodealHelper.GetJavaAdTypes(adType)) ?? false;
        }

        public bool Show(int showStyle)
        {
            if (AppodealJavaClass == null || UnityActivityJavaObject == null) return false;
            return AppodealJavaClass.CallStatic<bool>(AndroidConstants.JavaMethodName.Appodeal.Show, UnityActivityJavaObject, AndroidAppodealHelper.GetJavaShowStyle(showStyle));
        }

        public bool Show(int showStyle, string placement)
        {
            if (AppodealJavaClass == null || UnityActivityJavaObject == null) return false;
            return AppodealJavaClass.CallStatic<bool>(AndroidConstants.JavaMethodName.Appodeal.Show, UnityActivityJavaObject, AndroidAppodealHelper.GetJavaShowStyle(showStyle), placement);
        }

        public bool ShowBannerView(int yAxis, int xAxis, string placement)
        {
            if (AppodealBannerJavaObject == null || UnityActivityJavaObject == null) return false;
            return AppodealBannerJavaObject.Call<bool>(AndroidConstants.JavaMethodName.AppodealBannerView.ShowBannerView, UnityActivityJavaObject, xAxis, AndroidAppodealHelper.GetJavaYAxisPosition(yAxis), placement);
        }

        public bool ShowMrecView(int yAxis, int xAxis, string placement)
        {
            if (AppodealBannerJavaObject == null || UnityActivityJavaObject == null) return false;
            return AppodealBannerJavaObject.Call<bool>(AndroidConstants.JavaMethodName.AppodealBannerView.ShowMrecView, UnityActivityJavaObject, xAxis, AndroidAppodealHelper.GetJavaYAxisPosition(yAxis), placement);
        }

        public bool IsLoaded(int adTypes)
        {
            return AppodealJavaClass?.CallStatic<bool>(AndroidConstants.JavaMethodName.Appodeal.IsLoaded, AndroidAppodealHelper.GetJavaAdTypes(adTypes)) ?? false;
        }

        public void Cache(int adTypes)
        {
            if (AppodealJavaClass == null || UnityActivityJavaObject == null) return;
            AppodealJavaClass.CallStatic(AndroidConstants.JavaMethodName.Appodeal.Cache, UnityActivityJavaObject, AndroidAppodealHelper.GetJavaAdTypes(adTypes));
        }

        public void Hide(int adTypes)
        {
            if (AppodealJavaClass == null || UnityActivityJavaObject == null) return;
            AppodealJavaClass.CallStatic(AndroidConstants.JavaMethodName.Appodeal.Hide, UnityActivityJavaObject, AndroidAppodealHelper.GetJavaAdTypes(adTypes));
        }

        public void HideBannerView()
        {
            if (AppodealBannerJavaObject == null || UnityActivityJavaObject == null) return;
            AppodealBannerJavaObject.Call(AndroidConstants.JavaMethodName.AppodealBannerView.HideBannerView, UnityActivityJavaObject);
        }

        public void HideMrecView()
        {
            if (AppodealBannerJavaObject == null || UnityActivityJavaObject == null) return;
            AppodealBannerJavaObject.Call(AndroidConstants.JavaMethodName.AppodealBannerView.HideMrecView, UnityActivityJavaObject);
        }

        public bool IsPrecache(int adTypes)
        {
            return AppodealJavaClass?.CallStatic<bool>(AndroidConstants.JavaMethodName.Appodeal.IsPrecache, AndroidAppodealHelper.GetJavaAdTypes(adTypes)) ?? false;
        }

        public void SetAutoCache(int adTypes, bool isEnabled)
        {
            AppodealJavaClass?.CallStatic(AndroidConstants.JavaMethodName.Appodeal.SetAutoCache, AndroidAppodealHelper.GetJavaAdTypes(adTypes), isEnabled);
        }

        public void SetSmartBanners(bool areEnabled)
        {
            AppodealJavaClass?.CallStatic(AndroidConstants.JavaMethodName.Appodeal.SetSmartBanners, areEnabled);
        }

        public bool IsSmartBannersEnabled()
        {
            return AppodealJavaClass?.CallStatic<bool>(AndroidConstants.JavaMethodName.Appodeal.IsSmartBannersEnabled) ?? false;
        }

        public void SetBannerAnimation(bool isEnabled)
        {
            AppodealJavaClass?.CallStatic(AndroidConstants.JavaMethodName.Appodeal.SetBannerAnimation, isEnabled);
        }

        public void SetTabletBanners(bool areEnabled)
        {
            AppodealJavaClass?.CallStatic(AndroidConstants.JavaMethodName.Appodeal.SetTabletBanners, areEnabled);
        }

        public void SetBannerRotation(int leftBannerRotation, int rightBannerRotation)
        {
            AppodealJavaClass?.CallStatic(AndroidConstants.JavaMethodName.Appodeal.SetBannerRotation, leftBannerRotation, rightBannerRotation);
        }

        public void SetTesting(bool isEnabled)
        {
            AppodealJavaClass?.CallStatic(AndroidConstants.JavaMethodName.Appodeal.SetTesting, isEnabled);
        }

        public void SetLogLevel(AppodealLogLevel logLevel)
        {
            string javaLogLevelEnumValueName = logLevel switch
            {
                AppodealLogLevel.Debug => AndroidConstants.JavaFieldName.AppodealLogLevelDebug,
                AppodealLogLevel.Verbose => AndroidConstants.JavaFieldName.AppodealLogLevelVerbose,
                _ => AndroidConstants.JavaFieldName.AppodealLogLevelNone
            };

            try
            {
                using var logLevelJavaClass = new AndroidJavaClass(AndroidConstants.JavaClassName.AppodealLogLevel);
                var logLevelJavaObject = logLevelJavaClass.CallStatic<AndroidJavaObject>("valueOf", javaLogLevelEnumValueName);
                AppodealJavaClass?.CallStatic(AndroidConstants.JavaMethodName.Appodeal.SetLogLevel, logLevelJavaObject);
            }
            catch (Exception e)
            {
                AndroidAppodealHelper.LogIntegrationError(e.Message);
            }
        }

        public void SetChildDirectedTreatment(bool isEnabled)
        {
            AppodealJavaClass?.CallStatic(AndroidConstants.JavaMethodName.Appodeal.SetChildDirectedTreatment, AndroidAppodealHelper.GetJavaObject(isEnabled));
        }

        public void DisableNetwork(string network)
        {
            AppodealJavaClass?.CallStatic(AndroidConstants.JavaMethodName.Appodeal.DisableNetwork, network);
        }

        public void DisableNetwork(string network, int adTypes)
        {
            AppodealJavaClass?.CallStatic(AndroidConstants.JavaMethodName.Appodeal.DisableNetwork, network, AndroidAppodealHelper.GetJavaAdTypes(adTypes));
        }

        public void SetTriggerOnLoadedOnPrecache(int adTypes, bool shouldTriggerOnLoadedOnPrecache)
        {
            AppodealJavaClass?.CallStatic(AndroidConstants.JavaMethodName.Appodeal.SetTriggerOnLoadedOnPrecache, AndroidAppodealHelper.GetJavaAdTypes(adTypes), shouldTriggerOnLoadedOnPrecache);
        }

        public void MuteVideosIfCallsMuted(bool shouldMute)
        {
            AppodealJavaClass?.CallStatic(AndroidConstants.JavaMethodName.Appodeal.MuteVideosIfCallsMuted, shouldMute);
        }

        public void ShowTestScreen()
        {
            if (AppodealJavaClass == null || UnityActivityJavaObject == null) return;
            AppodealJavaClass.CallStatic(AndroidConstants.JavaMethodName.Appodeal.StartTestActivity, UnityActivityJavaObject);
        }

        public string GetVersion()
        {
            return AppodealJavaClass?.CallStatic<string>(AndroidConstants.JavaMethodName.Appodeal.GetVersion) ?? String.Empty;
        }

        public long GetSegmentId()
        {
            return AppodealJavaClass?.CallStatic<long>(AndroidConstants.JavaMethodName.Appodeal.GetSegmentId) ?? -1;
        }

        public bool CanShow(int adTypes)
        {
            return AppodealJavaClass?.CallStatic<bool>(AndroidConstants.JavaMethodName.Appodeal.CanShow, AndroidAppodealHelper.GetJavaAdTypes(adTypes)) ?? false;
        }

        public bool CanShow(int adTypes, string placement)
        {
            return AppodealJavaClass?.CallStatic<bool>(AndroidConstants.JavaMethodName.Appodeal.CanShow, AndroidAppodealHelper.GetJavaAdTypes(adTypes), placement) ?? false;
        }

        public void SetCustomFilter(string name, bool value)
        {
            AppodealJavaClass?.CallStatic(AndroidConstants.JavaMethodName.Appodeal.SetCustomFilter, name, value);
        }

        public void SetCustomFilter(string name, int value)
        {
            AppodealJavaClass?.CallStatic(AndroidConstants.JavaMethodName.Appodeal.SetCustomFilter, name, value);
        }

        public void SetCustomFilter(string name, double value)
        {
            AppodealJavaClass?.CallStatic(AndroidConstants.JavaMethodName.Appodeal.SetCustomFilter, name, value);
        }

        public void SetCustomFilter(string name, string value)
        {
            AppodealJavaClass?.CallStatic(AndroidConstants.JavaMethodName.Appodeal.SetCustomFilter, name, value);
        }

        public void ResetCustomFilter(string name)
        {
            AppodealJavaClass?.CallStatic(AndroidConstants.JavaMethodName.Appodeal.SetCustomFilter, name, null);
        }

        public void SetExtraData(string key, bool value)
        {
            AppodealJavaClass?.CallStatic(AndroidConstants.JavaMethodName.Appodeal.SetExtraData, key, value);
        }

        public void SetExtraData(string key, int value)
        {
            AppodealJavaClass?.CallStatic(AndroidConstants.JavaMethodName.Appodeal.SetExtraData, key, value);
        }

        public void SetExtraData(string key, double value)
        {
            AppodealJavaClass?.CallStatic(AndroidConstants.JavaMethodName.Appodeal.SetExtraData, key, value);
        }

        public void SetExtraData(string key, string value)
        {
            AppodealJavaClass?.CallStatic(AndroidConstants.JavaMethodName.Appodeal.SetExtraData, key, value);
        }

        public void ResetExtraData(string key)
        {
            AppodealJavaClass?.CallStatic(AndroidConstants.JavaMethodName.Appodeal.SetExtraData, key, null);
        }

        public void TrackInAppPurchase(double amount, string currency)
        {
            if (AppodealJavaClass == null || UnityActivityJavaObject == null) return;
            AppodealJavaClass.CallStatic(AndroidConstants.JavaMethodName.Appodeal.TrackInAppPurchase, UnityActivityJavaObject, amount, currency);
        }

        public List<string> GetNetworks(int adTypes)
        {
            var networksList = new List<string>();

            using var networks = AppodealJavaClass?.CallStatic<AndroidJavaObject>(AndroidConstants.JavaMethodName.Appodeal.GetNetworks, AndroidAppodealHelper.GetJavaAdTypes(adTypes));
            if (networks == null) return networksList;

            try
            {
                int countOfNetworks = networks.Call<int>("size");
                for(int i = 0; i < countOfNetworks; i++)
                {
                    networksList.Add(networks.Call<string>("get", i));
                }
            }
            catch (Exception e)
            {
                AndroidAppodealHelper.LogIntegrationError(e.Message);
            }

            return networksList;
        }

        public AppodealReward GetReward(string placement)
        {
            using var reward = String.IsNullOrEmpty(placement)
                ? AppodealJavaClass?.CallStatic<AndroidJavaObject>(AndroidConstants.JavaMethodName.Appodeal.GetReward)
                : AppodealJavaClass?.CallStatic<AndroidJavaObject>(AndroidConstants.JavaMethodName.Appodeal.GetReward, placement);

            return new AppodealReward
            {
                Amount = reward?.Call<double>(AndroidConstants.JavaMethodName.AppodealReward.GetAmount) ?? -1,
                Currency = reward?.Call<string>(AndroidConstants.JavaMethodName.AppodealReward.GetCurrency) ?? String.Empty
            };
        }

        public double GetPredictedEcpm(int adType)
        {
            return AppodealJavaClass?.CallStatic<double>(AndroidConstants.JavaMethodName.Appodeal.GetPredictedEcpm, AndroidAppodealHelper.GetJavaAdTypes(adType)) ?? -1;
        }

        public double GetPredictedEcpmForPlacement(int adType, string placement)
        {
            return String.IsNullOrEmpty(placement)
                ? AppodealJavaClass?.CallStatic<double>(AndroidConstants.JavaMethodName.Appodeal.GetPredictedEcpmByPlacement, AndroidAppodealHelper.GetJavaAdTypes(adType)) ?? -1
                : AppodealJavaClass?.CallStatic<double>(AndroidConstants.JavaMethodName.Appodeal.GetPredictedEcpmByPlacement, AndroidAppodealHelper.GetJavaAdTypes(adType), placement) ?? -1;
        }

        public void Destroy(int adTypes)
        {
            AppodealJavaClass?.CallStatic(AndroidConstants.JavaMethodName.Appodeal.Destroy, AndroidAppodealHelper.GetJavaAdTypes(adTypes));
        }

        public void SetUserId(string id)
        {
            AppodealJavaClass?.CallStatic(AndroidConstants.JavaMethodName.Appodeal.SetUserId, id);
        }

        public string GetUserId()
        {
            return AppodealJavaClass?.CallStatic<string>(AndroidConstants.JavaMethodName.Appodeal.GetUserId) ?? String.Empty;
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

        public void SetPurchaseCallbacks(IPurchaseListener listener)
        {
            AppodealCallbacks.Purchase.Instance.PurchaseEventsImpl.Listener = listener;
        }

        public void setSharedAdsInstanceAcrossActivities(bool isEnabled)
        {
            AppodealJavaClass?.CallStatic(AndroidConstants.JavaMethodName.Appodeal.SetSharedAdsInstanceAcrossActivities, isEnabled);
        }

        public void SetUseSafeArea(bool shouldUseSafeArea)
        {
            AppodealJavaClass?.CallStatic(AndroidConstants.JavaMethodName.Appodeal.SetUseSafeArea, shouldUseSafeArea);
        }

        public bool IsAutoCacheEnabled(int adType)
        {
            return AppodealJavaClass?.CallStatic<bool>(AndroidConstants.JavaMethodName.Appodeal.IsAutoCacheEnabled, AndroidAppodealHelper.GetJavaAdTypes(adType)) ?? false;
        }

        public void LogEvent(string eventName, Dictionary<string, object> eventParams, AppodealService services)
        {
            if (eventParams == null)
            {
                AppodealJavaClass?.CallStatic(AndroidConstants.JavaMethodName.Appodeal.LogEvent, eventName, null, (int)services);
                return;
            }

            var paramsFiltered = new Dictionary<string, object>();

            eventParams.Keys.Where(key => eventParams[key] is int || eventParams[key] is double || eventParams[key] is string)
                .ToList().ForEach(key => paramsFiltered.Add(key, eventParams[key]));

            try
            {
                using var map = new AndroidJavaObject(AndroidConstants.JavaClassName.HashMap);
                foreach (var entry in paramsFiltered)
                {
                    map.Call<AndroidJavaObject>("put", entry.Key, AndroidAppodealHelper.GetJavaObject(entry.Value));
                }

                AppodealJavaClass?.CallStatic(AndroidConstants.JavaMethodName.Appodeal.LogEvent, eventName, map, (int)services);
            }
            catch (Exception e)
            {
                AndroidAppodealHelper.LogIntegrationError(e.Message);
            }
        }

        public void ValidatePlayStoreInAppPurchase(IPlayStoreInAppPurchase purchase, IInAppPurchaseValidationListener listener)
        {
            if (AppodealJavaClass == null || UnityActivityJavaObject == null) return;
            if (purchase.NativeInAppPurchase is not AndroidPlayStoreInAppPurchase androidPurchase) return;

            AppodealCallbacks.InAppPurchase.Instance.PurchaseEventsImpl.Listener = listener;
            var purchaseCallback = new InAppPurchaseValidationCallbacks(AppodealCallbacks.InAppPurchase.Instance.PurchaseEventsImpl);

            AppodealJavaClass.CallStatic(AndroidConstants.JavaMethodName.Appodeal.ValidateInAppPurchase, UnityActivityJavaObject, androidPurchase.GetInAppPurchase(), purchaseCallback);
        }

        public void SetBidonEndpoint(string baseUrl)
        {
            if (baseUrl == null) return;
            AppodealJavaClass?.CallStatic(AndroidConstants.JavaMethodName.Appodeal.SetBidonEndpoint, baseUrl);
        }

        public string GetBidonEndpoint()
        {
            return AppodealJavaClass?.CallStatic<string>(AndroidConstants.JavaMethodName.Appodeal.GetBidonEndpoint) ?? String.Empty;
        }

        public bool ShowMediationDebugger(MediationDebuggerProvider provider)
        {
            try
            {
                var sdkClassPtr = AndroidJNI.FindClass(AndroidConstants.JavaClassName.AppLovinSdk.Replace('.', '/'));
                if (sdkClassPtr == IntPtr.Zero) return false;

                var getInstanceMethodPtr = AndroidJNI.GetStaticMethodID(sdkClassPtr, AndroidConstants.JavaMethodName.AppLovinSdk.GetInstance, "(Landroid/content/Context;)Lcom/applovin/sdk/AppLovinSdk;");
                if (getInstanceMethodPtr == IntPtr.Zero)
                {
                    AndroidJNI.DeleteLocalRef(sdkClassPtr);
                    return false;
                }

                var showDebuggerMethodPtr = AndroidJNI.GetMethodID(sdkClassPtr, AndroidConstants.JavaMethodName.AppLovinSdk.ShowMediationDebugger, "()V");
                if (showDebuggerMethodPtr == IntPtr.Zero)
                {
                    AndroidJNI.DeleteLocalRef(sdkClassPtr);
                    return false;
                }

                AndroidJNI.DeleteLocalRef(sdkClassPtr);

                using var sdkClass = new AndroidJavaClass(AndroidConstants.JavaClassName.AppLovinSdk);
                using var sdkInstance = sdkClass.CallStatic<AndroidJavaObject>(AndroidConstants.JavaMethodName.AppLovinSdk.GetInstance, UnityActivityJavaObject);
                if (sdkInstance == null) return false;
                sdkInstance.Call(AndroidConstants.JavaMethodName.AppLovinSdk.ShowMediationDebugger);

                return true;
            }
            catch (Exception e)
            {
                AndroidAppodealHelper.LogIntegrationError(e.Message);
                return false;
            }
        }

        public void SetLocationTracking(bool isEnabled)
        {
            AndroidAppodealHelper.LogMethodNotSupported();
        }

        public void ValidateAppStoreInAppPurchase(IAppStoreInAppPurchase purchase, IInAppPurchaseValidationListener listener)
        {
            AndroidAppodealHelper.LogMethodNotSupported();
        }
    }
}
