// ReSharper disable CheckNamespace

using System;
using System.Diagnostics.CodeAnalysis;

namespace AppodealStack.Monetization.Common
{
    /// <summary>
    /// This class is designed to store arguments of
    /// <see cref="AppodealCallbacks.RewardedVideo.OnClosed">RewardedVideo.OnClosed</see> event.
    /// </summary>
    public class RewardedVideoClosedEventArgs : EventArgs
    {
        /// <summary>
        /// Returns true if video has been fully watched, otherwise - false.
        /// </summary>
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public bool Finished { get; }

        /// <summary>
        /// Constructor for Rewarded Video Closed Event Args
        /// </summary>
        /// <param name="finished">Whether video has been fully watched.</param>
        public RewardedVideoClosedEventArgs(bool finished)
        {
            Finished = finished;
        }
    }
}
