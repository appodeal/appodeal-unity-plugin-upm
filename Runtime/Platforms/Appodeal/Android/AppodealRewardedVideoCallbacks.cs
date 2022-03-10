using System.Diagnostics.CodeAnalysis;
using AppodealStack.Mediation.Common;

namespace AppodealStack.Mediation.Platforms.Android
{
    /// <summary>
    /// Android implementation of <see langword="IRewardedVideoAdListener"/> interface.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    public class AppodealRewardedVideoCallbacks: UnityEngine.AndroidJavaProxy
    {
        private readonly IRewardedVideoAdListener listener;

        internal AppodealRewardedVideoCallbacks(IRewardedVideoAdListener listener) : base(
            "com.appodeal.ads.RewardedVideoCallbacks")
        {
            this.listener = listener;
        }

        private void onRewardedVideoLoaded(bool isPrecache)
        {
            listener.onRewardedVideoLoaded(isPrecache);
        }

        private void onRewardedVideoFailedToLoad()
        {
            listener.onRewardedVideoFailedToLoad();
        }

        private void onRewardedVideoShowFailed()
        {
            listener.onRewardedVideoShowFailed();
        }

        private void onRewardedVideoShown()
        {
            listener.onRewardedVideoShown();
        }

        private void onRewardedVideoFinished(double amount, UnityEngine.AndroidJavaObject name)
        {
            listener.onRewardedVideoFinished(amount, null);
        }

        private void onRewardedVideoFinished(double amount, string name)
        {
            listener.onRewardedVideoFinished(amount, name);
        }

        private void onRewardedVideoClosed(bool finished)
        {
            listener.onRewardedVideoClosed(finished);
        }

        private void onRewardedVideoExpired()
        {
            listener.onRewardedVideoExpired();
        }

        private void onRewardedVideoClicked()
        {
            listener.onRewardedVideoClicked();
        }
    }
}
