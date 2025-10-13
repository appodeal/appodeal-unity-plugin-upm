// ReSharper disable CheckNamespace

using System;
using System.Collections.Generic;

namespace AppodealStack.Monetization.Common
{
    internal class SdkProxyListener : ISdkProxyListener
    {
        public IAppodealInitializationListener InitListener { get; set; }

        public event EventHandler<SdkInitializedEventArgs> OnInitialized;

        public void OnInitializationFinished(List<string> errors)
        {
            InitListener?.OnInitializationFinished(errors);
            OnInitialized?.Invoke(this, new SdkInitializedEventArgs(errors));
        }
    }
}
