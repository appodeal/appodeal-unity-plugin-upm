// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Common
{
    /// <summary>
    /// <para>
    /// Interface containing signatures of ConsentInfoUpdate callback methods.
    /// </para>
    /// See <see href="https://docs.appodeal.com/unity/data-protection/gdpr-and-ccpa"/> for more details.
    /// </summary>
    public interface IConsentInfoUpdateListener
    {
        /// <summary>
        /// <para>
        /// Raised when the status of the Consent is obtained from Appodeal server.
        /// </para>
        /// See <see href="https://docs.appodeal.com/unity/data-protection/gdpr-and-ccpa?distribution=manual#update-consent-status"/> for more details.
        /// </summary>
        /// <param name="consent">object which can then be passed to Appodeal SDK.</param>
        void OnConsentInfoUpdated(IConsent consent);

        /// <summary>
        /// <para>
        /// Raised when obtaining status of the Consent from Appodeal server fails.
        /// </para>
        /// See <see href="https://docs.appodeal.com/unity/data-protection/gdpr-and-ccpa?distribution=manual#update-consent-status"/> for more details.
        /// </summary>
        /// <param name="error">object containing information about why the error occurred.</param>
        void OnFailedToUpdateConsentInfo(IConsentManagerException error);
    }
}
