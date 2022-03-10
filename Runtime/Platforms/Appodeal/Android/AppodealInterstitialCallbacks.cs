using System.Diagnostics.CodeAnalysis;
using AppodealStack.Mediation.Common;

namespace AppodealStack.Mediation.Platforms.Android
{
    /// <summary>
    /// Android implementation of <see langword="IInterstitialAdListener"/> interface.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    public class AppodealInterstitialCallbacks: UnityEngine.AndroidJavaProxy
    {
        private readonly IInterstitialAdListener listener;

        internal AppodealInterstitialCallbacks(IInterstitialAdListener listener) : base(
            "com.appodeal.ads.InterstitialCallbacks")
        {
            this.listener = listener;
        }

        private void onInterstitialLoaded(bool isPrecache)
        {
            listener.onInterstitialLoaded(isPrecache);
        }

        private void onInterstitialFailedToLoad()
        {
            listener.onInterstitialFailedToLoad();
        }

        private void onInterstitialShowFailed()
        {
            listener.onInterstitialShowFailed();
        }

        private void onInterstitialShown()
        {
            listener.onInterstitialShown();
        }

        private void onInterstitialClicked()
        {
            listener.onInterstitialClicked();
        }

        private void onInterstitialClosed()
        {
            listener.onInterstitialClosed();
        }

        private void onInterstitialExpired()
        {
            listener.onInterstitialExpired();
        }
    }
}
