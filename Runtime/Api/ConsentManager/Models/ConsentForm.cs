using System.Diagnostics.CodeAnalysis;
using AppodealStack.ConsentManager.Common;
using AppodealStack.ConsentManager.Platforms;

namespace AppodealStack.ConsentManager.Api
{
    /// <summary>
    /// <para>Consent Form Unity API for developers, including documentation.</para>
    /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "InvalidXmlDocComment")]
    public class ConsentForm
    {
        private readonly IConsentForm nativeConsentForm;

        private ConsentForm(IConsentForm builder)
        {
            nativeConsentForm = builder;
        }

        /// <summary>
        /// <para>Gets the wrapper over native (Android or iOS) <see langword="ConsentForm"/> object.</para>
        /// </summary>
        /// <returns>Object of type that implements the <see langword="IConsentForm"/> interface.</returns>
        public IConsentForm GetNativeConsent()
        {
            return nativeConsentForm;
        }

        /// <summary>
        /// <para>Loads the consent form data from server.</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <remarks>Once loading has finished, either <see langword="onConsentFormLoaded"/> or <see langword="onConsentFormError"/> callback method will be triggered.</remarks>
        public void load()
        {
            nativeConsentForm.load();
        }

        /// <summary>
        /// <para>Shows the loaded consent form window as activity. (Supported only for <see langword="Android"/> platform)</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <remarks>On <see langword="iOS"/>, calling this method will result in <see cref="showAsDialog"/> method to be fired.</remarks>
        public void showAsActivity()
        {
            nativeConsentForm.showAsActivity();
        }

        /// <summary>
        /// <para>Shows the loaded consent form window as dialog.</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        public void showAsDialog()
        {
            nativeConsentForm.showAsDialog();
        }

        /// <summary>
        /// <para>Checks whether or not the consent form is loaded.</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <returns>True if consent form is loaded, otherwise - false.</returns>
        public bool isLoaded()
        {
            return nativeConsentForm.isLoaded();
        }

        /// <summary>
        /// <para>Checks whether or not the consent form is currently displayed on the screen.</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <returns>True if consent form window is active, otherwise - false.</returns>
        public bool isShowing()
        {
            return nativeConsentForm.isShowing();
        }

        /// <summary>
        /// Builder class is responsible for creating an object of the <see langword="ConsentForm"/> class.
        /// </summary>
        public class Builder
        {
            private readonly IConsentFormBuilder nativeConsentFormBuilder;

            /// <summary>
            /// Public constructor of the <see langword="Builder"/> class.
            /// </summary>
            public Builder()
            {
                nativeConsentFormBuilder = ConsentManagerClientFactory.GetConsentFormBuilder();
            }

            /// <summary>
            /// Builds the ConsentForm object using all data you have set via the other Builder's methods.
            /// </summary>
            /// <returns>Object of type <see langword="ConsentForm"/>.</returns>
            public ConsentForm build()
            {
                return new ConsentForm(nativeConsentFormBuilder.build());
            }

            /// <summary>
            /// Sets ConsentForm callbacks listener.
            /// </summary>
            /// <param name="consentFormListener">class which implements AppodealStack.ConsentManager.Common.IConsentFormListener interface.</param>
            /// <returns>An instance of the builder class.</returns>
            public Builder withListener(IConsentFormListener consentFormListener)
            {
                nativeConsentFormBuilder.withListener(consentFormListener);
                return this;
            }
        }
    }
}
