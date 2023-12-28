using System.Threading;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
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
    public class AppodealRewardedVideoCallbacks : AndroidJavaProxy
    {
        private readonly IRewardedVideoAdListener _listener;
        private static SynchronizationContext _unityContext;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void GetContext() => _unityContext = SynchronizationContext.Current;

        internal AppodealRewardedVideoCallbacks(IRewardedVideoAdListener listener) : base("com.appodeal.ads.RewardedVideoCallbacks")
        {
            _listener = listener;
        }

        private void onRewardedVideoLoaded(bool isPrecache)
        {
            _unityContext?.Post(obj => _listener?.OnRewardedVideoLoaded(isPrecache), null);
        }

        private void onRewardedVideoFailedToLoad()
        {
            _unityContext?.Post(obj => _listener?.OnRewardedVideoFailedToLoad(), null);
        }

        private void onRewardedVideoShowFailed()
        {
            _unityContext?.Post(obj => _listener?.OnRewardedVideoShowFailed(), null);
        }

        private void onRewardedVideoShown()
        {
            _unityContext?.Post(obj => _listener?.OnRewardedVideoShown(), null);
        }

        private void onRewardedVideoFinished(double amount, string currency)
        {
            _unityContext?.Post(obj => _listener?.OnRewardedVideoFinished(amount, currency), null);
        }

        private void onRewardedVideoClosed(bool finished)
        {
            _unityContext?.Post(obj => _listener?.OnRewardedVideoClosed(finished), null);
        }

        private void onRewardedVideoExpired()
        {
            _unityContext?.Post(obj => _listener?.OnRewardedVideoExpired(), null);
        }

        private void onRewardedVideoClicked()
        {
            _unityContext?.Post(obj => _listener?.OnRewardedVideoClicked(), null);
        }
    }
}
