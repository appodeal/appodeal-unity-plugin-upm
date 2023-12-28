using System;

// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Common
{
    public interface IBannerAdProxyListener : IBannerAdListener
    {
        IBannerAdListener Listener { get; set; }
        event EventHandler<BannerLoadedEventArgs> OnLoaded;
        event EventHandler OnFailedToLoad;
        event EventHandler OnShown;
        event EventHandler OnShowFailed;
        event EventHandler OnClicked;
        event EventHandler OnExpired;
    }
}
