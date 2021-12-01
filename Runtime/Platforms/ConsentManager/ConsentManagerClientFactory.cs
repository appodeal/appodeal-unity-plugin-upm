using AppodealCM.Unity.Common;

namespace AppodealCM.Unity.Platforms
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
                return new Dummy();
#endif
        }

        public static IVendorBuilder GetVendorBuilder(string name, string bundle, string policyUrl)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
		return new AndroidVendorBuilder (name, bundle, policyUrl);
#elif UNITY_IPHONE && !UNITY_EDITOR
		return new iOSVendorBuilder(name, bundle, policyUrl);
#else
                return new Dummy();
#endif
        }

        public static IConsentFormBuilder GetConsentFormBuilder()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
		return new AndroidConsentFormBuilder();
#elif UNITY_IPHONE && !UNITY_EDITOR
                return new iOSConsentFormBuilder();
#else
                return new Dummy();
#endif
        }

        public static IConsentManagerException GetConsentManagerException()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
		return new AndroidConsentManagerException();
#elif UNITY_IPHONE && !UNITY_EDITOR
		return new iOSConsentManagerException();
#else
                return new Dummy();
#endif
        }
    }
}