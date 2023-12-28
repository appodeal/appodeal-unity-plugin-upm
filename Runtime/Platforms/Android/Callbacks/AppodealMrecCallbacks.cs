using System.Threading;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using AppodealStack.Monetization.Common;

// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Platforms.Android
{
    /// <summary>
    /// Android implementation of <see langword="IMrecAdListener"/> interface.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public class AppodealMrecCallbacks : AndroidJavaProxy
    {
        private readonly IMrecAdListener _listener;
        private static SynchronizationContext _unityContext;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void GetContext() => _unityContext = SynchronizationContext.Current;

        internal AppodealMrecCallbacks(IMrecAdListener listener) : base("com.appodeal.ads.MrecCallbacks")
        {
            _listener = listener;
        }

        private void onMrecLoaded(bool isPrecache)
        {
            _unityContext?.Post(obj => _listener?.OnMrecLoaded(isPrecache), null);
        }

        private void onMrecFailedToLoad()
        {
            _unityContext?.Post(obj => _listener?.OnMrecFailedToLoad(), null);
        }

        private void onMrecShown()
        {
            _unityContext?.Post(obj => _listener?.OnMrecShown(), null);
        }

        private void onMrecShowFailed()
        {
            _unityContext?.Post(obj => _listener?.OnMrecShowFailed(), null);
        }

        private void onMrecClicked()
        {
            _unityContext?.Post(obj => _listener?.OnMrecClicked(), null);
        }

        private void onMrecExpired()
        {
            _unityContext?.Post(obj => _listener?.OnMrecExpired(), null);
        }
    }
}
