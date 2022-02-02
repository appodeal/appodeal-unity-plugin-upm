using System.Diagnostics.CodeAnalysis;

namespace AppodealAds.Unity.Common
{
    /// <summary>
    /// <para>
    /// Interface containing signatures of Appodeal Banner callback methods.
    /// </para>
    /// See <see href="https://wiki.appodeal.com/en/unity/get-started/ad-types/banner#id-[Development]UnitySDK.Banner-BannerCallbacks"/> for more details.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "UnusedParameter.Global")]
    public interface IBannerAdListener
    {
        /// <summary>
        /// <para>
        /// Fires when Banner is loaded.
        /// </para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/ad-types/banner#id-[Development]UnitySDK.Banner-BannerCallbacks"/> for more details.
        /// </summary>
        /// <param name="height">banner height returned from ad network.</param>
        /// <param name="isPrecache">true if loaded ad is precache, otherwise - false.</param>
        void onBannerLoaded(int height, bool isPrecache);

        /// <summary>
        /// <para>
        /// Fires when Banner fails to load after passing the waterfall.
        /// </para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/ad-types/banner#id-[Development]UnitySDK.Banner-BannerCallbacks"/> for more details.
        /// </summary>
        /// <remarks>If auto cache is enabled, the next attempt to load ad will start automatically, after some delay.</remarks>
        void onBannerFailedToLoad();

        /// <summary>
        /// <para>
        /// Fires a few seconds after Banner is displayed on the screen.
        /// </para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/ad-types/banner#id-[Development]UnitySDK.Banner-BannerCallbacks"/> for more details.
        /// </summary>
        void onBannerShown();

        /// <summary>
        /// <para>
        /// Fires when user clicks on the Banner ad.
        /// </para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/ad-types/banner#id-[Development]UnitySDK.Banner-BannerCallbacks"/> for more details.
        /// </summary>
        void onBannerClicked();

        /// <summary>
        /// <para>
        /// Fires when Banner expires and should not be used.
        /// </para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/ad-types/banner#id-[Development]UnitySDK.Banner-BannerCallbacks"/> for more details.
        /// </summary>
        /// <remarks>This callback won't be fired, unless you are loading and not showing ad creative for hours or even days.</remarks>
        void onBannerExpired();
    }
}
