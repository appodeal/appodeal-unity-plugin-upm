using System.Diagnostics.CodeAnalysis;
using AppodealStack.Monetization.Common;

#if UNITY_ANDROID && !UNITY_EDITOR
using AppodealStack.Monetization.Platforms.Android;
#elif UNITY_IPHONE && !UNITY_EDITOR
using AppodealStack.Monetization.Platforms.Ios;
#else
using AppodealStack.Monetization.Platforms.Dummy;
#endif

// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Platforms
{
    [SuppressMessage("ReSharper", "UnusedParameter.Global")]
    public static class AppodealAdsClientFactory
    {
        public static IAppodealAdsClient GetAppodealAdsClient()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            return new AndroidAppodealClient();
#elif UNITY_IPHONE && !UNITY_EDITOR
            return new IosAppodealClient();
#else
            return new DummyAppodealClient();
#endif
        }

        public static IPlayStoreInAppPurchaseBuilder GetPlayStoreInAppPurchaseBuilder(PlayStorePurchaseType purchaseType)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            return new AndroidPlayStoreInAppPurchaseBuilder(purchaseType);
#elif UNITY_IPHONE && !UNITY_EDITOR
            return null;
#else
            return new DummyPlayStoreInAppPurchaseBuilder();
#endif
        }

        public static IAppStoreInAppPurchaseBuilder GetAppStoreInAppPurchaseBuilder(AppStorePurchaseType purchaseType)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            return null;
#elif UNITY_IPHONE && !UNITY_EDITOR
            return new IosAppStoreInAppPurchaseBuilder(purchaseType);
#else
            return new DummyAppStoreInAppPurchaseBuilder();
#endif
        }
    }
}
