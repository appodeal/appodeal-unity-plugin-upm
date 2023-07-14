using UnityEngine;
using AppodealStack.ConsentManagement.Common;

// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Platforms.Android
{
    /// <summary>
    /// Android implementation of <see langword="IConsent"/> interface.
    /// </summary>
    public class AndroidConsent : IConsent
    {
        private readonly AndroidJavaObject _consent;

        public AndroidJavaObject GetConsentJavaObject()
        {
            return _consent;
        }

        public IConsent NativeConsent { get; }

        public AndroidConsent(AndroidJavaObject joConsent)
        {
            _consent = joConsent;
            NativeConsent = this;
        }

        public ConsentZone GetZone()
        {
            return GetConsentJavaObject().Call<AndroidJavaObject>("getZone").Call<string>("name") switch
            {
                "UNKNOWN" => ConsentZone.Unknown,
                "NONE" => ConsentZone.None,
                "GDPR" => ConsentZone.Gdpr,
                "CCPA" => ConsentZone.Ccpa,
                _ => ConsentZone.Unknown
            };
        }

        public ConsentStatus GetStatus()
        {
            return GetConsentJavaObject().Call<AndroidJavaObject>("getStatus").Call<string>("name") switch
            {
                "UNKNOWN" => ConsentStatus.Unknown,
                "NON_PERSONALIZED" => ConsentStatus.NonPersonalized,
                "PARTLY_PERSONALIZED" => ConsentStatus.PartlyPersonalized,
                "PERSONALIZED" => ConsentStatus.Personalized,
                _ => ConsentStatus.Unknown
            };
        }

        public ConsentAuthorizationStatus GetAuthorizationStatus()
        {
            Debug.Log("[APDUnity] [Consent] GetAuthorizationStatus() not supported on Android platform");
            return ConsentAuthorizationStatus.NotDetermined;
        }

        public HasConsent HasConsentForVendor(string bundle)
        {
            return GetConsentJavaObject().Call<bool>("hasConsentForVendor", Helper.GetJavaObject(bundle)) switch
            {
                true => HasConsent.True,
                false => HasConsent.False
            };
        }

        public string GetIabConsentString()
        {
            return GetConsentJavaObject().Call<string>("getIABConsentString");
        }
    }
}
