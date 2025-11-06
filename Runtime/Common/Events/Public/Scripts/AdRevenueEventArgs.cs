// ReSharper disable CheckNamespace

using System;
using System.Diagnostics.CodeAnalysis;

namespace AppodealStack.Monetization.Common
{
    /// <summary>
    /// This class is designed to store arguments of
    /// <see cref="AppodealCallbacks.AdRevenue.OnReceived">AdRevenue.OnReceived</see> event.
    /// </summary>
    public class AdRevenueEventArgs : EventArgs
    {
        /// <summary>
        /// Returns object of type <see cref="AppodealAdRevenue"/> containing data about the last ad impression.
        /// </summary>
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public AppodealAdRevenue Ad { get; }

        /// <summary>
        /// Constructor for Ad Revenue Event Args
        /// </summary>
        /// <param name="ad">AppodealAdRevenue data object.</param>
        public AdRevenueEventArgs(AppodealAdRevenue ad)
        {
            Ad = ad;
        }
    }
}
