using System.Diagnostics.CodeAnalysis;
using AppodealStack.Monetization.Common;

// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Platforms.Android
{
    /// <summary>
    /// Android implementation of <see langword="IInterstitialAdListener"/> interface.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public class AppodealInterstitialCallbacks : UnityEngine.AndroidJavaProxy
    {
        private readonly IInterstitialAdListener _listener;

        internal AppodealInterstitialCallbacks(IInterstitialAdListener listener) : base("com.appodeal.ads.InterstitialCallbacks")
        {
            _listener = listener;
        }

        private void onInterstitialLoaded(bool isPrecache)
        {
            _listener?.OnInterstitialLoaded(isPrecache);
        }

        private void onInterstitialFailedToLoad()
        {
            _listener?.OnInterstitialFailedToLoad();
        }

        private void onInterstitialShowFailed()
        {
            _listener?.OnInterstitialShowFailed();
        }

        private void onInterstitialShown()
        {
            _listener?.OnInterstitialShown();
        }

        private void onInterstitialClicked()
        {
            _listener?.OnInterstitialClicked();
        }

        private void onInterstitialClosed()
        {
            _listener?.OnInterstitialClosed();
        }

        private void onInterstitialExpired()
        {
            _listener?.OnInterstitialExpired();
        }
    }
}
