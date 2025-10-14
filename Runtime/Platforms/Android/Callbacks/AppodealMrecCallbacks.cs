// ReSharper disable CheckNamespace

using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.Scripting;
using AppodealStack.Monetization.Common;

namespace AppodealStack.Monetization.Platforms.Android
{
    /// <summary>
    /// Android implementation of the <see cref="AppodealStack.Monetization.Common.IMrecAdListener"/> interface.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal class AppodealMrecCallbacks : AndroidJavaProxy
    {
        private readonly IMrecAdListener _listener;

        internal AppodealMrecCallbacks(IMrecAdListener listener) : base(AndroidConstants.JavaInterfaceName.MrecCallbacks)
        {
            _listener = listener;
        }

        [Preserve]
        private void onMrecLoaded(bool isPrecache)
        {
            UnityMainThreadDispatcher.Post(_ => _listener?.OnMrecLoaded(isPrecache));
        }

        [Preserve]
        private void onMrecFailedToLoad()
        {
            UnityMainThreadDispatcher.Post(_ => _listener?.OnMrecFailedToLoad());
        }

        [Preserve]
        private void onMrecShown()
        {
            UnityMainThreadDispatcher.Post(_ => _listener?.OnMrecShown());
        }

        [Preserve]
        private void onMrecShowFailed()
        {
            UnityMainThreadDispatcher.Post(_ => _listener?.OnMrecShowFailed());
        }

        [Preserve]
        private void onMrecClicked()
        {
            UnityMainThreadDispatcher.Post(_ => _listener?.OnMrecClicked());
        }

        [Preserve]
        private void onMrecExpired()
        {
            UnityMainThreadDispatcher.Post(_ => _listener?.OnMrecExpired());
        }
    }
}
