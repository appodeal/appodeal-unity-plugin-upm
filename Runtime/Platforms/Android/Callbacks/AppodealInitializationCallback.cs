// ReSharper Disable CheckNamespace

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.Scripting;
using AppodealStack.Monetization.Common;

namespace AppodealStack.Monetization.Platforms.Android
{
    /// <summary>
    /// Android implementation of the <see langword="IAppodealInitializationListener"/> interface.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal class AppodealInitializationCallback : AndroidJavaProxy
    {
        private readonly IAppodealInitializationListener _listener;

        internal AppodealInitializationCallback(IAppodealInitializationListener listener) : base("com.appodeal.ads.initializing.ApdInitializationCallback")
        {
            _listener = listener;
        }

        [Preserve]
        private void onInitializationFinished(AndroidJavaObject errors)
        {
            if (errors == null)
            {
                UnityMainThreadDispatcher.Post(_ => _listener?.OnInitializationFinished(null));
                return;
            }

            var errorsList = new List<string>();

            int countOfErrors = errors.Call<int>("size");
            for (int i = 0; i < countOfErrors; i++)
            {
                errorsList.Add(errors.Call<AndroidJavaObject>("get", i).Call<string>("toString"));
            }

            UnityMainThreadDispatcher.Post(_ => _listener?.OnInitializationFinished(errorsList));
        }
    }
}
