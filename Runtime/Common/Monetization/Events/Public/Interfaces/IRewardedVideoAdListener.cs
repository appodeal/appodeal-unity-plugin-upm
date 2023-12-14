// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Common
{
    /// <summary>
    /// <para>
    /// Interface containing signatures of Appodeal Rewarded video callback methods.
    /// </para>
    /// See <see href="https://docs.appodeal.com/unity/ad-types/rewarded-video?distribution=manual#callbacks"/> for more details.
    /// </summary>
    public interface IRewardedVideoAdListener
    {
        /// <summary>
        /// <para>
        /// Raised when Rewarded Video is loaded.
        /// </para>
        /// See <see href="https://docs.appodeal.com/unity/ad-types/rewarded-video?distribution=manual#callbacks"/> for more details.
        /// </summary>
        /// <param name="isPrecache">true if loaded ad is precache, otherwise - false.</param>
        void OnRewardedVideoLoaded(bool isPrecache);

        /// <summary>
        /// <para>
        /// Raised when Rewarded Video fails to load after passing the waterfall.
        /// </para>
        /// See <see href="https://docs.appodeal.com/unity/ad-types/rewarded-video?distribution=manual#callbacks"/> for more details.
        /// </summary>
        /// <remarks>If auto cache is enabled, the next attempt to load ad will start automatically, after some delay.</remarks>
        void OnRewardedVideoFailedToLoad();

        /// <summary>
        /// <para>
        /// Raised when attempt to show Rewarded Video fails for some reason.
        /// </para>
        /// See <see href="https://docs.appodeal.com/unity/ad-types/rewarded-video?distribution=manual#callbacks"/> for more details.
        /// </summary>
        void OnRewardedVideoShowFailed();

        /// <summary>
        /// <para>
        /// Raised a few seconds after Rewarded Video is displayed on the screen.
        /// </para>
        /// See <see href="https://docs.appodeal.com/unity/ad-types/rewarded-video?distribution=manual#callbacks"/> for more details.
        /// </summary>
        void OnRewardedVideoShown();

        /// <summary>
        /// <para>
        /// Raised when Rewarded Video has been viewed to the end.
        /// </para>
        /// See <see href="https://docs.appodeal.com/unity/ad-types/rewarded-video?distribution=manual#callbacks"/> for more details.
        /// </summary>
        /// <param name="amount">amount of reward.</param>
        /// <param name="currency">reward currency.</param>
        void OnRewardedVideoFinished(double amount, string currency);

        /// <summary>
        /// <para>
        /// Raised when user closes Rewarded Video.
        /// </para>
        /// See <see href="https://docs.appodeal.com/unity/ad-types/rewarded-video?distribution=manual#callbacks"/> for more details.
        /// </summary>
        /// <param name="finished">true if video has been fully watched, otherwise - false.</param>
        void OnRewardedVideoClosed(bool finished);

        /// <summary>
        /// <para>
        /// Raised when Rewarded Video expires and should not be used.
        /// </para>
        /// See <see href="https://docs.appodeal.com/unity/ad-types/rewarded-video?distribution=manual#callbacks"/> for more details.
        /// </summary>
        /// <remarks>This callback won't be fired, unless you are loading and not showing ad creative for hours or even days.</remarks>
        void OnRewardedVideoExpired();

        /// <summary>
        /// <para>
        /// Raised when user clicks on Rewarded Video ad.
        /// </para>
        /// See <see href="https://docs.appodeal.com/unity/ad-types/rewarded-video?distribution=manual#callbacks"/> for more details.
        /// </summary>
        void OnRewardedVideoClicked();
    }
}
