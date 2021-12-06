using System.Diagnostics.CodeAnalysis;
using AppodealCM.Unity.Common;
using AppodealCM.Unity.Platforms;

namespace AppodealCM.Unity.Api
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class ConsentManager
    {
        private readonly IConsentManager nativeConsentManager;

        private IConsentManager GetNativeConsentManager()
        {
            return nativeConsentManager;
        }

        private ConsentManager()
        {
            nativeConsentManager = ConsentManagerClientFactory.GetConsentManager();
        }

        public static ConsentManager getInstance()
        {
            return new ConsentManager();
        }

        public void requestConsentInfoUpdate(string appodealAppKey, IConsentInfoUpdateListener listener)
        {
            GetNativeConsentManager().requestConsentInfoUpdate(appodealAppKey, listener);
        }

        public void disableAppTrackingTransparencyRequest()
        {
            GetNativeConsentManager().disableAppTrackingTransparencyRequest();
        }

        public void setCustomVendor(Vendor customVendor)
        {
            nativeConsentManager.setCustomVendor(customVendor);
        }

        public Vendor getCustomVendor(string bundle)
        {
            return nativeConsentManager.getCustomVendor(bundle);
        }

        public ConsentManagerStorage getStorage()
        {
            return nativeConsentManager.getStorage();
        }

        public ConsentShouldShow shouldShowConsentDialog()
        {
            return nativeConsentManager.shouldShowConsentDialog();
        }

        public void setStorage(ConsentManagerStorage iabStorage)
        {
            nativeConsentManager.setStorage(iabStorage);
        }

        public ConsentZone getConsentZone()
        {
            return nativeConsentManager.getConsentZone();
        }

        public ConsentStatus getConsentStatus()
        {
            return nativeConsentManager.getConsentStatus();
        }

        public Consent getConsent()
        {
            return nativeConsentManager.getConsent();
        }
    }
}
