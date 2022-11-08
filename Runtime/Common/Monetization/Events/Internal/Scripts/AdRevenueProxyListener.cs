using System;

// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Common
{
    public class AdRevenueProxyListener : IAdRevenueProxyListener
    {
        public IAdRevenueListener Listener { get; set; }

        public event EventHandler<AdRevenueEventArgs> OnReceived;

        public void OnAdRevenueReceived(AppodealAdRevenue ad)
        {
            Listener?.OnAdRevenueReceived(ad);
            OnReceived?.Invoke(this, new AdRevenueEventArgs(ad));
        }
    }
}
