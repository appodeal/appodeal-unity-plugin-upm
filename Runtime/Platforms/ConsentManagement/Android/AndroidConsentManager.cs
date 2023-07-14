using UnityEngine;
using System;
using System.Diagnostics.CodeAnalysis;
using AppodealStack.ConsentManagement.Common;

// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Platforms.Android
{
    /// <summary>
    /// Android implementation of <see langword="IConsentManager"/> interface.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    public class AndroidConsentManager : IConsentManager
    {
        private AndroidJavaObject _consentManager;
        private AndroidJavaObject _activity;

        private AndroidJavaObject GetConsentManagerJavaObject()
        {
            return _consentManager ??= new AndroidJavaObject("com.appodeal.consent.ConsentManager");
        }

        private AndroidJavaObject GetActivity()
        {
            return _activity ??= new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        }

        private ConsentInfoUpdateCallbacks GetConsentInfoCallback(IConsentInfoUpdateListener listener)
        {
            ConsentManagerCallbacks.ConsentInfo.Instance.ConsentInfoEventsImpl.Listener = listener;
            return new ConsentInfoUpdateCallbacks(ConsentManagerCallbacks.ConsentInfo.Instance.ConsentInfoEventsImpl);
        }

        public void RequestConsentInfoUpdate(string appodealAppKey, IConsentInfoUpdateListener listener)
        {
            GetConsentManagerJavaObject().CallStatic("requestConsentInfoUpdate", GetActivity(), appodealAppKey, GetConsentInfoCallback(listener));
        }

        public void SetCustomVendor(IVendor customVendor)
        {
            var androidVendor = customVendor.NativeVendor as AndroidVendor;
            if (androidVendor == null) return;
            GetConsentManagerJavaObject().CallStatic<AndroidJavaObject>("getCustomVendors").Call<AndroidJavaObject>("put", androidVendor.GetBundle(), androidVendor.GetVendorJavaObject());
        }

        public IVendor GetCustomVendor(string bundle)
        {
            var vendor = GetConsentManagerJavaObject().CallStatic<AndroidJavaObject>("getCustomVendors").Call<AndroidJavaObject>("get", Helper.GetJavaObject(bundle));
            return vendor == null ? null : new AndroidVendor(vendor);
        }

        public ConsentManagerStorage GetStorage()
        {
            return GetConsentManagerJavaObject().CallStatic<AndroidJavaObject>("getStorage").Call<string>("name") switch
            {
                "NONE" => ConsentManagerStorage.None,
                "SHARED_PREFERENCE" => ConsentManagerStorage.SharedPreference,
                _ => ConsentManagerStorage.None
            };
        }

        public void SetStorage(ConsentManagerStorage iabStorage)
        {
            switch (iabStorage)
            {
                case ConsentManagerStorage.None:
                    GetConsentManagerJavaObject().CallStatic("setStorage",
                        new AndroidJavaClass("com.appodeal.consent.ConsentManager$Storage")
                            .GetStatic<AndroidJavaObject>("NONE"));
                    break;
                case ConsentManagerStorage.SharedPreference:
                    GetConsentManagerJavaObject().CallStatic("setStorage",
                        new AndroidJavaClass("com.appodeal.consent.ConsentManager$Storage")
                            .GetStatic<AndroidJavaObject>("SHARED_PREFERENCE"));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(iabStorage), iabStorage, null);
            }
        }

        public ConsentShouldShow ShouldShowConsentDialog()
        {
            return GetConsentManagerJavaObject().CallStatic<bool>("getShouldShow") switch
            {
                true => ConsentShouldShow.True,
                false => ConsentShouldShow.False
            };
        }

        public ConsentZone GetConsentZone()
        {
            return GetConsentManagerJavaObject().CallStatic<AndroidJavaObject>("getConsentZone").Call<string>("name") switch
            {
                "UNKNOWN" => ConsentZone.Unknown,
                "NONE" => ConsentZone.None,
                "GDPR" => ConsentZone.Gdpr,
                "CCPA" => ConsentZone.Ccpa,
                _ => ConsentZone.Unknown
            };
        }

        public ConsentStatus GetConsentStatus()
        {
            return GetConsentManagerJavaObject().CallStatic<AndroidJavaObject>("getConsentStatus").Call<string>("name") switch
            {
                "UNKNOWN" => ConsentStatus.Unknown,
                "PERSONALIZED" => ConsentStatus.Personalized,
                "NON_PERSONALIZED" => ConsentStatus.NonPersonalized,
                "PARTLY_PERSONALIZED" => ConsentStatus.PartlyPersonalized,
                _ => ConsentStatus.Unknown
            };
        }

        public IConsent GetConsent()
        {
            return new AndroidConsent(GetConsentManagerJavaObject().CallStatic<AndroidJavaObject>("getConsent"));
        }

        public void DisableAppTrackingTransparencyRequest()
        {
            Debug.Log("Not supported on Android platform");
        }
    }
}
