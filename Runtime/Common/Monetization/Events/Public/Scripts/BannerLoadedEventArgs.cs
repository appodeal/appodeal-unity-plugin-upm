using System;

// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Common
{
    /// <summary>
    /// This class is designed to store arguments of
    /// <see cref="AppodealEvents.Banner.OnLoaded">Banner.OnLoaded</see> event.
    /// </summary>
    public class BannerLoadedEventArgs : EventArgs
    {
        /// <summary>
        /// Returns banner height received from ad networks.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Returns true if loaded ad is precache, otherwise - false.
        /// </summary>
        public bool IsPrecache { get; }

        /// <summary>
        /// Constructor for Banner Loaded Event Args
        /// </summary>
        /// <param name="height">Height of the loaded banner ad.</param>
        /// <param name="isPrecache">Whether or not loaded ad is precache.</param>
        public BannerLoadedEventArgs(int height, bool isPrecache)
        {
            Height = height;
            IsPrecache = isPrecache;
        }
    }
}
