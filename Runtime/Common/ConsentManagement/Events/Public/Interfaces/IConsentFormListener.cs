// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Common
{
    /// <summary>
    /// <para>
    /// Interface containing signatures of ConsentForm callback methods.
    /// </para>
    /// See <see href="https://docs.appodeal.com/unity/data-protection/gdpr-and-ccpa"/> for more details.
    /// </summary>
    public interface IConsentFormListener
    {
        /// <summary>
        /// <para>
        /// Raised when the Consent Form is successfully loaded.
        /// </para>
        /// See <see href="https://docs.appodeal.com/unity/data-protection/gdpr-and-ccpa?distribution=manual#handling-presentation-callbacks"/> for more details.
        /// </summary>
        void OnConsentFormLoaded();

        /// <summary>
        /// <para>
        /// Raised when loading or showing of the Consent Form fails.
        /// </para>
        /// See <see href="https://docs.appodeal.com/unity/data-protection/gdpr-and-ccpa?distribution=manual#handling-presentation-callbacks"/> for more details.
        /// </summary>
        /// <param name="consentManagerException">object containing information about why the error occurred.</param>
        void OnConsentFormError(IConsentManagerException consentManagerException);

        /// <summary>
        /// <para>
        /// Raised when the Consent Form window appears on the screen.
        /// </para>
        /// See <see href="https://docs.appodeal.com/unity/data-protection/gdpr-and-ccpa?distribution=manual#handling-presentation-callbacks"/> for more details.
        /// </summary>
        void OnConsentFormOpened();

        /// <summary>
        /// <para>
        /// Raised when the Consent Form window is closed.
        /// </para>
        /// See <see href="https://docs.appodeal.com/unity/data-protection/gdpr-and-ccpa?distribution=manual#handling-presentation-callbacks"/> for more details.
        /// </summary>
        /// <param name="consent">object which can then be passed to Appodeal SDK.</param>
        void OnConsentFormClosed(IConsent consent);
    }
}
