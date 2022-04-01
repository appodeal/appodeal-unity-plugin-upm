using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems;
using AppodealStack.Monetization.Common;
using AppodealStack.UnityEditor.Utils;
using AppodealStack.ConsentManagement.Api;
using AppodealStack.ConsentManagement.Common;

// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Platforms.Dummy
{
    #region EditorAdClass
    
    internal class EditorAd
    {
        public bool             IsAutoCacheEnabled = true;
        public bool             IsInitialized;
        public bool             IsPrecache = false;
        public string           PrefabName;
        public GameObject       GameObject;
        public readonly int     Type;
        public readonly string  Name;
        public readonly Vector2 Size;

        public EditorAd(int type, GameObject gameObject, string prefabName, string name, Vector2 size)
        {
            this.Type = type;
            this.GameObject = gameObject;
            this.PrefabName = prefabName;
            this.Name = name;
            this.Size = size;
        }
    }

    #endregion

    /// <summary>
    /// Unity Editor implementation of <see langword="IAppodealAdsClient"/> interface.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    [SuppressMessage("ReSharper", "Unity.NoNullPropagation")]
    [SuppressMessage("ReSharper", "RedundantTypeArgumentsOfMethod")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    [SuppressMessage("ReSharper", "UnusedMethodReturnValue.Local")]
    public class DummyAppodealClient : IAppodealAdsClient
    {
        #region Variables

        private const string    HorizontalInterstitialAssetName = "InterstitialH";
        private const string    VerticalInterstitialAssetName = "InterstitialV";
        private const string    HorizontalVideoAssetName = "VideoH";
        private const string    VerticalVideoAssetName = "VideoV";

        private const string    FailedToLoad = "FailedToLoad";
        private const string    Closed = "Closed";
        private const string    Shown = "Shown";
        private const string    Loaded = "Loaded";
        private const string    Clicked = "Clicked";
        private const string    Finished = "Finished";

        private const int       INTERSTITIAL = 1;
        private const int       REWARDED_VIDEO = 2;
        private const int       MREC = 4;
        private const int       BANNER = 8;
        private const int       BANNER_VIEW = 16;
        private const int       BANNER_BOTTOM = 32;
        private const int       BANNER_TOP = 64;
        private const int       BANNER_LEFT = 128;
        private const int       BANNER_RIGHT = 256;

        private readonly Dictionary<int,string> bannerPositions = new Dictionary<int,string> {
            { BANNER_BOTTOM, "BannerBottomAd" },
            { BANNER_TOP, "BannerTopAd" },
            { BANNER_LEFT, "BannerLeftAd" },
            { BANNER_RIGHT, "BannerRightAd" },
            { BANNER_VIEW, "BannerViewAd" } };
        
        private readonly Dictionary<int,EditorAd> ads = new Dictionary<int,EditorAd> {
            {INTERSTITIAL, new EditorAd(INTERSTITIAL, null, "InterstitialAd", "Interstitial", Vector2.zero)},
            {BANNER, new EditorAd(BANNER, null, "BannerBottomAd", "Banner", new Vector2(600, 95))},
            {REWARDED_VIDEO, new EditorAd(REWARDED_VIDEO, null, "RewardedAd", "RewardedVideo", Vector2.zero)},
            {MREC, new EditorAd(MREC, null, "MrecAd", "Mrec", new Vector2(420, 350))} };
        
        private IInterstitialAdListener     interstitialAdListener;
        private IBannerAdListener           bannerAdListener;
        private IRewardedVideoAdListener    rewardedVideoAdListener;
        private IMrecAdListener             mrecAdListener;
        private IAppodealInitializationListener appodealInitializationListener;

        private VideoPlayer     videoPlayer;
        private Toggle          loggingToggle;

        private bool            _isLoggingEnabled;
        private bool            _isSDKInitialized;
        private bool            _hasConsentGiven;
        private bool            _shouldReward;
        private bool            isLoggingEnabled { get {return _isLoggingEnabled;} set {_isLoggingEnabled = value;} }
        private bool            isSDKInitialized { get {return _isSDKInitialized;} set {_isSDKInitialized = value;} }
        private bool            hasConsentGiven { get {return _hasConsentGiven;} set {_hasConsentGiven = value;} }
        private bool            shouldReward { get {return _shouldReward;} set {_shouldReward = value;} }

        #endregion
        
        private static int nativeAdTypesForType(int adTypes)
        {
            var nativeAdTypes = 0;

            if ((adTypes & AppodealAdType.Interstitial) > 0)
            {
                nativeAdTypes |= INTERSTITIAL;
            }

            if ((adTypes & AppodealAdType.Banner) > 0)
            {
                nativeAdTypes |= BANNER;
            }

            if ((adTypes & AppodealAdType.Mrec) > 0)
            {
                nativeAdTypes |= MREC;
            }

            if ((adTypes & AppodealAdType.RewardedVideo) > 0)
            {
                nativeAdTypes |= REWARDED_VIDEO;
            }

            return nativeAdTypes;
        }

        private static int nativeShowStyleForType(int adTypes)
        {
            if ((adTypes & AppodealShowStyle.Interstitial) > 0)
            {
                return INTERSTITIAL;
            }

            if ((adTypes & AppodealShowStyle.BannerTop) > 0)
            {
                return BANNER_TOP;
            }

            if ((adTypes & AppodealShowStyle.BannerBottom) > 0)
            {
                return BANNER_BOTTOM;
            }

            if ((adTypes & AppodealShowStyle.BannerLeft) > 0)
            {
                return BANNER_LEFT;
            }

            if ((adTypes & AppodealShowStyle.BannerRight) > 0)
            {
                return BANNER_RIGHT;
            }

            if ((adTypes & AppodealShowStyle.RewardedVideo) > 0)
            {
                return REWARDED_VIDEO;
            }

            return 0;
        }

        #region SimAppodealLogicForEditor
        
        private void SimSetAutoCache(int adTypes, bool isEnabled)
        {
            ads.Keys.Where(key => (adTypes & key) > 0).ToList().ForEach(key => ads[key].IsAutoCacheEnabled = isEnabled);
        }

        private void SimInitAdTypes(int adTypes)
        {
            ads.Keys.Where(key => (adTypes & key) > 0).ToList().ForEach(key => ads[key].IsInitialized = true);
            ads.Keys.Where(key => ads[key].IsAutoCacheEnabled && ads[key].IsInitialized).ToList().ForEach(key => SimCacheAd(key));

            if (appodealInitializationListener != null) appodealInitializationListener.OnInitializationFinished(null);
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
            return GetEditorAdObjectByAdType(adType) == null ? false : GetEditorAdObjectByAdType(adType).IsInitialized;
        }

        private bool SimCheckIfAdTypeIsLoaded(int adType)
        {
            return GetEditorAdObjectByAdType(adType)?.GameObject != null;
        }

        private bool SimCacheAd(int adType)
        {
            EditorAd ad = GetEditorAdObjectByAdType(adType);
            string prefabName = ad?.PrefabName;

            if (string.IsNullOrEmpty(prefabName))
            {
                SimFireCallback(ad?.Name, FailedToLoad);
                return false;
            }

            var defaultPath = Path.Combine(AppodealEditorConstants.PackagePath, "Editor/EditorAds/", prefabName, ".prefab");
            var assetGuids = AssetDatabase.FindAssets(prefabName);
            var prefabPath = assetGuids.Length < 1 ? defaultPath : AssetDatabase.GUIDToAssetPath(assetGuids[0]);

            var adPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            var adGameObject = Object.Instantiate<GameObject>(adPrefab, Vector3.zero, Quaternion.identity);

            if (adGameObject == null)
            {
                SimFireCallback(ad.Name, FailedToLoad);
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

                        if (ad.Type == INTERSTITIAL) SimFireCallback(ad.Name, Closed);
                        else if (ad.Type == REWARDED_VIDEO) SimFireCallback(ad.Name, Closed, shouldReward);
                        
                        if (shouldReward)
                        {
                            shouldReward = false;
                            SimFireCallback(ad.Name, Finished, 50d, "diamonds");
                        }

                        videoPlayer = null;

                        if (ad.IsAutoCacheEnabled) SimCacheAd(ad.Type);
                    });
                }
                else if (btn.gameObject.name == "Panel")
                {
                    btn.onClick.AddListener(() =>
                    {
                        SimFireCallback(ad.Name, Clicked);
                        Application.OpenURL("https://wiki.appodeal.com/en/unity/get-started");
                    });
                }
            }

            ad.GameObject = adGameObject;
            
            if (ad.Type == BANNER) SimFireCallback(ad.Name, Loaded, 80, false);
            else SimFireCallback(ad.Name, Loaded, false);

            return true;
        }

        private bool SimShowAdAtPos(int adType, Vector2 pos)
        {
            EditorAd ad = GetEditorAdObjectByAdType(adType);

            if (SimShowAd(adType))
            {
                Vector2 calculatedPos = Vector2.zero;

                if (pos.x == AppodealViewPosition.HorizontalCenter || pos.x == AppodealViewPosition.HorizontalSmart) calculatedPos.x = (Screen.width - ad.Size.x) / 2;
                else if (pos.x == AppodealViewPosition.HorizontalLeft) calculatedPos.x = 0;
                else if (pos.x == AppodealViewPosition.HorizontalRight) calculatedPos.x = Screen.width - ad.Size.x;
                else calculatedPos.x = pos.x;

                if (pos.y == AppodealViewPosition.VerticalBottom) calculatedPos.y = ad.Size.y - Screen.height;
                else if (pos.y == AppodealViewPosition.VerticalTop) calculatedPos.y = 0;
                else calculatedPos.y = - pos.y;

                if (calculatedPos.x < 0 || calculatedPos.y > 0 || calculatedPos.x > Screen.width - ad.Size.x || calculatedPos.y < ad.Size.y - Screen.height) return false;

                var rt = ad.GameObject.transform.Find("Panel").GetComponent<RectTransform>();
                rt.anchoredPosition = calculatedPos;
                
                return true;
            }
            else return false;
        }

        private bool SimShowAd(int adType)
        {
            if (!SimCheckIfSDKInitialized(nameof(Show))) return false;
            
            if (!SimCheckIfAdTypeInitialized(adType)) return false;

            if (!SimCheckIfAdTypeIsLoaded(adType)) return false;

            EditorAd ad = GetEditorAdObjectByAdType(adType);

            switch (ad.Type) {
                case BANNER:
                    SetBannerPosition(ad, adType);
                    SetBannerWidth(ad, adType);
                    break;
                case INTERSTITIAL:
                    var img = ad.GameObject.transform.Find("Panel").GetComponent<Image>();
                    var sprite = Screen.height < Screen.width ? Resources.Load<Sprite>(HorizontalInterstitialAssetName) : Resources.Load<Sprite>(VerticalInterstitialAssetName);
                    img.sprite = sprite;
                    break;

                case REWARDED_VIDEO:
                    videoPlayer = ad.GameObject.GetComponentInChildren<VideoPlayer>();
                    var videoClip = Screen.height < Screen.width ? Resources.Load<VideoClip>(HorizontalVideoAssetName) : Resources.Load<VideoClip>(VerticalVideoAssetName);
                    videoPlayer.clip = videoClip;
                    break;
            }
            
            ad.GameObject.SetActive(true);
            SimFireCallback(ad.Name, Shown);
            videoPlayer?.Play();

            return true;
        }

        private void SimHideAd(int adType)
        {
            EditorAd ad = GetEditorAdObjectByAdType(adType);

            if (ad == null) return;

            if (ad.PrefabName == "BannerViewAd" && adType != BANNER_VIEW) return;
            else if (ad.PrefabName != "BannerViewAd" && adType == BANNER_VIEW) return;

            ad.GameObject?.SetActive(false);
        }

        private void SimDestroyAd(int adType)
        {
            if (GetEditorAdObjectByAdType(adType) != null) GetEditorAdObjectByAdType(adType).GameObject = null;
        }

        private bool SimIsPrecache(int adType)
        {
            return GetEditorAdObjectByAdType(adType)?.IsPrecache ?? false;
        }

        private bool SimIsAutoCacheEnabled(int adType)
        {
            return GetEditorAdObjectByAdType(adType)?.IsAutoCacheEnabled ?? false;
        }

        private void SimFireCallback(string adTypeName, string methodName, object param1 = null, object param2 = null)
        {
            switch (adTypeName)
            {
                case "Interstitial":
                    if (methodName == FailedToLoad) interstitialAdListener?.OnInterstitialFailedToLoad();
                    else if (methodName == Closed) interstitialAdListener?.OnInterstitialClosed();
                    else if (methodName == Shown) interstitialAdListener?.OnInterstitialShown();
                    else if (methodName == Loaded) interstitialAdListener?.OnInterstitialLoaded((param1 != null && param1 is bool) ? (bool)param1 : false);
                    else if (methodName == Clicked) interstitialAdListener?.OnInterstitialClicked();
                    break;
                case "Banner":
                    if (methodName == FailedToLoad) bannerAdListener?.OnBannerFailedToLoad();
                    else if (methodName == Shown) bannerAdListener?.OnBannerShown();
                    else if (methodName == Loaded) bannerAdListener?.OnBannerLoaded((param1 != null && param1 is int) ? (int)param1 : 50, (param2 != null && param2 is bool) ? (bool)param2 : false);
                    else if (methodName == Clicked) bannerAdListener?.OnBannerClicked();
                    break;
                case "RewardedVideo":
                    if (methodName == FailedToLoad) rewardedVideoAdListener?.OnRewardedVideoFailedToLoad();
                    else if (methodName == Closed) rewardedVideoAdListener?.OnRewardedVideoClosed((param1 != null && param1 is bool) ? (bool)param1 : false);
                    else if (methodName == Shown) rewardedVideoAdListener?.OnRewardedVideoShown();
                    else if (methodName == Loaded) rewardedVideoAdListener?.OnRewardedVideoLoaded((param1 != null && param1 is bool) ? (bool)param1 : false);
                    else if (methodName == Clicked) rewardedVideoAdListener?.OnRewardedVideoClicked();
                    else if (methodName == Finished) rewardedVideoAdListener?.OnRewardedVideoFinished((param1 != null && param1 is double) ? (double)param1 : 100d, (param2 != null && param2 is string) ? (string)param2 : "coins");
                    break;
                case "Mrec":
                    if (methodName == FailedToLoad) mrecAdListener?.OnMrecFailedToLoad();
                    else if (methodName == Shown) mrecAdListener?.OnMrecShown();
                    else if (methodName == Loaded) mrecAdListener?.OnMrecLoaded((param1 != null && param1 is bool) ? (bool)param1 : false);
                    else if (methodName == Clicked) mrecAdListener?.OnMrecClicked();
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
            if (adType == BANNER || bannerPositions.ContainsKey(adType))
            {
                ads.TryGetValue(BANNER, out ad);
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

            if (ad.PrefabName != newPrefabName)
            {
                ad.PrefabName = newPrefabName;
                Object.Destroy(ad.GameObject);
                ad.GameObject = null;
                SimCacheAd(adType);
            }
        }

        private void SetBannerWidth(EditorAd ad, int adType)
        {
            var rt = ad.GameObject.transform.Find("Panel").GetComponent<RectTransform>();
            if (adType == BANNER_LEFT || adType == BANNER_RIGHT) rt.sizeDelta = new Vector2(ad.Size.y, Mathf.Min(Screen.height, ad.Size.x));
            else rt.sizeDelta = new Vector2(Mathf.Min(Screen.width, ad.Size.x), ad.Size.y);
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

        public void Initialize(string appKey, int adTypes, IAppodealInitializationListener listener)
        {
            appodealInitializationListener = listener;
            initialize(appKey, adTypes);
        }

        public void initialize(string appKey, int adTypes)
        {
            if (isSDKInitialized) return;

            Debug.LogWarning("There is only simplified workflow of Appodeal SDK simulated in Editor. Make sure to test advertising on a real Android/iOS device before publishing.");
            isSDKInitialized = true;
            SimInitAdTypes(nativeAdTypesForType(adTypes));
        }

        public void initialize(string appKey, int adTypes, bool hasConsent)
        {
            initialize(appKey, adTypes);
            hasConsentGiven = hasConsent;
        }

        public void initialize(string appKey, int adTypes, Consent consent)
        {
            initialize(appKey, adTypes);
            hasConsentGiven = consent?.GetAuthorizationStatus() == ConsentAuthorizationStatus.Authorized;
        }

        public bool IsInitialized(int adType)
        {
            return SimCheckIfAdTypeInitialized(nativeAdTypesForType(adType));
        }

        public bool Show(int adTypes)
        {
            return SimShowAd(nativeShowStyleForType(adTypes));
        }

        public bool Show(int adTypes, string placement)
        {
            return Show(adTypes);
        }

        public bool ShowBannerView(int yAxis, int xAxis, string placement)
        {
            return SimShowAdAtPos(BANNER_VIEW, new Vector2(xAxis, yAxis));
        }

        public bool ShowMrecView(int yAxis, int xGravity, string placement)
        {
            return SimShowAdAtPos(MREC, new Vector2(xGravity, yAxis));
        }

        public bool IsLoaded(int adTypes)
        {
            return SimCheckIfAdTypeIsLoaded(nativeAdTypesForType(adTypes));
        }

        public void Cache(int adTypes)
        {
            if (!SimCheckIfSDKInitialized(nameof(Show))) return;
            
            if (!IsInitialized(adTypes))
            {
                return;
            }

            if (IsLoaded(adTypes))
            {
                return;
            }

            SimCacheAd(nativeAdTypesForType(adTypes));
        }

        public void Hide(int adTypes)
        {
            SimHideAd(nativeAdTypesForType(adTypes));
        }

        public void HideBannerView()
        {
            SimHideAd(BANNER_VIEW);
        }

        public void HideMrecView()
        {
            SimHideAd(MREC);
        }

        public bool IsPrecache(int adTypes)
        {
            return SimIsPrecache(nativeAdTypesForType(adTypes));
        }

        public void SetAutoCache(int adTypes, bool autoCache)
        {
            SimSetAutoCache(nativeAdTypesForType(adTypes), autoCache);
        }

        public bool CanShow(int adTypes)
        {
            return IsLoaded(adTypes);
        }

        public bool CanShow(int adTypes, string placement)
        {
            return IsLoaded(adTypes);
        }

        public bool IsAutoCacheEnabled(int adType)
        {
            return SimIsAutoCacheEnabled(nativeAdTypesForType(adType));
        }

        public void Destroy(int adTypes)
        {
            SimDestroyAd(nativeAdTypesForType(adTypes));
        }

        public void SetLogLevel(AppodealLogLevel logging)
        {
            isLoggingEnabled = logging != AppodealLogLevel.None;
        }

        public void SetInterstitialCallbacks(IInterstitialAdListener listener)
        {
            SimSetInterstitialCallbacks(listener);
        }

        public void SetRewardedVideoCallbacks(IRewardedVideoAdListener listener)
        {
            SimSetRewardedVideoCallbacks(listener);
        }

        public void SetBannerCallbacks(IBannerAdListener listener)
        {
            SimSetBannerCallbacks(listener);
        }

        public void SetMrecCallbacks(IMrecAdListener listener)
        {
            SimSetMrecCallbacks(listener);
        }

        #endregion

        #region MethodsNotSupportedInEditor

        public void SetSmartBanners(bool value)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.SetSmartBanners method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public bool IsSmartBannersEnabled()
        {
             if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.IsSmartBannersEnabled method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
             return false;
        }

        public void SetBannerAnimation(bool value)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.SetBannerAnimation method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void SetTabletBanners(bool value)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.SetTabletBanners method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void SetBannerRotation(int leftBannerRotation, int rightBannerRotation)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.SetBannerRotation method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void SetTesting(bool test)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.SetTesting method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void SetChildDirectedTreatment(bool value)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.SetChildDirectedTreatment method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void updateConsent(bool value)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.updateConsent method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void UpdateConsent(Consent consent)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.UpdateConsent method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void UpdateConsentGdpr(GdprUserConsent consent)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.UpdateConsentGDPR method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void UpdateConsentCcpa(CcpaUserConsent consent)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.UpdateConsentCCPA method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void DisableNetwork(string network)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.DisableNetwork method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void DisableNetwork(string network, int adTypes)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.DisableNetwork method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void SetLocationTracking(bool value)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.SetLocationTracking method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void disableLocationPermissionCheck()
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.disableLocationPermissionCheck method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void SetTriggerOnLoadedOnPrecache(int adTypes, bool onLoadedTriggerBoth)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.SetTriggerOnLoadedOnPrecache method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void MuteVideosIfCallsMuted(bool value)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.MuteVideosIfCallsMuted method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void ShowTestScreen()
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.ShowTestScreen method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public string GetVersion()
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.GetVersion method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
            return string.Empty;
        }

        public long GetSegmentId()
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.GetSegmentId method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
            return -1;
        }

        public void SetCustomFilter(string name, bool value)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.SetCustomFilter method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void SetCustomFilter(string name, int value)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.SetCustomFilter method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void SetCustomFilter(string name, double value)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.SetCustomFilter method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void SetCustomFilter(string name, string value)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.SetCustomFilter method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void ResetCustomFilter(string name)
        {
             if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.ResetCustomFilter method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void TrackInAppPurchase(double amount, string currency)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.TrackInAppPurchase method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public List<string> GetNetworks(int adTypes)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.GetNetworks method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
            return new List<string>();
        }

        public KeyValuePair<string, double> GetRewardParameters()
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.GetRewardParameters method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
            return new KeyValuePair<string, double>("USD", 0);
        }

        public KeyValuePair<string, double> GetRewardParameters(string placement)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.GetRewardParameters method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
            return new KeyValuePair<string, double>("USD", 0);
        }

        public double GetPredictedEcpm(int adType)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.GetPredictedEcpm method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
            return 0;
        }

        public void SetExtraData(string key, bool value)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.SetExtraData method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void SetExtraData(string key, int value)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.SetExtraData method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void SetExtraData(string key, double value)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.SetExtraData method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void SetExtraData(string key, string value)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.SetExtraData method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void ResetExtraData(string key)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.ResetExtraData method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void setSharedAdsInstanceAcrossActivities(bool value)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.setSharedAdsInstanceAcrossActivities method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void SetUseSafeArea(bool value)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.SetUseSafeArea method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void SetUserId(string id)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.SetUserId method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public string GetUserId()
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.GetUserId method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
            return "";
        }

        public void setUserAge(int age)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.SetUserAge method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void setUserGender(AppodealUserGender gender)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.setGender method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void LogEvent(string eventName, Dictionary<string, object> eventParams)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.LogEvent method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void ValidateInAppPurchaseAndroid(IInAppPurchase purchase, IInAppPurchaseValidationListener listener)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.ValidateInAppPurchaseAndroid method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void ValidateInAppPurchaseIos(string productIdentifier, string price, string currency, string transactionId ,string additionalParams, IosPurchaseType type, IInAppPurchaseValidationListener listener)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.ValidateInAppPurchaseIos method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        #endregion
    }
}
