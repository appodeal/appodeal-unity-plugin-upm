using System;

// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Common
{
    public class BannerAdProxyListener : IBannerAdProxyListener
    {
        public IBannerAdListener Listener { get; set; }

        public event EventHandler<BannerLoadedEventArgs> OnLoaded;
        public event EventHandler OnFailedToLoad;
        public event EventHandler OnShown;
        public event EventHandler OnShowFailed;
        public event EventHandler OnClicked;
        public event EventHandler OnExpired;

        public void OnBannerLoaded(int height, bool isPrecache)
        {
            Listener?.OnBannerLoaded(height, isPrecache);
            OnLoaded?.Invoke(this, new BannerLoadedEventArgs(height, isPrecache));
        }

        public void OnBannerFailedToLoad()
        {
            Listener?.OnBannerFailedToLoad();
            OnFailedToLoad?.Invoke(this, EventArgs.Empty);
        }

        public void OnBannerShown()
        {
            Listener?.OnBannerShown();
            OnShown?.Invoke(this, EventArgs.Empty);
        }

        public void OnBannerShowFailed()
        {
            Listener?.OnBannerShowFailed();
            OnShowFailed?.Invoke(this, EventArgs.Empty);
        }

        public void OnBannerClicked()
        {
            Listener?.OnBannerClicked();
            OnClicked?.Invoke(this, EventArgs.Empty);
        }

        public void OnBannerExpired()
        {
            Listener?.OnBannerExpired();
            OnExpired?.Invoke(this, EventArgs.Empty);
        }
    }
}
