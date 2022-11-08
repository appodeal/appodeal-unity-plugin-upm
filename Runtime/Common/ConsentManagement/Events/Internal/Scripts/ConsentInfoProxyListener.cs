using System;

// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Common
{
    public class ConsentInfoProxyListener : IConsentInfoProxyListener
    {
        public IConsentInfoUpdateListener Listener { get; set; }

        public event EventHandler<ConsentEventArgs> OnUpdated;
        public event EventHandler<ConsentManagerExceptionEventArgs> OnFailedToUpdate;

        public void OnConsentInfoUpdated(IConsent consent)
        {
            Listener?.OnConsentInfoUpdated(consent);
            OnUpdated?.Invoke(this, new ConsentEventArgs(consent));
        }

        public void OnFailedToUpdateConsentInfo(IConsentManagerException error)
        {
            Listener?.OnFailedToUpdateConsentInfo(error);
            OnFailedToUpdate?.Invoke(this, new ConsentManagerExceptionEventArgs(error));
        }
    }
}
