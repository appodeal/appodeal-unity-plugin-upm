// ReSharper disable CheckNamespace

using System;

namespace AppodealStack.Monetization.Common
{
    internal interface IRewardedVideoAdProxyListener : IRewardedVideoAdListener
    {
        IRewardedVideoAdListener Listener { get; set; }
        event EventHandler<AdLoadedEventArgs> OnLoaded;
        event EventHandler OnFailedToLoad;
        event EventHandler OnShown;
        event EventHandler OnShowFailed;
        event EventHandler<RewardedVideoClosedEventArgs> OnClosed;
        event EventHandler<RewardedVideoFinishedEventArgs> OnFinished;
        event EventHandler OnClicked;
        event EventHandler OnExpired;
    }
}
