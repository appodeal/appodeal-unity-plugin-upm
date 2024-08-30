// ReSharper Disable CheckNamespace

using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.Scripting;
using AppodealStack.Monetization.Common;

namespace AppodealStack.Monetization.Platforms.Android
{
    /// <summary>
    /// Android implementation of the <see langword="IAdRevenueListener"/> interface.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal class AppodealAdRevenueCallback : AndroidJavaProxy
    {
        private readonly IAdRevenueListener _listener;

        internal AppodealAdRevenueCallback(IAdRevenueListener listener) : base("com.appodeal.ads.revenue.AdRevenueCallbacks")
        {
            _listener = listener;
        }

        [Preserve]
        private void onAdRevenueReceive(AndroidJavaObject ad)
        {
            if (ad == null)
            {
                UnityMainThreadDispatcher.Post(_ => _listener?.OnAdRevenueReceived(null));
                return;
            }

            var adRevenue = new AppodealAdRevenue
            {
                AdType = ad.Call<string>("getAdTypeString"),
                NetworkName = ad.Call<string>("getNetworkName"),
                AdUnitName = ad.Call<string>("getAdUnitName"),
                DemandSource = ad.Call<string>("getDemandSource"),
                Placement = ad.Call<string>("getPlacement"),
                Revenue = ad.Call<double>("getRevenue"),
                Currency = ad.Call<string>("getCurrency"),
                RevenuePrecision = ad.Call<string>("getRevenuePrecision")
            };

            UnityMainThreadDispatcher.Post(_ => _listener?.OnAdRevenueReceived(adRevenue));
        }
    }
}
