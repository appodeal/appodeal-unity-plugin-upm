using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AppodealCM.Unity.Common;

namespace AppodealCM.Unity.Platforms
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedParameter.Global")]
    public interface IConsentManager
    {
        void requestConsentInfoUpdate(string appodealAppKey, IConsentInfoUpdateListener listener);
        void setCustomVendor(Vendor customVendor);
        Vendor getCustomVendor(string bundle);
        ConsentManagerStorage getStorage();
        void setStorage(ConsentManagerStorage iabStorage);
        ConsentShouldShow shouldShowConsentDialog();
        ConsentZone getConsentZone();
        ConsentStatus getConsentStatus();
        Consent getConsent();
        void disableAppTrackingTransparencyRequest();
    }    
}