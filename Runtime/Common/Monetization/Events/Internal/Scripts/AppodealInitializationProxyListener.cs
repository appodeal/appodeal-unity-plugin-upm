using System;
using System.Collections.Generic;

// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Common
{
    public class AppodealInitializationProxyListener : IAppodealInitializationProxyListener
    {
        public IAppodealInitializationListener Listener { get; set; }
        
        public event EventHandler<SdkInitializedEventArgs> OnSdkInitialized;
        
        public void OnInitializationFinished(List<string> errors)
        {
            Listener?.OnInitializationFinished(errors);
            OnSdkInitialized?.Invoke(this, new SdkInitializedEventArgs(errors));
        }
    }
}
