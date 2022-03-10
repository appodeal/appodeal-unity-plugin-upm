using AppodealStack.ConsentManager.Common;

#if UNITY_ANDROID && !UNITY_EDITOR
using AppodealStack.ConsentManager.Platforms.Android;
#elif UNITY_IPHONE && !UNITY_EDITOR
using AppodealStack.ConsentManager.Platforms.iOS;
#else
using AppodealStack.ConsentManager.Platforms.Dummy;
#endif

namespace AppodealStack.ConsentManager.Platforms
{
    public static class ConsentManagerClientFactory
    {
        public static IConsentManager GetConsentManager()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            return new AndroidConsentManager ();
#elif UNITY_IPHONE && !UNITY_EDITOR
		    return new iOSConsentManager();
#else
            return new DummyConsentManagerClient();
#endif
        }

        public static IVendorBuilder GetVendorBuilder(string name, string bundle, string policyUrl)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
		    return new AndroidVendorBuilder (name, bundle, policyUrl);
#elif UNITY_IPHONE && !UNITY_EDITOR
		    return new iOSVendorBuilder(name, bundle, policyUrl);
#else
            return new DummyConsentManagerClient();
#endif
        }

        public static IConsentFormBuilder GetConsentFormBuilder()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
		    return new AndroidConsentFormBuilder();
#elif UNITY_IPHONE && !UNITY_EDITOR
            return new iOSConsentFormBuilder();
#else
            return new DummyConsentManagerClient();
#endif
        }

        public static IConsentManagerException GetConsentManagerException()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
		    return new AndroidConsentManagerException();
#elif UNITY_IPHONE && !UNITY_EDITOR
		    return new iOSConsentManagerException();
#else
            return new DummyConsentManagerClient();
#endif
        }
    }
}
