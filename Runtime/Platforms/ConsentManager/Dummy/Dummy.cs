using UnityEngine;
using System.Collections.Generic;
using AppodealCM.Unity.Common;

namespace AppodealCM.Unity.Platforms
{
    public class Dummy : IConsentManager, IConsentForm, IVendor, IVendorBuilder, IConsentFormBuilder,
        IConsentManagerException, IConsent
    {
        #region Dummy

        private const string DummyMessage = "Not supported on this platform";

        public void requestConsentInfoUpdate(string appodealAppKey, IConsentInfoUpdateListener listener)
        {
            Debug.Log(DummyMessage);
        }

        public void setCustomVendor(Vendor customVendor)
        {
            Debug.Log(DummyMessage);
        }

        public Vendor getCustomVendor(string bundle)
        {
            Debug.Log(DummyMessage);
            return null;
        }

        public ConsentManagerStorage getStorage()
        {
            Debug.Log(DummyMessage);
            return ConsentManagerStorage.NONE;
        }

        public void setStorage(ConsentManagerStorage iabStorage)
        {
            Debug.Log(DummyMessage);
        }

        public ConsentShouldShow shouldShowConsentDialog()
        {
            Debug.Log(DummyMessage);
            return ConsentShouldShow.UNKNOWN;
        }

        public ConsentZone getConsentZone()
        {
            Debug.Log(DummyMessage);
            return ConsentZone.UNKNOWN;
        }

        public ConsentStatus getConsentStatus()
        {
            Debug.Log(DummyMessage);
            return ConsentStatus.UNKNOWN;
        }

        public Consent getConsent()
        {
            Debug.Log(DummyMessage);
            return null;
        }

        public void disableAppTrackingTransparencyRequest()
        {
            Debug.Log(DummyMessage);
        }

        public void load()
        {
            Debug.Log(DummyMessage);
        }

        public void showAsActivity()
        {
            Debug.Log(DummyMessage);
        }

        public void showAsDialog()
        {
            Debug.Log(DummyMessage);
        }

        public bool isLoaded()
        {
            Debug.Log(DummyMessage);
            return false;
        }

        public bool isShowing()
        {
            Debug.Log(DummyMessage);
            return false;
        }

        public string getName()
        {
            Debug.Log(DummyMessage);
            return DummyMessage;
        }

        public string getBundle()
        {
            Debug.Log(DummyMessage);
            return DummyMessage;
        }

        public string getPolicyUrl()
        {
            Debug.Log(DummyMessage);
            return DummyMessage;
        }

        public List<int> getPurposeIds()
        {
            Debug.Log(DummyMessage);
            return new List<int>();
        }

        public List<int> getFeatureIds()
        {
            Debug.Log(DummyMessage);
            return new List<int>();
        }

        public List<int> getLegitimateInterestPurposeIds()
        {
            Debug.Log(DummyMessage);
            return new List<int>();
        }

        IVendor IVendorBuilder.build()
        {
            Debug.Log(DummyMessage);
            return null;
        }

        public void withListener(IConsentFormListener consentFormListener)
        {
            Debug.Log(DummyMessage);
        }

        public void setPurposeIds(IEnumerable<int> purposeIds)
        {
            Debug.Log(DummyMessage);
        }

        public void setFeatureIds(IEnumerable<int> featureIds)
        {
            Debug.Log(DummyMessage);
        }

        public void setLegitimateInterestPurposeIds(IEnumerable<int> legitimateInterestPurposeIds)
        {
            Debug.Log(DummyMessage);
        }

        IConsentForm IConsentFormBuilder.build()
        {
            Debug.Log(DummyMessage);
            return null;
        }

        public string getReason()
        {
            Debug.Log(DummyMessage);
            return DummyMessage;
        }

        public int getCode()
        {
            Debug.Log(DummyMessage);
            return 0;
        }

        public ConsentZone getZone()
        {
            Debug.Log(DummyMessage);
            return ConsentZone.UNKNOWN;
        }

        public ConsentStatus getStatus()
        {
            Debug.Log(DummyMessage);
            return ConsentStatus.UNKNOWN;
        }

        public ConsentAuthorizationStatus getAuthorizationStatus()
        {
            Debug.Log(DummyMessage);
            return ConsentAuthorizationStatus.NOT_DETERMINED;
        }

        public HasConsent hasConsentForVendor(string bundle)
        {
            Debug.Log(DummyMessage);
            return HasConsent.UNKNOWN;
        }

        public string getIabConsentString()
        {
            Debug.Log(DummyMessage);
            return DummyMessage;
        }

        #endregion
    }
}