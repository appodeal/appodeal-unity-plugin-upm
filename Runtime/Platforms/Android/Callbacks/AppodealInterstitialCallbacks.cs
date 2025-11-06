// ReSharper disable CheckNamespace

using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.Scripting;
using AppodealStack.Monetization.Common;

namespace AppodealStack.Monetization.Platforms.Android
{
    /// <summary>
    /// Android implementation of the <see cref="AppodealStack.Monetization.Common.IInterstitialAdListener"/> interface.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal class AppodealInterstitialCallbacks : AndroidJavaProxy
    {
        private readonly IInterstitialAdListener _listener;

        internal AppodealInterstitialCallbacks(IInterstitialAdListener listener) : base(AndroidConstants.JavaInterfaceName.InterstitialCallbacks)
        {
            _listener = listener;
        }

        [Preserve]
        private void onInterstitialLoaded(bool isPrecache)
        {
            UnityMainThreadDispatcher.Post(_ => _listener?.OnInterstitialLoaded(isPrecache));
        }

        [Preserve]
        private void onInterstitialFailedToLoad()
        {
            UnityMainThreadDispatcher.Post(_ => _listener?.OnInterstitialFailedToLoad());
        }

        [Preserve]
        private void onInterstitialShowFailed()
        {
            UnityMainThreadDispatcher.Post(_ => _listener?.OnInterstitialShowFailed());
        }

        [Preserve]
        private void onInterstitialShown()
        {
            UnityMainThreadDispatcher.Post(_ => _listener?.OnInterstitialShown());
        }

        [Preserve]
        private void onInterstitialClicked()
        {
            UnityMainThreadDispatcher.Post(_ => _listener?.OnInterstitialClicked());
        }

        [Preserve]
        private void onInterstitialClosed()
        {
            UnityMainThreadDispatcher.Post(_ => _listener?.OnInterstitialClosed());
        }

        [Preserve]
        private void onInterstitialExpired()
        {
            UnityMainThreadDispatcher.Post(_ => _listener?.OnInterstitialExpired());
        }
    }
}
