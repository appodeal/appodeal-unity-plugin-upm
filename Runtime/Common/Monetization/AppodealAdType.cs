// ReSharper Disable CheckNamespace

namespace AppodealStack.Monetization.Common
{
    /// <summary>
    /// <para>
    /// Static class containing all currently supported ad types.
    /// </para>
    /// See <see href="https://wiki.appodeal.com/en/unity/get-started"/> for more details.
    /// </summary>
    public static class AppodealAdType
    {
        /// <summary>Initializes only Appodeal SDK, but not the ad types.</summary>
        public const int None = 0;

        /// <summary>
        /// <para>
        /// Interstitial ads are full-screen advertisements.
        /// </para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/ad-types/interstitial"/> for more details.
        /// </summary>
        public const int Interstitial = 1;

        /// <summary>
        /// <para>
        /// Banner ads are rectangular graphic, usually located either at the top or bottom of the screen. 
        /// </para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/ad-types/banner"/> for more details.
        /// </summary>
        public const int Banner = 2;

        /// <summary>
        /// <para>
        /// Rewarded video is a full-screen user-initiated ad type. It allows end-users to get in-app rewards or other benefits in exchange for viewing a video ad.
        /// </para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/ad-types/rewarded-video"/> for more details.
        /// </summary>
        public const int RewardedVideo = 4;

        /// <summary>
        /// <para>
        /// MREC is 300x250 banner. This type can be useful if the application has a large free area for placing a banner in the interface.
        /// </para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/ad-types/mrec"/> for more details.
        /// </summary>
        public const int Mrec = 8;
    }
}