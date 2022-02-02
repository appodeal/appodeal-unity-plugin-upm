using System.Diagnostics.CodeAnalysis;

namespace AppodealAds.Unity.Common
{
    /// <summary>
    /// <para>
    /// Static class containing all currently supported ad types.
    /// </para>
    /// See <see href="https://wiki.appodeal.com/en/unity/get-started"/> for more details.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    public static class AppodealAdType {
        /// <summary>Initializes only Appodeal SDK, but not the ad types.</summary>
        public const int NONE = 0;

        /// <summary>
        /// <para>
        /// Interstitial ads are full-screen advertisements.
        /// </para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/ad-types/interstitial"/> for more details.
        /// </summary>
        public const int INTERSTITIAL = 3;

        /// <summary>
        /// <para>
        /// Banner ads are rectangular graphic, usually located either at the top or bottom of the screen. 
        /// </para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/ad-types/banner"/> for more details.
        /// </summary>
        public const int BANNER = 4;

        /// <summary>
        /// <para>
        /// BANNER_BOTTOM is a horizontal banner located at the bottom of the screen.
        /// </para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/ad-types/banner"/> for more details.
        /// </summary>
        /// <remarks>Use <see langword="AppodealAdType.BANNER"/> to initialize banners.</remarks>
        public const int BANNER_BOTTOM = 8;

        /// <summary>
        /// <para>
        /// BANNER_TOP is a horizontal banner located at the top of the screen.
        /// </para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/ad-types/banner"/> for more details.
        /// </summary>
        /// <remarks>Use <see langword="AppodealAdType.BANNER"/> to initialize banners.</remarks>
        public const int BANNER_TOP = 16;

        /// <summary>
        /// <para>
        /// BANNER_LEFT is a vertical banner located at the left side of the screen.
        /// </para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/ad-types/banner"/> for more details.
        /// </summary>
        /// <remarks>Use <see langword="AppodealAdType.BANNER"/> to initialize banners.</remarks>
        public const int BANNER_LEFT = 1024;

        /// <summary>
        /// <para>
        /// BANNER_RIGHT is a vertical banner located at the right side of the screen.
        /// </para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/ad-types/banner"/> for more details.
        /// </summary>
        /// <remarks>Use <see langword="AppodealAdType.BANNER"/> to initialize banners.</remarks>
        public const int BANNER_RIGHT = 2048;

        /// <summary>
        /// <para>
        /// BANNER_VIEW is a horizontal banner. You can controll its position by setting the desired offset.
        /// </para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/ad-types/banner"/> for more details.
        /// <para>
        /// Use <see langword="Appodeal.showBannerView()"/> to display BANNER_VIEW.
        /// </para>
        /// </summary>
        /// <remarks>Use <see langword="AppodealAdType.BANNER"/> to initialize banners.</remarks>
        public const int BANNER_VIEW = 64;

        /// <summary>
        /// <para>
        /// Rewarded video is a full-screen user-initiated ad type. It allows end-users to get in-app rewards or other benefits in exchange for viewing a video ad.
        /// </para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/ad-types/rewarded-video"/> for more details.
        /// </summary>
        public const int REWARDED_VIDEO = 128;

#if UNITY_ANDROID || UNITY_EDITOR

        /// <summary>
        /// <para>
        /// Non-Skippable video is simply the same as <see langword="AppodealAdType.REWARDED_VIDEO"/> but with no reward callback.
        /// </para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/ad-types/rewarded-video"/> for more details.
        /// </summary>
        /// <remarks>It cannot be used along with <see langword="AppodealAdType.REWARDED_VIDEO"/> ad type.</remarks>
        public const int NON_SKIPPABLE_VIDEO = 128;

#elif UNITY_IPHONE

        /// <summary>
        /// <para>
        /// Non-Skippable video is simply the same as <see langword="AppodealAdType.REWARDED_VIDEO"/> but with no reward callback.
        /// </para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/ad-types/rewarded-video"/> for more details.
        /// </summary>
        /// <remarks>It cannot be used along with <see langword="AppodealAdType.REWARDED_VIDEO"/> ad type.</remarks>
		public const int NON_SKIPPABLE_VIDEO = 256;

#endif

        /// <summary>
        /// <para>
        /// MREC is 300x250 banner. This type can be useful if the application has a large free area for placing a banner in the interface.
        /// </para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/ad-types/mrec"/> for more details.
        /// </summary>
        public const int MREC = 512;
    }
}
