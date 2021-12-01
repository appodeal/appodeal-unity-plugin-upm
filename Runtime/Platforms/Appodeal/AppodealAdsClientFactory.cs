using AppodealAds.Unity.Common;
#if UNITY_ANDROID && !UNITY_EDITOR
using AppodealAds.Unity.Platforms.Android;
#elif UNITY_IPHONE && !UNITY_EDITOR
using AppodealAds.Unity.Platforms.iOS;
#else
using AppodealAds.Unity.Platforms.Dummy;
#endif

namespace AppodealAds.Unity.Platforms
{
    public static class AppodealAdsClientFactory
    {
        public static IAppodealAdsClient GetAppodealAdsClient()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
			return new AndroidAppodealClient();
#elif UNITY_IPHONE && !UNITY_EDITOR
			return AppodealAdsClient.Instance;
#else
            return new DummyClient();
#endif
        }
    }
}