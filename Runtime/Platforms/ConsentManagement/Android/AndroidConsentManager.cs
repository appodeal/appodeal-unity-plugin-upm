using UnityEngine;
using System;
using AppodealStack.ConsentManagement.Common;

// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Platforms.Android
{
    /// <summary>
    /// Android implementation of <see langword="IConsentManager"/> interface.
    /// </summary>
    public class AndroidConsentManager : IConsentManager
    {
        private AndroidJavaObject _consentManager;
        private AndroidJavaObject _activity;

        private AndroidJavaObject GetConsentManagerJavaObject()
        {
            if (_consentManager != null) return _consentManager;
            var consentManagerClass = new AndroidJavaClass("com.explorestack.consent.ConsentManager");
            _consentManager = consentManagerClass.CallStatic<AndroidJavaObject>("getInstance", GetActivity());

            return _consentManager;
        }

        private AndroidJavaObject GetActivity()
        {
            if (_activity != null) return _activity;
            var playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            _activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity");

            return _activity;
        }

        public void RequestConsentInfoUpdate(string appodealAppKey, IConsentInfoUpdateListener listener)
        {
            GetConsentManagerJavaObject().Call("requestConsentInfoUpdate", appodealAppKey, new ConsentInfoUpdateCallbacks(listener));
        }

        public void SetCustomVendor(IVendor customVendor)
        {
            var androidVendor = customVendor.NativeVendorObject as AndroidVendor;
            GetConsentManagerJavaObject().Call("setCustomVendor", androidVendor?.GetVendorJavaObject());
        }

        public IVendor GetCustomVendor(string bundle)
        {
            return new AndroidVendor(GetConsentManagerJavaObject().Call<AndroidJavaObject>("getCustomVendor", Helper.GetJavaObject(bundle)));
        }

        public ConsentManagerStorage GetStorage()
        {
            var storage = ConsentManagerStorage.None;

            switch (GetConsentManagerJavaObject().Call<AndroidJavaObject>("getStorage").Call<string>("name"))
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
                    GetConsentManagerJavaObject().Call("setStorage",
                        new AndroidJavaClass("com.explorestack.consent.ConsentManager$Storage")
                            .GetStatic<AndroidJavaObject>("NONE"));
                    break;
                case ConsentManagerStorage.SharedPreference:
                    GetConsentManagerJavaObject().Call("setStorage",
                        new AndroidJavaClass("com.explorestack.consent.ConsentManager$Storage")
                            .GetStatic<AndroidJavaObject>("SHARED_PREFERENCE"));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(iabStorage), iabStorage, null);
            }
        }

        public ConsentShouldShow ShouldShowConsentDialog()
        {
            var shouldShow = ConsentShouldShow.Unknown;

            switch (GetConsentManagerJavaObject().Call<AndroidJavaObject>("shouldShowConsentDialog").Call<string>("name"))
            {
                case "UNKNOWN":
                    shouldShow = ConsentShouldShow.Unknown;
                    break;
                case "TRUE":
                    shouldShow = ConsentShouldShow.True;
                    break;
                case "FALSE":
                    shouldShow = ConsentShouldShow.False;
                    break;
            }

            return shouldShow;
        }

        public ConsentZone GetConsentZone()
        {
            var zone = ConsentZone.Unknown;

            switch (GetConsentManagerJavaObject().Call<AndroidJavaObject>("getConsentZone").Call<string>("name"))
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

            switch (GetConsentManagerJavaObject().Call<AndroidJavaObject>("getConsentStatus").Call<string>("name"))
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
            return new AndroidConsent(GetConsentManagerJavaObject().Call<AndroidJavaObject>("getConsent"));
        }

        public void DisableAppTrackingTransparencyRequest()
        {
            Debug.Log("Not supported on Android platform");
        }
    }
}
