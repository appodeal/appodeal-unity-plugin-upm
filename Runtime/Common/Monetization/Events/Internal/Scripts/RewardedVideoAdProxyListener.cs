using System;

// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Common
{
    public class RewardedVideoAdProxyListener : IRewardedVideoAdProxyListener
    {
        public IRewardedVideoAdListener Listener { get; set; }

        public event EventHandler<AdLoadedEventArgs> OnLoaded;
        public event EventHandler OnFailedToLoad;
        public event EventHandler OnShown;
        public event EventHandler OnShowFailed;
        public event EventHandler<RewardedVideoClosedEventArgs> OnClosed;
        public event EventHandler<RewardedVideoFinishedEventArgs> OnFinished;
        public event EventHandler OnClicked;
        public event EventHandler OnExpired;

        public void OnRewardedVideoLoaded(bool isPrecache)
        {
            Listener?.OnRewardedVideoLoaded(isPrecache);
            OnLoaded?.Invoke(this, new AdLoadedEventArgs(isPrecache));
        }

        public void OnRewardedVideoFailedToLoad()
        {
            Listener?.OnRewardedVideoFailedToLoad();
            OnFailedToLoad?.Invoke(this, EventArgs.Empty);
        }

        public void OnRewardedVideoShown()
        {
            Listener?.OnRewardedVideoShown();
            OnShown?.Invoke(this, EventArgs.Empty);
        }

        public void OnRewardedVideoShowFailed()
        {
            Listener?.OnRewardedVideoShowFailed();
            OnShowFailed?.Invoke(this, EventArgs.Empty);
        }

        public void OnRewardedVideoClosed(bool finished)
        {
            Listener?.OnRewardedVideoClosed(finished);
            OnClosed?.Invoke(this, new RewardedVideoClosedEventArgs(finished));
        }

        public void OnRewardedVideoFinished(double amount, string currency)
        {
            Listener?.OnRewardedVideoFinished(amount, currency);
            OnFinished?.Invoke(this, new RewardedVideoFinishedEventArgs(amount, currency));
        }

        public void OnRewardedVideoClicked()
        {
            Listener?.OnRewardedVideoClicked();
            OnClicked?.Invoke(this, EventArgs.Empty);
        }

        public void OnRewardedVideoExpired()
        {
            Listener?.OnRewardedVideoExpired();
            OnExpired?.Invoke(this, EventArgs.Empty);
        }
    }
}
