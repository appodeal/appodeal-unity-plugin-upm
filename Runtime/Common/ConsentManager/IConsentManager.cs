using System.Diagnostics.CodeAnalysis;

namespace AppodealStack.ConsentManager.Common
{
    /// <summary>
    /// Interface containing all <see langword="ConsentManager"/> API methods' signatures.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "InvalidXmlDocComment")]
    [SuppressMessage("ReSharper", "UnusedParameter.Global")]
    public interface IConsentManager
    {
        void requestConsentInfoUpdate(string appodealAppKey, IConsentInfoUpdateListener listener);
        void setCustomVendor(IVendor customVendor);
        IVendor getCustomVendor(string bundle);
        ConsentManagerStorage getStorage();
        void setStorage(ConsentManagerStorage iabStorage);
        ConsentShouldShow shouldShowConsentDialog();
        ConsentZone getConsentZone();
        ConsentStatus getConsentStatus();
        IConsent getConsent();
        void disableAppTrackingTransparencyRequest();
    }
}
