using AOT;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AppodealStack.ConsentManagement.Common;

// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Platforms.Ios
{
    /// <summary>
    /// iOS implementation of <see langword="IConsentManager"/> interface.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class IosConsentManager : IConsentManager
    {
        private readonly ConsentManagerObjCBridge _bridge;
        private static IConsentInfoUpdateListener _consentInfoUpdateListener;

        private ConsentManagerObjCBridge GetConsentManagerObjCBridge()
        {
            return _bridge;
        }

        private void SetConsentInfoListener(IConsentInfoUpdateListener listener)
        {
            ConsentManagerCallbacks.ConsentInfo.Instance.ConsentInfoEventsImpl.Listener = listener;
            _consentInfoUpdateListener = ConsentManagerCallbacks.ConsentInfo.Instance.ConsentInfoEventsImpl;
        }

        public IosConsentManager()
        {
            _bridge = new ConsentManagerObjCBridge();
        }

        public IosConsentManager(IntPtr intPtr)
        {
            _bridge = new ConsentManagerObjCBridge(intPtr);
        }

        public void RequestConsentInfoUpdate(string appodealAppKey, IConsentInfoUpdateListener listener)
        {
            SetConsentInfoListener(listener);
            ConsentManagerObjCBridge.RequestConsentInfoUpdate(appodealAppKey, OnConsentInfoUpdated, OnFailedToUpdateConsentInfo);
        }

        public void DisableAppTrackingTransparencyRequest()
        {
            ConsentManagerObjCBridge.DisableAppTrackingTransparencyRequest();
        }

        public void SetCustomVendor(IVendor customVendor)
        {
            var vendor = customVendor.NativeVendor as IosVendor;
            GetConsentManagerObjCBridge().SetCustomVendor(vendor?.GetIntPtr() ?? IntPtr.Zero);
        }

        public IVendor GetCustomVendor(string bundle)
        {
            return new IosVendor(GetConsentManagerObjCBridge().GetCustomVendor(bundle));
        }

        public ConsentManagerStorage GetStorage()
        {
            var storage = ConsentManagerStorage.None;

            switch (GetConsentManagerObjCBridge().GetStorage())
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
            GetConsentManagerObjCBridge().SetStorage(iabStorage.ToString());
        }

        public string GetIabConsentString()
        {
            return GetConsentManagerObjCBridge().GetIabConsentString();
        }

        public ConsentShouldShow ShouldShowConsentDialog()
        {
            var shouldShow = ConsentShouldShow.Unknown;

            switch (GetConsentManagerObjCBridge().ShouldShowConsentDialog())
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

            switch (GetConsentManagerObjCBridge().GetConsentZone())
            {
                case "UNKNOWN":
                    zone = ConsentZone.Unknown;
                    break;
                case "CCPA":
                    zone = ConsentZone.Ccpa;
                    break;
                case "GDPR":
                    zone = ConsentZone.Gdpr;
                    break;
                case "NONE":
                    zone = ConsentZone.None;
                    break;
            }

            return zone;
        }

        public ConsentStatus GetConsentStatus()
        {
            var status = ConsentStatus.Unknown;

            switch (GetConsentManagerObjCBridge().GetConsentStatus())
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
            return new IosConsent(GetConsentManagerObjCBridge().GetConsent());
        }

        #region ConsentInfoUpdate delegate

        [MonoPInvokeCallback(typeof(ConsentInfoUpdatedCallback))]
        private static void OnConsentInfoUpdated(IntPtr consent)
        {
            _consentInfoUpdateListener?.OnConsentInfoUpdated(new IosConsent(consent));
        }

        [MonoPInvokeCallback(typeof(ConsentInfoUpdatedFailedCallback))]
        private static void OnFailedToUpdateConsentInfo(IntPtr error)
        {
            _consentInfoUpdateListener?.OnFailedToUpdateConsentInfo(new IosConsentManagerException(error));
        }

        #endregion
    }

    public static class IosHelper
    {
        public static IEnumerable<int> GetEnumerable(IEnumerable<int> enumerable)
        {
            return enumerable as int[] ?? enumerable.ToArray();
        }
    }
}
