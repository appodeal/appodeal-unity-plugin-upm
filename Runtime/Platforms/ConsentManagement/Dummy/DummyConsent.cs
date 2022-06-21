using UnityEngine;
using System.Diagnostics.CodeAnalysis;
using AppodealStack.ConsentManagement.Common;

// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Platforms.Dummy
{
    /// <summary>
    /// Unity Editor implementation of <see langword="IConsent"/> interface.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "ReplaceAutoPropertyWithComputedProperty")]
    public class DummyConsent : IConsent
    {
        public ConsentZone GetZone()
        {
            Debug.Log(Utils.GetDummyMessage());
            return ConsentZone.Unknown;
        }

        public ConsentStatus GetStatus()
        {
            Debug.Log(Utils.GetDummyMessage());
            return ConsentStatus.Unknown;
        }

        public ConsentAuthorizationStatus GetAuthorizationStatus()
        {
            Debug.Log(Utils.GetDummyMessage());
            return ConsentAuthorizationStatus.NotDetermined;
        }

        public HasConsent HasConsentForVendor(string bundle)
        {
            Debug.Log(Utils.GetDummyMessage());
            return HasConsent.Unknown;
        }

        public string GetIabConsentString()
        {
            Debug.Log(Utils.GetDummyMessage());
            return Utils.GetDummyMessage();
        }

        public IConsent NativeConsent { get; } = null;
    }
}
