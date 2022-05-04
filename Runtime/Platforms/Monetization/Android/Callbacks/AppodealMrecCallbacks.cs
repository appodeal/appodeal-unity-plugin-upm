using System.Diagnostics.CodeAnalysis;
using AppodealStack.Monetization.Common;

// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Platforms.Android
{
    /// <summary>
    /// Android implementation of <see langword="IMrecAdListener"/> interface.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public class AppodealMrecCallbacks : UnityEngine.AndroidJavaProxy
    {
        private readonly IMrecAdListener _listener;

        internal AppodealMrecCallbacks(IMrecAdListener listener) : base("com.appodeal.ads.MrecCallbacks")
        {
            _listener = listener;
        }

        private void onMrecLoaded(bool isPrecache)
        {
            _listener?.OnMrecLoaded(isPrecache);
        }

        private void onMrecFailedToLoad()
        {
            _listener?.OnMrecFailedToLoad();
        }

        private void onMrecShown()
        {
            _listener?.OnMrecShown();
        }

        private void onMrecShowFailed()
        {
            _listener?.OnMrecShowFailed();
        }

        private void onMrecClicked()
        {
            _listener?.OnMrecClicked();
        }

        private void onMrecExpired()
        {
            _listener?.OnMrecExpired();
        }
    }
}
