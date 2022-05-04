using System.Diagnostics.CodeAnalysis;

// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Common
{
    /// <summary>
    /// <para>
    /// Enumeration containing all possible states of Appodeal Log Level.
    /// </para>
    /// See <see href="https://wiki.appodeal.com/en/unity/get-started/advanced/logging"/> for more details.
    /// </summary>
    public enum AppodealLogLevel
    {
        None,
        Debug,
        Verbose
    }

    /// <summary>
    /// <para>
    /// Enumeration containing genders of the end-users.
    /// </para>
    /// See <see href="https://wiki.appodeal.com/en/unity/get-started/advanced/set-user-data"/> for more details.
    /// </summary>
    public enum AppodealUserGender
    {
        Other,
        Male,
        Female
    }

    /// <summary>
    /// <para>
    /// Enumeration containing all possible statuses of the Consent for GDPR consent zone.
    /// </para>
    /// See <see href=""/> for more details.
    /// </summary>
    public enum GdprUserConsent
    {
        Unknown,
        Personalized,
        NonPersonalized
    }

    /// <summary>
    /// <para>
    /// Enumeration containing all possible statuses of the Consent for CCPA consent zone.
    /// </para>
    /// See <see href=""/> for more details.
    /// </summary>
    public enum CcpaUserConsent
    {
        Unknown,
        OptIn,
        OptOut
    }

    /// <summary>
    /// <para>
    /// Enumeration containing all supported types of In-App purchases for Google Play Store.
    /// </para>
    /// See <see href=""/> for more details.
    /// </summary>
    public enum PlayStorePurchaseType
    {
        Subs,
        InApp
    }

    /// <summary>
    /// <para>
    /// Enumeration containing all supported types of In-App purchases for Apple App Store.
    /// </para>
    /// See <see href=""/> for more details.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public enum AppStorePurchaseType
    {
        Consumable,
        NonConsumable,
        AutoRenewableSubscription,
        NonRenewingSubscription
    }
}
