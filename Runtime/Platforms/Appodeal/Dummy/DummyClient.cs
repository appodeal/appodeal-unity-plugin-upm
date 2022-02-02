using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AppodealAds.Unity.Common;
using AppodealCM.Unity.Common;
using AppodealCM.Unity.Platforms;
using AppodealAds.Unity.Editor.Utils;

namespace AppodealAds.Unity.Platforms.Dummy
{
    #region EditorAdClass
    
    internal class EditorAd
    {
        public bool             isAutoCacheEnabled = true;
        public bool             isInitialized;
        public bool             isPrecache;
        public string           prefabName;
        public GameObject       gameObject;
        public readonly int     type;
        public readonly string  name;
        public readonly Vector2 size;

        public EditorAd(int type, GameObject gameObject, string prefabName, string name, Vector2 size)
        {
            this.type = type;
            this.gameObject = gameObject;
            this.prefabName = prefabName;
            this.name = name;
            this.size = size;
        }
    }

    #endregion

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class DummyClient : IAppodealAdsClient
    {
        #region Variables

        private const string    HORIZONTAL_INTERSTITIAL_ASSET_NAME = "InterstitialH";
        private const string    VERTICAL_INTERSTITIAL_ASSET_NAME = "InterstitialV";
        private const string    HORIZONTAL_VIDEO_ASSET_NAME = "VideoH";
        private const string    VERTICAL_VIDEO_ASSET_NAME = "VideoV";

        private const string    FAILED_TO_LOAD = "FailedToLoad";
        private const string    CLOSED = "Closed";
        private const string    SHOWN = "Shown";
        private const string    LOADED = "Loaded";
        private const string    CLICKED = "Clicked";
        private const string    FINISHED = "Finished";

        private readonly Dictionary<int,string> bannerPositions = new Dictionary<int,string> {
            { AppodealAdType.BANNER_BOTTOM, "BannerBottomAd" },
            { AppodealAdType.BANNER_TOP, "BannerTopAd" },
            { AppodealAdType.BANNER_LEFT, "BannerLeftAd" },
            { AppodealAdType.BANNER_RIGHT, "BannerRightAd" },
            { AppodealAdType.BANNER_VIEW, "BannerViewAd" } };
        
        private readonly Dictionary<int,EditorAd> ads = new Dictionary<int,EditorAd> {
            {AppodealAdType.INTERSTITIAL, new EditorAd(AppodealAdType.INTERSTITIAL, null, "InterstitialAd", "Interstitial", Vector2.zero)},
            {AppodealAdType.BANNER, new EditorAd(AppodealAdType.BANNER, null, "BannerBottomAd", "Banner", new Vector2(600, 95))},
            {AppodealAdType.REWARDED_VIDEO, new EditorAd(AppodealAdType.REWARDED_VIDEO, null, "RewardedAd", "RewardedVideo", Vector2.zero)},
            {AppodealAdType.MREC, new EditorAd(AppodealAdType.MREC, null, "MrecAd", "Mrec", new Vector2(420, 350))} };
        
        private IInterstitialAdListener     interstitialAdListener;
        private IBannerAdListener           bannerAdListener;
        private IRewardedVideoAdListener    rewardedVideoAdListener;
        private IMrecAdListener             mrecAdListener;

        private VideoPlayer     videoPlayer;
        private Toggle          loggingToggle;

        private bool            _isLoggingEnabled;
        private bool            _isSDKInitialized;
        private bool            _hasConsentGiven;
        private bool            _shouldReward;
        private bool            isLoggingEnabled { get {return _isLoggingEnabled;} set {_isLoggingEnabled = value; } }
        private bool            isSDKInitialized { get {return _isSDKInitialized;} set {_isSDKInitialized = value; } }
        private bool            hasConsentGiven { get {return _hasConsentGiven;} set {_hasConsentGiven = value; } }
        private bool            shouldReward { get {return _shouldReward;} set {_shouldReward = value; } }

        #endregion

        #region SimAppodealLogicForEditor
        
        private void SimSetAutoCache(int adTypes, bool isEnabled)
        {
            ads.Keys.Where(key => (adTypes & key) > 0).ToList().ForEach(key => ads[key].isAutoCacheEnabled = isEnabled);
        }

        private void SimInitAdTypes(int adTypes)
        {
            ads.Keys.Where(key => (adTypes & key) > 0).ToList().ForEach(key => ads[key].isInitialized = true);
            ads.Keys.Where(key => ads[key].isAutoCacheEnabled == true).ToList().ForEach(key => SimCacheAd(key));
        }

        private bool SimCheckIfSDKInitialized(string methodName)
        {
            if (!isSDKInitialized)
            {
                Debug.LogError($"Initialize Appodeal SDK before calling {methodName} method");
                return false;
            }

            return true;
        }

        private bool SimCheckIfAdTypeInitialized(int adType)
        {
            return GetEditorAdObjectByAdType(adType) == null ? false : GetEditorAdObjectByAdType(adType).isInitialized;
        }

        private bool SimCheckIfAdTypeIsLoaded(int adType)
        {
            return GetEditorAdObjectByAdType(adType)?.gameObject != null ? true : false;
        }

        private bool SimCacheAd(int adType)
        {
            EditorAd ad = GetEditorAdObjectByAdType(adType);
            string prefabName = ad?.prefabName;

            if (string.IsNullOrEmpty(prefabName))
            {
                SimFireCallback(ad.name, FAILED_TO_LOAD);
                return false;
            }

            var defaultPath = Path.Combine(AppodealEditorConstants.PackagePath, "Editor/EditorAds/", prefabName, ".prefab");
            var assetGuids = AssetDatabase.FindAssets(prefabName);
            var prefabPath = assetGuids.Length < 1 ? defaultPath : AssetDatabase.GUIDToAssetPath(assetGuids[0]);

            var adPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            var adGameObject = Object.Instantiate<GameObject>(adPrefab, Vector3.zero, Quaternion.identity);

            if (adGameObject == null)
            {
                SimFireCallback(ad.name, FAILED_TO_LOAD);
                return false;
            }

            adGameObject.SetActive(false);
            
            if (Object.FindObjectsOfType<EventSystem>().Length < 1)
            {
                adGameObject.transform.Find("EventSystem")?.gameObject.SetActive(true);
            }

            Object.DontDestroyOnLoad(adGameObject);

            var buttons = adGameObject.GetComponentsInChildren<Button>(true);
            var toggle = adGameObject.GetComponentInChildren<Toggle>();

            foreach (Button btn in buttons)
            {
                if (btn.gameObject.name == "CloseButton")
                {
                    btn.onClick.AddListener(() =>
                    {
                        shouldReward = toggle?.isOn ?? false;
                        
                        Object.Destroy(adGameObject);

                        if (ad.type == AppodealAdType.INTERSTITIAL) SimFireCallback(ad.name, CLOSED);
                        else if (ad.type == AppodealAdType.REWARDED_VIDEO) SimFireCallback(ad.name, CLOSED, shouldReward);
                        
                        if (shouldReward)
                        {
                            shouldReward = false;
                            SimFireCallback(ad.name, FINISHED, 50d, "diamonds");
                        }

                        videoPlayer = null;

                        if (ad.isAutoCacheEnabled) SimCacheAd(ad.type);
                    });
                }
                else if (btn.gameObject.name == "Panel")
                {
                    btn.onClick.AddListener(() =>
                    {
                        SimFireCallback(ad.name, CLICKED);
                        Application.OpenURL("https://wiki.appodeal.com/en/unity/get-started");
                    });
                }
            }

            ad.gameObject = adGameObject;
            
            if (ad.type == AppodealAdType.BANNER) SimFireCallback(ad.name, LOADED, 80, false);
            else SimFireCallback(ad.name, LOADED, false);

            return true;
        }

        private bool SimShowAdAtPos(int adType, Vector2 pos)
        {
            EditorAd ad = GetEditorAdObjectByAdType(adType);

            if (SimShowAd(adType))
            {
                Vector2 calulatedPos = Vector2.zero;

                if (pos.x == AppodealViewPosition.HORIZONTAL_CENTER || pos.x == AppodealViewPosition.HORIZONTAL_SMART) calulatedPos.x = (Screen.width - ad.size.x) / 2;
                else if (pos.x == AppodealViewPosition.HORIZONTAL_LEFT) calulatedPos.x = 0;
                else if (pos.x == AppodealViewPosition.HORIZONTAL_RIGHT) calulatedPos.x = Screen.width - ad.size.x;
                else calulatedPos.x = pos.x;

                if (pos.y == AppodealViewPosition.VERTICAL_BOTTOM) calulatedPos.y = ad.size.y - Screen.height;
                else if (pos.y == AppodealViewPosition.VERTICAL_TOP) calulatedPos.y = 0;
                else calulatedPos.y = - pos.y;

                if (calulatedPos.x < 0 || calulatedPos.y > 0 || calulatedPos.x > Screen.width - ad.size.x || calulatedPos.y < ad.size.y - Screen.height) return false;

                var rt = ad.gameObject.transform.Find("Panel").GetComponent<RectTransform>();
                rt.anchoredPosition = calulatedPos;
                
                return true;
            }
            else return false;
        }

        private bool SimShowAd(int adType)
        {
            if (!SimCheckIfSDKInitialized(nameof(show))) return false;
            
            if (!SimCheckIfAdTypeInitialized(adType)) return false;

            if (!SimCheckIfAdTypeIsLoaded(adType)) return false;

            EditorAd ad = GetEditorAdObjectByAdType(adType);

            switch (ad.type) {
                case AppodealAdType.BANNER:
                    SetBannerPosition(ad, adType);
                    SetBannerWidth(ad, adType);
                    break;
                case AppodealAdType.INTERSTITIAL:
                    var img = ad.gameObject.transform.Find("Panel").GetComponent<Image>();
                    var sprite = Screen.height < Screen.width ? Resources.Load<Sprite>(HORIZONTAL_INTERSTITIAL_ASSET_NAME) : Resources.Load<Sprite>(VERTICAL_INTERSTITIAL_ASSET_NAME);
                    img.sprite = sprite;
                    break;

                case AppodealAdType.REWARDED_VIDEO:
                    videoPlayer = ad.gameObject.GetComponentInChildren<VideoPlayer>();
                    var videoClip = Screen.height < Screen.width ? Resources.Load<VideoClip>(HORIZONTAL_VIDEO_ASSET_NAME) : Resources.Load<VideoClip>(VERTICAL_VIDEO_ASSET_NAME);
                    videoPlayer.clip = videoClip;
                    break;
            }
            
            ad.gameObject.SetActive(true);
            SimFireCallback(ad.name, SHOWN);
            videoPlayer?.Play();

            return true;
        }

        private void SimHideAd(int adType)
        {
            EditorAd ad = GetEditorAdObjectByAdType(adType);

            if (ad == null) return;

            if (ad.prefabName == "BannerViewAd" && adType != AppodealAdType.BANNER_VIEW) return;
            else if (ad.prefabName != "BannerViewAd" && adType == AppodealAdType.BANNER_VIEW) return;

            ad.gameObject?.SetActive(false);
        }

        private void SimDestroyAd(int adType)
        {
            if (GetEditorAdObjectByAdType(adType) != null) GetEditorAdObjectByAdType(adType).gameObject = null;
        }

        private bool SimIsPrecache(int adType)
        {
            return GetEditorAdObjectByAdType(adType)?.isPrecache ?? false;
        }

        private bool SimIsAutoCacheEnabled(int adType)
        {
            return GetEditorAdObjectByAdType(adType)?.isAutoCacheEnabled ?? false;
        }

        private void SimFireCallback(string adTypeName, string methodName, object param1 = null, object param2 = null)
        {
            switch (adTypeName)
            {
                case "Interstitial":
                    if (methodName == FAILED_TO_LOAD) interstitialAdListener?.onInterstitialFailedToLoad();
                    else if (methodName == CLOSED) interstitialAdListener?.onInterstitialClosed();
                    else if (methodName == SHOWN) interstitialAdListener?.onInterstitialShown();
                    else if (methodName == LOADED) interstitialAdListener?.onInterstitialLoaded((param1 != null && param1 is bool) ? (bool)param1 : false);
                    else if (methodName == CLICKED) interstitialAdListener?.onInterstitialClicked();
                    break;
                case "Banner":
                    if (methodName == FAILED_TO_LOAD) bannerAdListener?.onBannerFailedToLoad();
                    else if (methodName == SHOWN) bannerAdListener?.onBannerShown();
                    else if (methodName == LOADED) bannerAdListener?.onBannerLoaded((param1 != null && param1 is int) ? (int)param1 : 50, (param2 != null && param2 is bool) ? (bool)param2 : false);
                    else if (methodName == CLICKED) bannerAdListener?.onBannerClicked();
                    break;
                case "RewardedVideo":
                    if (methodName == FAILED_TO_LOAD) rewardedVideoAdListener?.onRewardedVideoFailedToLoad();
                    else if (methodName == CLOSED) rewardedVideoAdListener?.onRewardedVideoClosed((param1 != null && param1 is bool) ? (bool)param1 : false);
                    else if (methodName == SHOWN) rewardedVideoAdListener?.onRewardedVideoShown();
                    else if (methodName == LOADED) rewardedVideoAdListener?.onRewardedVideoLoaded((param1 != null && param1 is bool) ? (bool)param1 : false);
                    else if (methodName == CLICKED) rewardedVideoAdListener?.onRewardedVideoClicked();
                    else if (methodName == FINISHED) rewardedVideoAdListener?.onRewardedVideoFinished((param1 != null && param1 is double) ? (double)param1 : 100d, (param2 != null && param2 is string) ? (string)param2 : "coins");
                    break;
                case "Mrec":
                    if (methodName == FAILED_TO_LOAD) mrecAdListener?.onMrecFailedToLoad();
                    else if (methodName == SHOWN) mrecAdListener?.onMrecShown();
                    else if (methodName == LOADED) mrecAdListener?.onMrecLoaded((param1 != null && param1 is bool) ? (bool)param1 : false);
                    else if (methodName == CLICKED) mrecAdListener?.onMrecClicked();
                    break;
            }
        }

        private void SimSetInterstitialCallbacks(IInterstitialAdListener listener)
        {
            interstitialAdListener = listener;
        }

        private void SimSetRewardedVideoCallbacks(IRewardedVideoAdListener listener)
        {
            rewardedVideoAdListener = listener;
        }

        private void SimSetBannerCallbacks(IBannerAdListener listener)
        {
            bannerAdListener = listener;
        }

        private void SimSetMrecCallbacks(IMrecAdListener listener)
        {
            mrecAdListener = listener;
        }

        private EditorAd GetEditorAdObjectByAdType(int adType)
        {
            EditorAd ad;
            if (adType == AppodealAdType.BANNER || bannerPositions.ContainsKey(adType))
            {
                ads.TryGetValue(AppodealAdType.BANNER, out ad);
            }
            else
            {
                ads.TryGetValue(adType, out ad);
            }

            return ad;
        }

        private void SetBannerPosition(EditorAd ad, int adType)
        {
            string newPrefabName = GetBannerPrefabNameByBannerPosition(adType);
            if (string.IsNullOrEmpty(newPrefabName)) return;

            if (ad.prefabName != newPrefabName)
            {
                ad.prefabName = newPrefabName;
                Object.Destroy(ad.gameObject);
                ad.gameObject = null;
                SimCacheAd(adType);
            }
        }

        private void SetBannerWidth(EditorAd ad, int adType)
        {
            var rt = ad.gameObject.transform.Find("Panel").GetComponent<RectTransform>();
            if (adType == AppodealAdType.BANNER_LEFT || adType == AppodealAdType.BANNER_RIGHT) rt.sizeDelta = new Vector2(ad.size.y, Mathf.Min(Screen.height, ad.size.x));
            else rt.sizeDelta = new Vector2(Mathf.Min(Screen.width, ad.size.x), ad.size.y);
        }

        private string GetBannerPrefabNameByBannerPosition(int adType)
        {
            string prefabName;
            bannerPositions.TryGetValue(adType, out prefabName);

            return prefabName;
        }

        private bool CheckIfLoggingEnabled()
        {
            if (loggingToggle == null) 
            {
                loggingToggle = GameObject.Find("Logging Toggle")?.GetComponent<Toggle>();
                isLoggingEnabled = loggingToggle?.isOn ?? false;
            }
             
            return isLoggingEnabled;
        }

        #endregion

        #region ImplementedMethods

        public void initialize(string appKey, int adTypes)
        {
            if (isSDKInitialized) return;

            Debug.LogWarning("There is only simplified workflow of Appodeal SDK simulated in Editor. Make sure to test advertising on a real Android/iOS device before publishing.");
            isSDKInitialized = true;
            SimInitAdTypes(adTypes);
        }

        public void initialize(string appKey, int adTypes, bool hasConsent)
        {
            initialize(appKey, adTypes);
            hasConsentGiven = hasConsent;
        }

        public void initialize(string appKey, int adTypes, Consent consent)
        {
            initialize(appKey, adTypes);
            hasConsentGiven = consent?.getAuthorizationStatus() == ConsentAuthorizationStatus.AUTHORIZED ? true : false;
        }

        public bool isInitialized(int adType)
        {
            return SimCheckIfAdTypeInitialized(adType);
        }

        public bool show(int adTypes)
        {
            return SimShowAd(adTypes);
        }

        public bool show(int adTypes, string placement)
        {
            return show(adTypes);
        }

        public bool showBannerView(int YAxis, int XAxis, string Placement)
        {
            return SimShowAdAtPos(AppodealAdType.BANNER_VIEW, new Vector2(XAxis, YAxis));
        }

        public bool showMrecView(int YAxis, int XGravity, string Placement)
        {
            return SimShowAdAtPos(AppodealAdType.MREC, new Vector2(XGravity, YAxis));
        }

        public bool isLoaded(int adTypes)
        {
            return SimCheckIfAdTypeIsLoaded(adTypes);
        }

        public void cache(int adTypes)
        {
            if (!SimCheckIfSDKInitialized(nameof(show))) return;
            
            if (!isInitialized(adTypes))
            {
                return;
            }

            if (isLoaded(adTypes))
            {
                return;
            }

            SimCacheAd(adTypes);
        }

        public void hide(int adTypes)
        {
            SimHideAd(adTypes);
        }

        public void hideBannerView()
        {
            hide(AppodealAdType.BANNER_VIEW);
        }

        public void hideMrecView()
        {
            hide(AppodealAdType.MREC);
        }

        public bool isPrecache(int adTypes)
        {
            return SimIsPrecache(adTypes);
        }

        public void setAutoCache(int adTypes, bool autoCache)
        {
            SimSetAutoCache(adTypes, autoCache);
        }

        public bool canShow(int adTypes)
        {
            return isLoaded(adTypes);
        }

        public bool canShow(int adTypes, string placement)
        {
            return isLoaded(adTypes);
        }

        public bool isAutoCacheEnabled(int adType)
        {
            return SimIsAutoCacheEnabled(adType);
        }

        public void destroy(int adTypes)
        {
            SimDestroyAd(adTypes);
        }

        public void setLogLevel(AppodealLogLevel logging)
        {
            isLoggingEnabled = logging == AppodealLogLevel.None ? false : true;
        }

        public void setInterstitialCallbacks(IInterstitialAdListener listener)
        {
            SimSetInterstitialCallbacks(listener);
        }

        public void setRewardedVideoCallbacks(IRewardedVideoAdListener listener)
        {
            SimSetRewardedVideoCallbacks(listener);
        }

        public void setBannerCallbacks(IBannerAdListener listener)
        {
            SimSetBannerCallbacks(listener);
        }

        public void setMrecCallbacks(IMrecAdListener listener)
        {
            SimSetMrecCallbacks(listener);
        }

        #endregion

        #region MethodsNotSupportedInEditor

        public void setSmartBanners(bool value)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.setSmartBanners method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void setBannerAnimation(bool value)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.setBannerAnimation method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void setTabletBanners(bool value)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.setTabletBanners method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void setBannerRotation(int leftBannerRotation, int rightBannerRotation)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.setBannerRotation method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void setTesting(bool test)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.setTesting method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void setChildDirectedTreatment(bool value)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.setChildDirectedTreatment method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void updateConsent(bool value)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.updateConsent method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void updateConsent(Consent consent)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.updateConsent method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void disableNetwork(string network)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.disableNetwork method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void disableNetwork(string network, int adTypes)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.disableNetwork method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void setLocationTracking(bool value)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.disableLocationPermissionCheck method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void disableLocationPermissionCheck()
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.disableLocationPermissionCheck method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void setTriggerOnLoadedOnPrecache(int adTypes, bool onLoadedTriggerBoth)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.setTriggerOnLoadedOnPrecache method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void muteVideosIfCallsMuted(bool value)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.muteVideosIfCallsMuted method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void showTestScreen()
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.showTestScreen method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public string getVersion()
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.getVersion method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
            return string.Empty;
        }

        public void setCustomFilter(string name, bool value)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.setCustomFilter method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void setCustomFilter(string name, int value)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.setCustomFilter method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void setCustomFilter(string name, double value)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.setCustomFilter method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void setCustomFilter(string name, string value)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.setCustomFilter method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void trackInAppPurchase(double amount, string currency)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.trackInAppPurchase method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public KeyValuePair<string, double> getRewardParameters()
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.getRewardParameters method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
            return new KeyValuePair<string, double>("USD", 0);
        }

        public KeyValuePair<string, double> getRewardParameters(string placement)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.getRewardParameters method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
            return new KeyValuePair<string, double>("USD", 0);
        }

        public double getPredictedEcpm(int adType)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.getPredictedEcpm method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
            return 0;
        }

        public void setExtraData(string key, bool value)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.setExtraData(string key, bool value) method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void setExtraData(string key, int value)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.setExtraData(string key, int value) method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void setExtraData(string key, double value)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.setExtraDatastring(string key, double value) method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void setExtraData(string key, string value)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.setExtraData(string key, string value) method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void setSharedAdsInstanceAcrossActivities(bool value)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.setSharedAdsInstanceAcrossActivities method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void setUseSafeArea(bool value)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.setUseSafeArea method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void getUserSettings()
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.getUserSettings method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void setUserId(string id)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.setUserId method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void setUserAge(int age)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.setUserAge method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void setUserGender(AppodealUserGender gender)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.setGender method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void setNonSkippableVideoCallbacks(INonSkippableVideoAdListener listener)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.setNonSkippableVideoCallbacks method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        #endregion
    }
}
