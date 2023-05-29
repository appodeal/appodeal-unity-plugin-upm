using System.Threading;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
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
        private static SynchronizationContext _unityContext;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void GetContext() => _unityContext = SynchronizationContext.Current;

        internal ConsentFormCallbacks(IConsentFormListener listener) : base("com.appodeal.consent.IConsentFormListener")
        {
            _listener = listener;
        }

        private void onConsentFormLoaded(AndroidJavaObject consentForm)
        {
            _unityContext?.Post(obj => _listener?.OnConsentFormLoaded(), null);
        }

        private void onConsentFormError(AndroidJavaObject exception)
        {
            _unityContext?.Post(obj => _listener?.OnConsentFormError(new AndroidConsentManagerException(exception)), null);
        }

        private void onConsentFormOpened()
        {
            _unityContext?.Post(obj => _listener?.OnConsentFormOpened(), null);
        }

        private void onConsentFormClosed(AndroidJavaObject consent)
        {
            _unityContext?.Post(obj => _listener?.OnConsentFormClosed(new AndroidConsent(consent)), null);
        }
    }
}
