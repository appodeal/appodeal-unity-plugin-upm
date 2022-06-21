using UnityEngine;
using System.Diagnostics.CodeAnalysis;
using AppodealStack.ConsentManagement.Common;

// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Platforms.Android
{
    /// <summary>
    /// Android implementation of <see langword="IConsentFormListener"/> interface.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    public class ConsentFormCallbacks : AndroidJavaProxy
    {
        private readonly IConsentFormListener _listener;

        internal ConsentFormCallbacks(IConsentFormListener listener) : base("com.appodeal.consent.IConsentFormListener")
        {
            _listener = listener;
        }

        private void onConsentFormLoaded(AndroidJavaObject consentForm)
        {
            _listener?.OnConsentFormLoaded();
        }

        private void onConsentFormError(AndroidJavaObject exception)
        {
            _listener?.OnConsentFormError(new AndroidConsentManagerException(exception));
        }

        private void onConsentFormOpened()
        {
            _listener?.OnConsentFormOpened();
        }

        private void onConsentFormClosed(AndroidJavaObject consent)
        {
            _listener?.OnConsentFormClosed(new AndroidConsent(consent));
        }
    }
}
