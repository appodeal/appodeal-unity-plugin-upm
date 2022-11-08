using System;

// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Common
{
    public class ConsentFormProxyListener : IConsentFormProxyListener
    {
        public IConsentFormListener Listener { get; set; }

        public event EventHandler OnLoaded;
        public event EventHandler<ConsentManagerExceptionEventArgs> OnExceptionOccurred;
        public event EventHandler OnOpened;
        public event EventHandler<ConsentEventArgs> OnClosed;

        public void OnConsentFormLoaded()
        {
            Listener?.OnConsentFormLoaded();
            OnLoaded?.Invoke(this, EventArgs.Empty);
        }

        public void OnConsentFormError(IConsentManagerException exception)
        {
            Listener?.OnConsentFormError(exception);
            OnExceptionOccurred?.Invoke(this, new ConsentManagerExceptionEventArgs(exception));
        }

        public void OnConsentFormOpened()
        {
            Listener?.OnConsentFormOpened();
            OnOpened?.Invoke(this, EventArgs.Empty);
        }

        public void OnConsentFormClosed(IConsent consent)
        {
            Listener?.OnConsentFormClosed(consent);
            OnClosed?.Invoke(this, new ConsentEventArgs(consent));
        }
    }
}
