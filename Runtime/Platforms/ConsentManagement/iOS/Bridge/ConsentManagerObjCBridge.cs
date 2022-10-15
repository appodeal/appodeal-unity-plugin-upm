using System;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;

// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Platforms.Ios
{
    internal delegate void ConsentInfoUpdatedCallback(IntPtr consent);
    internal delegate void ConsentInfoUpdatedFailedCallback(IntPtr error);

    [SuppressMessage("ReSharper", "NotAccessedField.Local")]
    [SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Global")]
    internal class ConsentManagerObjCBridge
    {
        private readonly IntPtr _consentManager;

        public ConsentManagerObjCBridge()
        {
            _consentManager = GetConsentManager();
        }

        public ConsentManagerObjCBridge(IntPtr intPtr)
        {
            _consentManager = intPtr;
        }

        public static void RequestConsentInfoUpdate(string appodealAppKey,
                                                    ConsentInfoUpdatedCallback onConsentInfoUpdated,
                                                    ConsentInfoUpdatedFailedCallback onFailedToUpdateConsentInfo)
        {
            CmRequestConsentInfoUpdate(appodealAppKey, onConsentInfoUpdated, onFailedToUpdateConsentInfo);
        }

        public static void DisableAppTrackingTransparencyRequest()
        {
            CmDisableAppTrackingTransparencyRequest();
        }

        public void SetCustomVendor(IntPtr customVendor)
        {
            CmSetCustomVendor(customVendor);
        }

        public IntPtr GetCustomVendor(string bundle)
        {
            return CmGetCustomVendor(bundle);
        }

        public string GetStorage()
        {
            return CmGetStorage();
        }

        public void SetStorage(string storage)
        {
            CmSetStorage(storage);
        }

        public string GetIabConsentString()
        {
            return CmGetIabConsentString();
        }

        public string ShouldShowConsentDialog()
        {
            return CmShouldShowConsentDialog();
        }

        public string GetConsentZone()
        {
            return CmGetConsentZone();
        }

        public string GetConsentStatus()
        {
            return CmGetConsentStatus();
        }

        public IntPtr GetConsent()
        {
            return CmGetConsent();
        }

        [DllImport("__Internal")]
        private static extern void CmRequestConsentInfoUpdate(string appodealAppKey,
                                                              ConsentInfoUpdatedCallback onConsentInfoUpdated,
                                                              ConsentInfoUpdatedFailedCallback onFailedToUpdateConsentInfo);

        [DllImport("__Internal")]
        private static extern void CmDisableAppTrackingTransparencyRequest();

        [DllImport("__Internal")]
        private static extern IntPtr GetConsentManager();

        [DllImport("__Internal")]
        private static extern void CmSetCustomVendor(IntPtr customVendor);

        [DllImport("__Internal")]
        private static extern IntPtr CmGetCustomVendor(string bundle);

        [DllImport("__Internal")]
        private static extern string CmGetStorage();

        [DllImport("__Internal")]
        private static extern void CmSetStorage(string storage);

        [DllImport("__Internal")]
        private static extern string CmGetIabConsentString();

        [DllImport("__Internal")]
        private static extern string CmShouldShowConsentDialog();

        [DllImport("__Internal")]
        private static extern string CmGetConsentZone();

        [DllImport("__Internal")]
        private static extern string CmGetConsentStatus();

        [DllImport("__Internal")]
        private static extern IntPtr CmGetConsent();
    }
}
