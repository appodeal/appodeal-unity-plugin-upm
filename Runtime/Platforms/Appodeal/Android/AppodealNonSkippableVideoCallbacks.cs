using System.Diagnostics.CodeAnalysis;
using AppodealStack.Mediation.Common;

namespace AppodealStack.Mediation.Platforms.Android
{
    /// <summary>
    /// Android implementation of <see langword="INonSkippableVideoAdListener"/> interface.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    public class AppodealNonSkippableVideoCallbacks: UnityEngine.AndroidJavaProxy
    {
        private readonly INonSkippableVideoAdListener listener;

        internal AppodealNonSkippableVideoCallbacks(INonSkippableVideoAdListener listener) : base(
            "com.appodeal.ads.NonSkippableVideoCallbacks")
        {
            this.listener = listener;
        }

        private void onNonSkippableVideoLoaded(bool isPrecache)
        {
            listener.onNonSkippableVideoLoaded(isPrecache);
        }

        private void onNonSkippableVideoFailedToLoad()
        {
            listener.onNonSkippableVideoFailedToLoad();
        }

        private void onNonSkippableVideoShowFailed()
        {
            listener.onNonSkippableVideoShowFailed();
        }

        private void onNonSkippableVideoShown()
        {
            listener.onNonSkippableVideoShown();
        }

        private void onNonSkippableVideoFinished()
        {
            listener.onNonSkippableVideoFinished();
        }

        private void onNonSkippableVideoClosed(bool finished)
        {
            listener.onNonSkippableVideoClosed(finished);
        }

        private void onNonSkippableVideoExpired()
        {
            listener.onNonSkippableVideoExpired();
        }
    }
}
