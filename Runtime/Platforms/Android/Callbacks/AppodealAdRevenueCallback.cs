using System.Threading;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using AppodealStack.Monetization.Common;

// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Platforms.Android
{
    /// <summary>
    /// Android implementation of <see langword="IAdRevenueListener"/> interface.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public class AppodealAdRevenueCallback : AndroidJavaProxy
    {
        private readonly IAdRevenueListener _listener;
        private static SynchronizationContext _unityContext;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void GetContext() => _unityContext = SynchronizationContext.Current;

        internal AppodealAdRevenueCallback(IAdRevenueListener listener) : base("com.appodeal.ads.revenue.AdRevenueCallbacks")
        {
            _listener = listener;
        }

        private void onAdRevenueReceive(AndroidJavaObject ad)
        {
            _unityContext?.Post(obj => _listener?.OnAdRevenueReceived(
                new AppodealAdRevenue
                {
                    AdType = ad.Call<string>("getAdTypeString"),
                    NetworkName = ad.Call<string>("getNetworkName"),
                    AdUnitName = ad.Call<string>("getAdUnitName"),
                    DemandSource = ad.Call<string>("getDemandSource"),
                    Placement = ad.Call<string>("getPlacement"),
                    Revenue = ad.Call<double>("getRevenue"),
                    Currency = ad.Call<string>("getCurrency"),
                    RevenuePrecision = ad.Call<string>("getRevenuePrecision")
                }
            ), null);
        }
    }
}
