using UnityEngine;
using AppodealStack.ConsentManagement.Common;

// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Platforms.Android
{
    /// <summary>
    /// Android implementation of <see langword="IConsentFormBuilder"/> interface.
    /// </summary>
    public class AndroidConsentFormBuilder : IConsentFormBuilder
    {
        private readonly AndroidJavaObject _consentFormBuilder;
        private AndroidJavaObject _activity;
        private AndroidJavaObject _consentForm;

        private AndroidJavaObject GetConsentFormBuilderJavaObject()
        {
            return _consentFormBuilder;
        }

        public AndroidConsentFormBuilder()
        {
            _consentFormBuilder = new AndroidJavaObject("com.explorestack.consent.ConsentForm$Builder", GetActivity());
        }

        private AndroidJavaObject GetActivity()
        {
            if (_activity != null) return _activity;
            var playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            _activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity");

            return _activity;
        }

        public IConsentForm Build()
        {
            _consentForm = GetConsentFormBuilderJavaObject().Call<AndroidJavaObject>("build");
            return new AndroidConsentForm(_consentForm);
        }

        public void WithListener(IConsentFormListener consentFormListener)
        {
            GetConsentFormBuilderJavaObject().Call<AndroidJavaObject>("withListener", new ConsentFormCallbacks(consentFormListener));
        }
    }
}
