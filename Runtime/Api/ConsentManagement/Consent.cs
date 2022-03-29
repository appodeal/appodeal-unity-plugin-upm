using System;
using System.Diagnostics.CodeAnalysis;
using AppodealStack.ConsentManagement.Common;

// ReSharper disable CheckNamespace
namespace AppodealStack.ConsentManagement.Api
{
    /// <summary>
    /// <para>Provides access to consent-related data.</para>
    /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Consent : IConsent
    {
        private readonly IConsent _consent;

        private IConsent GetNativeConsent()
        {
            return _consent;
        }

        /// <summary>
        /// Provides access to a native Vendor object.
        /// </summary>
        public object NativeConsentObject { get; }

        /// <summary>
        /// Public constructor of the <see langword="Consent"/> class.
        /// </summary>
        /// <param name="consent">class which implements AppodealStack.ConsentManager.Common.IConsent interface.</param>
        public Consent(IConsent consent)
        {
            _consent = consent;
            NativeConsentObject = consent.NativeConsentObject;
        }

        /// <summary>
        /// <para>Gets the ConsentZone object that contains information about whether on not the user is subject to either GDPR or CCPA.</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <returns>Object of type <see langword="ConsentZone"/>.</returns>
        public ConsentZone GetZone()
        {
            return GetNativeConsent().GetZone();
        }

        /// <summary>
        /// <para>Gets the ConsentStatus object that contains information about whether on not the user has granted consent on collecting personal data.</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <returns>Object of type <see langword="ConsentStatus"/>.</returns>
        public ConsentStatus GetStatus()
        {
            return GetNativeConsent().GetStatus();
        }

        /// <summary>
        /// <para>Gets the ConsentAuthorizationStatus object that contains information about whether the user responded to consent request and, if so, what the response was.</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <returns>Object of type <see langword="ConsentAuthorizationStatus"/>.</returns>
        public ConsentAuthorizationStatus GetAuthorizationStatus()
        {
            return GetNativeConsent().GetAuthorizationStatus();
        }

        /// <summary>
        /// <para>Gets the HasConsent object that contains information about whether on not the user has granted consent on collecting personal data for specific vendor.</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <param name="bundle">id of the vendor for which you want to check the consent status.</param>
        /// <returns>Object of type <see langword="HasConsent"/>.</returns>
        public HasConsent HasConsentForVendor(string bundle)
        {
            return GetNativeConsent().HasConsentForVendor(bundle);
        }

        /// <summary>
        /// <para>Gets the consent string in IAB format.</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <remarks>It is available only after the consent request was show. (Synchronization is required)</remarks>
        /// <returns>IAB-formatted consent as string.</returns>
        public string GetIabConsentString()
        {
            return GetNativeConsent().GetIabConsentString();
        }

        #region Deprecated Methods

        [Obsolete("It will be removed in the next release. Use the capitalized version (GetZone) of this method instead.", false)]
        public ConsentZone getZone()
        {
            return GetNativeConsent().GetZone();
        }

        [Obsolete("It will be removed in the next release. Use the capitalized version (GetStatus) of this method instead.", false)]
        public ConsentStatus getStatus()
        {
            return GetNativeConsent().GetStatus();
        }

        [Obsolete("It will be removed in the next release. Use the capitalized version (GetAuthorizationStatus) of this method instead.", false)]
        public ConsentAuthorizationStatus getAuthorizationStatus()
        {
            return GetNativeConsent().GetAuthorizationStatus();
        }

        [Obsolete("It will be removed in the next release. Use the capitalized version (HasConsentForVendor) of this method instead.", false)]
        public HasConsent hasConsentForVendor(string bundle)
        {
            return GetNativeConsent().HasConsentForVendor(bundle);
        }

        [Obsolete("It will be removed in the next release. Use the capitalized version (GetIabConsentString) of this method instead.", false)]
        public string getIabConsentString()
        {
            return GetNativeConsent().GetIabConsentString();
        }

        #endregion

    }
}
