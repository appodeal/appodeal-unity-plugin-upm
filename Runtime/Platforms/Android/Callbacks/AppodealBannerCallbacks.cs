using System.Threading;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using AppodealStack.Monetization.Common;

// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Platforms.Android
{
    /// <summary>
    /// Android implementation of <see langword="IBannerAdListener"/> interface.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public class AppodealBannerCallbacks : AndroidJavaProxy
    {
        private readonly IBannerAdListener _listener;
        private static SynchronizationContext _unityContext;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void GetContext() => _unityContext = SynchronizationContext.Current;

        internal AppodealBannerCallbacks(IBannerAdListener listener) : base("com.appodeal.ads.BannerCallbacks")
        {
            _listener = listener;
        }

        private void onBannerLoaded(int height, bool isPrecache)
        {
            _unityContext?.Post(obj => _listener?.OnBannerLoaded(height, isPrecache), null);
        }

        private void onBannerFailedToLoad()
        {
            _unityContext?.Post(obj => _listener?.OnBannerFailedToLoad(), null);
        }

        private void onBannerShown()
        {
            _unityContext?.Post(obj => _listener?.OnBannerShown(), null);
        }

        private void onBannerShowFailed()
        {
            _unityContext?.Post(obj => _listener?.OnBannerShowFailed(), null);
        }

        private void onBannerClicked()
        {
            _unityContext?.Post(obj => _listener?.OnBannerClicked(), null);
        }

        private void onBannerExpired()
        {
            _unityContext?.Post(obj => _listener?.OnBannerExpired(), null);
        }
    }
}
