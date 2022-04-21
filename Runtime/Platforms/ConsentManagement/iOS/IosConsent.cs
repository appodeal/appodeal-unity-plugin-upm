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
            var zone = ConsentZone.Unknown;

            switch (ConsentObjCBridge.GetZone())
            {
                case "UNKNOWN":
                    zone = ConsentZone.Unknown;
                    break;
                case "NONE":
                    zone = ConsentZone.None;
                    break;
                case "CCPA":
                    zone = ConsentZone.Ccpa;
                    break;
                case "GDPR":
                    zone = ConsentZone.Gdpr;
                    break;
            }

            return zone;
        }

        public ConsentStatus GetStatus()
        {
            var status = ConsentStatus.Unknown;

            switch (ConsentObjCBridge.GetStatus())
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

        public ConsentAuthorizationStatus GetAuthorizationStatus()
        {
            var authorizationStatus = ConsentAuthorizationStatus.NotDetermined;

            switch (ConsentObjCBridge.GetAuthorizationStatus())
            {
                case "NOT_DETERMINED":
                    authorizationStatus = ConsentAuthorizationStatus.NotDetermined;
                    break;
                case "DENIED":
                    authorizationStatus = ConsentAuthorizationStatus.Denied;
                    break;
                case "RESTRICTED":
                    authorizationStatus = ConsentAuthorizationStatus.Restricted;
                    break;
                case "AUTHORIZED":
                    authorizationStatus = ConsentAuthorizationStatus.Authorized;
                    break;
            }

            return authorizationStatus;
        }

        public HasConsent HasConsentForVendor(string bundle)
        {
            var hasConsent = HasConsent.Unknown;

            switch (ConsentObjCBridge.GetStatus())
            {
                case "UNKNOWN":
                    hasConsent = HasConsent.Unknown;
                    break;
                case "TRUE":
                    hasConsent = HasConsent.True;
                    break;
                case "FALSE":
                    hasConsent = HasConsent.False;
                    break;
            }

            return hasConsent;
        }

        public string GetIabConsentString()
        {
            return ConsentObjCBridge.GetIabConsentString();
        }
    }
}
