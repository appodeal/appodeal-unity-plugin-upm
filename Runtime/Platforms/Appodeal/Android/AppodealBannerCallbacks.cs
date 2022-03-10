using System.Diagnostics.CodeAnalysis;
using AppodealStack.Mediation.Common;

namespace AppodealStack.Mediation.Platforms.Android
{
    /// <summary>
    /// Android implementation of <see langword="IBannerAdListener"/> interface.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    public class AppodealBannerCallbacks: UnityEngine.AndroidJavaProxy
    {
        private readonly IBannerAdListener listener;

        internal AppodealBannerCallbacks(IBannerAdListener listener) : base("com.appodeal.ads.BannerCallbacks")
        {
            this.listener = listener;
        }

        private void onBannerLoaded(int height, bool isPrecache)
        {
            listener.onBannerLoaded(height, isPrecache);
        }

        private void onBannerFailedToLoad()
        {
            listener.onBannerFailedToLoad();
        }

        private void onBannerShown()
        {
            listener.onBannerShown();
        }

        private void onBannerClicked()
        {
            listener.onBannerClicked();
        }

        private void onBannerExpired()
        {
            listener.onBannerExpired();
        }
    }
}
