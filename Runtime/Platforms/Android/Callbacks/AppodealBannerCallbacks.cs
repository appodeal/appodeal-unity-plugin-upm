// ReSharper disable CheckNamespace

using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.Scripting;
using AppodealStack.Monetization.Common;

namespace AppodealStack.Monetization.Platforms.Android
{
    /// <summary>
    /// Android implementation of the <see cref="AppodealStack.Monetization.Common.IBannerAdListener"/> interface.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal class AppodealBannerCallbacks : AndroidJavaProxy
    {
        private readonly IBannerAdListener _listener;

        internal AppodealBannerCallbacks(IBannerAdListener listener) : base(AndroidConstants.JavaInterfaceName.BannerCallbacks)
        {
            _listener = listener;
        }

        [Preserve]
        private void onBannerLoaded(int height, bool isPrecache)
        {
            UnityMainThreadDispatcher.Post(_ => _listener?.OnBannerLoaded(height, isPrecache));
        }

        [Preserve]
        private void onBannerFailedToLoad()
        {
            UnityMainThreadDispatcher.Post(_ => _listener?.OnBannerFailedToLoad());
        }

        [Preserve]
        private void onBannerShown()
        {
            UnityMainThreadDispatcher.Post(_ => _listener?.OnBannerShown());
        }

        [Preserve]
        private void onBannerShowFailed()
        {
            UnityMainThreadDispatcher.Post(_ => _listener?.OnBannerShowFailed());
        }

        [Preserve]
        private void onBannerClicked()
        {
            UnityMainThreadDispatcher.Post(_ => _listener?.OnBannerClicked());
        }

        [Preserve]
        private void onBannerExpired()
        {
            UnityMainThreadDispatcher.Post(_ => _listener?.OnBannerExpired());
        }
    }
}
