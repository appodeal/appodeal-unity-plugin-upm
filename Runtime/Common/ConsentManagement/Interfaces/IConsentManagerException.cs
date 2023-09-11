using System.Diagnostics.CodeAnalysis;

// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Common
{
    /// <summary>
    /// <para>Provides access to information about exceptions thrown by the <see langword="Consent Manager"/>.</para>
    /// See <see href="https://docs.appodeal.com/unity/data-protection/gdpr-and-ccpa"/> for more details.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public interface IConsentManagerException
    {
        /// <summary>
        /// <para>Gets the exception reason thrown by the Consent Manager.</para>
        /// See <see href="https://docs.appodeal.com/unity/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <returns>Exception reason as string.</returns>
        string GetReason();

        /// <summary>
        /// <para>Gets the exception code thrown by the Consent Manager.</para>
        /// See <see href="https://docs.appodeal.com/unity/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <returns>Exception code as integer.</returns>
        int GetCode();
    }
}
