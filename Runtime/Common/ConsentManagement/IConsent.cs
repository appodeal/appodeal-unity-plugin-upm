// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Common
{
    /// <summary>
    /// <para>
    /// Interface containing method signatures of the <see langword="Consent"/> class.
    /// </para>
    /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
    /// </summary>
    public interface IConsent
    {
        ConsentZone GetZone();
        ConsentStatus GetStatus();
        ConsentAuthorizationStatus GetAuthorizationStatus();
        HasConsent HasConsentForVendor(string bundle);
        string GetIabConsentString();
        object NativeConsentObject { get; }
    }
}
