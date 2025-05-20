// ReSharper disable CheckNamespace

using System;

namespace AppodealStack.Monetization.Common
{
    internal interface IPurchaseProxyListener : IPurchaseListener
    {
        IPurchaseListener Listener { get; set; }
        event EventHandler<PurchaseValidatedEventArgs> OnValidationSucceeded;
        event EventHandler<PurchaseValidationFailedEventArgs> OnValidationFailed;
    }
}
