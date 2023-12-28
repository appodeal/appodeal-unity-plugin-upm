using System;

// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Common
{
    public interface ISdkProxyListener : IAppodealInitializationListener
    {
        IAppodealInitializationListener InitListener { get; set; }
        event EventHandler<SdkInitializedEventArgs> OnInitialized;
    }
}
