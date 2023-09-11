// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Common
{
    /// <summary>
    /// <para>
    /// Enumeration containing all possible storage types for ConsentManager.
    /// </para>
    /// See <see href="https://docs.appodeal.com/unity/data-protection/gdpr-and-ccpa"/> for more details.
    /// </summary>
    public enum ConsentManagerStorage
    {
        None,
        SharedPreference
    }

    /// <summary>
    /// <para>
    /// Enumeration containing all possible statuses of the Consent.
    /// </para>
    /// See <see href="https://docs.appodeal.com/unity/data-protection/gdpr-and-ccpa"/> for more details.
    /// </summary>
    public enum ConsentStatus
    {
        Unknown,
        NonPersonalized,
        PartlyPersonalized,
        Personalized
    }

    /// <summary>
    /// <para>
    /// Enumeration containing all possible Consent zones.
    /// </para>
    /// See <see href="https://docs.appodeal.com/unity/data-protection/gdpr-and-ccpa"/> for more details.
    /// </summary>
    public enum ConsentZone
    {
        Unknown,
        None,
        Gdpr,
        Ccpa
    }

    /// <summary>
    /// <para>
    /// Enumeration containing information on whether or not the Consent has been given.
    /// </para>
    /// See <see href="https://docs.appodeal.com/unity/data-protection/gdpr-and-ccpa"/> for more details.
    /// </summary>
    public enum HasConsent
    {
        Unknown,
        True,
        False
    }

    /// <summary>
    /// <para>
    /// Enumeration containing information on whether or not the Consent window should be shown.
    /// </para>
    /// See <see href="https://docs.appodeal.com/unity/data-protection/gdpr-and-ccpa"/> for more details.
    /// </summary>
    public enum ConsentShouldShow
    {
        Unknown,
        True,
        False
    }

    /// <summary>
    /// <para>
    /// Enumeration containing all possible states of the Consent Authorization status.
    /// </para>
    /// See <see href="https://docs.appodeal.com/unity/data-protection/gdpr-and-ccpa"/> for more details.
    /// </summary>
    public enum ConsentAuthorizationStatus
    {
        NotDetermined,
        Restricted,
        Denied,
        Authorized
    }
}
