// ReSharper disable CheckNamespace

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.Scripting;
using AppodealStack.Monetization.Common;

namespace AppodealStack.Monetization.Platforms.Android
{
    /// <summary>
    /// Android implementation of the <see cref="AppodealStack.Monetization.Common.IAppodealInitializationListener"/> interface.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal class AppodealInitializationCallback : AndroidJavaProxy
    {
        private readonly IAppodealInitializationListener _listener;

        internal AppodealInitializationCallback(IAppodealInitializationListener listener) : base(AndroidConstants.JavaInterfaceName.InitializationCallback)
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

            try
            {
                int countOfErrors = errors.Call<int>("size");
                for (int i = 0; i < countOfErrors; i++)
                {
                    errorsList.Add(errors.Call<AndroidJavaObject>("get", i).Call<string>("toString"));
                }
            }
            catch (Exception e)
            {
                AndroidAppodealHelper.LogIntegrationError(e.Message);
            }

            UnityMainThreadDispatcher.Post(_ => _listener?.OnInitializationFinished(errorsList));
        }
    }
}
