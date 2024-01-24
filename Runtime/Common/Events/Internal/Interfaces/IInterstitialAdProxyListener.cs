using System;

// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Common
{
    public interface IInterstitialAdProxyListener : IInterstitialAdListener
    {
        IInterstitialAdListener Listener { get; set; }
        event EventHandler<AdLoadedEventArgs> OnLoaded;
        event EventHandler OnFailedToLoad;
        event EventHandler OnShown;
        event EventHandler OnShowFailed;
        event EventHandler OnClosed;
        event EventHandler OnClicked;
        event EventHandler OnExpired;
    }
}
