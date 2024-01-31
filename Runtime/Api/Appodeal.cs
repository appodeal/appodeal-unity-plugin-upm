using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AppodealStack.Monetization.Common;
using AppodealStack.Monetization.Platforms;

// ReSharper disable CheckNamespace
namespace AppodealStack.Monetization.Api
{
    /// <summary>
    /// <para>
    /// Appodeal Unity API for developers, including documentation.
    /// </para>
    /// See <see href="https://docs.appodeal.com/unity/get-started?distribution=upm"/> for more details.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static class Appodeal
    {
        private static IAppodealAdsClient _client;

        private static IAppodealAdsClient GetInstance()
        {
            return _client ??= AppodealAdsClientFactory.GetAppodealAdsClient();
        }

        /// <summary>
        /// <para>
        /// Initializes the relevant (Android or iOS) Appodeal SDK.
        /// See <see langword="OnAppodealInitialized()"/> for resulting triggered event.
        /// </para>
        /// <example>To initialize only interstitials use:<code>Appodeal.Initialize(appKey, AppodealAdType.Interstitial, this);</code></example>
        /// <example>To initialize only banners use:<code>Appodeal.Initialize(appKey, AppodealAdType.Banner, this);</code></example>
        /// <example>To initialize only rewarded video use:<code>Appodeal.Initialize(appKey, AppodealAdType.RewardedVideo, this);</code></example>
        /// <example>To initialize only 300*250 banners use:<code>Appodeal.Initialize(appKey, AppodealAdType.Mrec, this);</code></example>
        /// <example>To initialize multiple ad types use the <see langword="|"/> operator:<code>Appodeal.Initialize(appKey, AppodealAdType.Interstitial | AppodealAdType.Banner, this);</code></example>
        /// </summary>
        /// <remarks>See <see href="https://docs.appodeal.com/unity/get-started?distribution=upm#step-3-initialize-sdk"/> for more details.</remarks>
        /// <param name="appKey">appodeal app key that was assigned to your app when it was created.</param>
        /// <param name="adTypes">type of advertisement you want to initialize.</param>
        /// <param name="listener">class which implements AppodealStack.Mediation.Common.IAppodealInitializeListener interface.</param>
        public static void Initialize(string appKey, int adTypes, IAppodealInitializationListener listener = null)
        {
            GetInstance().Initialize(appKey, adTypes, listener);
        }

        /// <summary>
        /// <para>Checks whether or not ad type is initialized.</para>
        /// <example>To check interstitials use:<code>Appodeal.IsInitialized(AppodealAdType.Interstitial);</code></example>
        /// <example>To check banners use:<code>Appodeal.IsInitialized(AppodealAdType.Banner);</code></example>
        /// <example>To check rewarded video use:<code>Appodeal.IsInitialized(AppodealAdType.RewardedVideo);</code></example>
        /// <example>To check 300*250 banners use:<code>Appodeal.IsInitialized(AppodealAdType.Mrec);</code></example>
        /// </summary>
        /// <param name="adType">type of advertisement.</param>
        /// <returns>True if ad type is initialized, otherwise - false.</returns>
        public static bool IsInitialized(int adType)
        {
            return GetInstance().IsInitialized(adType);
        }

        /// <summary>
        /// <para>Checks whether or not auto cache is enabled for the specified ad type.</para>
        /// <example>To check interstitials use:<code>Appodeal.IsAutoCacheEnabled(AppodealAdType.Interstitial);</code></example>
        /// <example>To check banners use:<code>Appodeal.IsAutoCacheEnabled(AppodealAdType.Banner);</code></example>
        /// <example>To check rewarded video use:<code>Appodeal.IsAutoCacheEnabled(AppodealAdType.RewardedVideo);</code></example>
        /// <example>To check 300*250 banners use:<code>Appodeal.IsAutoCacheEnabled(AppodealAdType.Mrec);</code></example>
        /// </summary>
        /// <param name="adType">type of advertisement.</param>
        /// <returns>True if auto cache is enabled, otherwise - false.</returns>
        public static bool IsAutoCacheEnabled(int adType)
        {
            return GetInstance().IsAutoCacheEnabled(adType);
        }

        /// <summary>
        /// <para>
        /// Sets Interstitial ad type callbacks.
        /// </para>
        /// Read <see href="https://docs.appodeal.com/unity/advanced/main-thread-callbacks?distribution=upm"/> before implementing callbacks.
        /// </summary>
        /// <remarks>See <see href="https://docs.appodeal.com/unity/ad-types/interstitial?distribution=manual#callbacks"/> for more details.</remarks>
        /// <param name="listener">class which implements AppodealStack.Mediation.Common.IInterstitialAdListener interface.</param>
        public static void SetInterstitialCallbacks(IInterstitialAdListener listener)
        {
            GetInstance().SetInterstitialCallbacks(listener);
        }

        /// <summary>
        /// <para>
        /// Sets Rewarded Video ad type callbacks.
        /// </para>
        /// Read <see href="https://docs.appodeal.com/unity/advanced/main-thread-callbacks?distribution=upm"/> before implementing callbacks.
        /// </summary>
        /// <remarks>See <see href="https://docs.appodeal.com/unity/ad-types/rewarded-video?distribution=manual#callbacks"/> for more details.</remarks>
        /// <param name="listener">class which implements AppodealStack.Mediation.Common.IRewardedVideoAdListener interface.</param>
        public static void SetRewardedVideoCallbacks(IRewardedVideoAdListener listener)
        {
            GetInstance().SetRewardedVideoCallbacks(listener);
        }

        /// <summary>
        /// <para>
        /// Sets Banner ad type callbacks.
        /// </para>
        /// Read <see href="https://docs.appodeal.com/unity/advanced/main-thread-callbacks?distribution=upm"/> before implementing callbacks.
        /// </summary>
        /// <remarks>See <see href="https://docs.appodeal.com/unity/ad-types/banner?distribution=manual#callbacks"/> for more details.</remarks>
        /// <param name="listener">class which implements AppodealStack.Mediation.Common.IBannerAdListener interface.</param>
        public static void SetBannerCallbacks(IBannerAdListener listener)
        {
            GetInstance().SetBannerCallbacks(listener);
        }

        /// <summary>
        /// <para>
        /// Sets Mrec ad type callbacks.
        /// </para>
        /// Read <see href="https://docs.appodeal.com/unity/advanced/main-thread-callbacks?distribution=upm"/> before implementing callbacks.
        /// </summary>
        /// <remarks>See <see href="https://docs.appodeal.com/unity/ad-types/mrec?distribution=manual#callbacks"/> for more details.</remarks>
        /// <param name="listener">class which implements AppodealStack.Mediation.Common.IMrecAdListener interface.</param>
        public static void SetMrecCallbacks(IMrecAdListener listener)
        {
            GetInstance().SetMrecCallbacks(listener);
        }

        /// <summary>
        /// <para>
        /// Sets Ad Revenue callback.
        /// </para>
        /// Read <see href="https://docs.appodeal.com/unity/advanced/main-thread-callbacks?distribution=upm"/> before implementing callbacks.
        /// </summary>
        /// <remarks>See <see href="https://docs.appodeal.com/unity/advanced/ad-revenue-callback?distribution=manual#callback-implementation"/> for more details.</remarks>
        /// <param name="listener">class which implements AppodealStack.Mediation.Common.IAdRevenueListener interface.</param>
        public static void SetAdRevenueCallback(IAdRevenueListener listener)
        {
            GetInstance().SetAdRevenueCallback(listener);
        }

        /// <summary>
        /// <para>
        /// Caches ads in a manual mode.
        /// Use it only if <see cref="IsAutoCacheEnabled"/> is set to false.
        /// </para>
        /// <example>To cache interstitials use:<code>Appodeal.Cache(AppodealAdType.Interstitial);</code></example>
        /// <example>To cache banners use:<code>Appodeal.Cache(AppodealAdType.Banner);</code></example>
        /// <example>To cache rewarded video use:<code>Appodeal.Cache(AppodealAdType.RewardedVideo);</code></example>
        /// <example>To cache 300*250 banners use:<code>Appodeal.Cache(AppodealAdType.Mrec);</code></example>
        /// Note, manual caching of Mrec ads via <see langword="Appodeal.Cache()"/> method works only for  <see langword="Android"/> platform.
        /// </summary>
        /// <remarks>See <see href="https://faq.appodeal.com/en/articles/2658522-sdk-caching"/> for more details.</remarks>
        /// <param name="adType">type of advertisement.</param>
        public static void Cache(int adType)
        {
            GetInstance().Cache(adType);
        }

        /// <summary>
        /// <para>
        /// Shows an advertisement.
        /// You can use <see cref="IsLoaded"/> method first, to check if an ad is currently available.
        /// </para>
        /// <example>To show interstitials use:<code>Appodeal.Show(AppodealShowStyle.Interstitial);</code></example>
        /// <example>To show horizontal banner at top use:<code>Appodeal.Show(AppodealShowStyle.BannerTop);</code></example>
        /// <example>To show horizontal banner at bottom use:<code>Appodeal.Show(AppodealShowStyle.BannerBottom);</code></example>
        /// <example>To show vertical banner at left use:<code>Appodeal.Show(AppodealShowStyle.BannerLeft);</code></example>
        /// <example>To show vertical banner at right use:<code>Appodeal.Show(AppodealShowStyle.BannerRight);</code></example>
        /// <example>To show rewarded video use:<code>Appodeal.Show(AppodealShowStyle.RewardedVideo);</code></example>
        /// </summary>
        ///<remarks>To show banner at custom position and Mrec ads use <see langword="Appodeal.ShowBannerView()"/> or <see langword="Appodeal.ShowMrecView()"/> methods instead.</remarks>
        /// <param name="showStyle">show style of advertisement.</param>
        /// <returns>True if an ad was shown, otherwise - false.</returns>
        public static bool Show(int showStyle)
        {
            return GetInstance().Show(showStyle);
        }

        /// <summary>
        /// <para>
        /// Shows an advertisement in a specified placement.
        /// See <see href="https://faq.appodeal.com/en/articles/1154394-placements"/> for more details.
        /// </para>
        /// <example>To show interstitials use:<code>Appodeal.Show(AppodealShowStyle.Interstitial, placementName);</code></example>
        /// <example>To show horizontal banner at top use:<code>Appodeal.Show(AppodealShowStyle.BannerTop, placementName);</code></example>
        /// <example>To show horizontal banner at bottom use:<code>Appodeal.Show(AppodealShowStyle.BannerBottom, placementName);</code></example>
        /// <example>To show vertical banner at left use:<code>Appodeal.Show(AppodealShowStyle.BannerLeft, placementName);</code></example>
        /// <example>To show vertical banner at right use:<code>Appodeal.Show(AppodealShowStyle.BannerRight, placementName);</code></example>
        /// <example>To show rewarded video use:<code>Appodeal.Show(AppodealShowStyle.RewardedVideo, placementName);</code></example>
        /// </summary>
        ///<remarks>To show banner at custom position and Mrec ads use <see langword="Appodeal.ShowBannerView()"/> or <see langword="Appodeal.ShowMrecView()"/> methods instead.</remarks>
        /// <param name="showStyle">show style of advertisement.</param>
        /// <param name="placement">name of placement.</param>
        /// <returns>True if an ad was shown, otherwise - false.</returns>
        public static bool Show(int showStyle, string placement)
        {
            return GetInstance().Show(showStyle, placement);
        }

        /// <summary>
        /// <para>
        /// Shows banner at custom position on the screen.
        /// You can either use predefined <see langword="AppodealViewPosition"/> constants or set any desired offset.
        /// </para>
        /// <example>To show banner at top left you can use:<code>Appodeal.ShowBannerView(AppodealViewPosition.VerticalTop, AppodealViewPosition.HorizontalLeft, placementName);</code></example>
        /// <example>To show banner at bottom center you can use:<code>Appodeal.ShowBannerView(AppodealViewPosition.VerticalBottom, AppodealViewPosition.HorizontalCenter, placementName);</code></example>
        /// </summary>
        /// <remarks>If banner view is out of screen because of the offset you specified, advertisement will not be shown.</remarks>
        /// <param name="yAxis">y axis offset for banner view.</param>
        /// <param name="xGravity">x axis offset for banner view.</param>
        /// <param name="placement">name of placement.</param>
        /// <returns>True if an ad was shown, otherwise - false.</returns>
        public static bool ShowBannerView(int yAxis, int xGravity, string placement)
        {
            return GetInstance().ShowBannerView(yAxis, xGravity, placement);
        }

        /// <summary>
        /// <para>
        /// Shows Mrec at custom position on the screen.
        /// You can either use predefined <see langword="AppodealViewPosition"/> constants or set any desired offset.
        /// </para>
        /// <example>To show Mrec at top left you can use:<code>Appodeal.ShowMrecView(AppodealViewPosition.VerticalTop, AppodealViewPosition.HorizontalLeft placementName);</code></example>
        /// <example>To show Mrec at bottom center you can use:<code>Appodeal.ShowMrecView(AppodealViewPosition.VerticalBottom, AppodealViewPosition.HorizontalCenter, placementName);</code></example>
        /// </summary>
        /// <remarks>If Mrec view is out of screen because of the offset you specified, advertisement will not be shown.</remarks>
        /// <param name="yAxis">y axis offset for Mrec view.</param>
        /// <param name="xGravity">x axis offset for Mrec view.</param>
        /// <param name="placement">name of placement.</param>
        /// <returns>True if an ad was shown, otherwise - false.</returns>
        public static bool ShowMrecView(int yAxis, int xGravity, string placement)
        {
            return GetInstance().ShowMrecView(yAxis, xGravity, placement);
        }

        /// <summary>
        /// <para>
        /// Hides active banner from screen.
        /// </para>
        /// <example>Usage example:<code>Appodeal.Hide(AppodealAdType.Banner);</code></example>
        /// </summary>
        /// <remarks>It can only be applied to banners shown via <see cref="Show(int, string)"/> method.</remarks>
        /// <param name="adType">type of advertisement.</param>
        public static void Hide(int adType)
        {
            GetInstance().Hide(adType);
        }

        /// <summary>Hides active banner view from screen.</summary>
        /// <remarks>It can only be applied to banners shown via <see cref="ShowBannerView"/> method.</remarks>
        public static void HideBannerView()
        {
            GetInstance().HideBannerView();
        }

        /// <summary>Hides active Mrec view from screen</summary>
        /// <remarks>It can only be applied to banners shown via <see cref="ShowMrecView"/> method.</remarks>
        public static void HideMrecView()
        {
            GetInstance().HideMrecView();
        }

        /// <summary>
        /// <para>
        /// Defines whether or not auto cache is enabled for specified ad types (It is <see langword="true"/> for all ad types by default).
        /// </para>
        /// <para>
        /// Call before the SDK initialization.
        /// </para>
        /// You can also use <see cref="IsAutoCacheEnabled"/> to check if auto cache is enabled for specified ad type.
        /// </summary>
        /// <remarks>See <see href="https://faq.appodeal.com/en/articles/2658522-sdk-caching"/> for more details.</remarks>
        /// <param name="adTypes">types of advertisement.</param>
        /// <param name="autoCache">true to enable auto cache, false to disable.</param>
        public static void SetAutoCache(int adTypes, bool autoCache)
        {
            GetInstance().SetAutoCache(adTypes, autoCache);
        }

        /// <summary>
        /// <para>
        /// Defines whether or not <see langword="On[AdType]Loaded"/> callback should be fired if precache is loaded.
        /// </para>
        /// Call before the SDK initialization.
        /// </summary>
        /// <remarks>See <see href="https://faq.appodeal.com/en/articles/94830-ad-loading-time-and-precache"/> for more details.</remarks>
        /// <param name="adTypes">types of advertisement. Currently supported for interstitial, rewarded video, banner and Mrec ad types.</param>
        /// <param name="onLoadedTriggerBoth">true - onLoaded will be triggered when precache or regular ad are loaded. false - onLoaded will trigger only when regular ad is loaded (default).</param>
        public static void SetTriggerOnLoadedOnPrecache(int adTypes, bool onLoadedTriggerBoth)
        {
            GetInstance().SetTriggerOnLoadedOnPrecache(adTypes, onLoadedTriggerBoth);
        }

        /// <summary>
        /// <para>
        /// Checks whether or not an ad of a specified ad type is loaded.
        /// </para>
        /// <example>To check if interstitial is loaded use:<code>Appodeal.IsLoaded(AppodealAdType.Interstitial);</code></example>
        /// <example>To check if rewarded video is loaded use:<code>Appodeal.IsLoaded(AppodealAdType.RewardedVideo);</code></example>
        /// <example>To check if banner is loaded use:<code>Appodeal.IsLoaded(AppodealAdType.Banner);</code></example>
        /// <example>To check if 300*250 banner is loaded use:<code>Appodeal.IsLoaded(AppodealAdType.Mrec);</code></example>
        /// </summary>
        /// <param name="adType">type of advertisement.</param>
        /// <returns>True if advertisement is loaded, otherwise - false.</returns>
        public static bool IsLoaded(int adType)
        {
            return GetInstance().IsLoaded(adType);
        }

        /// <summary>
        /// <para>
        /// Checks whether or not currently loaded ad is precache.
        /// </para>
        /// <example>To check if interstitial is precache use:<code>Appodeal.IsPrecache(AppodealAdType.Interstitial);</code></example>
        /// </summary>
        /// <param name="adType">type of advertisement. Currently supported only for AppodealAdType.Interstitial.</param>
        /// <returns>True if advertisement is loaded and is precache, otherwise - false.</returns>
        public static bool IsPrecache(int adType)
        {
            return GetInstance().IsPrecache(adType);
        }

        /// <summary>
        /// <para>
        /// Defines whether or not smart banners should be used (It is <see langword="true"/> by default).
        /// </para>
        /// Call before the SDK initialization.
        /// </summary>
        /// <remarks>See <see href="https://faq.appodeal.com/en/articles/2684869-banner-sizes"/> for more details.</remarks>
        /// <param name="enabled">true to enable smart banners, false to disable.</param>
        public static void SetSmartBanners(bool enabled)
        {
            GetInstance().SetSmartBanners(enabled);
        }

        /// <summary>
        /// <para>
        /// Checks whether or not smart banners feature is enabled. (It is <see langword="true"/> by default).
        /// </para>
        /// It is usually used along with the <see cref="SetSmartBanners"/> method.
        /// </summary>
        /// <remarks>See <see href="https://faq.appodeal.com/en/articles/2684869-banner-sizes"/> for more details.</remarks>
        /// <returns>True if smart banners are enabled, otherwise - false.</returns>
        public static bool IsSmartBannersEnabled()
        {
            return GetInstance().IsSmartBannersEnabled();
        }

        /// <summary>
        /// <para>
        /// Defines whether or not 728*90 banners should be used (It is <see langword="false"/> by default).
        /// </para>
        /// Call before the SDK initialization.
        /// </summary>
        /// <remarks>See <see href="https://faq.appodeal.com/en/articles/2684869-banner-sizes"/> for more details.</remarks>
        /// <param name="enabled">true to enable tablet banners, false to disable.</param>
        public static void SetTabletBanners(bool enabled)
        {
            GetInstance().SetTabletBanners(enabled);
        }

        /// <summary>
        /// <para>
        /// Defines whether or not banner animation should be used (It is <see langword="true"/> by default).
        /// </para>
        /// Call before the SDK initialization.
        /// </summary>
        /// <param name="enabled">true to enable banner animation, false to disable.</param>
        public static void SetBannerAnimation(bool enabled)
        {
            GetInstance().SetBannerAnimation(enabled);
        }

        /// <summary>
        /// Sets rotation for AppodealShowStyle.BannerLeft and AppodealShowStyle.BannerRight types (by default: left = -90, right = 90).
        /// </summary>
        /// <param name="leftBannerRotation">rotation for AppodealShowStyle.BannerLeft.</param>
        /// <param name="rightBannerRotation">rotation for AppodealShowStyle.BannerRight.</param>
        public static void SetBannerRotation(int leftBannerRotation, int rightBannerRotation)
        {
            GetInstance().SetBannerRotation(leftBannerRotation, rightBannerRotation);
        }

        /// <summary>
        /// Defines whether or not safe area of the screen can be used. (Supported only for <see langword="Android"/> platform)
        /// </summary>
        /// <param name="value">true to enable usage of safe area, false to disable.</param>
        public static void SetUseSafeArea(bool value)
        {
            GetInstance().SetUseSafeArea(value);
        }

        /// <summary>
        /// Tracks in-app purchase information and sends info to our servers. It can be then used to segment users.
        /// </summary>
        /// <remarks>See <see href="https://docs.appodeal.com/unity/advanced/segments-placements?distribution=upm#bought-in-apps"/> for more details.</remarks>
        /// <param name="amount">amount of purchase.</param>
        /// <param name="currency">currency of purchase.</param>
        public static void TrackInAppPurchase(double amount, string currency)
        {
            GetInstance().TrackInAppPurchase(amount, currency);
        }

        /// <summary>
        /// <para>
        /// Gets a list of available ad networks for certain ad type.
        /// </para>
        /// <example>Usage example:<code>Appodeal.GetNetworks(AppodealAdType.Interstitial);</code></example>
        /// </summary>
        /// <param name="adType">type of advertisement.</param>
        /// <returns>List of available ad networks for the specified ad type.</returns>
        public static List<string> GetNetworks(int adType)
        {
            return GetInstance().GetNetworks(adType);
        }

        /// <summary>
        /// <para>
        /// Disables specified ad network for all ad types.
        /// </para>
        /// <example>To disable Facebook use:<code>Appodeal.DisableNetwork(AppodealNetworks.Facebook);</code></example>
        /// </summary>
        /// <remarks>We recommend using <see langword="AppodealNetworks"/> class to access networks' names.</remarks>
        /// <param name="network">network name.</param>
        public static void DisableNetwork(string network)
        {
            GetInstance().DisableNetwork(network);
        }

        /// <summary>
        /// <para>
        /// Disables specified ad network for specified ad types.
        /// </para>
        /// <example>To disable Facebook for banners only use:<code>Appodeal.DisableNetwork(AppodealNetworks.Facebook, AppodealAdType.Banner);</code></example>
        /// </summary>
        /// <remarks>We recommend using <see langword="AppodealNetworks"/> class to access networks' names.</remarks>
        /// <param name="network">network name.</param>
        /// <param name="adType">types of advertisement.</param>
        public static void DisableNetwork(string network, int adType)
        {
            GetInstance().DisableNetwork(network, adType);
        }

        /// <summary>
        /// Defines whether or not location tracking is allowed. (Supported only for <see langword="iOS"/> platform)
        /// </summary>
        /// <remarks>On android location tracking is always enabled if the corresponding permission was given.</remarks>
        /// <param name="value">true to enable location tracking, false to disable.</param>
        public static void SetLocationTracking(bool value)
        {
            GetInstance().SetLocationTracking(value);
        }

        /// <summary>Sets user id.</summary>
        /// <remarks>See <see href="https://docs.appodeal.com/unity/advanced/user-data?distribution=upm#set-user-id"/> for more details.</remarks>
        /// <param name="id">user id.</param>
        public static void SetUserId(string id)
        {
            GetInstance().SetUserId(id);
        }

        /// <summary>Gets user id.</summary>
        /// <remarks>See <see href="https://docs.appodeal.com/unity/advanced/user-data?distribution=upm#set-user-id"/> for more details.</remarks>
        /// <returns>User id as string.</returns>
        public static string GetUserId()
        {
            return GetInstance().GetUserId();
        }

        /// <summary>Gets native SDK version.</summary>
        /// <returns>Appodeal (Android or iOS) SDK version string.</returns>
        public static string GetNativeSDKVersion()
        {
            return GetInstance().GetVersion();
        }

        /// <summary>Gets active segment id.</summary>
        /// <remarks>See <see href="https://faq.appodeal.com/en/collections/107529-segments"/> for more details.</remarks>
        /// <returns>Segment id as long.</returns>
        public static long GetSegmentId()
        {
            return GetInstance().GetSegmentId();
        }

        /// <summary>
        /// <para>
        /// Defines whether or not test mode should be enabled.
        /// </para>
        /// Call before the SDK initialization.
        /// </summary>
        /// <remarks>See <see href="https://docs.appodeal.com/unity/advanced/testing?distribution=upm#enable-test-mode"/> for more details.</remarks>
        /// <param name="test">true if test mode is enabled, otherwise - false.</param>
        public static void SetTesting(bool test)
        {
            GetInstance().SetTesting(test);
        }

        /// <summary>
        /// <para>
        /// Defines the log level of Appodeal SDK.
        /// </para>
        /// Use <see langword="AppodealLogLevel"/> enum to access possible values.
        /// </summary>
        /// <remarks>All logs will be written with tag "Appodeal".</remarks>
        /// <param name="log">log-level state of AppodealLogLevel type .</param>
        public static void SetLogLevel(AppodealLogLevel log)
        {
            GetInstance().SetLogLevel(log);
        }

        /// <summary>
        /// Adds a key-value pair to Custom Segment Filters.
        /// </summary>
        /// <remarks>See <see href="https://faq.appodeal.com/en/articles/1133533-segment-filters"/> for more details.</remarks>
        /// <param name="name">name of the filter.</param>
        /// <param name="value">filter value.</param>
        public static void SetCustomFilter(string name, bool value)
        {
            GetInstance().SetCustomFilter(name, value);
        }

        /// <summary>
        /// Adds a key-value pair to Custom Segment Filters.
        /// </summary>
        /// <remarks>See <see href="https://faq.appodeal.com/en/articles/1133533-segment-filters"/> for more details.</remarks>
        /// <param name="name">name of the filter.</param>
        /// <param name="value">filter value.</param>
        public static void SetCustomFilter(string name, int value)
        {
            GetInstance().SetCustomFilter(name, value);
        }

        /// <summary>
        /// Adds a key-value pair to Custom Segment Filters.
        /// </summary>
        /// <remarks>See <see href="https://faq.appodeal.com/en/articles/1133533-segment-filters"/> for more details.</remarks>
        /// <param name="name">name of the filter.</param>
        /// <param name="value">filter value.</param>
        public static void SetCustomFilter(string name, double value)
        {
            GetInstance().SetCustomFilter(name, value);
        }

        /// <summary>
        /// Adds a key-value pair to Custom Segment Filters.
        /// </summary>
        /// <remarks>See <see href="https://faq.appodeal.com/en/articles/1133533-segment-filters"/> for more details.</remarks>
        /// <param name="name">name of the filter.</param>
        /// <param name="value">filter value.</param>
        public static void SetCustomFilter(string name, string value)
        {
            GetInstance().SetCustomFilter(name, value);
        }

        /// <summary>
        /// <para>
        /// Resets custom filter value by the provided key.
        /// </para>
        /// See <see href="https://faq.appodeal.com/en/articles/1133533-segment-filters"/> for more details.
        /// </summary>
        /// <remarks>Use it to remove a filter, that was previously set via one of the <see langword="SetCustomFilter()"/> methods.</remarks>
        /// <param name="name">name of the filter.</param>
        public static void ResetCustomFilter(string name)
        {
            GetInstance().ResetCustomFilter(name);
        }

        /// <summary>
        /// Checks whether or not advertisement can be shown within <see langword="default"/> placement.
        /// </summary>
        /// <remarks>See <see href="https://faq.appodeal.com/en/collections/107523-placements"/> for more details.</remarks>
        /// <param name="adType">type of advertisement.</param>
        /// <returns>True if an ad can be shown within default placement, otherwise - false.</returns>
        public static bool CanShow(int adType)
        {
            return GetInstance().CanShow(adType);
        }

        /// <summary>
        /// Checks whether or not advertisement can be shown within <see langword="specified"/> placement.
        /// </summary>
        /// <remarks>See <see href="https://faq.appodeal.com/en/collections/107523-placements"/> for more details.</remarks>
        /// <param name="adType">type of advertisement.</param>
        /// <param name="placement">placement name.</param>
        /// <returns>True if an ad can be shown within specified placement, otherwise - false.</returns>
        public static bool CanShow(int adType, string placement)
        {
            return GetInstance().CanShow(adType, placement);
        }

        /// <summary>
        /// <para>
        /// Gets reward data for <see langword="specified"/> placement.
        /// </para>
        /// If placement name is not specified, default one will be used.
        /// </summary>
        /// <remarks>See <see href="https://faq.appodeal.com/en/articles/1133435-reward-setting"/> for more details.</remarks>
        /// <param name="placement">name of the placement as displayed in dashboard.</param>
        /// <returns>Object of type <see cref="AppodealReward"/>.</returns>
        public static AppodealReward GetReward(string placement = null)
        {
            return GetInstance().GetReward(placement);
        }

        /// <summary>
        /// Defines whether or not videos should be muted if call volume is set to 0. (Supported only for <see langword="Android"/> platform)
        /// </summary>
        /// <remarks>It is <see langword="false"/> by default.</remarks>
        /// <param name="value">true - mute videos if calls are muted, false - do not mute videos.</param>
        public static void MuteVideosIfCallsMuted(bool value)
        {
            GetInstance().MuteVideosIfCallsMuted(value);
        }

        /// <summary>
        /// Displays test screen. (Supported only for <see langword="Android"/> platform)
        /// </summary>
        /// <remarks>See <see href="https://docs.appodeal.com/unity/advanced/testing?distribution=upm#test-adapters-integration"/> for more details.</remarks>
        public static void ShowTestScreen()
        {
            GetInstance().ShowTestScreen();
        }

        /// <summary>
        /// <para>
        /// Disables data collection for children's apps (It is <see langword="false"/> by default, unless you marked the app with COPPA flag on the website).
        /// </para>
        /// Call before the SDK initialization.
        /// </summary>
        /// <remarks>See <see href="https://docs.appodeal.com/unity/data-protection/coppa?distribution=upm"/> for more details.</remarks>
        /// <param name="value">true to disable data collection for kids apps.</param>
        public static void SetChildDirectedTreatment(bool value)
        {
            GetInstance().SetChildDirectedTreatment(value);
        }

        /// <summary>
        /// Destroys the cached ad. (Supported only for <see langword="Android"/> platform)
        /// </summary>
        /// <remarks>See <see href="https://docs.appodeal.com/unity/ad-types/banner?distribution=upm#destroy-hidden-banner"/> for more details.</remarks>
        /// <param name="adType">type of advertisement. Currently supported only for AppodealAdType.Banner and AppodealAdType.Mrec</param>
        public static void Destroy(int adType)
        {
            GetInstance().Destroy(adType);
        }

        /// <summary>
        /// Adds a key-value pair to Appodeal Extra Data.
        /// </summary>
        /// <remarks>See <see href="https://docs.appodeal.com/unity/advanced/user-data?distribution=upm#send-extra-data"/> for more details.</remarks>
        /// <param name="key">unique identifier.</param>
        /// <param name="value">variable that will be added to Extra Data by key.</param>
        public static void SetExtraData(string key, bool value)
        {
            GetInstance().SetExtraData(key, value);
        }

        /// <summary>
        /// Adds a key-value pair to Appodeal Extra Data.
        /// </summary>
        /// <remarks>See <see href="https://docs.appodeal.com/unity/advanced/user-data?distribution=upm#send-extra-data"/> for more details.</remarks>
        /// <param name="key">unique identifier.</param>
        /// <param name="value">variable that will be added to Extra Data by key.</param>
        public static void SetExtraData(string key, int value)
        {
            GetInstance().SetExtraData(key, value);
        }

        /// <summary>
        /// Adds a key-value pair to Appodeal Extra Data.
        /// </summary>
        /// <remarks>See <see href="https://docs.appodeal.com/unity/advanced/user-data?distribution=upm#send-extra-data"/> for more details.</remarks>
        /// <param name="key">unique identifier.</param>
        /// <param name="value">variable that will be added to Extra Data by key.</param>
        public static void SetExtraData(string key, double value)
        {
            GetInstance().SetExtraData(key, value);
        }

        /// <summary>
        /// Adds a key-value pair to Appodeal Extra Data.
        /// </summary>
        /// <remarks>See <see href="https://docs.appodeal.com/unity/advanced/user-data?distribution=upm#send-extra-data"/> for more details.</remarks>
        /// <param name="key">unique identifier.</param>
        /// <param name="value">variable that will be added to Extra Data by key.</param>
        public static void SetExtraData(string key, string value)
        {
            GetInstance().SetExtraData(key, value);
        }

        /// <summary>
        /// <para>
        /// Resets extra data value by the provided key.
        /// </para>
        /// See <see href="https://docs.appodeal.com/unity/advanced/user-data?distribution=upm#send-extra-data"/> for more details.
        /// </summary>
        /// <remarks>Use it to remove an extra data, that was previously set via one of the <see langword="SetExtraData()"/> methods.</remarks>
        /// <param name="key">unique identifier.</param>
        public static void ResetExtraData(string key)
        {
            GetInstance().ResetExtraData(key);
        }

        /// <summary>
        /// Gets predicted eCPM for certain ad type.
        /// </summary>
        /// <param name="adType">type of advertisement.</param>
        public static double GetPredictedEcpm(int adType)
        {
            return GetInstance().GetPredictedEcpm(adType);
        }

        /// <summary>
        /// Gets predicted eCPM for certain ad type and placement.
        /// </summary>
        /// <param name="adType">type of advertisement.</param>
        /// <param name="placement">name of Appodeal placement from dashboard.</param>
        public static double GetPredictedEcpmForPlacement(int adType, string placement)
        {
            return GetInstance().GetPredictedEcpmForPlacement(adType, placement);
        }

        /// <summary>
        /// <para>Sends event data to all connected analytic services such as Firebase, Adjust, AppsFlyer and Facebook.</para>
        /// See <see href="https://docs.appodeal.com/unity/advanced/event-tracking?distribution=upm#step-1-how-to-track-in-app-events"/> for more details.
        /// </summary>
        /// <remarks>
        /// <para>Event parameter values must be one of the following types:  <see langword="string"/>, <see langword="double"/>, or <see langword="int"/></para>
        /// If event has no params, call the shorten version of this method by passing only name of the event.
        /// </remarks>
        /// <param name="eventName">name of the event.</param>
        /// <param name="eventParams">parameters of the event if any.</param>
        public static void LogEvent(string eventName, Dictionary<string, object> eventParams = null)
        {
            GetInstance().LogEvent(eventName, eventParams);
        }

        /// <summary>
        /// <para>
        /// Validates In-App purchase. (Supported only for <see langword="Android"/> platform)
        /// </para>
        /// See <see href="https://docs.appodeal.com/unity/advanced/in-app-purchases?distribution=upm"/> for more details.
        /// </summary>
        /// <remarks>If the purchase is valid, this method will also call <see cref="TrackInAppPurchase"/> method under the hood.</remarks>
        /// <param name="purchase">object of type PlayStoreInAppPurchase, containing all data about the purchase.</param>
        /// <param name="listener">class which implements AppodealStack.Mediation.Common.IInAppPurchaseValidationListener interface.</param>
        public static void ValidatePlayStoreInAppPurchase(IPlayStoreInAppPurchase purchase, IInAppPurchaseValidationListener listener = null)
        {
            GetInstance().ValidatePlayStoreInAppPurchase(purchase, listener);
        }

        /// <summary>
        /// <para>
        /// Validates In-App purchase. (Supported only for <see langword="iOS"/> platform)
        /// </para>
        /// See <see href="https://docs.appodeal.com/unity/advanced/in-app-purchases?distribution=upm"/> for more details.
        /// </summary>
        /// <remarks>If the purchase is valid, this method will also call <see cref="TrackInAppPurchase"/> method under the hood.</remarks>
        /// <param name="purchase">object of type AppStoreInAppPurchase, containing all data about the purchase.</param>
        /// <param name="listener">class which implements AppodealStack.Mediation.Common.IInAppPurchaseValidationListener interface.</param>
        public static void ValidateAppStoreInAppPurchase(IAppStoreInAppPurchase purchase, IInAppPurchaseValidationListener listener = null)
        {
            GetInstance().ValidateAppStoreInAppPurchase(purchase, listener);
        }

    #region Deprecated methods

        [Obsolete("Will be changed in a future release.", false)]
        public static void setSharedAdsInstanceAcrossActivities(bool sharedAdsInstanceAcrossActivities)
        {
            GetInstance().setSharedAdsInstanceAcrossActivities(sharedAdsInstanceAcrossActivities);
        }

    #endregion

    }
}
