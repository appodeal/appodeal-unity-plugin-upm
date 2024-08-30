// ReSharper Disable CheckNamespace

using System;

namespace AppodealStack.Monetization.Common
{
    internal interface IAdRevenueProxyListener : IAdRevenueListener
    {
        IAdRevenueListener Listener { get; set; }
        event EventHandler<AdRevenueEventArgs> OnReceived;
    }
}
