// ReSharper disable CheckNamespace

using System;

namespace AppodealStack.Monetization.Common
{
    internal interface IInAppPurchaseValidationProxyListener : IInAppPurchaseValidationListener
    {
        IInAppPurchaseValidationListener Listener { get; set; }
        event EventHandler<InAppPurchaseEventArgs> OnValidationSucceeded;
        event EventHandler<InAppPurchaseEventArgs> OnValidationFailed;
    }
}
