using System.Diagnostics.CodeAnalysis;
using AppodealCM.Unity.Common;

namespace AppodealCM.Unity.Platforms
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class ConsentForm
    {
        private readonly IConsentForm nativeConsentForm;

        private ConsentForm(IConsentForm builder)
        {
            nativeConsentForm = builder;
        }

        public IConsentForm GetNativeConsent()
        {
            return nativeConsentForm;
        }

        public void load()
        {
            nativeConsentForm.load();
        }

        public void showAsActivity()
        {
            nativeConsentForm.showAsActivity();
        }

        public void showAsDialog()
        {
            nativeConsentForm.showAsDialog();
        }

        public bool isLoaded()
        {
            return nativeConsentForm.isLoaded();
        }

        public bool isShowing()
        {
            return nativeConsentForm.isShowing();
        }

        public class Builder
        {
            private readonly IConsentFormBuilder nativeConsentFormBuilder;

            public Builder()
            {
                nativeConsentFormBuilder = ConsentManagerClientFactory.GetConsentFormBuilder();
            }

            public ConsentForm build()
            {
                return new ConsentForm(nativeConsentFormBuilder.build());
            }

            public Builder withListener(IConsentFormListener consentFormListener)
            {
                nativeConsentFormBuilder.withListener(consentFormListener);
                return this;
            }
        }
    }
}
