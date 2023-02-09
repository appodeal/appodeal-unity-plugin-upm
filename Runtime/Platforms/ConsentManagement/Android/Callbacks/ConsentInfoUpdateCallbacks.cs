using System.Threading;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
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
        private static SynchronizationContext _unityContext;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void GetContext() => _unityContext = SynchronizationContext.Current;

        internal ConsentInfoUpdateCallbacks(IConsentInfoUpdateListener listener) : base("com.appodeal.consent.IConsentInfoUpdateListener")
        {
            _listener = listener;
        }

        private void onConsentInfoUpdated(AndroidJavaObject consent)
        {
            _unityContext?.Post(obj => _listener?.OnConsentInfoUpdated(new AndroidConsent(consent)), null);
        }

        private void onFailedToUpdateConsentInfo(AndroidJavaObject error)
        {
            _unityContext?.Post(obj => _listener?.OnFailedToUpdateConsentInfo(new AndroidConsentManagerException(error)), null);
        }
    }
}
