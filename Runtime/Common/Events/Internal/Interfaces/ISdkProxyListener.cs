// ReSharper disable CheckNamespace

using System;

namespace AppodealStack.Monetization.Common
{
    internal interface ISdkProxyListener : IAppodealInitializationListener
    {
        IAppodealInitializationListener InitListener { get; set; }
        event EventHandler<SdkInitializedEventArgs> OnInitialized;
    }
}
