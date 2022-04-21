using System;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;

// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Platforms.Ios
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "NotAccessedField.Local")]
    internal class ConsentFormObjCBridge
    {
        private readonly IntPtr _consentForm;

        public ConsentFormObjCBridge()
        {
            _consentForm = GetConsentForm();
        }

        public ConsentFormObjCBridge(IntPtr intPtr)
        {
            _consentForm = intPtr;
        }

        public static void Load()
        {
            CfLoad();
        }

        public static void ShowAsActivity()
        {
            CfShowAsActivity();
        }

        public static void ShowAsDialog()
        {
            CfShowAsDialog();
        }

        public static bool IsLoaded()
        {
            return CfIsLoaded();
        }

        public static bool IsShowing()
        {
            return CfIsShowing();
        }

        [DllImport("__Internal")]
        private static extern IntPtr GetConsentForm();

        [DllImport("__Internal")]
        private static extern void CfLoad();

        [DllImport("__Internal")]
        private static extern void CfShowAsActivity();

        [DllImport("__Internal")]
        private static extern void CfShowAsDialog();

        [DllImport("__Internal")]
        private static extern bool CfIsLoaded();

        [DllImport("__Internal")]
        private static extern bool CfIsShowing();
    }
}
