using System.Diagnostics.CodeAnalysis;

namespace AppodealAds.Unity.Common
{
    /// <summary>
    /// Interface containing signatures of Appodeal Non-Skippable video callback methods.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "UnusedParameter.Global")]
    public interface INonSkippableVideoAdListener
    {
        /// <summary>
        /// Fires when Non-Skippable Video is loaded.
        /// </summary>
        /// <param name="isPrecache">true if loaded ad is precache, otherwise - false.</param>
        void onNonSkippableVideoLoaded(bool isPrecache);

        /// <summary>
        /// Fires when Non-Skippable Video fails to load after passing the waterfall.
        /// </summary>
        /// <remarks>If auto cache is enabled, the next attempt to load ad will start automatically, after some delay.</remarks>
        void onNonSkippableVideoFailedToLoad();

        /// <summary>
        /// Fires when attempt to show Non-Skippable Video fails for some reason.
        /// </summary>
        void onNonSkippableVideoShowFailed();

        /// <summary>
        /// Fires a few seconds after Non-Skippable Video is displayed on the screen.
        /// </summary>
        void onNonSkippableVideoShown();

        /// <summary>
        /// Fires when Non-Skippable Video has been viewed to the end.
        /// </summary>
        void onNonSkippableVideoFinished();

        /// <summary>
        /// Fires when user closes Non-Skippable Video.
        /// </summary>
        /// <param name="finished">true if video has been fully watched, otherwise - false.</param>
        void onNonSkippableVideoClosed(bool finished);

        /// <summary>
        /// Fires when Non-Skippable Video expires and should not be used.
        /// </summary>
        /// <remarks>This callback won't be fired, unless you are loading and not showing ad creative for hours or even days.</remarks>
        void onNonSkippableVideoExpired();
    }
}
