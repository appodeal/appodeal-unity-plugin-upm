using System.Diagnostics.CodeAnalysis;
using AppodealStack.Monetization.Common;

// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Platforms.Android
{
    /// <summary>
    /// Android implementation of <see langword="IRewardedVideoAdListener"/> interface.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    public class AppodealRewardedVideoCallbacks : UnityEngine.AndroidJavaProxy
    {
        private readonly IRewardedVideoAdListener _listener;

        internal AppodealRewardedVideoCallbacks(IRewardedVideoAdListener listener) : base("com.appodeal.ads.RewardedVideoCallbacks")
        {
            _listener = listener;
        }

        private void onRewardedVideoLoaded(bool isPrecache)
        {
            _listener?.OnRewardedVideoLoaded(isPrecache);
        }

        private void onRewardedVideoFailedToLoad()
        {
            _listener?.OnRewardedVideoFailedToLoad();
        }

        private void onRewardedVideoShowFailed()
        {
            _listener?.OnRewardedVideoShowFailed();
        }

        private void onRewardedVideoShown()
        {
            _listener?.OnRewardedVideoShown();
        }

        private void onRewardedVideoFinished(double amount, UnityEngine.AndroidJavaObject currency)
        {
            _listener?.OnRewardedVideoFinished(amount, null);
        }

        private void onRewardedVideoFinished(double amount, string currency)
        {
            _listener?.OnRewardedVideoFinished(amount, currency);
        }

        private void onRewardedVideoClosed(bool finished)
        {
            _listener?.OnRewardedVideoClosed(finished);
        }

        private void onRewardedVideoExpired()
        {
            _listener?.OnRewardedVideoExpired();
        }

        private void onRewardedVideoClicked()
        {
            _listener?.OnRewardedVideoClicked();
        }
    }
}
