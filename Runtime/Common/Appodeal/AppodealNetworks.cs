using System.Diagnostics.CodeAnalysis;

namespace AppodealStack.Mediation.Common
{
    /// <summary>
    /// <para>
    /// Static class containing all currently supported ad networks.
    /// </para>
    /// Its variables can be used as arguments for <see langword="Appodeal.disableNetwork()"/> methods.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "InvalidXmlDocComment")]
    public static class AppodealNetworks
    {
        public const string VUNGLE = "vungle";
        public const string APPLOVIN = "applovin";
        public const string ADCOLONY = "adcolony";
        public const string MY_TARGET = "my_target";
        public const string NOTSY = "notsy";
        public const string BIDMACHINE = "bidmachine";
        public const string AMAZON_ADS = "amazon_ads";
        public const string ADMOB = "admob";
        public const string UNITY_ADS = "unity_ads";
        public const string FACEBOOK = "facebook";
        public const string YANDEX = "yandex";
        public const string APPODEAL = "appodeal";
        public const string IRONSOURCE = "ironsource";
        public const string A4G = "a4g";
        public const string MRAID = "mraid";
        public const string NAST = "nast";
        public const string OGURY = "ogury";
        public const string VAST = "vast";
    }
}
