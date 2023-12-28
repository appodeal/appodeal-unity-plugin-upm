using System.Threading;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using AppodealStack.Monetization.Common;

// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Platforms.Android
{
    /// <summary>
    /// Android implementation of <see langword="IInterstitialAdListener"/> interface.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public class AppodealInterstitialCallbacks : AndroidJavaProxy
    {
        private readonly IInterstitialAdListener _listener;
        private static SynchronizationContext _unityContext;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void GetContext() => _unityContext = SynchronizationContext.Current;

        internal AppodealInterstitialCallbacks(IInterstitialAdListener listener) : base("com.appodeal.ads.InterstitialCallbacks")
        {
            _listener = listener;
        }

        private void onInterstitialLoaded(bool isPrecache)
        {
            _unityContext?.Post(obj => _listener?.OnInterstitialLoaded(isPrecache), null);
        }

        private void onInterstitialFailedToLoad()
        {
            _unityContext?.Post(obj => _listener?.OnInterstitialFailedToLoad(), null);
        }

        private void onInterstitialShowFailed()
        {
            _unityContext?.Post(obj => _listener?.OnInterstitialShowFailed(), null);
        }

        private void onInterstitialShown()
        {
            _unityContext?.Post(obj => _listener?.OnInterstitialShown(), null);
        }

        private void onInterstitialClicked()
        {
            _unityContext?.Post(obj => _listener?.OnInterstitialClicked(), null);
        }

        private void onInterstitialClosed()
        {
            _unityContext?.Post(obj => _listener?.OnInterstitialClosed(), null);
        }

        private void onInterstitialExpired()
        {
            _unityContext?.Post(obj => _listener?.OnInterstitialExpired(), null);
        }
    }
}
