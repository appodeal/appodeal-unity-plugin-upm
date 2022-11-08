using System;

// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Common
{
    public class MrecAdProxyListener : IMrecAdProxyListener
    {
        public IMrecAdListener Listener { get; set; }

        public event EventHandler<AdLoadedEventArgs> OnLoaded;
        public event EventHandler OnFailedToLoad;
        public event EventHandler OnShown;
        public event EventHandler OnShowFailed;
        public event EventHandler OnClicked;
        public event EventHandler OnExpired;

        public void OnMrecLoaded(bool isPrecache)
        {
            Listener?.OnMrecLoaded(isPrecache);
            OnLoaded?.Invoke(this, new AdLoadedEventArgs(isPrecache));
        }

        public void OnMrecFailedToLoad()
        {
            Listener?.OnMrecFailedToLoad();
            OnFailedToLoad?.Invoke(this, EventArgs.Empty);
        }

        public void OnMrecShown()
        {
            Listener?.OnMrecShown();
            OnShown?.Invoke(this, EventArgs.Empty);
        }

        public void OnMrecShowFailed()
        {
            Listener?.OnMrecShowFailed();
            OnShowFailed?.Invoke(this, EventArgs.Empty);
        }

        public void OnMrecClicked()
        {
            Listener?.OnMrecClicked();
            OnClicked?.Invoke(this, EventArgs.Empty);
        }

        public void OnMrecExpired()
        {
            Listener?.OnMrecExpired();
            OnExpired?.Invoke(this, EventArgs.Empty);
        }
    }
}
