using System;

// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Common
{
    /// <summary>
    /// This class is designed to store arguments of
    /// <see cref="AppodealEvents.Mrec.OnLoaded">Mrec.OnLoaded</see>,
    /// <see cref="AppodealEvents.Interstitial.OnLoaded">Interstitial.OnLoaded</see> and
    /// <see cref="AppodealEvents.RewardedVideo.OnLoaded">RewardedVideo.OnLoaded</see> events.
    /// </summary>
    public class AdLoadedEventArgs : EventArgs
    {
        /// <summary>
        /// Returns true if loaded ad is precache, otherwise - false.
        /// </summary>
        public bool IsPrecache { get; }

        /// <summary>
        /// Constructor for Ad Loaded Event Args
        /// </summary>
        /// <param name="isPrecache">Whether or not loaded ad is precache.</param>
        public AdLoadedEventArgs(bool isPrecache)
        {
            IsPrecache = isPrecache;
        }
    }
}
