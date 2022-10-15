using System;

// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Common
{
    /// <summary>
    /// This class is designed to store arguments of
    /// <see cref="AppodealEvents.RewardedVideo.OnClosed">RewardedVideo.OnClosed</see> event.
    /// </summary>
    public class RewardedVideoClosedEventArgs : EventArgs
    {
        /// <summary>
        /// Returns true if video has been fully watched, otherwise - false.
        /// </summary>
        public bool Finished { get; }

        /// <summary>
        /// Constructor for Rewarded Video Closed Event Args
        /// </summary>
        /// <param name="finished">Whether or not video has been fully watched.</param>
        public RewardedVideoClosedEventArgs(bool finished)
        {
            Finished = finished;
        }
    }
}
