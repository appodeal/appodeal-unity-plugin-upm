using System;
using System.Collections.Generic;

// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Common
{
    public class SdkProxyListener : ISdkProxyListener
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
