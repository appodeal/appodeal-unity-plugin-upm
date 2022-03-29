using System;
using System.Diagnostics.CodeAnalysis;
using AppodealStack.ConsentManagement.Common;
using AppodealStack.ConsentManagement.Platforms;

// ReSharper disable CheckNamespace
namespace AppodealStack.ConsentManagement.Api
{
    /// <summary>
    /// <para>Consent Form Unity API for developers, including documentation.</para>
    /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ConsentForm
    {
        private readonly IConsentForm _consentForm;

        private IConsentForm GetNativeConsentForm()
        {
            return _consentForm;
        }

        private ConsentForm(IConsentForm builder)
        {
            _consentForm = builder;
        }

        /// <summary>
        /// <para>Loads the consent form data from server.</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <remarks>Once loading has finished, either <see langword="OnConsentFormLoaded"/> or <see langword="OnConsentFormError"/> callback method will be triggered.</remarks>
        public void Load()
        {
            GetNativeConsentForm().Load();
        }

        /// <summary>
        /// <para>Shows the loaded consent form window as activity. (Supported only for <see langword="Android"/> platform)</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <remarks>On <see langword="iOS"/>, calling this method will result in <see cref="ShowAsDialog"/> method to be fired.</remarks>
        public void ShowAsActivity()
        {
            GetNativeConsentForm().ShowAsActivity();
        }

        /// <summary>
        /// <para>Shows the loaded consent form window as dialog.</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        public void ShowAsDialog()
        {
            GetNativeConsentForm().ShowAsDialog();
        }

        /// <summary>
        /// <para>Checks whether or not the consent form is loaded.</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <returns>True if consent form is loaded, otherwise - false.</returns>
        public bool IsLoaded()
        {
            return GetNativeConsentForm().IsLoaded();
        }

        /// <summary>
        /// <para>Checks whether or not the consent form is currently displayed on the screen.</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <returns>True if consent form window is active, otherwise - false.</returns>
        public bool IsShowing()
        {
            return GetNativeConsentForm().IsShowing();
        }

        #region Deprecated Methods

        [Obsolete("It will be removed in the next release. Use the capitalized version (Load) of this method instead.", false)]
        public void load()
        {
            GetNativeConsentForm().Load();
        }

        [Obsolete("It will be removed in the next release. Use the capitalized version (ShowAsActivity) of this method instead.", false)]
        public void showAsActivity()
        {
            GetNativeConsentForm().ShowAsActivity();
        }

        [Obsolete("It will be removed in the next release. Use the capitalized version (ShowAsDialog) of this method instead.", false)]
        public void showAsDialog()
        {
            GetNativeConsentForm().ShowAsDialog();
        }

        [Obsolete("It will be removed in the next release. Use the capitalized version (IsLoaded) of this method instead.", false)]
        public bool isLoaded()
        {
            return GetNativeConsentForm().IsLoaded();
        }

        [Obsolete("It will be removed in the next release. Use the capitalized version (IsShowing) of this method instead.", false)]
        public bool isShowing()
        {
            return GetNativeConsentForm().IsShowing();
        }

        #endregion

        /// <summary>
        /// Builder class is responsible for creating an object of the <see langword="ConsentForm"/> class.
        /// </summary>
        public class Builder
        {
            private readonly IConsentFormBuilder _consentFormBuilder;

            private IConsentFormBuilder GetNativeConsentFormBuilder()
            {
                return _consentFormBuilder;
            }

            /// <summary>
            /// Public constructor of the <see langword="Builder"/> class.
            /// </summary>
            public Builder()
            {
                _consentFormBuilder = ConsentManagerClientFactory.GetConsentFormBuilder();
            }

            /// <summary>
            /// Builds the ConsentForm object using all data you have set via the other Builder's methods.
            /// </summary>
            /// <returns>Object of type <see langword="ConsentForm"/>.</returns>
            public ConsentForm Build()
            {
                return new ConsentForm(GetNativeConsentFormBuilder().Build());
            }

            /// <summary>
            /// Sets ConsentForm callbacks listener.
            /// </summary>
            /// <param name="consentFormListener">class which implements AppodealStack.ConsentManager.Common.IConsentFormListener interface.</param>
            /// <returns>An instance of the builder class.</returns>
            public Builder WithListener(IConsentFormListener consentFormListener)
            {
                GetNativeConsentFormBuilder().WithListener(consentFormListener);
                return this;
            }

            #region Deprecated Methods

            [Obsolete("It will be removed in the next release. Use the capitalized version (Build) of this method instead.", false)]
            public ConsentForm build()
            {
                return new ConsentForm(GetNativeConsentFormBuilder().Build());
            }

            [Obsolete("It will be removed in the next release. Use the capitalized version (WithListener) of this method instead.", false)]
            public Builder withListener(IConsentFormListener consentFormListener)
            {
                GetNativeConsentFormBuilder().WithListener(consentFormListener);
                return this;
            }

            #endregion

        }
    }
}