using UnityEngine;
using AppodealStack.ConsentManagement.Common;

// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Platforms.Dummy
{
    /// <summary>
    /// Unity Editor implementation of <see langword="IConsentManager"/> interface.
    /// </summary>
    public class DummyConsentManager : IConsentManager
    {
        public void RequestConsentInfoUpdate(string appodealAppKey, IConsentInfoUpdateListener listener)
        {
            Debug.Log(Utils.GetDummyMessage());
        }

        public void SetCustomVendor(IVendor customVendor)
        {
            Debug.Log(Utils.GetDummyMessage());
        }

        public IVendor GetCustomVendor(string bundle)
        {
            Debug.Log(Utils.GetDummyMessage());
            return null;
        }

        public ConsentManagerStorage GetStorage()
        {
            Debug.Log(Utils.GetDummyMessage());
            return ConsentManagerStorage.None;
        }

        public void SetStorage(ConsentManagerStorage iabStorage)
        {
            Debug.Log(Utils.GetDummyMessage());
        }

        public ConsentShouldShow ShouldShowConsentDialog()
        {
            Debug.Log(Utils.GetDummyMessage());
            return ConsentShouldShow.Unknown;
        }

        public ConsentZone GetConsentZone()
        {
            Debug.Log(Utils.GetDummyMessage());
            return ConsentZone.Unknown;
        }

        public ConsentStatus GetConsentStatus()
        {
            Debug.Log(Utils.GetDummyMessage());
            return ConsentStatus.Unknown;
        }

        public IConsent GetConsent()
        {
            Debug.Log(Utils.GetDummyMessage());
            return null;
        }

        public void DisableAppTrackingTransparencyRequest()
        {
            Debug.Log(Utils.GetDummyMessage());
        }
    }
}
