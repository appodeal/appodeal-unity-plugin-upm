using System;

// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Common
{
    /// <summary>
    /// This class is designed to store arguments of
    /// <see cref="AppodealEvents.InAppPurchase.OnValidationSucceeded">InAppPurchase.OnValidationSucceeded</see> and
    /// <see cref="AppodealEvents.InAppPurchase.OnValidationFailed">InAppPurchase.OnValidationFailed</see> events.
    /// </summary>
    public class InAppPurchaseEventArgs : EventArgs
    {
        /// <summary>
        /// Returns json-formatted string containing purchase data and errors, if any.
        /// </summary>
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
