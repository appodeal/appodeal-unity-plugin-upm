// ReSharper disable CheckNamespace

using System;
using System.Diagnostics.CodeAnalysis;

namespace AppodealStack.Monetization.Common
{
    /// <summary>
    /// This class is designed to store arguments of
    /// <see cref="AppodealCallbacks.Mrec.OnLoaded">Mrec.OnLoaded</see>,
    /// <see cref="AppodealCallbacks.Interstitial.OnLoaded">Interstitial.OnLoaded</see> and
    /// <see cref="AppodealCallbacks.RewardedVideo.OnLoaded">RewardedVideo.OnLoaded</see> events.
    /// </summary>
    public class AdLoadedEventArgs : EventArgs
    {
        /// <summary>
        /// Returns true if loaded ad is precache, otherwise - false.
        /// </summary>
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public bool IsPrecache { get; }

        /// <summary>
        /// Constructor for Ad Loaded Event Args
        /// </summary>
        /// <param name="isPrecache">Whether loaded ad is precache.</param>
        public AdLoadedEventArgs(bool isPrecache)
        {
            IsPrecache = isPrecache;
        }
    }
}
