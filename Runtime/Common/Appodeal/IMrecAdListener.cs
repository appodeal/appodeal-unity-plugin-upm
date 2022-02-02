using System.Diagnostics.CodeAnalysis;

namespace AppodealAds.Unity.Common
{
    /// <summary>
    /// <para>
    /// Interface containing signatures of Appodeal Mrec callback methods.
    /// </para>
    /// See <see href="https://wiki.appodeal.com/en/unity/get-started/ad-types/mrec#id-[Development]UnitySDK.MREC-MRECCallbacks"/> for more details.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "UnusedParameter.Global")]
    public interface IMrecAdListener
    {
        /// <summary>
        /// <para>
        /// Fires when Mrec is loaded.
        /// </para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/ad-types/mrec#id-[Development]UnitySDK.MREC-MRECCallbacks"/> for more details.
        /// </summary>
        /// <param name="isPrecache">true if loaded ad is precache, otherwise - false.</param>
        void onMrecLoaded(bool isPrecache);

        /// <summary>
        /// <para>
        /// Fires when Mrec fails to load after passing the waterfall.
        /// </para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/ad-types/mrec#id-[Development]UnitySDK.MREC-MRECCallbacks"/> for more details.
        /// </summary>
        /// <remarks>If auto cache is enabled, the next attempt to load ad will start automatically, after some delay.</remarks>
        void onMrecFailedToLoad();

        /// <summary>
        /// <para>
        /// Fires a few seconds after Mrec is displayed on the screen.
        /// </para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/ad-types/mrec#id-[Development]UnitySDK.MREC-MRECCallbacks"/> for more details.
        /// </summary>
        void onMrecShown();

        /// <summary>
        /// <para>
        /// Fires when user clicks on Mrec ad.
        /// </para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/ad-types/mrec#id-[Development]UnitySDK.MREC-MRECCallbacks"/> for more details.
        /// </summary>
        void onMrecClicked();

        /// <summary>
        /// <para>
        /// Fires when Mrec expires and should not be used.
        /// </para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/ad-types/mrec#id-[Development]UnitySDK.MREC-MRECCallbacks"/> for more details.
        /// </summary>
        /// <remarks>This callback won't be fired, unless you are loading and not showing ad creative for hours or even days.</remarks>
        void onMrecExpired();
    }
}
