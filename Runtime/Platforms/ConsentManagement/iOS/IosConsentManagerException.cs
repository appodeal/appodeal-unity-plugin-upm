using System;
using System.Diagnostics.CodeAnalysis;
using AppodealStack.ConsentManagement.Common;

// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Platforms.Ios
{
    /// <summary>
    /// iOS implementation of <see langword="IConsentManagerException"/> interface.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class IosConsentManagerException : IConsentManagerException
    {
        private readonly ConsentManagerExceptionObjCBridge _bridge;

        public IosConsentManagerException()
        {
            _bridge = new ConsentManagerExceptionObjCBridge();
        }

        public IosConsentManagerException(IntPtr intPtr)
        {
            _bridge = new ConsentManagerExceptionObjCBridge(intPtr);
        }

        public string GetReason()
        {
            return _bridge.GetReason();
        }

        public int GetCode()
        {
            return _bridge.GetCode();
        }
    }
}
