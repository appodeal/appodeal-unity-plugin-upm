using System;
using System.Collections.Generic;
using AppodealStack.ConsentManagement.Common;

// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Platforms.Ios
{
    /// <summary>
    /// iOS implementation of <see langword="IVendor"/> interface.
    /// </summary>
    public class IosVendor : IVendor
    {
        private readonly VendorObjCBridge _bridge;

        public IVendor NativeVendor { get; }

        public IosVendor(IntPtr vendor)
        {
            _bridge = new VendorObjCBridge(vendor);
            NativeVendor = this;
        }

        public IntPtr GetIntPtr()
        {
            return _bridge.GetIntPtr();
        }

        public string GetName()
        {
            return VendorObjCBridge.GetName();
        }

        public string GetBundle()
        {
            return VendorObjCBridge.GetBundle();
        }

        public string GetPolicyUrl()
        {
            return VendorObjCBridge.GetPolicyUrl();
        }

        public List<int> GetPurposeIds()
        {
            return VendorObjCBridge.GetPurposeIds();
        }

        public List<int> GetFeatureIds()
        {
            return VendorObjCBridge.GetFeatureIds();
        }

        public List<int> GetLegitimateInterestPurposeIds()
        {
            return VendorObjCBridge.GetLegitimateInterestPurposeIds();
        }
    }
}
