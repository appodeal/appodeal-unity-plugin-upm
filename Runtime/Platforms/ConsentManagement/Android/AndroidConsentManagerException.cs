using UnityEngine;
using AppodealStack.ConsentManagement.Common;

// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Platforms.Android
{
    /// <summary>
    /// Android implementation of <see langword="IConsentManagerException"/> interface.
    /// </summary>
    public class AndroidConsentManagerException : IConsentManagerException
    {
        private readonly AndroidJavaObject _consentManagerException;

        private AndroidJavaObject GetConsentManagerExceptionJavaObject()
        {
            return _consentManagerException;
        }

        public AndroidConsentManagerException(AndroidJavaObject androidJavaObject)
        {
            _consentManagerException = androidJavaObject;
        }

        public AndroidConsentManagerException()
        {
            _consentManagerException = new AndroidJavaObject("com.explorestack.consent.exception.ConsentManagerException");
        }

        public string GetReason()
        {
            return GetConsentManagerExceptionJavaObject().Call<string>("getReason");
        }

        public int GetCode()
        {
            return GetConsentManagerExceptionJavaObject().Call<int>("getCode");
        }
    }
}
