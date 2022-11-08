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
            return _consentManager ?? (_consentManager = new AndroidJavaObject("com.appodeal.consent.ConsentManager"));
        }

        private AndroidJavaObject GetActivity()
        {
            return _activity ?? (_activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"));
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
            var storage = ConsentManagerStorage.None;

            switch (GetConsentManagerJavaObject().CallStatic<AndroidJavaObject>("getStorage").Call<string>("name"))
            {
                case "NONE":
                    storage = ConsentManagerStorage.None;
                    break;
                case "SHARED_PREFERENCE":
                    storage = ConsentManagerStorage.SharedPreference;
                    break;
            }

            return storage;
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
            var shouldShow = ConsentShouldShow.Unknown;

            switch (GetConsentManagerJavaObject().CallStatic<bool>("getShouldShow"))
            {
                case true:
                    shouldShow = ConsentShouldShow.True;
                    break;
                case false:
                    shouldShow = ConsentShouldShow.False;
                    break;
            }

            return shouldShow;
        }

        public ConsentZone GetConsentZone()
        {
            var zone = ConsentZone.Unknown;

            switch (GetConsentManagerJavaObject().CallStatic<AndroidJavaObject>("getConsentZone").Call<string>("name"))
            {
                case "UNKNOWN":
                    zone = ConsentZone.Unknown;
                    break;
                case "NONE":
                    zone = ConsentZone.None;
                    break;
                case "GDPR":
                    zone = ConsentZone.Gdpr;
                    break;
                case "CCPA":
                    zone = ConsentZone.Ccpa;
                    break;
            }

            return zone;
        }

        public ConsentStatus GetConsentStatus()
        {
            var status = ConsentStatus.Unknown;

            switch (GetConsentManagerJavaObject().CallStatic<AndroidJavaObject>("getConsentStatus").Call<string>("name"))
            {
                case "UNKNOWN":
                    status = ConsentStatus.Unknown;
                    break;
                case "PERSONALIZED":
                    status = ConsentStatus.Personalized;
                    break;
                case "NON_PERSONALIZED":
                    status = ConsentStatus.NonPersonalized;
                    break;
                case "PARTLY_PERSONALIZED":
                    status = ConsentStatus.PartlyPersonalized;
                    break;
            }

            return status;
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
