namespace AppodealStack.ConsentManager.Common
{
    /// <summary>
    /// <para>
    /// Enumeration containing all possible storage types for ConsentManager.
    /// </para>
    /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
    /// </summary>
    public enum ConsentManagerStorage
    {
        NONE,
        SHARED_PREFERENCE
    }

    /// <summary>
    /// <para>
    /// Enumeration containing all possible statuses of the Consent.
    /// </para>
    /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
    /// </summary>
    public enum ConsentStatus
    {
        UNKNOWN,
        NON_PERSONALIZED,
        PARTLY_PERSONALIZED,
        PERSONALIZED
    }

    /// <summary>
    /// <para>
    /// Enumeration containing all possible Consent zones.
    /// </para>
    /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
    /// </summary>
    public enum ConsentZone
    {
        UNKNOWN,
        NONE,
        GDPR,
        CCPA
    }

    /// <summary>
    /// <para>
    /// Enumeration containing information on whether or not the Consent has been given.
    /// </para>
    /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
    /// </summary>
    public enum HasConsent
    {
        UNKNOWN,
        TRUE,
        FALSE
    }

    /// <summary>
    /// <para>
    /// Enumeration containing information on whether or not the Consent window should be shown.
    /// </para>
    /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
    /// </summary>
    public enum ConsentShouldShow
    {
        UNKNOWN,
        TRUE,
        FALSE
    }

    /// <summary>
    /// <para>
    /// Enumeration containing all possible states of the Consent Authorization status.
    /// </para>
    /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
    /// </summary>
    public enum ConsentAuthorizationStatus
    {
        NOT_DETERMINED,
        RESTRICTED,
        DENIED,
        AUTHORIZED
    }
}
