using System.Diagnostics.CodeAnalysis;

// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Common
{
    /// <summary>
    /// <para>
    /// Static class containing all currently supported ad networks.
    /// </para>
    /// Its fields can be used as arguments for the <see langword="Appodeal.DisableNetwork()"/> methods.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static class AppodealNetworks
    {
        public const string A4G = "a4g";
        public const string AdColony = "adcolony";
        public const string Admob = "admob";
        public const string AppLovin = "applovin";
        public const string Appodeal = "appodeal";
        public const string BidMachine = "bidmachine";
        public const string Meta = "facebook";
        public const string IronSource = "ironsource";
        public const string Mraid = "mraid";
        public const string MyTarget = "my_target";
        public const string Nast = "nast";
        public const string Notsy = "notsy";
        public const string UnityAds = "unity_ads";
        public const string Vast = "vast";
        public const string Vungle = "vungle";
        public const string Yandex = "yandex";
    }
}
