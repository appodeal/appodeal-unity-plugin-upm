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
            return GetConsentManagerObjCBridge().GetStorage() switch
            {
                "NONE" => ConsentManagerStorage.None,
                "SHARED_PREFERENCE" => ConsentManagerStorage.SharedPreference,
                _ => ConsentManagerStorage.None
            };
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
            return GetConsentManagerObjCBridge().ShouldShowConsentDialog() switch
            {
                "UNKNOWN" => ConsentShouldShow.Unknown,
                "TRUE" => ConsentShouldShow.True,
                "FALSE" => ConsentShouldShow.False,
                _ => ConsentShouldShow.Unknown
            };
        }

        public ConsentZone GetConsentZone()
        {
            return GetConsentManagerObjCBridge().GetConsentZone() switch
            {
                "UNKNOWN" => ConsentZone.Unknown,
                "CCPA" => ConsentZone.Ccpa,
                "GDPR" => ConsentZone.Gdpr,
                "NONE" => ConsentZone.None,
                _ => ConsentZone.Unknown
            };
        }

        public ConsentStatus GetConsentStatus()
        {
            return GetConsentManagerObjCBridge().GetConsentStatus() switch
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
