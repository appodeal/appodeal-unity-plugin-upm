using System.Diagnostics.CodeAnalysis;
using AppodealStack.ConsentManager.Common;

namespace AppodealStack.ConsentManager.Api
{
    /// <summary>
    /// <para>Provides access to consent-related data.</para>
    /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "InvalidXmlDocComment")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class Consent : IConsent
    {
        private readonly IConsent consent;

        /// <summary>
        /// Public constructor of the <see langword="Consent"/> class.
        /// </summary>
        /// <param name="consent">class which implements AppodealStack.ConsentManager.Common.IConsent interface.</param>
        public Consent(IConsent consent)
        {
            this.consent = consent;
        }

        /// <summary>
        /// <para>Gets the wrapper over native (Android or iOS) <see langword="Consent"/> object.</para>
        /// </summary>
        /// <returns>Object of type that implements the <see langword="IConsent"/> interface.</returns>
        public IConsent getConsent()
        {
            return consent;
        }

        /// <summary>
        /// <para>Gets the ConsentZone object that contains information about whether on not the user is subject to either GDPR or CCPA.</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <returns>Object of type <see langword="ConsentZone"/>.</returns>
        public ConsentZone getZone()
        {
            return consent.getZone();
        }

        /// <summary>
        /// <para>Gets the ConsentStatus object that contains information about whether on not the user has granted consent on collecting personal data.</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <returns>Object of type <see langword="ConsentStatus"/>.</returns>
        public ConsentStatus getStatus()
        {
            return consent.getStatus();
        }

        /// <summary>
        /// <para>Gets the ConsentAuthorizationStatus object that contains information about whether the user responded to consent request and, if so, what the response was.</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <returns>Object of type <see langword="ConsentAuthorizationStatus"/>.</returns>
        public ConsentAuthorizationStatus getAuthorizationStatus()
        {
            return consent.getAuthorizationStatus();
        }

        /// <summary>
        /// <para>Gets the HasConsent object that contains information about whether on not the user has granted consent on collecting personal data for specific vendor.</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <param name="bundle">id of the vendor for which you want to check the consent status.</param>
        /// <returns>Object of type <see langword="HasConsent"/>.</returns>
        public HasConsent hasConsentForVendor(string bundle)
        {
            return consent.hasConsentForVendor(bundle);
        }

        /// <summary>
        /// <para>Gets the consent string in IAB format.</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <remarks>It is available only after the consent request was show. (Synchronization is required)</remarks>
        /// <returns>IAB-formated consent as string.</returns>
        public string getIabConsentString()
        {
            return consent.getIabConsentString();
        }
    }
}
