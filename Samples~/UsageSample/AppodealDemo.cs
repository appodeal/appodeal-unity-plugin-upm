using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;
using AppodealCM.Unity.Api;
using AppodealCM.Unity.Common;
using AppodealCM.Unity.Platforms;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

namespace AppodealSample
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "ParameterHidesMember")]
    public class AppodealDemo : MonoBehaviour, IConsentFormListener, IConsentInfoUpdateListener,
        IBannerAdListener, IMrecAdListener, IRewardedVideoAdListener, IInterstitialAdListener
    {
        #region Constants

        private const string CACHE_INTERSTITIAL = "CACHE INTERSTITIAL";
        private const string SHOW_INTERSTITIAL = "SHOW INTERSTITIAL";
        private const string CACHE_REWARDED_VIDEO = "CACHE REWARDED VIDEO";

        #endregion

        #region UI

        [SerializeField] public GameObject consentManagerPanel;
        [SerializeField] public GameObject appodealPanel;
        [SerializeField] public Text pluginVersionText;
        [SerializeField] public Toggle testingToggle;
        [SerializeField] public Toggle loggingToggle;
        [SerializeField] public Button interstitialButton;
        [SerializeField] public Button rewardedVideoButton;

        #endregion

        #region Application keys

#if UNITY_EDITOR && !UNITY_ANDROID && !UNITY_IPHONE
        public static string appKey = "";
#elif UNITY_ANDROID
       public static string appKey = "fee50c333ff3825fd6ad6d38cff78154de3025546d47a84f";
#elif UNITY_IPHONE
       public static string appKey = "466de0d625e01e8811c588588a42a55970bc7c132649eede";
#else
	public static string appKey = "";
#endif

        #endregion

        private ConsentForm consentForm;
        private ConsentManager consentManager;
        private bool isShouldSaveConsentForm;
        private Consent currentConsent;

        private void Awake() {
            Assert.IsNotNull(consentManagerPanel);
            Assert.IsNotNull(appodealPanel);
            Assert.IsNotNull(pluginVersionText);
            Assert.IsNotNull(testingToggle);
            Assert.IsNotNull(loggingToggle);
            Assert.IsNotNull(interstitialButton);
            Assert.IsNotNull(rewardedVideoButton);
        }

        private void Start()
        {
            pluginVersionText.text = $"Appodeal Unity Plugin v{AppodealVersions.APPODEAL_PLUGIN_VERSION}";
            consentManagerPanel.gameObject.SetActive(true);
            appodealPanel.gameObject.SetActive(false);

            interstitialButton.GetComponentInChildren<Text>().text = CACHE_INTERSTITIAL;
            rewardedVideoButton.GetComponentInChildren<Text>().text = CACHE_REWARDED_VIDEO;

            consentManager = ConsentManager.getInstance();
        }

        private void OnDestroy()
        {
            Appodeal.destroy(AppodealAdType.BANNER);
        }

        public void RequestConsentInfoUpdate()
        {
            consentManager.requestConsentInfoUpdate(appKey, this);
        }

        public void SetCustomVendor()
        {
            var customVendor = new Vendor.Builder(
                    "Appodeal Test",
                    "com.appodeal.test",
                    "https://customvendor.com")
                .setPurposeIds(new List<int> {100, 200, 300})
                .setFeatureId(new List<int> {400, 500, 600})
                .setLegitimateInterestPurposeIds(new List<int> {700, 800, 900})
                .build();

            consentManager.setCustomVendor(customVendor);

            var vendor = consentManager.getCustomVendor("com.appodeal.test");
            if (vendor == null) return;
            Debug.Log("Vendor getName: " + vendor.getName());
            Debug.Log("Vendor getBundle: " + vendor.getBundle());
            Debug.Log("Vendor getPolicyUrl: " + vendor.getPolicyUrl());
            foreach (var purposeId in vendor.getPurposeIds())
            {
                Debug.Log("Vendor getPurposeIds: " + purposeId);
            }

            foreach (var featureId in vendor.getFeatureIds())
            {
                Debug.Log("Vendor getFeatureIds: " + featureId);
            }

            foreach (var legitimateInterestPurposeId in vendor.getLegitimateInterestPurposeIds())
            {
                Debug.Log("Vendor getLegitimateInterestPurposeIds: " + legitimateInterestPurposeId);
            }
        }

        public void ShouldShowForm()
        {
            Debug.Log("shouldShowConsentDialog: " + consentManager.shouldShowConsentDialog());
        }

        public void GetConsentZone()
        {
            Debug.Log("getConsentZone: " + consentManager.getConsentZone());
        }

        public void GetConsentStatus()
        {
            Debug.Log("getConsentStatus: " + consentManager.getConsentStatus());
        }

        public void LoadConsentForm()
        {
            consentForm = new ConsentForm.Builder().withListener(this).build();
            if (consentForm != null)
            {
                consentForm.load();
            }
        }

        public void IsLoadedConsentForm()
        {
            if (consentForm != null)
            {
                Debug.Log("isLoadedConsentForm:  " + consentForm.isLoaded());
            }
        }

        public void ShowFormAsActivity()
        {
            if (consentForm != null)
            {
                consentForm.showAsActivity();
            }
            else
            {
                Debug.Log("showForm - false");
            }
        }

        public void ShowFormAsDialog()
        {
            if (consentForm != null)
            {
                consentForm.showAsDialog();
            }
            else
            {
                Debug.Log("showForm - false");
            }
        }

        public void PrintIABString()
        {
            Debug.Log("Consent IAB String is: " + consentManager.getConsent().getIabConsentString());
        }

        public void PrintCurrentConsent()
        {
            if (consentManager.getConsent() == null) return;
            Debug.Log(
                "consent.getIabConsentString() - " + consentManager.getConsent().getIabConsentString());
            Debug.Log(
                "consent.hasConsentForVendor() - " +
                consentManager.getConsent().hasConsentForVendor("com.appodeal.test"));
            Debug.Log("consent.getStatus() - " + consentManager.getConsent().getStatus());
            Debug.Log("consent.getZone() - " + consentManager.getConsent().getZone());
        }

        public void PrintAuthorizationStatus()
        {
            if (consentManager.getConsent() == null) return;
            Debug.Log($"AuthorizationStatus - {consentManager.getConsent().getAuthorizationStatus()} ");
        }

        public void ShowAppodealLogic()
        {
            consentManagerPanel.SetActive(false);
            appodealPanel.SetActive(true);
        }

        public void Initialize()
        {
            InitWithConsent(currentConsent != null);
        }

        public void InitWithConsent(bool isConsent)
        {
            Appodeal.setTesting(testingToggle.isOn);
            Appodeal.setLogLevel(loggingToggle.isOn ? AppodealLogLevel.Verbose : AppodealLogLevel.None);
            Appodeal.setUserId("1");
            Appodeal.setUserAge(1);
            Appodeal.setUserGender(Gender.OTHER);
            Appodeal.disableLocationPermissionCheck();
            Appodeal.setTriggerOnLoadedOnPrecache(AppodealAdType.INTERSTITIAL, true);
            Appodeal.setSmartBanners(true);
            Appodeal.setBannerAnimation(false);
            Appodeal.setTabletBanners(false);
            Appodeal.setBannerBackground(false);
            Appodeal.setChildDirectedTreatment(false);
            Appodeal.muteVideosIfCallsMuted(true);
            Appodeal.setSharedAdsInstanceAcrossActivities(true);
            Appodeal.setAutoCache(AppodealAdType.INTERSTITIAL, false);
            Appodeal.setAutoCache(AppodealAdType.REWARDED_VIDEO, false);
            Appodeal.setUseSafeArea(true);

            Appodeal.setBannerCallbacks(this);
            Appodeal.setInterstitialCallbacks(this);
            Appodeal.setRewardedVideoCallbacks(this);
            Appodeal.setMrecCallbacks(this);

            if (isConsent)
            {
                Appodeal.initialize(appKey,
                    AppodealAdType.INTERSTITIAL | AppodealAdType.BANNER | AppodealAdType.REWARDED_VIDEO | AppodealAdType.MREC,
                    currentConsent);
            }
            else
            {
                Appodeal.initialize(appKey,
                    AppodealAdType.INTERSTITIAL | AppodealAdType.BANNER | AppodealAdType.REWARDED_VIDEO | AppodealAdType.MREC,
                    true);
            }

            Appodeal.setCustomFilter("newBoolean", true);
            Appodeal.setCustomFilter("newInt", 1234567890);
            Appodeal.setCustomFilter("newDouble", 123.123456789);
            Appodeal.setCustomFilter("newString", "newStringFromSDK");
        }

        public void ShowInterstitial()
        {
            if (Appodeal.isLoaded(AppodealAdType.INTERSTITIAL) && Appodeal.canShow(AppodealAdType.INTERSTITIAL, "default") && !Appodeal.isPrecache(AppodealAdType.INTERSTITIAL))
            {
                Appodeal.show(AppodealAdType.INTERSTITIAL);
            }
            else
            {
                Appodeal.cache(AppodealAdType.INTERSTITIAL);
            }
        }

        public void ShowRewardedVideo()
        {
            if (Appodeal.isLoaded(AppodealAdType.REWARDED_VIDEO) && Appodeal.canShow(AppodealAdType.REWARDED_VIDEO, "default"))
            {
                Appodeal.show(AppodealAdType.REWARDED_VIDEO);
            }
            else
            {
                Appodeal.cache(AppodealAdType.REWARDED_VIDEO);
            }
        }

        public void ShowBannerBottom()
        {
            Appodeal.show(AppodealAdType.BANNER_BOTTOM, "default");
        }

        public void ShowBannerTop()
        {
            Appodeal.show(AppodealAdType.BANNER_TOP, "default");
        }

        public void HideBanner()
        {
            Appodeal.hide(AppodealAdType.BANNER);
        }

        public void ShowBannerView()
        {
            Appodeal.showBannerView(AppodealViewPosition.VERTICAL_BOTTOM,
                AppodealViewPosition.HORIZONTAL_CENTER, "default");
        }

        public void HideBannerView()
        {
            Appodeal.hideBannerView();
        }

        public void ShowMrecView()
        {
            Appodeal.showMrecView(AppodealViewPosition.VERTICAL_TOP,
                AppodealViewPosition.HORIZONTAL_CENTER, "default");
        }

        public void HideMrecView()
        {
            Appodeal.hideMrecView();
        }

        public void ShowBannerLeft()
        {
            Appodeal.show(AppodealAdType.BANNER_LEFT);
        }

        public void ShowBannerRight()
        {
            Appodeal.show(AppodealAdType.BANNER_RIGHT);
        }

        #region ConsentFormListener

        public void onConsentFormLoaded()
        {
            Debug.Log("ConsentFormListener - onConsentFormLoaded");
        }

        public void onConsentFormError(ConsentManagerException exception)
        {
            Debug.Log($"ConsentFormListener - onConsentFormError, reason - {exception.getReason()}");
        }

        public void onConsentFormOpened()
        {
            Debug.Log("ConsentFormListener - onConsentFormOpened");
        }

        public void onConsentFormClosed(Consent consent)
        {
            currentConsent = consent;
            Debug.Log($"ConsentFormListener - onConsentFormClosed, consentStatus - {consent.getStatus()}");
        }

        #endregion

        #region ConsentInfoUpdateListener

        public void onConsentInfoUpdated(Consent consent)
        {
            currentConsent = consent;
            Debug.Log("onConsentInfoUpdated");
        }

        public void onFailedToUpdateConsentInfo(ConsentManagerException error)
        {
            Debug.Log($"onFailedToUpdateConsentInfo");

            if (error == null) return;
            Debug.Log($"onFailedToUpdateConsentInfo Reason: {error.getReason()}");

            switch (error.getCode())
            {
                case 0:
                    Debug.Log("onFailedToUpdateConsentInfo - UNKNOWN");
                    break;
                case 1:
                    Debug.Log(
                        "onFailedToUpdateConsentInfo - INTERNAL - Error on SDK side. Includes JS-bridge or encoding/decoding errors");
                    break;
                case 2:
                    Debug.Log("onFailedToUpdateConsentInfo - NETWORKING - HTTP errors, parse request/response ");
                    break;
                case 3:
                    Debug.Log("onFailedToUpdateConsentInfo - INCONSISTENT - Incorrect SDK API usage");
                    break;
            }
        }

        #endregion

        #region Banner callback handlers

        public void onBannerLoaded(int height, bool precache)
        {
            Debug.Log("onBannerLoaded");
            Debug.Log($"Banner height - {height}");
            Debug.Log($"Banner precache - {precache}");
        }

        public void onBannerFailedToLoad()
        {
            Debug.Log("onBannerFailedToLoad");
        }

        public void onBannerShown()
        {
            print("onBannerShown");
        }

        public void onBannerClicked()
        {
            print("onBannerClicked");
        }

        public void onBannerExpired()
        {
            print("onBannerExpired");
        }

        #endregion

        #region Interstitial callback handlers

        public void onInterstitialLoaded(bool isPrecache)
        {
            if (!isPrecache)
            {
                interstitialButton.GetComponentInChildren<Text>().text = SHOW_INTERSTITIAL;
            }
            else
            {
                Debug.Log("Appodeal. Interstitial loaded. isPrecache - true");
            }

            Debug.Log("onInterstitialLoaded");
        }

        public void onInterstitialFailedToLoad()
        {
            Debug.Log("onInterstitialFailedToLoad");
        }

        public void onInterstitialShowFailed()
        {
            Debug.Log("onInterstitialShowFailed");
        }

        public void onInterstitialShown()
        {
            Debug.Log("onInterstitialShown");
        }

        public void onInterstitialClosed()
        {
            interstitialButton.GetComponentInChildren<Text>().text = CACHE_INTERSTITIAL;
            Debug.Log("onInterstitialClosed");
        }

        public void onInterstitialClicked()
        {
            Debug.Log("onInterstitialClicked");
        }

        public void onInterstitialExpired()
        {
            Debug.Log("onInterstitialExpired");
        }

        #endregion

        #region Rewarded Video callback handlers

        public void onRewardedVideoLoaded(bool isPrecache)
        {
            rewardedVideoButton.GetComponentInChildren<Text>().text = "SHOW REWARDED VIDEO";
            print("onRewardedVideoLoaded");
        }

        public void onRewardedVideoFailedToLoad()
        {
            print("onRewardedVideoFailedToLoad");
        }

        public void onRewardedVideoShowFailed()
        {
            print("onRewardedVideoShowFailed");
        }

        public void onRewardedVideoShown()
        {
            print("onRewardedVideoShown");
        }

        public void onRewardedVideoClosed(bool finished)
        {
            rewardedVideoButton.GetComponentInChildren<Text>().text = "CACHE REWARDED VIDEO";
            print($"onRewardedVideoClosed. Finished - {finished}");
        }

        public void onRewardedVideoFinished(double amount, string name)
        {
            print("onRewardedVideoFinished. Reward: " + amount + " " + name);
        }

        public void onRewardedVideoExpired()
        {
            print("onRewardedVideoExpired");
        }

        public void onRewardedVideoClicked()
        {
            print("onRewardedVideoClicked");
        }

        #endregion

        #region Mrec callback handlers

        public void onMrecLoaded(bool precache)
        {
            print($"onMrecLoaded. Precache - {precache}");
        }

        public void onMrecFailedToLoad()
        {
            print("onMrecFailedToLoad");
        }

        public void onMrecShown()
        {
            print("onMrecShown");
        }

        public void onMrecClicked()
        {
            print("onMrecClicked");
        }

        public void onMrecExpired()
        {
            print("onMrecExpired");
        }

        #endregion

    }
}
