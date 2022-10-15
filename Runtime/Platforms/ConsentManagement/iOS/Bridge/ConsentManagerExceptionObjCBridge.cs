using System;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;

// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Platforms.Ios
{
    [SuppressMessage("ReSharper", "NotAccessedField.Local")]
    [SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Global")]
    internal class ConsentManagerExceptionObjCBridge
    {
        private readonly IntPtr _consentManagerException;

        public ConsentManagerExceptionObjCBridge()
        {
            _consentManagerException = GetConsentManagerException();
        }

        public ConsentManagerExceptionObjCBridge(IntPtr intPtr)
        {
            _consentManagerException = intPtr;
        }

        public string GetReason()
        {
            return CmeGetReason();
        }

        public int GetCode()
        {
            return CmeGetCode();
        }

        [DllImport("__Internal")]
        private static extern IntPtr GetConsentManagerException();

        [DllImport("__Internal")]
        private static extern string CmeGetReason();

        [DllImport("__Internal")]
        private static extern int CmeGetCode();
    }
}
