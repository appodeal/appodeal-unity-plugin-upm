using System.Diagnostics.CodeAnalysis;
using AppodealStack.Monetization.Common;

// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Platforms.Android
{
    /// <summary>
    /// Android implementation of <see langword="IBannerAdListener"/> interface.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public class AppodealBannerCallbacks : UnityEngine.AndroidJavaProxy
    {
        private readonly IBannerAdListener _listener;

        internal AppodealBannerCallbacks(IBannerAdListener listener) : base("com.appodeal.ads.BannerCallbacks")
        {
            _listener = listener;
        }

        private void onBannerLoaded(int height, bool isPrecache)
        {
            _listener?.OnBannerLoaded(height, isPrecache);
        }

        private void onBannerFailedToLoad()
        {
            _listener?.OnBannerFailedToLoad();
        }

        private void onBannerShown()
        {
            _listener?.OnBannerShown();
        }

        private void onBannerShowFailed()
        {
            _listener?.OnBannerShowFailed();
        }

        private void onBannerClicked()
        {
            _listener?.OnBannerClicked();
        }

        private void onBannerExpired()
        {
            _listener?.OnBannerExpired();
        }
    }
}
