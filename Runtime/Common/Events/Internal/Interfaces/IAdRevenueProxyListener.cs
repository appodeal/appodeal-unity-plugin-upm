using System;

// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Common
{
    public interface IAdRevenueProxyListener : IAdRevenueListener
    {
        IAdRevenueListener Listener { get; set; }
        event EventHandler<AdRevenueEventArgs> OnReceived;
    }
}
