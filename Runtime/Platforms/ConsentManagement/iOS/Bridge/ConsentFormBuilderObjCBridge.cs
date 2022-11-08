using System;
using System.Runtime.InteropServices;

// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Platforms.Ios
{
    internal delegate void ConsentFormCallback();
    internal delegate void ConsentFormCallbackError(IntPtr error);
    internal delegate void ConsentFormCallbackClosed(IntPtr consent);

    internal class ConsentFormBuilderObjCBridge
    {
        private readonly IntPtr _consentFormBuilder;

        public ConsentFormBuilderObjCBridge()
        {
            _consentFormBuilder = GetConsentForm();
        }

        public ConsentFormBuilderObjCBridge(IntPtr intPtr)
        {
            _consentFormBuilder = intPtr;
        }

        public IntPtr GetIntPtr()
        {
            return _consentFormBuilder;
        }

        public static void WithListener(ConsentFormCallback onConsentFormLoaded,
                                        ConsentFormCallbackError onConsentFormError,
                                        ConsentFormCallback onConsentFormOpened,
                                        ConsentFormCallbackClosed onConsentFormClosed)
        {
            CfbWithListener(onConsentFormLoaded, onConsentFormError, onConsentFormOpened, onConsentFormClosed);
        }

        [DllImport("__Internal")]
        private static extern IntPtr GetConsentForm();

        [DllImport("__Internal")]
        private static extern void CfbWithListener(ConsentFormCallback onConsentFormLoaded,
                                                   ConsentFormCallbackError onConsentFormError,
                                                   ConsentFormCallback onConsentFormOpened,
                                                   ConsentFormCallbackClosed onConsentFormClosed);
    }
}
