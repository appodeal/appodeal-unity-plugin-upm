// ReSharper disable CheckNamespace

using System;
using System.Diagnostics.CodeAnalysis;

namespace AppodealStack.Monetization.Common
{
    /// <summary>
    /// This class is designed to store arguments of
    /// <see cref="AppodealCallbacks.RewardedVideo.OnFinished">RewardedVideo.OnFinished</see> event.
    /// </summary>
    public class RewardedVideoFinishedEventArgs : EventArgs
    {
        /// <summary>
        /// Returns reward amount for watching an ad if specified in dashboard for current placement.
        /// </summary>
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public double Amount { get; }

        /// <summary>
        /// Returns reward currency for watching an ad if specified in dashboard for current placement.
        /// </summary>
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public string Currency { get; }

        /// <summary>
        /// Constructor for Rewarded Video Finished Event Args
        /// </summary>
        /// <param name="amount">Amount of reward.</param>
        /// <param name="currency">Reward currency.</param>
        public RewardedVideoFinishedEventArgs(double amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }
    }
}
