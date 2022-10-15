using System;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;

// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Platforms.Ios
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    internal class ConsentObjCBridge
    {
        private readonly IntPtr _consent;

        public ConsentObjCBridge(IntPtr intPtr)
        {
            _consent = intPtr;
        }

        public IntPtr GetIntPtr()
        {
            return _consent;
        }

        public static string GetZone()
        {
            return ConsentGetZone();
        }

        public static string GetStatus()
        {
            return ConsentGetStatus();
        }

        public static string GetAuthorizationStatus()
        {
            return ConsentGetAuthorizationStatus();
        }

        public static string GetIabConsentString()
        {
            return ConsentGetIabConsentString();
        }

        public string HasConsentForVendor(string bundle)
        {
            return ConsentHasConsentForVendor(bundle);
        }

        [DllImport("__Internal")]
        private static extern string ConsentGetZone();

        [DllImport("__Internal")]
        private static extern string ConsentGetStatus();

        [DllImport("__Internal")]
        private static extern string ConsentGetAuthorizationStatus();

        [DllImport("__Internal")]
        private static extern string ConsentGetIabConsentString();

        [DllImport("__Internal")]
        private static extern string ConsentHasConsentForVendor(string bundle);
    }
}
