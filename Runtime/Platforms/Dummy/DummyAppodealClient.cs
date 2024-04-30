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
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    [SuppressMessage("ReSharper", "RedundantTypeArgumentsOfMethod")]
    [SuppressMessage("ReSharper", "Unity.NoNullPropagation")]
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    [SuppressMessage("ReSharper", "MergeSequentialChecks")]
    [SuppressMessage("ReSharper", "MergeCastWithTypeCheck")]
    [SuppressMessage("ReSharper", "SimplifyConditionalTernaryExpression")]
    [SuppressMessage("ReSharper", "ConvertClosureToMethodGroup")]
    public class DummyAppodealClient : IAppodealAdsClient
    {
        #region Variables

        private const string    HorizontalInterstitialAssetName = "ApdInterstitialH";
        private const string    VerticalInterstitialAssetName = "ApdInterstitialV";
        private const string    HorizontalVideoAssetName = "ApdVideoH";
        private const string    VerticalVideoAssetName = "ApdVideoV";

        private const string    FailedToLoad = "FailedToLoad";
        private const string    Closed = "Closed";
        private const string    Shown = "Shown";
        private const string    Loaded = "Loaded";
        private const string    Clicked = "Clicked";
        private const string    Finished = "Finished";

        private const int       Interstitial = 1;
        private const int       RewardedVideo = 2;
        private const int       Mrec = 4;
        private const int       Banner = 8;
        private const int       BannerView = 16;
        private const int       BannerBottom = 32;
        private const int       BannerTop = 64;
        private const int       BannerLeft = 128;
        private const int       BannerRight = 256;

        private readonly Dictionary<int,string> _bannerPositions = new Dictionary<int,string> {
            { BannerBottom, "ApdBannerBottomAd" },
            { BannerTop, "ApdBannerTopAd" },
            { BannerLeft, "ApdBannerLeftAd" },
            { BannerRight, "ApdBannerRightAd" },
            { BannerView, "ApdBannerViewAd" } };

        private readonly Dictionary<int,EditorAd> _ads = new Dictionary<int,EditorAd> {
            {Interstitial, new EditorAd(Interstitial, null, "ApdInterstitialAd", "Interstitial", Vector2.zero)},
            {Banner, new EditorAd(Banner, null, "ApdBannerBottomAd", "Banner", new Vector2(320, 50))},
            {RewardedVideo, new EditorAd(RewardedVideo, null, "ApdRewardedAd", "RewardedVideo", Vector2.zero)},
            {Mrec, new EditorAd(Mrec, null, "ApdMrecAd", "Mrec", new Vector2(300, 250))} };

        private IInterstitialAdListener     _interstitialAdListener;
        private IBannerAdListener           _bannerAdListener;
        private IRewardedVideoAdListener    _rewardedVideoAdListener;
        private IMrecAdListener             _mrecAdListener;
        private IAdRevenueListener          _adRevenueListener;
        private IAppodealInitializationListener _appodealInitializationListener;

        private VideoPlayer     _videoPlayer;
        private Toggle          _loggingToggle;

        private bool            IsLoggingEnabled { get; set; }
        private bool            IsSDKInitialized { get; set; }
        private bool            ShouldReward { get; set; }

        #endregion

        private static int NativeAdTypesForType(int adTypes)
        {
            var nativeAdTypes = 0;

            if ((adTypes & AppodealAdType.Interstitial) > 0)
            {
                nativeAdTypes |= Interstitial;
            }

            if ((adTypes & AppodealAdType.Banner) > 0)
            {
                nativeAdTypes |= Banner;
            }

            if ((adTypes & AppodealAdType.Mrec) > 0)
            {
                nativeAdTypes |= Mrec;
            }

            if ((adTypes & AppodealAdType.RewardedVideo) > 0)
            {
                nativeAdTypes |= RewardedVideo;
            }

            return nativeAdTypes;
        }

        private static int NativeShowStyleForType(int adTypes)
        {
            if ((adTypes & AppodealShowStyle.Interstitial) > 0)
            {
                return Interstitial;
            }

            if ((adTypes & AppodealShowStyle.BannerTop) > 0)
            {
                return BannerTop;
            }

            if ((adTypes & AppodealShowStyle.BannerBottom) > 0)
            {
                return BannerBottom;
            }

            if ((adTypes & AppodealShowStyle.BannerLeft) > 0)
            {
                return BannerLeft;
            }

            if ((adTypes & AppodealShowStyle.BannerRight) > 0)
            {
                return BannerRight;
            }

            if ((adTypes & AppodealShowStyle.RewardedVideo) > 0)
            {
                return RewardedVideo;
            }

            return 0;
        }

        #region SimAppodealLogicForEditor

        private void SimSetAutoCache(int adTypes, bool isEnabled)
        {
            _ads.Keys.Where(key => (adTypes & key) > 0).ToList().ForEach(key => _ads[key].IsAutoCacheEnabled = isEnabled);
        }

        private void SimInitAdTypes(int adTypes)
        {
            _ads.Keys.Where(key => (adTypes & key) > 0).ToList().ForEach(key => _ads[key].IsInitialized = true);
            _ads.Keys.Where(key => _ads[key].IsAutoCacheEnabled && _ads[key].IsInitialized).ToList().ForEach(key => SimCacheAd(key));

            if (_appodealInitializationListener != null) _appodealInitializationListener.OnInitializationFinished(null);
        }

        private bool SimCheckIfSDKInitialized(string methodName)
        {
            if (!IsSDKInitialized)
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

        private void SimCacheAd(int adType)
        {
            EditorAd ad = GetEditorAdObjectByAdType(adType);
            string prefabName = ad?.PrefabName;

            if (string.IsNullOrEmpty(prefabName))
            {
                SimFireCallback(ad?.Name, FailedToLoad);
                return;
            }

            string defaultPath = $"{AppodealEditorConstants.PackagePath}/{AppodealEditorConstants.EditorAdPrefabsPath}/{prefabName}.prefab";
            var assetGuids = AssetDatabase.FindAssets($"{prefabName} t:prefab");
            var prefabPath = assetGuids.Length < 1 ? defaultPath : AssetDatabase.GUIDToAssetPath(assetGuids[0]);

            var adPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            var adGameObject = Object.Instantiate<GameObject>(adPrefab, Vector3.zero, Quaternion.identity);

            if (adGameObject == null)
            {
                SimFireCallback(ad.Name, FailedToLoad);
                return;
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
                        ShouldReward = toggle?.isOn ?? false;

                        Object.Destroy(adGameObject);

                        if (ad.Type == Interstitial) SimFireCallback(ad.Name, Closed);
                        else if (ad.Type == RewardedVideo) SimFireCallback(ad.Name, Closed, ShouldReward);

                        if (ShouldReward)
                        {
                            ShouldReward = false;
                            SimFireCallback(ad.Name, Finished, 50d, "diamonds");
                        }

                        _videoPlayer = null;

                        if (ad.IsAutoCacheEnabled) SimCacheAd(ad.Type);
                    });
                }
                else if (btn.gameObject.name == "Panel")
                {
                    btn.onClick.AddListener(() =>
                    {
                        SimFireCallback(ad.Name, Clicked);
                        Application.OpenURL("https://docs.appodeal.com/unity/get-started");
                    });
                }
            }

            ad.GameObject = adGameObject;

            if (ad.Type == Banner) SimFireCallback(ad.Name, Loaded, 80, false);
            else SimFireCallback(ad.Name, Loaded, false);
        }

        private bool SimShowAdAtPos(int adType, Vector2 pos)
        {
            EditorAd ad = GetEditorAdObjectByAdType(adType);

            if (SimShowAd(adType))
            {
                Vector2 calculatedPos = Vector2.zero;

                if (pos.x == AppodealViewPosition.HorizontalCenter || pos.x == AppodealViewPosition.HorizontalSmart) calculatedPos.x = (Screen.width - ad.Size.x * Screen.dpi / 160) / 2;
                else if (pos.x == AppodealViewPosition.HorizontalLeft) calculatedPos.x = 0;
                else if (pos.x == AppodealViewPosition.HorizontalRight) calculatedPos.x = Screen.width - ad.Size.x * Screen.dpi / 160;
                else calculatedPos.x = pos.x;

                if (pos.y == AppodealViewPosition.VerticalBottom) calculatedPos.y = ad.Size.y * Screen.dpi / 160 - Screen.height;
                else if (pos.y == AppodealViewPosition.VerticalTop) calculatedPos.y = 0;
                else calculatedPos.y = - pos.y;

                if (calculatedPos.x < 0 || calculatedPos.y > 0 || calculatedPos.x > Screen.width - ad.Size.x * Screen.dpi / 160 || calculatedPos.y < ad.Size.y * Screen.dpi / 160 - Screen.height) return false;

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
                case Mrec:
                    SetBannerWidth(ad, adType);
                    break;
                case Banner:
                    SetBannerPosition(ad, adType);
                    SetBannerWidth(ad, adType);
                    break;
                case Interstitial:
                    var img = ad.GameObject.transform.Find("Panel").GetComponent<Image>();
                    var sprite = Screen.height < Screen.width ? Resources.Load<Sprite>(HorizontalInterstitialAssetName) : Resources.Load<Sprite>(VerticalInterstitialAssetName);
                    img.sprite = sprite;
                    break;
                case RewardedVideo:
                    _videoPlayer = ad.GameObject.GetComponentInChildren<VideoPlayer>();
                    var videoClip = Screen.height < Screen.width ? Resources.Load<VideoClip>(HorizontalVideoAssetName) : Resources.Load<VideoClip>(VerticalVideoAssetName);
                    _videoPlayer.clip = videoClip;
                    break;
            }

            ad.GameObject.SetActive(true);
            SimFireCallback(ad.Name, Shown);
            _videoPlayer?.Play();

            _adRevenueListener?.OnAdRevenueReceived(
                new AppodealAdRevenue
                {
                    AdType = ad.Name,
                    NetworkName = "UnityEditor",
                    AdUnitName = $"Test{ad.Name}AdUnit",
                    DemandSource = "TestAds",
                    Placement = "default",
                    Revenue = 42d,
                    Currency = "USD",
                    RevenuePrecision = "undefined"
                }
            );

            return true;
        }

        private void SimHideAd(int adType)
        {
            EditorAd ad = GetEditorAdObjectByAdType(adType);

            if (ad == null) return;

            if (ad.PrefabName == "ApdBannerViewAd" && adType != BannerView) return;
            else if (ad.PrefabName != "ApdBannerViewAd" && adType == BannerView) return;

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
                    if (methodName == FailedToLoad) _interstitialAdListener?.OnInterstitialFailedToLoad();
                    else if (methodName == Closed) _interstitialAdListener?.OnInterstitialClosed();
                    else if (methodName == Shown) _interstitialAdListener?.OnInterstitialShown();
                    else if (methodName == Loaded) _interstitialAdListener?.OnInterstitialLoaded((param1 != null && param1 is bool) ? (bool)param1 : false);
                    else if (methodName == Clicked) _interstitialAdListener?.OnInterstitialClicked();
                    break;
                case "Banner":
                    if (methodName == FailedToLoad) _bannerAdListener?.OnBannerFailedToLoad();
                    else if (methodName == Shown) _bannerAdListener?.OnBannerShown();
                    else if (methodName == Loaded) _bannerAdListener?.OnBannerLoaded((param1 != null && param1 is int) ? (int)param1 : 50, (param2 != null && param2 is bool) ? (bool)param2 : false);
                    else if (methodName == Clicked) _bannerAdListener?.OnBannerClicked();
                    break;
                case "RewardedVideo":
                    if (methodName == FailedToLoad) _rewardedVideoAdListener?.OnRewardedVideoFailedToLoad();
                    else if (methodName == Closed) _rewardedVideoAdListener?.OnRewardedVideoClosed((param1 != null && param1 is bool) ? (bool)param1 : false);
                    else if (methodName == Shown) _rewardedVideoAdListener?.OnRewardedVideoShown();
                    else if (methodName == Loaded) _rewardedVideoAdListener?.OnRewardedVideoLoaded((param1 != null && param1 is bool) ? (bool)param1 : false);
                    else if (methodName == Clicked) _rewardedVideoAdListener?.OnRewardedVideoClicked();
                    else if (methodName == Finished) _rewardedVideoAdListener?.OnRewardedVideoFinished((param1 != null && param1 is double) ? (double)param1 : 100d, (param2 != null && param2 is string) ? (string)param2 : "coins");
                    break;
                case "Mrec":
                    if (methodName == FailedToLoad) _mrecAdListener?.OnMrecFailedToLoad();
                    else if (methodName == Shown) _mrecAdListener?.OnMrecShown();
                    else if (methodName == Loaded) _mrecAdListener?.OnMrecLoaded((param1 != null && param1 is bool) ? (bool)param1 : false);
                    else if (methodName == Clicked) _mrecAdListener?.OnMrecClicked();
                    break;
            }
        }

        private void SimSetInterstitialCallbacks(IInterstitialAdListener listener)
        {
            AppodealCallbacks.Interstitial.Instance.InterstitialAdEventsImpl.Listener = listener;
        }

        private void SimSetRewardedVideoCallbacks(IRewardedVideoAdListener listener)
        {
            AppodealCallbacks.RewardedVideo.Instance.RewardedVideoAdEventsImpl.Listener = listener;
        }

        private void SimSetBannerCallbacks(IBannerAdListener listener)
        {
            AppodealCallbacks.Banner.Instance.BannerAdEventsImpl.Listener = listener;
        }

        private void SimSetMrecCallbacks(IMrecAdListener listener)
        {
            AppodealCallbacks.Mrec.Instance.MrecAdEventsImpl.Listener = listener;
        }

        private void SimSetAdRevenueCallback(IAdRevenueListener listener)
        {
            AppodealCallbacks.AdRevenue.Instance.AdRevenueEventsImpl.Listener = listener;
        }

        private EditorAd GetEditorAdObjectByAdType(int adType)
        {
            EditorAd ad;
            if (adType == Banner || _bannerPositions.ContainsKey(adType))
            {
                _ads.TryGetValue(Banner, out ad);
            }
            else
            {
                _ads.TryGetValue(adType, out ad);
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
            if (adType == BannerLeft || adType == BannerRight) rt.sizeDelta = new Vector2(ad.Size.y * Screen.dpi / 160, Mathf.Min(Screen.height, ad.Size.x * Screen.dpi / 160));
            else rt.sizeDelta = new Vector2(Mathf.Min(Screen.width, ad.Size.x * Screen.dpi / 160), ad.Size.y * Screen.dpi / 160);
        }

        private string GetBannerPrefabNameByBannerPosition(int adType)
        {
            _bannerPositions.TryGetValue(adType, out var prefabName);

            return prefabName;
        }

        private bool CheckIfLoggingEnabled()
        {
            if (_loggingToggle == null)
            {
                _loggingToggle = GameObject.Find("Logging Toggle")?.GetComponent<Toggle>();
                IsLoggingEnabled = _loggingToggle?.isOn ?? false;
            }

            return IsLoggingEnabled;
        }

        private void SetCallbacks()
        {
            _adRevenueListener = AppodealCallbacks.AdRevenue.Instance.AdRevenueEventsImpl;
            _appodealInitializationListener = AppodealCallbacks.Sdk.Instance.SdkEventsImpl;
            _mrecAdListener = AppodealCallbacks.Mrec.Instance.MrecAdEventsImpl;
            _bannerAdListener = AppodealCallbacks.Banner.Instance.BannerAdEventsImpl;
            _interstitialAdListener = AppodealCallbacks.Interstitial.Instance.InterstitialAdEventsImpl;
            _rewardedVideoAdListener = AppodealCallbacks.RewardedVideo.Instance.RewardedVideoAdEventsImpl;
        }

        #endregion

        #region ImplementedMethods

        public void Initialize(string appKey, int adTypes, IAppodealInitializationListener listener)
        {
            if (IsSDKInitialized) return;

            AppodealCallbacks.Sdk.Instance.SdkEventsImpl.InitListener = listener;
            SetCallbacks();

            Debug.LogWarning("There is only simplified workflow of Appodeal SDK simulated in Editor. Make sure to test advertising on a real Android/iOS device before publishing.");
            IsSDKInitialized = true;
            SimInitAdTypes(NativeAdTypesForType(adTypes));
        }

        public bool IsInitialized(int adType)
        {
            return SimCheckIfAdTypeInitialized(NativeAdTypesForType(adType));
        }

        public bool Show(int adTypes)
        {
            return SimShowAd(NativeShowStyleForType(adTypes));
        }

        public bool Show(int adTypes, string placement)
        {
            return Show(adTypes);
        }

        public bool ShowBannerView(int yAxis, int xAxis, string placement)
        {
            return SimShowAdAtPos(BannerView, new Vector2(xAxis, yAxis));
        }

        public bool ShowMrecView(int yAxis, int xGravity, string placement)
        {
            return SimShowAdAtPos(Mrec, new Vector2(xGravity, yAxis));
        }

        public bool IsLoaded(int adTypes)
        {
            return SimCheckIfAdTypeIsLoaded(NativeAdTypesForType(adTypes));
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

            SimCacheAd(NativeAdTypesForType(adTypes));
        }

        public void Hide(int adTypes)
        {
            SimHideAd(NativeAdTypesForType(adTypes));
        }

        public void HideBannerView()
        {
            SimHideAd(BannerView);
        }

        public void HideMrecView()
        {
            SimHideAd(Mrec);
        }

        public bool IsPrecache(int adTypes)
        {
            return SimIsPrecache(NativeAdTypesForType(adTypes));
        }

        public void SetAutoCache(int adTypes, bool autoCache)
        {
            SimSetAutoCache(NativeAdTypesForType(adTypes), autoCache);
        }

        public bool CanShow(int adTypes)
        {
            return IsLoaded(adTypes);
        }

        public bool CanShow(int adTypes, string placement)
        {
            return IsLoaded(adTypes);
        }

        public AppodealReward GetReward(string placement)
        {
            return new AppodealReward() { Amount = 42d, Currency = null };
        }

        public bool IsAutoCacheEnabled(int adType)
        {
            return SimIsAutoCacheEnabled(NativeAdTypesForType(adType));
        }

        public void Destroy(int adTypes)
        {
            SimDestroyAd(NativeAdTypesForType(adTypes));
        }

        public void SetLogLevel(AppodealLogLevel logging)
        {
            IsLoggingEnabled = logging != AppodealLogLevel.None;
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

        public void SetAdRevenueCallback(IAdRevenueListener listener)
        {
            SimSetAdRevenueCallback(listener);
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

        public double GetPredictedEcpm(int adType)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.GetPredictedEcpm method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
            return 0;
        }

        public double GetPredictedEcpmForPlacement(int adType, string placement)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.GetPredictedEcpmForPlacement method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
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

        public void LogEvent(string eventName, Dictionary<string, object> eventParams)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.LogEvent method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void ValidatePlayStoreInAppPurchase(IPlayStoreInAppPurchase purchase, IInAppPurchaseValidationListener listener)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.ValidatePlayStoreInAppPurchase method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        public void ValidateAppStoreInAppPurchase(IAppStoreInAppPurchase purchase, IInAppPurchaseValidationListener listener)
        {
            if (CheckIfLoggingEnabled()) Debug.Log("Calling Appodeal.ValidateAppStoreInAppPurchase method on an unsupported platform. Run your application on either Android or iOS device to test this method.");
        }

        #endregion
    }
}
