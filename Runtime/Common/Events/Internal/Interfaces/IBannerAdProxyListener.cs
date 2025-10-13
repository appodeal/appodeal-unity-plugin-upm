// ReSharper disable CheckNamespace

using System;

namespace AppodealStack.Monetization.Common
{
    internal interface IBannerAdProxyListener : IBannerAdListener
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
