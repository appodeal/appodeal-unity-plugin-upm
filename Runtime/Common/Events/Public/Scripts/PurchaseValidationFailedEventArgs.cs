// ReSharper disable CheckNamespace

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AppodealStack.Monetization.Common
{
    /// <summary>
    /// This class is designed to store arguments of
    /// <see cref="AppodealCallbacks.Purchase.OnValidationFailed">Purchase.OnValidationFailed</see> event.
    /// </summary>
    public class PurchaseValidationFailedEventArgs : EventArgs
    {
        /// <summary>
        /// Returns details about the cause of the failure.
        /// </summary>
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public string Reason { get; }

        /// <summary>
        /// Returns a collection of json-formatted strings representing a map of key-value pairs.
        /// </summary>
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public IEnumerable<string> Purchases { get; }

        /// <summary>
        /// Constructor for Purchase Validated Event Args.
        /// </summary>
        /// <param name="reason">Contains details about the cause of the failure.</param>
        /// <param name="purchases">A collection of json-formatted strings representing a map of key-value pairs.</param>
        public PurchaseValidationFailedEventArgs(string reason, IEnumerable<string> purchases)
        {
            Reason = reason;
            Purchases = purchases;
        }
    }
}
