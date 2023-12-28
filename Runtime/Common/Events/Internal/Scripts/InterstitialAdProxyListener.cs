using System;

// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Common
{
    public class InterstitialAdProxyListener : IInterstitialAdProxyListener
    {
        public IInterstitialAdListener Listener { get; set; }

        public event EventHandler<AdLoadedEventArgs> OnLoaded;
        public event EventHandler OnFailedToLoad;
        public event EventHandler OnShown;
        public event EventHandler OnShowFailed;
        public event EventHandler OnClosed;
        public event EventHandler OnClicked;
        public event EventHandler OnExpired;

        public void OnInterstitialLoaded(bool isPrecache)
        {
            Listener?.OnInterstitialLoaded(isPrecache);
            OnLoaded?.Invoke(this, new AdLoadedEventArgs(isPrecache));
        }

        public void OnInterstitialFailedToLoad()
        {
            Listener?.OnInterstitialFailedToLoad();
            OnFailedToLoad?.Invoke(this, EventArgs.Empty);
        }

        public void OnInterstitialShown()
        {
            Listener?.OnInterstitialShown();
            OnShown?.Invoke(this, EventArgs.Empty);
        }

        public void OnInterstitialShowFailed()
        {
            Listener?.OnInterstitialShowFailed();
            OnShowFailed?.Invoke(this, EventArgs.Empty);
        }

        public void OnInterstitialClosed()
        {
            Listener?.OnInterstitialClosed();
            OnClosed?.Invoke(this, EventArgs.Empty);
        }

        public void OnInterstitialClicked()
        {
            Listener?.OnInterstitialClicked();
            OnClicked?.Invoke(this, EventArgs.Empty);
        }

        public void OnInterstitialExpired()
        {
            Listener?.OnInterstitialExpired();
            OnExpired?.Invoke(this, EventArgs.Empty);
        }
    }
}
