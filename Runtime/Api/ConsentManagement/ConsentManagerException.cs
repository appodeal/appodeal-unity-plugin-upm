using System;
using System.Diagnostics.CodeAnalysis;
using AppodealStack.ConsentManagement.Common;
using AppodealStack.ConsentManagement.Platforms;

// ReSharper disable CheckNamespace
namespace AppodealStack.ConsentManagement.Api
{
    /// <summary>
    /// <para>Provides access to information about exceptions thrown by the Consent Manager.</para>
    /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ConsentManagerException : IConsentManagerException
    {
        private readonly IConsentManagerException _consentManagerException;

        private IConsentManagerException GetNativeConsentManagerException()
        {
            return _consentManagerException;
        }

        /// <summary>
        /// Public constructor of the <see langword="ConsentManagerException"/> class.
        /// </summary>
        public ConsentManagerException()
        {
            _consentManagerException = ConsentManagerClientFactory.GetConsentManagerException();
        }

        /// <summary>
        /// Public constructor of the <see langword="ConsentManagerException"/> class.
        /// </summary>
        /// <param name="consentManagerException">class which implements AppodealStack.ConsentManager.Common.IConsentManagerException interface.</param>
        public ConsentManagerException(IConsentManagerException consentManagerException)
        {
            _consentManagerException = consentManagerException;
        }

        /// <summary>
        /// <para>Gets the exception reason thrown by the Consent Manager.</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <returns>Exception reason as string.</returns>
        public string GetReason()
        {
            return GetNativeConsentManagerException().GetReason();
        }

        /// <summary>
        /// <para>Gets the exception code thrown by the Consent Manager.</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <returns>Exception code as integer.</returns>
        public int GetCode()
        {
            return GetNativeConsentManagerException().GetCode();
        }

        #region Deprecated Methods

        [Obsolete("It will be removed in the next release. Use the capitalized version (GetReason) of this method instead.", false)]
        public string getReason()
        {
            return GetNativeConsentManagerException().GetReason();
        }

        [Obsolete("It will be removed in the next release. Use the capitalized version (GetCode) of this method instead.", false)]
        public int getCode()
        {
            return GetNativeConsentManagerException().GetCode();
        }

        #endregion

    }
}
