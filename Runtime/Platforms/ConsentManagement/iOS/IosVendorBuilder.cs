using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AppodealStack.ConsentManagement.Common;

// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Platforms.Ios
{
    /// <summary>
    /// iOS implementation of <see langword="IVendorBuilder"/> interface.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    public class IosVendorBuilder : IVendorBuilder
    {
        private readonly VendorBuilderObjCBridge _bridge;

        public IosVendorBuilder(string name, string bundle, string url)
        {
            _bridge = new VendorBuilderObjCBridge(name, bundle, url);
        }

        private IntPtr GetIntPtr()
        {
            return _bridge.GetIntPtr();
        }

        public IVendor Build()
        {
            return new IosVendor(GetIntPtr());
        }

        public void SetPurposeIds(IEnumerable<int> purposeIds)
        {
            VendorBuilderObjCBridge.SetPurposeIds(IosHelper.GetEnumerable(purposeIds));
        }

        public void SetFeatureIds(IEnumerable<int> featureIds)
        {
            VendorBuilderObjCBridge.SetFeatureIds(IosHelper.GetEnumerable(featureIds));
        }

        public void SetLegitimateInterestPurposeIds(IEnumerable<int> legitimateInterestPurposeIds)
        {
            VendorBuilderObjCBridge.SetLegitimateInterestPurposeIds(IosHelper.GetEnumerable(legitimateInterestPurposeIds));
        }
    }
}
