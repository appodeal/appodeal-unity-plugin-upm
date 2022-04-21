using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;

// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Platforms.Ios
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    internal class VendorObjCBridge
    {
        private readonly IntPtr _vendor;

        public VendorObjCBridge()
        {
            _vendor = GetVendor();
        }

        public VendorObjCBridge(IntPtr vendorIntPtr)
        {
            _vendor = vendorIntPtr;
        }

        public IntPtr GetIntPtr()
        {
            return _vendor;
        }

        public static string GetName()
        {
            return VendorGetName();
        }

        public static string GetBundle()
        {
            return VendorGetBundle();
        }

        public static string GetPolicyUrl()
        {
            return VendorGetPolicyUrl();
        }

        public static List<int> GetPurposeIds()
        {
            return GetList(VendorGetPurposeIds());
        }

        public static List<int> GetFeatureIds()
        {
            return GetList(VendorGetFeatureIds());
        }

        public static List<int> GetLegitimateInterestPurposeIds()
        {
           return GetList(VendorGetLegitimateInterestPurposeIds());
        }

        private static List<int> GetList(string raw)
        {
            return raw.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .Select(item => Convert.ToInt32(item)).ToList();
        }

        [DllImport("__Internal")]
        private static extern IntPtr GetVendor();

        [DllImport("__Internal")]
        private static extern string VendorGetName();

        [DllImport("__Internal")]
        private static extern string VendorGetBundle();

        [DllImport("__Internal")]
        private static extern string VendorGetPolicyUrl();

        [DllImport("__Internal")]
        private static extern string VendorGetPurposeIds();

        [DllImport("__Internal")]
        private static extern string VendorGetFeatureIds();

        [DllImport("__Internal")]
        private static extern string VendorGetLegitimateInterestPurposeIds();
    }
}
