using System;

// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Common
{
    public interface IAppodealInitializationProxyListener : IAppodealInitializationListener
    {
        IAppodealInitializationListener Listener { get; set; }
        event EventHandler<SdkInitializedEventArgs> OnSdkInitialized;
    }
}
