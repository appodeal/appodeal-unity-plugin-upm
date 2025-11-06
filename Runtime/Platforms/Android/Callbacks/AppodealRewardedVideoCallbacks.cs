// ReSharper disable CheckNamespace

using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.Scripting;
using AppodealStack.Monetization.Common;

namespace AppodealStack.Monetization.Platforms.Android
{
    /// <summary>
    /// Android implementation of the <see cref="AppodealStack.Monetization.Common.IRewardedVideoAdListener"/> interface.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal class AppodealRewardedVideoCallbacks : AndroidJavaProxy
    {
        private readonly IRewardedVideoAdListener _listener;

        internal AppodealRewardedVideoCallbacks(IRewardedVideoAdListener listener) : base(AndroidConstants.JavaInterfaceName.RewardedVideoCallbacks)
        {
            _listener = listener;
        }

        [Preserve]
        private void onRewardedVideoLoaded(bool isPrecache)
        {
            UnityMainThreadDispatcher.Post(_ => _listener?.OnRewardedVideoLoaded(isPrecache));
        }

        [Preserve]
        private void onRewardedVideoFailedToLoad()
        {
            UnityMainThreadDispatcher.Post(_ => _listener?.OnRewardedVideoFailedToLoad());
        }

        [Preserve]
        private void onRewardedVideoShowFailed()
        {
            UnityMainThreadDispatcher.Post(_ => _listener?.OnRewardedVideoShowFailed());
        }

        [Preserve]
        private void onRewardedVideoShown()
        {
            UnityMainThreadDispatcher.Post(_ => _listener?.OnRewardedVideoShown());
        }

        [Preserve]
        private void onRewardedVideoFinished(double amount, string currency)
        {
            UnityMainThreadDispatcher.Post(_ => _listener?.OnRewardedVideoFinished(amount, currency));
        }

        [Preserve]
        private void onRewardedVideoClosed(bool finished)
        {
            UnityMainThreadDispatcher.Post(_ => _listener?.OnRewardedVideoClosed(finished));
        }

        [Preserve]
        private void onRewardedVideoExpired()
        {
            UnityMainThreadDispatcher.Post(_ => _listener?.OnRewardedVideoExpired());
        }

        [Preserve]
        private void onRewardedVideoClicked()
        {
            UnityMainThreadDispatcher.Post(_ => _listener?.OnRewardedVideoClicked());
        }
    }
}
