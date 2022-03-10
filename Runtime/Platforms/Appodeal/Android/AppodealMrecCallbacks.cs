using System.Diagnostics.CodeAnalysis;
using AppodealStack.Mediation.Common;

namespace AppodealStack.Mediation.Platforms.Android
{
    /// <summary>
    /// Android implementation of <see langword="IMrecAdListener"/> interface.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    public class AppodealMrecCallbacks: UnityEngine.AndroidJavaProxy
    {
        private readonly IMrecAdListener listener;

        internal AppodealMrecCallbacks(IMrecAdListener listener) : base("com.appodeal.ads.MrecCallbacks")
        {
            this.listener = listener;
        }

        private void onMrecLoaded(bool isPrecache)
        {
            listener.onMrecLoaded(isPrecache);
        }

        private void onMrecFailedToLoad()
        {
            listener.onMrecFailedToLoad();
        }

        private void onMrecShown()
        {
            listener.onMrecShown();
        }

        private void onMrecClicked()
        {
            listener.onMrecClicked();
        }

        private void onMrecExpired()
        {
            listener.onMrecExpired();
        }
    }
}
