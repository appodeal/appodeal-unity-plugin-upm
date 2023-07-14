using UnityEngine;
using System.Diagnostics.CodeAnalysis;
using AppodealStack.ConsentManagement.Common;

// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Platforms.Android
{
    /// <summary>
    /// Android implementation of <see langword="IConsentForm"/> interface.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    public class AndroidConsentForm : IConsentForm
    {
        private readonly AndroidJavaObject _consentForm;

        private AndroidJavaObject _activity;

        private AndroidJavaObject GetActivity()
        {
            return _activity ??= new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        }

        private AndroidJavaObject GetConsentFormJavaObject()
        {
            return _consentForm;
        }

        private ConsentFormCallbacks GetConsentFormCallback(IConsentFormListener listener)
        {
            ConsentManagerCallbacks.ConsentForm.Instance.ConsentFormEventsImpl.Listener = listener;
            return new ConsentFormCallbacks(ConsentManagerCallbacks.ConsentForm.Instance.ConsentFormEventsImpl);
        }

        public AndroidConsentForm(AndroidJavaObject builder)
        {
            _consentForm = builder;
        }

        public AndroidConsentForm(IConsentFormListener consentFormListener)
        {
            _consentForm = new AndroidJavaObject("com.appodeal.consent.ConsentForm", GetActivity(), GetConsentFormCallback(consentFormListener));
        }

        public void Load()
        {
            GetConsentFormJavaObject().Call("load");
        }

        public void Show()
        {
            GetConsentFormJavaObject().Call("show");
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
