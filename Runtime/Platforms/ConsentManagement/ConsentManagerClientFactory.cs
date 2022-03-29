using AppodealStack.ConsentManagement.Common;

#if UNITY_ANDROID && !UNITY_EDITOR
using AppodealStack.ConsentManagement.Platforms.Android;
#elif UNITY_IPHONE && !UNITY_EDITOR
using AppodealStack.ConsentManagement.Platforms.Ios;
#else
using AppodealStack.ConsentManagement.Platforms.Dummy;
#endif

// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Platforms
{
    public static class ConsentManagerClientFactory
    {
        public static IConsentManager GetConsentManager()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            return new AndroidConsentManager();
#elif UNITY_IPHONE && !UNITY_EDITOR
	        return new IosConsentManager();
#else
            return new DummyConsentManager();
#endif
        }

        public static IVendorBuilder GetVendorBuilder(string name, string bundle, string policyUrl)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
		    return new AndroidVendorBuilder(name, bundle, policyUrl);
#elif UNITY_IPHONE && !UNITY_EDITOR
		    return new IosVendorBuilder(name, bundle, policyUrl);
#else
            return new DummyVendorBuilder();
#endif
        }

        public static IConsentFormBuilder GetConsentFormBuilder()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
		    return new AndroidConsentFormBuilder();
#elif UNITY_IPHONE && !UNITY_EDITOR
            return new IosConsentFormBuilder();
#else
            return new DummyConsentFormBuilder();
#endif
        }

        public static IConsentManagerException GetConsentManagerException()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
		    return new AndroidConsentManagerException();
#elif UNITY_IPHONE && !UNITY_EDITOR
		    return new IosConsentManagerException();
#else
            return new DummyConsentManagerException();
#endif
        }
    }
}
