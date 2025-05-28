// ReSharper disable CheckNamespace

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AppodealStack.Monetization.Common
{
    /// <summary>
    /// This class is designed to store arguments of
    /// <see cref="AppodealCallbacks.Purchase.OnValidationSucceeded">Purchase.OnValidationSucceeded</see> event.
    /// </summary>
    public class PurchaseValidatedEventArgs : EventArgs
    {
        /// <summary>
        /// Returns a collection of json-formatted strings representing a map of key-value pairs.
        /// </summary>
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public IEnumerable<string> Purchases { get; }

        /// <summary>
        /// Constructor for Purchase Validated Event Args.
        /// </summary>
        /// <param name="purchases">A collection of json-formatted strings representing a map of key-value pairs.</param>
        public PurchaseValidatedEventArgs(IEnumerable<string> purchases)
        {
            Purchases = purchases;
        }
    }
}
