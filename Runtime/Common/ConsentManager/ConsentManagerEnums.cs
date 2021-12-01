namespace AppodealCM.Unity.Common
{
    public enum ConsentManagerStorage
    {
        NONE,
        SHARED_PREFERENCE
    }

    public enum ConsentStatus
    {
        UNKNOWN,
        NON_PERSONALIZED,
        PARTLY_PERSONALIZED,
        PERSONALIZED
    }

    public enum ConsentZone
    {
        UNKNOWN,
        NONE,
        GDPR,
        CCPA
    }

    public enum HasConsent
    {
        UNKNOWN,
        TRUE,
        FALSE
    }

    public enum ConsentShouldShow
    {
        UNKNOWN,
        TRUE,
        FALSE
    }

    public enum ConsentAuthorizationStatus
    {
        NOT_DETERMINED,
        RESTRICTED,
        DENIED,
        AUTHORIZED
    }
}