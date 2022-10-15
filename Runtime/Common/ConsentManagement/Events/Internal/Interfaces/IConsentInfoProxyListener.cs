using System;

// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Common
{
    public interface IConsentInfoProxyListener : IConsentInfoUpdateListener
    {
        IConsentInfoUpdateListener Listener { get; set; }
        event EventHandler<ConsentEventArgs> OnUpdated;
        event EventHandler<ConsentManagerExceptionEventArgs> OnFailedToUpdate;
    }
}
