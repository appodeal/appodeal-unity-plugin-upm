using System;
using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AppodealAds.Unity.Common;
using AppodealAds.Unity.Platforms;
using AppodealCM.Unity.Api;
using AppodealCM.Unity.Platforms;

namespace AppodealAds.Unity.Api
{
    /// <summary>
    /// <para>
    /// Appodeal Unity API for developers, including documentation.
    /// </para>
    /// See <see href="https://wiki.appodeal.com/en/unity/get-started"/> for more details.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMethodReturnValue.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "InvalidXmlDocComment")]
    public static class Appodeal
    {
        private static IAppodealAdsClient client;

        private static IAppodealAdsClient getInstance()
        {
            return client ?? (client = AppodealAdsClientFactory.GetAppodealAdsClient());
        }

        /// <summary>
        /// <para>
        /// Initializes the relevant (Android or iOS) Appodeal SDK.
        /// See <see cref="onAppodealInitialized()"/> for resulting triggered event.
        /// </para>
        /// <example>To initialize only interstitials use:<code>Appodeal.initialize(appKey, AppodealAdType.INTERSTITIAL);</code></example>
        /// <example>To initialize only banners use:<code>Appodeal.initialize(appKey, AppodealAdType.BANNER);</code></example>
        /// <example>To initialize only rewarded video use:<code>Appodeal.initialize(appKey, AppodealAdType.REWARDED_VIDEO);</code></example>
        /// <example>To initialize only non-skippable video use:<code>Appodeal.initialize(appKey, AppodealAdType.NON_SKIPPABLE_VIDEO);</code></example>
        /// <example>To initialize only 300*250 banners use:<code>Appodeal.initialize(appKey, AppodealAdType.MREC);</code></example> 
        /// </summary>
        /// <remarks>See <see href="https://wiki.appodeal.com/en/unity/get-started#UnitySDK.GetStarted-Step3Step3.InitializeSDK"/> for more details.</remarks>
        /// <param name="appKey">Appodeal app key you received when you created an app.</param>
        /// <param name="adTypes">Type of advertisement you want to initialize.</param>
        /// <param name="listener">Appodeal Initialization callback.</param>
        public static void initialize(string appKey, int adTypes, IAppodealInitializeListener listener = null)
        {
            
        }
        
        /// <summary>
        /// <para>Checks whether or not ad type is initialized.</para>
        /// <example>To check interstitials use:<code>Appodeal.isInitialized(AppodealAdType.INTERSTITIAL);</code></example>
        /// <example>To check banners use:<code>Appodeal.isInitialized(AppodealAdType.BANNER);</code></example>
        /// <example>To check rewarded video use:<code>Appodeal.isInitialized(AppodealAdType.REWARDED_VIDEO);</code></example>
        /// <example>To check non-skippable video use:<code>Appodeal.isInitialized(AppodealAdType.NON_SKIPPABLE_VIDEO);</code></example>
        /// <example>To check 300*250 banners use:<code>Appodeal.isInitialized(AppodealAdType.MREC);</code></example> 
        /// </summary>
        /// <param name="adType">type of advertisement.</param>
        /// <returns>true if ad type is initialized, otherwise - false.</returns>
        public static bool isInitialized(int adType)
        {
            return getInstance().isInitialized(adType);
        }

        /// <summary>
        /// <para>Checks whether or not auto cache is enabled for the specified ad type.</para>
        /// <example>To check interstitials use:<code>Appodeal.isAutoCacheEnabled(AppodealAdType.INTERSTITIAL);</code></example>
        /// <example>To check banners use:<code>Appodeal.isAutoCacheEnabled(AppodealAdType.BANNER);</code></example>
        /// <example>To check rewarded video use:<code>Appodeal.isAutoCacheEnabled(AppodealAdType.REWARDED_VIDEO);</code></example>
        /// <example>To check non-skippable video use:<code>Appodeal.isAutoCacheEnabled(AppodealAdType.NON_SKIPPABLE_VIDEO);</code></example>
        /// <example>To check 300*250 banners use:<code>Appodeal.isAutoCacheEnabled(AppodealAdType.MREC);</code></example> 
        /// </summary>
        /// <param name="adType">type of advertisement.</param>
        /// <returns>true if auto cache is enabled, otherwise - false.</returns>
        public static bool isAutoCacheEnabled(int adType)
        {
            return getInstance().isAutoCacheEnabled(adType);
        }
        
        /// <summary>Sets Interstitial ad type callbacks.</summary>
        /// <remarks>See <see href="https://wiki.appodeal.com/en/unity/get-started/ad-types/interstitial#id-[Development]UnitySDK.Interstitial-InterstitialCallbacks"/> for more details.</remarks>
        /// <param name="listener">class which implements AppodealAds.Unity.Common.IInterstitialAdListener interface.</param>
        public static void setInterstitialCallbacks(IInterstitialAdListener listener)
        {
            getInstance().setInterstitialCallbacks(listener);
        }

        /// <summary>Sets Rewarded Video ad type callbacks.</summary>
        /// <remarks>See <see href="https://wiki.appodeal.com/en/unity/get-started/ad-types/rewarded-video#id-[Development]UnitySDK.Rewardedvideo-RewardedVideoCallbacks"/> for more details.</remarks>
        /// <param name="listener">class which implements AppodealAds.Unity.Common.IRewardedVideoAdListener interface.</param>
        public static void setRewardedVideoCallbacks(IRewardedVideoAdListener listener)
        {
            getInstance().setRewardedVideoCallbacks(listener);
        }
        
        /// <summary>Sets Non-Skippable Video ad type callbacks.</summary>
        /// <param name="listener">class which implements AppodealAds.Unity.Common.INonSkippableVideoAdListener interface.</param>
        public static void setNonSkippableVideoCallbacks(INonSkippableVideoAdListener listener)
        {
            getInstance().setNonSkippableVideoCallbacks(listener);
        }

        /// <summary>Sets Banner ad type callbacks.</summary>
        /// <remarks>See <see href="https://wiki.appodeal.com/en/unity/get-started/ad-types/banner#id-[Development]UnitySDK.Banner-BannerCallbacks"/> for more details.</remarks>
        /// <param name="listener">class which implements AppodealAds.Unity.Common.IBannerAdListener interface.</param>
        public static void setBannerCallbacks(IBannerAdListener listener)
        {
            getInstance().setBannerCallbacks(listener);
        }

        /// <summary>Sets Mrec ad type callbacks.</summary>
        /// <remarks>See <see href="https://wiki.appodeal.com/en/unity/get-started/ad-types/mrec#id-[Development]UnitySDK.MREC-MRECCallbacks"/> for more details.</remarks>
        /// <param name="listener">class which implements AppodealAds.Unity.Common.IMrecAdListener interface.</param>
        public static void setMrecCallbacks(IMrecAdListener listener)
        {
            getInstance().setMrecCallbacks(listener);
        }
        
        /// <summary>
        /// <para>
        /// Caches ads in a manual mode.
        /// Use it only if <see cref="Appodeal.isAutoCacheEnabled"/> is set to false.
        /// </para>
        /// <example>To cache interstitials use:<code>Appodeal.cache(AppodealAdType.INTERSTITIAL);</code></example>
        /// <example>To cache banners use:<code>Appodeal.cache(AppodealAdType.BANNER);</code></example>
        /// <example>To cache rewarded video use:<code>Appodeal.cache(AppodealAdType.REWARDED_VIDEO);</code></example>
        /// <example>To cache non-skippable video use:<code>Appodeal.cache(AppodealAdType.NON_SKIPPABLE_VIDEO);</code></example>
        /// <example>To cache 300*250 banners use:<code>Appodeal.cache(AppodealAdType.MREC);</code></example> 
        /// </summary>
        /// <remarks>See <see href="https://faq.appodeal.com/en/articles/2658522-sdk-caching"/> for more details.</remarks>
        /// <param name="adTypes">type of advertisement.</param>
        public static void cache(int adTypes)
        {
            getInstance().cache(adTypes);
        }

        /// <summary>
        /// <para>
        /// Shows an advertisement.
        /// You can use <see cref="Appodeal.isLoaded"/> method first, to check if an ad is currently available.
        /// </para>
        /// <example>To show interstitials use:<code>Appodeal.show(AppodealAdType.INTERSTITIAL);</code></example>
        /// <example>To show horizontal banner at top use:<code>Appodeal.show(AppodealAdType.BANNER_TOP);</code></example>
        /// <example>To show horizontal banner at bottom use:<code>Appodeal.show(AppodealAdType.BANNER_BOTTOM);</code></example>
        /// <example>To show vertical banner at left use:<code>Appodeal.show(AppodealAdType.BANNER_LEFT);</code></example>
        /// <example>To show vertical banner at right use:<code>Appodeal.show(AppodealAdType.BANNER_RIGHT);</code></example>
        /// <example>To show rewarded video use:<code>Appodeal.show(AppodealAdType.REWARDED_VIDEO);</code></example>
        /// <example>To show non-skippable video use:<code>Appodeal.show(AppodealAdType.NON_SKIPPABLE_VIDEO);</code></example>
        /// <example>To show 300*250 banners use:<code>Appodeal.show(AppodealAdType.MREC);</code></example> 
        /// </summary>
        /// <param name="adTypes">type of advertisement.</param>
        /// <returns>true if an ad was shown, otherwise - false</returns>
        public static bool show(int adTypes)
        {
            return getInstance().show(adTypes);
        }

        /// <summary>
        /// <para>
        /// Shows an advertisement in a specified placement.
        /// See <see href="https://faq.appodeal.com/en/articles/1154394-placements"/> for more details.
        /// </para>
        /// <example>To show interstitials use:<code>Appodeal.show(AppodealAdType.INTERSTITIAL, placementName);</code></example>
        /// <example>To show horizontal banner at top use:<code>Appodeal.show(AppodealAdType.BANNER_TOP, placementName);</code></example>
        /// <example>To show horizontal banner at bottom use:<code>Appodeal.show(AppodealAdType.BANNER_BOTTOM, placementName);</code></example>
        /// <example>To show vertical banner at left use:<code>Appodeal.show(AppodealAdType.BANNER_LEFT, placementName);</code></example>
        /// <example>To show vertical banner at right use:<code>Appodeal.show(AppodealAdType.BANNER_RIGHT, placementName);</code></example>
        /// <example>To show rewarded video use:<code>Appodeal.show(AppodealAdType.REWARDED_VIDEO, placementName);</code></example>
        /// <example>To show non-skippable video use:<code>Appodeal.show(AppodealAdType.NON_SKIPPABLE_VIDEO, placementName);</code></example>
        /// <example>To show 300*250 banners use:<code>Appodeal.show(AppodealAdType.MREC, placementName);</code></example> 
        /// </summary>
        /// <param name="adTypes">type of advertisement.</param>
        /// <param name="placement">name of placement.</param>
        /// <returns>true if an ad was shown, otherwise - false</returns>
        public static bool show(int adTypes, string placement)
        {
            return getInstance().show(adTypes, placement);
        }

        /// <summary>
        /// <para>
        /// Shows banner at a specified position on the screen.
        /// You can either use predefined <see cref="AppodealViewPosition"/> constants or set any desired offset.
        /// </para>
        /// <example>To show banner at top left use:<code>Appodeal.showBannerView(AppodealViewPosition.VERTICAL_TOP, AppodealViewPosition.HORIZONTAL_LEFT placementName);</code></example>
        /// <example>To show banner at bottom center use:<code>Appodeal.showBannerView(AppodealViewPosition.VERTICAL_BOTTOM, AppodealViewPosition.HORIZONTAL_CENTER, placementName);</code></example>
        /// </summary>
        /// <remarks>If banner view is out of screen because of the offset you specified, advertisement will not be shown.</remarks>
        /// <param name="YAxis">y axis offset for banner view.</param>
        /// <param name="XGravity">x axis offset for banner view.</param>
        /// <param name="placement">name of placement.</param>
        /// <returns>true if an ad was shown, otherwise - false</returns>
        public static bool showBannerView(int YAxis, int XGravity, string placement)
        {
            return getInstance().showBannerView(YAxis, XGravity, placement);
        }
        
        /// <summary>
        /// <para>
        /// Shows mrec at a specified position on the screen.
        /// You can either use predefined <see cref="AppodealViewPosition"/> constants or set any desired offset.
        /// </para>
        /// <example>To show mrec at top left use:<code>Appodeal.showMrecView(AppodealViewPosition.VERTICAL_TOP, AppodealViewPosition.HORIZONTAL_LEFT placementName);</code></example>
        /// <example>To show mrec at bottom center use:<code>Appodeal.showMrecView(AppodealViewPosition.VERTICAL_BOTTOM, AppodealViewPosition.HORIZONTAL_CENTER, placementName);</code></example>
        /// </summary>
        /// <remarks>If mrec view is out of screen because of the offset you specified, advertisement will not be shown.</remarks>
        /// <param name="YAxis">y axis offset for mrec view.</param>
        /// <param name="XGravity">x axis offset for mrec view.</param>
        /// <param name="placement">name of placement.</param>
        /// <returns>true if an ad was shown, otherwise - false</returns>
        public static bool showMrecView(int YAxis, int XGravity, string placement)
        {
            return getInstance().showMrecView(YAxis, XGravity, placement);
        }
        
        /// <summary>Hides active banner or mrec from screen.</summary>
        /// <remarks>It can only be applied to ads shown via <see cref="Appodeal.show"/> method.</remarks>
        /// <param name="adTypes">type of advertisement. Use AppodealAdType.BANNER for banners, and AppodealAdType.BANNER for mrec.</param>
        public static void hide(int adTypes)
        {
            getInstance().hide(adTypes);
        }

        /// <summary>Hides active banner view from screen.</summary>
        /// <remarks>It can only be applied to banners shown via <see cref="Appodeal.showBannerView"/> method.</remarks>
        public static void hideBannerView()
        {
            getInstance().hideBannerView();
        }

        /// <summary>Hides active mrec view from screen</summary>
        /// <remarks>It can only be applied to banners shown via <see cref="Appodeal.showMrecView"/> method.</remarks>
        public static void hideMrecView()
        {
            getInstance().hideMrecView();
        }
        
        /// <summary>
        /// <para>
        /// Defines whether or not auto cache is enabled for specified ad types (It is <see langword="true"/> for all ad types by default).
        /// </para>
        /// <para>
        /// Call before the SDK initialization.
        /// </para>
        /// You can also use <see cref="Appodeal.isAutoCacheEnabled"/> to check if auto cache is enabled for specified ad type.
        /// </summary>
        /// <remarks>See <see href="https://faq.appodeal.com/en/articles/2658522-sdk-caching"/> for more details.</remarks>
        /// <param name="adTypes">types of advertisement.</param>
        /// <param name="autoCache">true to enable auto cache, false to disable.</param>
        public static void setAutoCache(int adTypes, bool autoCache)
        {
            getInstance().setAutoCache(adTypes, autoCache);
        }
        
        /// <summary>
        /// <para>
        /// Defines whether or not <see langword="on[AdType]Loaded"/> callback should be fired if precache is loaded.
        /// </para>
        /// Call before the SDK initialization.
        /// </summary>
        /// <remarks>See <see href="https://faq.appodeal.com/en/articles/94830-ad-loading-time-and-precache"/> for more details.</remarks>
        /// <param name="adTypes">type of advertisement. Currently supported for interstitial, rewarded video, banner and MREC ad types.</param>
        /// <param name="onLoadedTriggerBoth">true - onLoaded will be triggered when precache or regular ad are loaded. false - onLoaded will trigger only when regular ad is loaded (default).</param>
        public static void setTriggerOnLoadedOnPrecache(int adTypes, bool onLoadedTriggerBoth)
        {
            getInstance().setTriggerOnLoadedOnPrecache(adTypes, onLoadedTriggerBoth);
        }

        /// <summary>
        /// <para>
        /// Checks whether or not an ad of a specified ad type is loaded.
        /// </para>
        /// <example>To check if interstitial is loaded use:<code>Appodeal.isLoaded(AppodealAdType.INTERSTITIAL);</code></example>
        /// <example>To check if rewarded video is loaded use:<code>Appodeal.isLoaded(AppodealAdType.REWARDED_VIDEO);</code></example>
        /// <example>To check if non-skippable video is loaded use:<code>Appodeal.isLoaded(AppodealAdType.NON_SKIPPABLE_VIDEO);</code></example>
        /// <example>To check if banner is loaded use:<code>Appodeal.isLoaded(AppodealAdType.BANNER);</code></example>
        /// <example>To check if 300*250 banner is loaded use:<code>Appodeal.isLoaded(AppodealAdType.MREC);</code></example>
        /// </summary>
        /// <param name="adTypes">type of advertisement.</param>
        /// <returns>true if advertisement is loaded, otherwise - false.</returns>
        public static bool isLoaded(int adTypes)
        {
            return getInstance().isLoaded(adTypes);
        }

        /// <summary>
        /// <para>
        /// Checks whether or not currently loaded ad is precache.
        /// </para>
        /// <example>To check if interstitial is precache use:<code>Appodeal.isPrecache(AppodealAdType.INTERSTITIAL);</code></example>
        /// </summary>
        /// <param name="adTypes">type of advertisement. Currently supported only for AppodealAdType.INTERSTITIAL.</param>
        /// <returns>true if advertisement is loaded and is precache, otherwise - false.</returns>
        public static bool isPrecache(int adTypes)
        {
            return getInstance().isPrecache(adTypes);
        }

        /// <summary>
        /// <para>
        /// Defines whether or not smart banners should be used (It is <see langword="true"/> by default).
        /// </para>
        /// Call before the SDK initialization.
        /// </summary>
        /// <remarks>See <see href="https://faq.appodeal.com/en/articles/2684869-banner-sizes"/> for more details.</remarks>
        /// <param name="enabled">true to enable smart banners, false to disable.</param>
        public static void setSmartBanners(bool enabled)
        {
            getInstance().setSmartBanners(enabled);
        }

        /// <summary>
        /// <para>
        /// Defines whether or not 728*90 banners should be used (It is <see langword="false"/> by default).
        /// </para>
        /// Call before the SDK initialization.
        /// </summary>
        /// <remarks>See <see href="https://faq.appodeal.com/en/articles/2684869-banner-sizes"/> for more details.</remarks>
        /// <param name="enabled">true to enable tablet banners, false to disable.</param>
        public static void setTabletBanners(bool enabled)
        {
            getInstance().setTabletBanners(enabled);
        }

        /// <summary>
        /// <para>
        /// Defines whether or not banner animation should be used (It is <see langword="true"/> by default).
        /// </para>
        /// Call before the SDK initialization.
        /// </summary>
        /// <param name="enabled">true to enable banner animation, false to disable.</param>
        public static void setBannerAnimation(bool enabled)
        {
            getInstance().setBannerAnimation(enabled);
        }

        /// <summary>
        /// Sets rotation for AppodealAdType.BANNER_LEFT and AppodealAdType.BANNER_RIGHT types (by default: left = -90, right = 90).
        /// </summary>
        /// <param name="leftBannerRotation">rotation for AppodealAdType.BANNER_LEFT.</param>
        /// <param name="rightBannerRotation">rotation for AppodealAdType.BANNER_RIGHT.</param>
        public static void setBannerRotation(int leftBannerRotation, int rightBannerRotation)
        {
            getInstance().setBannerRotation(leftBannerRotation, rightBannerRotation);
        }
        
        /// <summary>
        /// Defines whether or not safe area of the screen can be used (Supported only for <see langword="Android"/>).
        /// </summary> 
        /// <param name="value">true to enable usage of safe area, false to disable.</param>
        public static void setUseSafeArea(bool value)
        {
            getInstance().setUseSafeArea(value);
        }

        /// <summary>
        /// Tracks in-app purchase information and sends info to our servers.
        /// </summary>
        /// <remarks>See <see href="https://wiki.appodeal.com/en/unity/get-started/advanced/set-user-data#id-[Development]UnitySDK.SetUsersData-Trackin-apppurchases"/> for more details.</remarks>
        /// <param name="amount">amount of purchase.</param>
        /// <param name="currency">currency of purchase.</param>
        public static void trackInAppPurchase(double amount, string currency)
        {
            getInstance().trackInAppPurchase(amount, currency);
        }

        /// <summary>
        /// <para>
        /// Disables specified ad network for all ad types.
        /// </para>
        /// <example>To disable Facebook use:<code>Appodeal.disableNetwork(AppodealNetworks.FACEBOOK);</code></example>
        /// </summary>
        /// <remarks>We recommend using <see langword="AppodealNetworks"/> class to access networks' names.</remarks>
        /// <param name="network">network name.</param>
        public static void disableNetwork(string network)
        {
            getInstance().disableNetwork(network);
        }
        
        /// <summary>
        /// <para>
        /// Disables specified ad network for specified ad types.
        /// </para>
        /// <example>To disable Facebook for banners only use:<code>Appodeal.disableNetwork(AppodealNetworks.FACEBOOK, AppodealAdType.BANNER);</code></example>
        /// </summary>
        /// <remarks>We recommend using <see langword="AppodealNetworks"/> class to access networks' names.</remarks>
        /// <param name="network">network name.</param>
        /// <param name="adType">types of advertisement.</param>
        public static void disableNetwork(string network, int adType)
        {
            getInstance().disableNetwork(network, adType);
        }

        /// <summary>
        /// Defines whether or not location tracking is allowed (Supported only for <see langword="iOS"/>).
        /// </summary>
        /// <remarks>On android location tracking is always enabled if the corresponding permission was given.</remarks>
        /// <param name="value">true to enable location tracking, false to disable.</param>
        public static void setLocationTracking(bool value)
        {
            getInstance().setLocationTracking(value);
        }

        /// <summary>Sets user id.</summary>
        /// <remarks>See <see href="https://wiki.appodeal.com/en/unity/get-started/advanced/set-user-data"/> for more details.</remarks>
        /// <param name="id">user id.</param>
        public static void setUserId(string id)
        {
            getInstance().setUserId(id);
        }

        /// <summary>Sets user gender.</summary>
        /// <remarks>See <see href="https://wiki.appodeal.com/en/unity/get-started/advanced/set-user-data"/> for more details.</remarks>
        /// <param name="gender">user gender.</param>
        public static void setUserGender(AppodealUserGender gender)
        {
            getInstance().setUserGender(gender);
        }

        /// <summary>Sets user age.</summary>
        /// <remarks>See <see href="https://wiki.appodeal.com/en/unity/get-started/advanced/set-user-data"/> for more details.</remarks>
        /// <param name="age">user age.</param>
        public static void setUserAge(int age)
        {
            getInstance().setUserAge(age);
        }

        /// <summary>Gets native SDK version.</summary> 
        /// <returns>Appodeal (Android or iOS) SDK version string.</returns>
        public static string getNativeSDKVersion()
        {
            return getInstance().getVersion();
        }
        
        /// <summary>
        /// <para>
        /// Defines whether or not test mode should be enabled.
        /// </para>
        /// Call before the SDK initialization.
        /// </summary>
        /// <remarks>See <see href="https://wiki.appodeal.com/en/unity/get-started/advanced/testing#id-[Development]UnitySDK.Testing-Step2:TestYourSDKintegration"/> for more details.</remarks>
        /// <param name="test">true if test mode is enabled, otherwise - false.</param>
        public static void setTesting(bool test)
        {
            getInstance().setTesting(test);
        }

        /// <summary>
        /// <para>
        /// Defines the log level of Appodeal SDK.
        /// </para>
        /// Use <see langword="AppodealLogLevel"/> enum to access possible values.
        /// </summary>
        /// <remarks>All logs will be written with tag "Appodeal".</remarks>
        /// <param name="log">log-level state of AppodealLogLevel type .</param>
        public static void setLogLevel(AppodealLogLevel log)
        {
            getInstance().setLogLevel(log);
        }
        
        /// <summary>
        /// Sets custom segment filter.
        /// </summary>
        /// <param name="name">name of the filter.</param>
        /// <param name="value">filter value.</param>
        public static void setCustomFilter(string name, bool value)
        {
            getInstance().setCustomFilter(name, value);
        }
        
        /// <summary>
        /// Sets custom segment filter.
        /// </summary>
        /// <param name="name">name of the filter.</param>
        /// <param name="value">filter value.</param>
        public static void setCustomFilter(string name, int value)
        {
            getInstance().setCustomFilter(name, value);
        }

        /// <summary>
        /// Sets custom segment filter.
        /// </summary>
        /// <param name="name">name of the filter.</param>
        /// <param name="value">filter value.</param>
        public static void setCustomFilter(string name, double value)
        {
            getInstance().setCustomFilter(name, value);
        }

        /// <summary>
        /// Sets custom segment filter.
        /// </summary>
        /// <param name="name">name of the filter.</param>
        /// <param name="value">filter value.</param>
        public static void setCustomFilter(string name, string value)
        {
            getInstance().setCustomFilter(name, value);
        }
        
        /// <summary>
        /// Checks whether or not advertisement can be shown within <see langword="default"/> placement.
        /// </summary>
        /// <remarks>See <see href="https://faq.appodeal.com/en/collections/107523-placements"/> for more details.</remarks>
        /// <param name="adTypes">type of advertisement.</param>
        /// <returns>true if an ad can be shown within default placement, otherwise - false.</returns>
        public static bool canShow(int adTypes)
        {
            return getInstance().canShow(adTypes);
        }

        /// <summary>
        /// Checks whether or not advertisement can be shown within <see langword="specified"/> placement.
        /// </summary> 
        /// <remarks>See <see href="https://faq.appodeal.com/en/collections/107523-placements"/> for more details.</remarks>
        /// <param name="adTypes">type of advertisement.</param>
        /// <param name="placement">placement name.</param>
        /// <returns>true if an ad can be shown within specified placement, otherwise - false.</returns>
        public static bool canShow(int adTypes, string placement)
        {
            return getInstance().canShow(adTypes, placement);
        }
        
        /// <summary>
        /// Gets reward parameters for <see langword="default"/> placement.
        /// </summary> 
        /// <remarks>See <see href="https://faq.appodeal.com/en/articles/1133435-reward-setting"/> for more details.</remarks>
        /// <returns>reward currency as key; reward amount as value.</returns>
        public static KeyValuePair<string, double> getRewardParameters()
        {
            return getInstance().getRewardParameters();
        }

        /// <summary>
        /// Gets reward parameters for <see langword="specified"/> placement.
        /// </summary>
        /// <remarks>See <see href="https://faq.appodeal.com/en/articles/1133435-reward-setting"/> for more details.</remarks>
        /// <param name="placement">placement name.</param>
        /// <returns>reward currency as key; reward amount as value.</returns>
        public static KeyValuePair<string, double> getRewardParameters(string placement)
        {
            return getInstance().getRewardParameters(placement);
        }
        
        /// <summary>
        /// Defines whether or not videos should be muted if call volume is set to 0 (Supported only for <see langword="Android"/>).
        /// </summary>
        /// <remarks>It is <see langword="false"/> by default.</remarks>
        /// <param name="value">true - mute videos if calls are muted, false - do not mute videos.</param>
        public static void muteVideosIfCallsMuted(bool value)
        {
            getInstance().muteVideosIfCallsMuted(value);
        }
        
        /// <summary>
        /// Displays test screen (Supported only for <see langword="Android"/>).
        /// </summary>
        /// <remarks>See <see href="https://wiki.appodeal.com/en/unity/get-started/advanced/testing#id-[Development]UnitySDK.Testing-UsefulSDKmethods"/> for more details.</remarks>
        public static void showTestScreen()
        {
            getInstance().showTestScreen();
        }
        
        /// <summary>
        /// <para>
        /// Disables data collection for children's apps (It is <see langword="false"/> by default, unless you marked the app with COPPA flag on the website).
        /// </para>
        /// Call before the SDK initialization.
        /// </summary>
        /// <remarks>See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/coppa"/> for more details.</remarks>
        /// <param name="value">true to disable data collection for kids apps.</param>
        public static void setChildDirectedTreatment(bool value)
        {
            getInstance().setChildDirectedTreatment(value);
        }
        
        /// <summary>
        /// Destroys the cached ad (Supported only for <see langword="Android"/>).
        /// </summary> 
        /// <remarks>See <see href="https://wiki.appodeal.com/en/unity/get-started/ad-types/banner#id-[Development]UnitySDK.Banner-DestroyHiddenBanner"/> for more details.</remarks>
        /// <param name="adTypes">type of advertisement. Currently supported only for AppodealAdType.BANNER and AppodealAdType.MREC</param>
        public static void destroy(int adTypes)
        {
            getInstance().destroy(adTypes);
        }
        
        /// <summary>
        /// Sets extra data to Appodeal.
        /// </summary>
        /// <remarks>See <see href="https://wiki.appodeal.com/en/unity/get-started/advanced/set-user-data#id-[Development]UnitySDK.SetUsersData-Sendextradata"/> for more details.</remarks>
        /// <param name="key">associated with value.</param>
        /// <param name="value">value which will be saved in extra data by key.</param>
        public static void setExtraData(string key, bool value)
        {
            getInstance().setExtraData(key, value);
        }

        /// <summary>
        /// Sets extra data to Appodeal.
        /// </summary>
        /// <remarks>See <see href="https://wiki.appodeal.com/en/unity/get-started/advanced/set-user-data#id-[Development]UnitySDK.SetUsersData-Sendextradata"/> for more details.</remarks>
        /// <param name="key">associated with value.</param>
        /// <param name="value">value which will be saved in extra data by key.</param>
        public static void setExtraData(string key, int value)
        {
            getInstance().setExtraData(key, value);
        }

        /// <summary>
        /// Sets extra data to Appodeal.
        /// </summary>
        /// <remarks>See <see href="https://wiki.appodeal.com/en/unity/get-started/advanced/set-user-data#id-[Development]UnitySDK.SetUsersData-Sendextradata"/> for more details.</remarks>
        /// <param name="key">associated with value.</param>
        /// <param name="value">value which will be saved in extra data by key.</param>
        public static void setExtraData(string key, double value)
        {
            getInstance().setExtraData(key, value);
        }

        /// <summary>
        /// Sets extra data to Appodeal.
        /// </summary> 
        /// <remarks>See <see href="https://wiki.appodeal.com/en/unity/get-started/advanced/set-user-data#id-[Development]UnitySDK.SetUsersData-Sendextradata"/> for more details.</remarks>
        /// <param name="key">associated with value.</param>
        /// <param name="value">value which will be saved in extra data by key.</param>
        public static void setExtraData(string key, string value)
        {
            getInstance().setExtraData(key, value);
        }
        
        /// <summary>
        /// Gets predicted ecpm for certain ad type.
        /// </summary> 
        /// <param name="adType">type of advertisement.</param>
        public static double getPredictedEcpm(int adType)
        {
            return getInstance().getPredictedEcpm(adType);
        }

        /// <summary>
        /// Validates In-App purchase.
        /// </summary> 
        /// <param name="purchase"></param>
        public static void validateInAppPurchase()
        {

        }

    #region Deprecated methods

        /// <summary>
        /// Initializes the relevant (Android or iOS) Appodeal SDK.
        /// See <see cref="Appodeal.initialize"/> for resulting triggered event.
        /// <param name="appKey">Appodeal app key you received when you created an app.</param>
        /// <param name="adTypes">Type of advertisement you want to initialize.</param>
        /// 
        ///  To initialize only interstitials use <see cref="Appodeal.initialize(appKey, Appodeal.INTERSTITIAL);"/> 
        ///  To initialize only banners use <see cref="Appodeal.initialize(appKey, Appodeal.BANNER);"/> 
        ///  To initialize only rewarded video use <see cref="Appodeal.initialize(appKey, Appodeal.REWARDED_VIDEO);"/> 
        ///  To initialize only non-skippable video use <see cref="Appodeal.initialize(appKey, Appodeal.NON_SKIPPABLE_VIDEO);"/> 
        ///  To initialize only 300*250 banners use <see cref="Appodeal.initialize(appKey, Appodeal.MREC);"/> 
        /// </summary>
        // [Obsolete("This method is obsolete. Check the documentation for the new signature.", true)]
        public static void initialize(string appKey, int adTypes)
        {
            getInstance().initialize(appKey, adTypes);
        }

        /// <summary>
        /// Initializes the relevant (Android or iOS) Appodeal SDK.
        /// See <see cref="Appodeal.initialize"/> for resulting triggered event.
        /// <param name="appKey">Appodeal app key you received when you created an app.</param>
        /// <param name="adTypes">Type of advertisement you want to initialize.</param>
        /// <param name="hasConsent">User has given consent to the processing of personal data relating to him or her. https://www.eugdpr.org/.</param>
        /// 
        ///  To initialize only interstitials use <see cref="Appodeal.initialize(appKey, Appodeal.INTERSTITIAL, hasConsent);"/> 
        ///  To initialize only banners use <see cref="Appodeal.initialize(appKey, Appodeal.BANNER, hasConsent);"/> 
        ///  To initialize only rewarded video use <see cref="Appodeal.initialize(appKey, Appodeal.REWARDED_VIDEO, hasConsent);"/> 
        ///  To initialize only non-skippable video use <see cref="Appodeal.initialize(appKey, Appodeal.NON_SKIPPABLE_VIDEO, hasConsent);"/> 
        ///  To initialize only 300*250 banners use <see cref="Appodeal.initialize(appKey, Appodeal.MREC, hasConsent);"/> 
        /// </summary>
        // [Obsolete("This method is obsolete. Check the documentation for the new signature.", true)]
        public static void initialize(string appKey, int adTypes, bool hasConsent)
        {
            getInstance().initialize(appKey, adTypes, hasConsent);
        }

        /// <summary>
        /// Initializes the relevant (Android or iOS) Appodeal SDK.
        /// See <see cref="Appodeal.initialize"/> for resulting triggered event.
        /// <param name="appKey">Appodeal app key you received when you created an app.</param>
        /// <param name="adTypes">Type of advertisement you want to initialize.</param>
        /// <param name="consent">Consent info object from Stack ConsentManager SDK.</param>
        /// 
        ///  To initialize only interstitials use <see cref="Appodeal.initialize(appKey, Appodeal.INTERSTITIAL, consent);"/> 
        ///  To initialize only banners use <see cref="Appodeal.initialize(appKey, Appodeal.BANNER, consent);"/> 
        ///  To initialize only rewarded video use <see cref="Appodeal.initialize(appKey, Appodeal.REWARDED_VIDEO, consent);"/> 
        ///  To initialize only non-skippable video use <see cref="Appodeal.initialize(appKey, Appodeal.NON_SKIPPABLE_VIDEO, consent);"/> 
        ///  To initialize only 300*250 banners use <see cref="Appodeal.initialize(appKey, Appodeal.MREC, consent);"/> 
        /// </summary>
        // [Obsolete("This method is obsolete. Check the documentation for the new signature.", true)]
        public static void initialize(string appKey, int adTypes, Consent consent)
        {
            getInstance().initialize(appKey, adTypes, consent);
        }

        /// <summary>
        /// Update consent value for ad networks in Appodeal SDK
        /// See <see cref="Appodeal.updateConsent"/> for resulting triggered event.
        /// </summary>
        /// <param name="hasConsent">user's consent on processing of their personal data. https://www.eugdpr.org.</param>
        [Obsolete("This method is obsolete. Check the documentation for the new one.", true)]
        public static void updateConsent(bool hasConsent)
        {
            getInstance().updateConsent(hasConsent);
        }

        /// <summary>
        /// Update consent value for ad networks in Appodeal SDK
        /// See <see cref="Appodeal.updateConsent"/> for resulting triggered event.
        /// </summary>
        /// <param name="consent">user's consent on processing of their personal data. https://www.eugdpr.org.</param>
        [Obsolete("This method is obsolete. Check the documentation for the new one.", true)]
        public static void updateConsent(Consent consent)
        {
            getInstance().updateConsent(consent);
        }

        /// <summary>
        /// Enabling shared ads instance across activities (disabled by default).
        /// See <see cref="Appodeal.setSharedAdsInstanceAcrossActivities"/> for resulting triggered event.
        /// <param name="sharedAdsInstanceAcrossActivities">enabling or disabling shared ads instance across activities.</param>
        /// </summary>
        [Obsolete("This method is obsolete.", true)]
        public static void setSharedAdsInstanceAcrossActivities(bool sharedAdsInstanceAcrossActivities)
        {
            getInstance().setSharedAdsInstanceAcrossActivities(sharedAdsInstanceAcrossActivities);
        }

        /// <summary>
        /// Disabling location tracking (for iOS platform only).
        /// See <see cref="Appodeal.disableLocationPermissionCheck"/> for resulting triggered event.
        /// </summary>
        [Obsolete("This method is obsolete. Use setLocationTracking() instead.", true)]
        public static void disableLocationPermissionCheck()
        {
            getInstance().disableLocationPermissionCheck();
        }


        [Obsolete("This method is obsolete. It will be removed in the next release.", true)]
        public static void setBannerBackground(bool enabled) {}

    #endregion

    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class ExtraData
    {
        public const string APPSFLYER_ID = "appsflyer_id";
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class UserSettings
    {
        private static IAppodealAdsClient client;

        private static IAppodealAdsClient getInstance()
        {
            return client ?? (client = AppodealAdsClientFactory.GetAppodealAdsClient());
        }
        
        [Obsolete("This constructor is obsolete.", true)]
        public UserSettings()
        {
            getInstance().getUserSettings();
        }
    }
}
