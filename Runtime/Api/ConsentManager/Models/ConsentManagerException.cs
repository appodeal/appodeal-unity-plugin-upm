using System.Diagnostics.CodeAnalysis;
using AppodealStack.ConsentManager.Common;
using AppodealStack.ConsentManager.Platforms;

namespace AppodealStack.ConsentManager.Api
{
    /// <summary>
    /// <para>Provides access to information about exceptions thrown by the Consent Manager.</para>
    /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "InvalidXmlDocComment")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class ConsentManagerException : IConsentManagerException
    {
        private readonly IConsentManagerException consentManagerException;

        /// <summary>
        /// Public constructor of the <see langword="ConsentManagerException"/> class.
        /// </summary>
        public ConsentManagerException()
        {
            consentManagerException = ConsentManagerClientFactory.GetConsentManagerException();
        }

        /// <summary>
        /// Public constructor of the <see langword="ConsentManagerException"/> class.
        /// </summary>
        /// <param name="consentManagerException">class which implements AppodealStack.ConsentManager.Common.IConsentManagerException interface.</param>
        public ConsentManagerException(IConsentManagerException consentManagerException)
        {
            this.consentManagerException = consentManagerException;
        }

        /// <summary>
        /// <para>Gets the exception reason thrown by the Consent Manager.</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <returns>Exception reason as string.</returns>
        public string getReason()
        {
            return consentManagerException.getReason();
        }

        /// <summary>
        /// <para>Gets the exception code thrown by the Consent Manager.</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <returns>Exception code as integer.</returns>
        public int getCode()
        {
            return consentManagerException.getCode();
        }
    }
}
