using System.Threading;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using AppodealStack.Monetization.Common;

// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Platforms.Android
{
    /// <summary>
    /// Android implementation of <see langword="IAppodealInitializationListener"/> interface.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public class AppodealInitializationCallback : AndroidJavaProxy
    {
        private readonly IAppodealInitializationListener _listener;
        private static SynchronizationContext _unityContext;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void GetContext() => _unityContext = SynchronizationContext.Current;

        internal AppodealInitializationCallback(IAppodealInitializationListener listener) : base("com.appodeal.ads.initializing.ApdInitializationCallback")
        {
            _listener = listener;
        }

        private void onInitializationFinished(AndroidJavaObject errors)
        {
            if (errors == null)
            {
                _unityContext?.Post(obj => _listener?.OnInitializationFinished(null), null);
                return;
            }

            var errorsList = new List<string>();

            int countOfErrors = errors.Call<int>("size");
            for (int i = 0; i < countOfErrors; i++)
            {
                errorsList.Add(errors.Call<AndroidJavaObject>("get", i).Call<string>("toString"));
            }

            _unityContext?.Post(obj => _listener?.OnInitializationFinished(errorsList), null);
        }
    }
}
