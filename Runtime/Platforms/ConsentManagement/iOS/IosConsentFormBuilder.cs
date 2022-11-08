using AOT;
using System;
using System.Diagnostics.CodeAnalysis;
using AppodealStack.ConsentManagement.Common;

// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Platforms.Ios
{
    /// <summary>
    /// Builder for the <see langword="IosConsentForm"/> class.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class IosConsentFormBuilder
    {
        private readonly ConsentFormBuilderObjCBridge _bridge;
        private static IConsentFormListener _consentFormListener;

        public IosConsentFormBuilder()
        {
            _bridge = new ConsentFormBuilderObjCBridge();
        }

        public IosConsentFormBuilder(IntPtr intPtr)
        {
            _bridge = new ConsentFormBuilderObjCBridge(intPtr);
        }

        private IntPtr GetIntPtr()
        {
            return _bridge.GetIntPtr();
        }

        public IConsentForm Build()
        {
            return new IosConsentForm(GetIntPtr());
        }

        public void WithListener(IConsentFormListener listener)
        {
            SetConsentFormListener(listener);
            ConsentFormBuilderObjCBridge.WithListener(OnConsentFormLoaded, OnConsentFormError, OnConsentFormOpened, OnConsentFormClosed);
        }

        private void SetConsentFormListener(IConsentFormListener listener)
        {
            ConsentManagerCallbacks.ConsentForm.Instance.ConsentFormEventsImpl.Listener = listener;
            _consentFormListener = ConsentManagerCallbacks.ConsentForm.Instance.ConsentFormEventsImpl;
        }

        #region ConsentForm Callbacks

        [MonoPInvokeCallback(typeof(ConsentFormCallback))]
        private static void OnConsentFormLoaded()
        {
            _consentFormListener?.OnConsentFormLoaded();
        }

        [MonoPInvokeCallback(typeof(ConsentFormCallbackError))]
        private static void OnConsentFormError(IntPtr exception)
        {
            _consentFormListener?.OnConsentFormError(new IosConsentManagerException(exception));
        }

        [MonoPInvokeCallback(typeof(ConsentFormCallback))]
        private static void OnConsentFormOpened()
        {
            _consentFormListener?.OnConsentFormOpened();
        }

        [MonoPInvokeCallback(typeof(ConsentFormCallbackClosed))]
        private static void OnConsentFormClosed(IntPtr consent)
        {
            _consentFormListener?.OnConsentFormClosed(new IosConsent(consent));
        }

        #endregion
    }
}
