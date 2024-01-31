using System.Diagnostics.CodeAnalysis;

// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Common
{
    /// <summary>
    /// <para>
    /// Enumeration containing all possible states of Appodeal Log Level.
    /// </para>
    /// See <see href="https://docs.appodeal.com/unity/advanced/logging?distribution=upm"/> for more details.
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
    /// See <see href="https://docs.appodeal.com/unity/advanced/user-data?distribution=upm"/> for more details.
    /// </summary>
    public enum AppodealUserGender
    {
        Other,
        Male,
        Female
    }

    /// <summary>
    /// <para>
    /// Enumeration containing all supported types of In-App purchases for Google Play Store.
    /// </para>
    /// See <see href="https://docs.appodeal.com/unity/advanced/in-app-purchases"/> for more details.
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
    /// See <see href="https://docs.appodeal.com/unity/advanced/in-app-purchases"/> for more details.
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
