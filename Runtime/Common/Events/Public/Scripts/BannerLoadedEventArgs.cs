// ReSharper disable CheckNamespace

using System;
using System.Diagnostics.CodeAnalysis;

namespace AppodealStack.Monetization.Common
{
    /// <summary>
    /// This class is designed to store arguments of
    /// <see cref="AppodealCallbacks.Banner.OnLoaded">Banner.OnLoaded</see> event.
    /// </summary>
    public class BannerLoadedEventArgs : EventArgs
    {
        /// <summary>
        /// Returns banner height received from ad networks.
        /// </summary>
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public int Height { get; }

        /// <summary>
        /// Returns true if loaded ad is precache, otherwise - false.
        /// </summary>
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public bool IsPrecache { get; }

        /// <summary>
        /// Constructor for Banner Loaded Event Args
        /// </summary>
        /// <param name="height">Height of the loaded banner ad.</param>
        /// <param name="isPrecache">Whether loaded ad is precache.</param>
        public BannerLoadedEventArgs(int height, bool isPrecache)
        {
            Height = height;
            IsPrecache = isPrecache;
        }
    }
}
