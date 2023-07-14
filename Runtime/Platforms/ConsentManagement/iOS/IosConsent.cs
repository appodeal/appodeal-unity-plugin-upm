using System;
using System.Diagnostics.CodeAnalysis;
using AppodealStack.ConsentManagement.Common;

// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Platforms.Ios
{
    /// <summary>
    /// iOS implementation of <see langword="IConsent"/> interface.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class IosConsent : IConsent
    {
        private readonly ConsentObjCBridge _bridge;

        public IConsent NativeConsent { get; }

        public IosConsent(IntPtr intPtr)
        {
            _bridge = new ConsentObjCBridge(intPtr);
            NativeConsent = this;
        }

        public IntPtr GetIntPtr()
        {
            return _bridge.GetIntPtr();
        }

        public ConsentZone GetZone()
        {
            return ConsentObjCBridge.GetZone() switch
            {
                "UNKNOWN" => ConsentZone.Unknown,
                "NONE" => ConsentZone.None,
                "CCPA" => ConsentZone.Ccpa,
                "GDPR" => ConsentZone.Gdpr,
                _ => ConsentZone.Unknown
            };
        }

        public ConsentStatus GetStatus()
        {
            return ConsentObjCBridge.GetStatus() switch
            {
                "UNKNOWN" => ConsentStatus.Unknown,
                "PERSONALIZED" => ConsentStatus.Personalized,
                "NON_PERSONALIZED" => ConsentStatus.NonPersonalized,
                "PARTLY_PERSONALIZED" => ConsentStatus.PartlyPersonalized,
                _ => ConsentStatus.Unknown
            };
        }

        public ConsentAuthorizationStatus GetAuthorizationStatus()
        {
            return ConsentObjCBridge.GetAuthorizationStatus() switch
            {
                "NOT_DETERMINED" => ConsentAuthorizationStatus.NotDetermined,
                "DENIED" => ConsentAuthorizationStatus.Denied,
                "RESTRICTED" => ConsentAuthorizationStatus.Restricted,
                "AUTHORIZED" => ConsentAuthorizationStatus.Authorized,
                _ => ConsentAuthorizationStatus.NotDetermined
            };
        }

        public HasConsent HasConsentForVendor(string bundle)
        {
            return ConsentObjCBridge.GetStatus() switch
            {
                "UNKNOWN" => HasConsent.Unknown,
                "TRUE" => HasConsent.True,
                "FALSE" => HasConsent.False,
                _ => HasConsent.Unknown
            };
        }

        public string GetIabConsentString()
        {
            return ConsentObjCBridge.GetIabConsentString();
        }
    }
}
