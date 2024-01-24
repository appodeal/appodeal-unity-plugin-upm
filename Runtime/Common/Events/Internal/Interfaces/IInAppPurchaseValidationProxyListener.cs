using System;

// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Common
{
    public interface IInAppPurchaseValidationProxyListener : IInAppPurchaseValidationListener
    {
        IInAppPurchaseValidationListener Listener { get; set; }
        event EventHandler<InAppPurchaseEventArgs> OnValidationSucceeded;
        event EventHandler<InAppPurchaseEventArgs> OnValidationFailed;
    }
}
