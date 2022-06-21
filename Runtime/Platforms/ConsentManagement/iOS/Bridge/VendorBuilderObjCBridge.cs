using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;

// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Platforms.Ios
{
    internal class VendorBuilderObjCBridge
    {
        private readonly IntPtr _vendorBuilder;

        public VendorBuilderObjCBridge(string name, string bundle, string url)
        {
            _vendorBuilder = GetVendor(name, bundle, url);
        }

        public IntPtr GetIntPtr()
        {
            return _vendorBuilder;
        }

        public static void SetPurposeIds(IEnumerable<int> purposeIds)
        {
            VbSetPurposeIds(CommaSeparatedStringFromList(purposeIds));
        }

        public static void SetFeatureIds(IEnumerable<int> featureIds)
        {
            VbSetFeatureIds(CommaSeparatedStringFromList(featureIds));
        }

        public static void SetLegitimateInterestPurposeIds(IEnumerable<int> legitimateInterestPurposeIds)
        {
            VbSetLegitimateInterestPurposeIds(CommaSeparatedStringFromList(legitimateInterestPurposeIds));
        }

        private static string CommaSeparatedStringFromList(IEnumerable<int> list)
        {
            return string.Join(",", list.Select(n => n.ToString()).ToArray());
        }

        [DllImport("__Internal")]
        private static extern IntPtr GetVendor(string name, string bundle, string url);

        [DllImport("__Internal")]
        private static extern void VbSetPurposeIds(string purposeIds);

        [DllImport("__Internal")]
        private static extern void VbSetFeatureIds(string featureIds);

        [DllImport("__Internal")]
        private static extern void VbSetLegitimateInterestPurposeIds(string purposeIds);
    }
}
