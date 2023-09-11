using System.Diagnostics.CodeAnalysis;

// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Common
{
    /// <summary>
    /// <para>Provides access to consent-related data.</para>
    /// See <see href="https://docs.appodeal.com/unity/data-protection/gdpr-and-ccpa"/> for more details.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public interface IConsent
    {
        /// <summary>
        /// <para>Gets the ConsentZone object that contains information about whether on not the user is subject to either GDPR or CCPA.</para>
        /// See <see href="https://docs.appodeal.com/unity/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <returns>Object of type <see langword="ConsentZone"/>.</returns>
        ConsentZone GetZone();

        /// <summary>
        /// <para>Gets the ConsentStatus object that contains information about whether on not the user has granted consent on collecting personal data.</para>
        /// See <see href="https://docs.appodeal.com/unity/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <returns>Object of type <see langword="ConsentStatus"/>.</returns>
        ConsentStatus GetStatus();

        /// <summary>
        /// <para>Gets the ConsentAuthorizationStatus object that contains information about whether the user responded to consent request and, if so, what the response was.</para>
        /// See <see href="https://docs.appodeal.com/unity/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <returns>Object of type <see langword="ConsentAuthorizationStatus"/>.</returns>
        ConsentAuthorizationStatus GetAuthorizationStatus();

        /// <summary>
        /// <para>Gets the HasConsent object that contains information about whether on not the user has granted consent on collecting personal data for specific vendor.</para>
        /// See <see href="https://docs.appodeal.com/unity/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <param name="bundle">id of the vendor for which you want to check the consent status.</param>
        /// <returns>Object of type <see langword="HasConsent"/>.</returns>
        HasConsent HasConsentForVendor(string bundle);

        /// <summary>
        /// <para>Gets the consent string in IAB format.</para>
        /// See <see href="https://docs.appodeal.com/unity/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <remarks>It is available only after the consent request was show. (Synchronization is required)</remarks>
        /// <returns>IAB-formatted consent as string.</returns>
        string GetIabConsentString();

        /// <summary>
        /// Provides access to a native Consent object.
        /// </summary>
        IConsent NativeConsent { get; }
    }
}
