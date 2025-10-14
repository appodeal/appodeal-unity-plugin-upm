// ReSharper disable CheckNamespace

using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.Scripting;
using AppodealStack.Monetization.Common;

namespace AppodealStack.Monetization.Platforms.Android
{
    /// <summary>
    /// Android implementation of the <see cref="AppodealStack.Monetization.Common.IAdRevenueListener"/> interface.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal class AppodealAdRevenueCallback : AndroidJavaProxy
    {
        private readonly IAdRevenueListener _listener;

        internal AppodealAdRevenueCallback(IAdRevenueListener listener) : base(AndroidConstants.JavaInterfaceName.AdRevenueCallback)
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
                AdType = ad.Call<string>(AndroidConstants.JavaMethodName.AdRevenue.GetAdTypeString),
                NetworkName = ad.Call<string>(AndroidConstants.JavaMethodName.AdRevenue.GetNetworkName),
                AdUnitName = ad.Call<string>(AndroidConstants.JavaMethodName.AdRevenue.GetAdUnitName),
                DemandSource = ad.Call<string>(AndroidConstants.JavaMethodName.AdRevenue.GetDemandSource),
                Placement = ad.Call<string>(AndroidConstants.JavaMethodName.AdRevenue.GetPlacement),
                Revenue = ad.Call<double>(AndroidConstants.JavaMethodName.AdRevenue.GetRevenue),
                Currency = ad.Call<string>(AndroidConstants.JavaMethodName.AdRevenue.GetCurrency),
                RevenuePrecision = ad.Call<string>(AndroidConstants.JavaMethodName.AdRevenue.GetRevenuePrecision)
            };

            UnityMainThreadDispatcher.Post(_ => _listener?.OnAdRevenueReceived(adRevenue));
        }
    }
}
