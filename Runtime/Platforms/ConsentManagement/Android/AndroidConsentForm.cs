using UnityEngine;
using AppodealStack.ConsentManagement.Common;

// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Platforms.Android
{
    /// <summary>
    /// Android implementation of <see langword="IConsentForm"/> interface.
    /// </summary>
    public class AndroidConsentForm : IConsentForm
    {
        private readonly AndroidJavaObject _consentForm;

        private AndroidJavaObject GetConsentFormJavaObject()
        {
            return _consentForm;
        }

        public AndroidConsentForm(AndroidJavaObject builder)
        {
            _consentForm = builder;
        }

        public void Load()
        {
            GetConsentFormJavaObject().Call("load");
        }

        public void ShowAsActivity()
        {
            GetConsentFormJavaObject().Call("showAsActivity");
        }

        public void ShowAsDialog()
        {
            GetConsentFormJavaObject().Call("showAsDialog");
        }

        public bool IsLoaded()
        {
            return GetConsentFormJavaObject().Call<bool>("isLoaded");
        }

        public bool IsShowing()
        {
            return GetConsentFormJavaObject().Call<bool>("isShowing");
        }
    }
}
