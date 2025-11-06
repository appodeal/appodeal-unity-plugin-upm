// ReSharper disable CheckNamespace

using System;

namespace AppodealStack.Monetization.Common
{
    internal interface IMrecAdProxyListener : IMrecAdListener
    {
        IMrecAdListener Listener { get; set; }
        event EventHandler<AdLoadedEventArgs> OnLoaded;
        event EventHandler OnFailedToLoad;
        event EventHandler OnShown;
        event EventHandler OnShowFailed;
        event EventHandler OnClicked;
        event EventHandler OnExpired;
    }
}
