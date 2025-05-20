// ReSharper disable CheckNamespace

using System;
using System.Collections.Generic;
using System.Linq;

namespace AppodealStack.Monetization.Common
{
    internal class PurchaseProxyListener : IPurchaseProxyListener
    {
        public IPurchaseListener Listener { get; set; }

        public event EventHandler<PurchaseValidatedEventArgs> OnValidationSucceeded;
        public event EventHandler<PurchaseValidationFailedEventArgs> OnValidationFailed;

        public void OnPurchaseValidationSucceeded(IEnumerable<string> purchases)
        {
            var successfulPurchases = purchases.ToList();
            Listener?.OnPurchaseValidationSucceeded(successfulPurchases);
            OnValidationSucceeded?.Invoke(this, new PurchaseValidatedEventArgs(successfulPurchases));
        }

        public void OnPurchaseValidationFailed(string reason, IEnumerable<string> purchases)
        {
            var failedPurchases = purchases.ToList();
            Listener?.OnPurchaseValidationFailed(reason, failedPurchases);
            OnValidationFailed?.Invoke(this, new PurchaseValidationFailedEventArgs(reason, failedPurchases));
        }
    }
}
