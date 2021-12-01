using System.Diagnostics.CodeAnalysis;

namespace AppodealCM.Unity.Common
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedParameter.Global")]
    public interface IConsent
    {
        ConsentZone getZone();
        ConsentStatus getStatus();
        ConsentAuthorizationStatus getAuthorizationStatus();
        HasConsent hasConsentForVendor(string bundle);
        string getIabConsentString();
    }
}