// ReSharper disable CheckNamespace

using System;
using System.Diagnostics.CodeAnalysis;

namespace AppodealStack.Monetization.Common
{
    /// <summary>
    /// This class is designed to store arguments of
    /// <see cref="AppodealCallbacks.InAppPurchase.OnValidationSucceeded">InAppPurchase.OnValidationSucceeded</see> and
    /// <see cref="AppodealCallbacks.InAppPurchase.OnValidationFailed">InAppPurchase.OnValidationFailed</see> events.
    /// </summary>
    public class InAppPurchaseEventArgs : EventArgs
    {
        /// <summary>
        /// Returns json-formatted string containing purchase data and errors, if any.
        /// </summary>
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public string Json { get; }

        /// <summary>
        /// Constructor for InApp Purchase Event Args
        /// </summary>
        /// <param name="json">InAppPurchase data and errors, if any.</param>
        public InAppPurchaseEventArgs(string json)
        {
            Json = json;
        }
    }
}
