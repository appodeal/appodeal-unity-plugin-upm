// ReSharper disable CheckNamespace

using System;
using System.Diagnostics.CodeAnalysis;

namespace AppodealStack.Monetization.Common
{
    /// <summary>
    /// <para>
    /// Enumeration containing all possible states of Appodeal Log Level.
    /// </para>
    /// See <see href="https://docs.appodeal.com/unity/advanced/logging?distribution=upm"/> for more details.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
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
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
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
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
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

    /// <summary>
    /// <para>
    /// Enumeration containing all supported providers of mediation debugging tools.
    /// </para>
    /// See <see href="https://docs.appodeal.com/unity/advanced/testing?distribution=upm"/> for more details.
    /// </summary>
    public enum MediationDebuggerProvider
    {
        AppLovinSdk,
    }

    [Flags]
    public enum AppodealService
    {
        Adjust = 1 << 0,
        AppsFlyer = 1 << 1,
        Facebook = 1 << 2,
        Firebase = 1 << 3,
        All = Adjust | AppsFlyer | Facebook | Firebase
    }
}
