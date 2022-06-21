using UnityEngine;
using System;
using System.Collections.Generic;
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
            var zone = ConsentZone.Unknown;

            switch (GetConsentJavaObject().Call<AndroidJavaObject>("getZone").Call<string>("name"))
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

        public ConsentStatus GetStatus()
        {
            var status = ConsentStatus.Unknown;

            switch (GetConsentJavaObject().Call<AndroidJavaObject>("getStatus").Call<string>("name"))
            {
                case "UNKNOWN":
                    status = ConsentStatus.Unknown;
                    break;
                case "NON_PERSONALIZED":
                    status = ConsentStatus.NonPersonalized;
                    break;
                case "PARTLY_PERSONALIZED":
                    status = ConsentStatus.PartlyPersonalized;
                    break;
                case "PERSONALIZED":
                    status = ConsentStatus.Personalized;
                    break;
            }

            return status;
        }

        public ConsentAuthorizationStatus GetAuthorizationStatus()
        {
            Debug.Log("[APDUnity] [Consent] GetAuthorizationStatus() not supported on Android platform");
            return ConsentAuthorizationStatus.NotDetermined;
        }

        public HasConsent HasConsentForVendor(string bundle)
        {
            var hasConsent = HasConsent.Unknown;

            switch (GetConsentJavaObject().Call<bool>("hasConsentForVendor", Helper.GetJavaObject(bundle)))
            {
                case true:
                    hasConsent = HasConsent.True;
                    break;
                case false:
                    hasConsent = HasConsent.False;
                    break;
            }

            return hasConsent;
        }

        public List<IVendor> GetAcceptedVendors()
        {
            var vendors = new List<IVendor>();

            AndroidJNI.PushLocalFrame(100);
            using (var joPurposeIdsList = GetConsentJavaObject().Call<AndroidJavaObject>("getAcceptedVendors"))
            {
                for (var i = 0; i < joPurposeIdsList.Call<int>("size"); i++)
                {
                    using (var vendor = joPurposeIdsList.Call<AndroidJavaObject>("get", i))
                    {
                        vendors.Add((new AndroidVendor(vendor)));
                    }
                }
            }
            AndroidJNI.PopLocalFrame(IntPtr.Zero);

            return vendors;
        }

        public string GetIabConsentString()
        {
            return GetConsentJavaObject().Call<string>("getIABConsentString");
        }
    }
}
