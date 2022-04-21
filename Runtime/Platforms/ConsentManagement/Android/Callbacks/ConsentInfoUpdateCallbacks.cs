using UnityEngine;
using System.Diagnostics.CodeAnalysis;
using AppodealStack.ConsentManagement.Common;

// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Platforms.Android
{
    /// <summary>
    /// Android implementation of <see langword="IConsentInfoUpdateListener"/> interface.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public class ConsentInfoUpdateCallbacks : AndroidJavaProxy
    {
        private readonly IConsentInfoUpdateListener _listener;

        internal ConsentInfoUpdateCallbacks(IConsentInfoUpdateListener listener) : base("com.appodeal.consent.IConsentInfoUpdateListener")
        {
            _listener = listener;
        }

        private void onConsentInfoUpdated(AndroidJavaObject consent)
        {
            _listener?.OnConsentInfoUpdated(new AndroidConsent(consent));
        }

        private void onFailedToUpdateConsentInfo(AndroidJavaObject error)
        {
            _listener?.OnFailedToUpdateConsentInfo(new AndroidConsentManagerException(error));
        }
    }
}
