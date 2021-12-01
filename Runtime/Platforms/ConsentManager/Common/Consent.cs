using System.Diagnostics.CodeAnalysis;
using AppodealCM.Unity.Common;

namespace AppodealCM.Unity.Platforms
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class Consent : IConsent
    {
        private readonly IConsent consent;

        public Consent(IConsent consent)
        {
            this.consent = consent;
        }

        public IConsent getConsent()
        {
            return consent;
        }

        public ConsentZone getZone()
        {
            return consent.getZone();
        }

        public ConsentStatus getStatus()
        {
            return consent.getStatus();
        }

        public ConsentAuthorizationStatus getAuthorizationStatus()
        {
            return consent.getAuthorizationStatus();
        }

        public HasConsent hasConsentForVendor(string bundle)
        {
            return consent.hasConsentForVendor(bundle);
        }

        public string getIabConsentString()
        {
            return consent.getIabConsentString();
        }
    }
}