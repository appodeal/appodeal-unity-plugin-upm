// ReSharper disable CheckNamespace

using System.Collections.Generic;

namespace AppodealStack.Monetization.Common
{
    /// <summary>
    /// <para>
    /// Interface containing signatures of Appodeal purchase-validation callback methods for AppsFlyer ROI360 feature.
    /// </para>
    /// See <see href="https://docs.appodeal.com/unity/advanced/in-app-purchases"/> for more details.
    /// </summary>
    public interface IPurchaseListener
    {
        /// <summary>
        /// <para>
        /// Invoked when purchases have been successfully validated and tracked.
        /// </para>
        /// See <see href="https://docs.appodeal.com/unity/advanced/in-app-purchases"/> for more details.
        /// </summary>
        /// <param name="purchases">A collection of successfully processed purchases. Each purchase is a json-formatted string representing a map of key-value pairs.</param>
        void OnPurchaseValidationSucceeded(IEnumerable<string> purchases);

        /// <summary>
        /// <para>
        /// Invoked when purchase validation fails.
        /// </para>
        /// See <see href="https://docs.appodeal.com/unity/advanced/in-app-purchases"/> for more details.
        /// </summary>
        /// <param name="reason">Contains details about the cause of the failure.</param>
        /// <param name="purchases">A collection of purchases that failed validation. Each purchase is a json-formatted string representing a map of key-value pairs.</param>
        void OnPurchaseValidationFailed(string reason, IEnumerable<string> purchases);
    }
}
