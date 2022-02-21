using AppodealStack.Mediation.Common;

#if UNITY_ANDROID && !UNITY_EDITOR
using AppodealStack.Mediation.Platforms.Android;
#elif UNITY_IPHONE && !UNITY_EDITOR
using AppodealStack.Mediation.Platforms.iOS;
#else
using AppodealStack.Mediation.Platforms.Dummy;
#endif

namespace AppodealStack.Mediation.Platforms
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
