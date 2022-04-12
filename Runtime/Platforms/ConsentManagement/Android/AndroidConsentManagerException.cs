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

        public string GetReason()
        {
            return GetConsentManagerExceptionJavaObject().Call<string>("getMessage");
        }

        public int GetCode()
        {
            string reason = GetConsentManagerExceptionJavaObject().Call<string>("getEvent");
            return reason == "LoadingError" ? 2 : 1;
        }
    }
}
