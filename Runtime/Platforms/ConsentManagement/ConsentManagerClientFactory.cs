using System.Diagnostics.CodeAnalysis;
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
    [SuppressMessage("ReSharper", "UnusedParameter.Global")]
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

        public static IConsentForm GetConsentForm(IConsentFormListener listener)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            return new AndroidConsentForm(listener);
#elif UNITY_IPHONE && !UNITY_EDITOR
            var builder = new IosConsentFormBuilder();
            builder.WithListener(listener);
            return builder.Build();
#else
            return new DummyConsentForm();
#endif
        }
    }
}
