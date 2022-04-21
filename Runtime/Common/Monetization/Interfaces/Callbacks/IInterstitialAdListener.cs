// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Common
{
    /// <summary>
    /// <para>
    /// Interface containing signatures of Appodeal Interstitial callback methods.
    /// </para>
    /// See <see href="https://wiki.appodeal.com/en/unity/get-started/ad-types/interstitial#id-[Development]UnitySDK.Interstitial-InterstitialCallbacks"/> for more details.
    /// </summary>
    public interface IInterstitialAdListener
    {
        /// <summary>
        /// <para>
        /// Fires when Interstitial is loaded.
        /// </para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/ad-types/interstitial#id-[Development]UnitySDK.Interstitial-InterstitialCallbacks"/> for more details.
        /// </summary>
        /// <param name="isPrecache">true if loaded ad is precache, otherwise - false.</param>
        void OnInterstitialLoaded(bool isPrecache);

        /// <summary>
        /// <para>
        /// Fires when Interstitial fails to load after passing the waterfall.
        /// </para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/ad-types/interstitial#id-[Development]UnitySDK.Interstitial-InterstitialCallbacks"/> for more details.
        /// </summary>
        /// <remarks>If auto cache is enabled, the next attempt to load ad will start automatically, after some delay.</remarks>
        void OnInterstitialFailedToLoad();

        /// <summary>
        /// <para>
        /// Fires when attempt to show Interstitial fails for some reason.
        /// </para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/ad-types/interstitial#id-[Development]UnitySDK.Interstitial-InterstitialCallbacks"/> for more details.
        /// </summary>
        void OnInterstitialShowFailed();

        /// <summary>
        /// <para>
        /// Fires a few seconds after Interstitial is displayed on the screen.
        /// </para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/ad-types/interstitial#id-[Development]UnitySDK.Interstitial-InterstitialCallbacks"/> for more details.
        /// </summary>
        void OnInterstitialShown();

        /// <summary>
        /// <para>
        /// Fires when user closes Interstitial.
        /// </para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/ad-types/interstitial#id-[Development]UnitySDK.Interstitial-InterstitialCallbacks"/> for more details.
        /// </summary>
        void OnInterstitialClosed();

        /// <summary>
        /// <para>
        /// Fires when user clicks on the Interstitial ad.
        /// </para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/ad-types/interstitial#id-[Development]UnitySDK.Interstitial-InterstitialCallbacks"/> for more details.
        /// </summary>
        void OnInterstitialClicked();

        /// <summary>
        /// <para>
        /// Fires when Interstitial expires and should not be used.
        /// </para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/ad-types/interstitial#id-[Development]UnitySDK.Interstitial-InterstitialCallbacks"/> for more details.
        /// </summary>
        /// <remarks>This callback won't be fired, unless you are loading and not showing ad creative for hours or even days.</remarks>
        void OnInterstitialExpired();
    }
}
