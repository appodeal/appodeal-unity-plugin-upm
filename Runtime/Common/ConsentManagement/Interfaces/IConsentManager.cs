// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Common
{
    /// <summary>
    /// Interface containing all <see langword="ConsentManager"/> API methods' signatures.
    /// </summary>
    public interface IConsentManager
    {
        void RequestConsentInfoUpdate(string appodealAppKey, IConsentInfoUpdateListener listener);
        void SetCustomVendor(IVendor customVendor);
        IVendor GetCustomVendor(string bundle);
        ConsentManagerStorage GetStorage();
        void SetStorage(ConsentManagerStorage iabStorage);
        ConsentShouldShow ShouldShowConsentDialog();
        ConsentZone GetConsentZone();
        ConsentStatus GetConsentStatus();
        IConsent GetConsent();
        void DisableAppTrackingTransparencyRequest();
    }
}
