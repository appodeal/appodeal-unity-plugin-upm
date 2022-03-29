using UnityEngine;
using System.Collections.Generic;
using AppodealStack.ConsentManagement.Common;

// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Platforms.Android
{
    /// <summary>
    /// Android implementation of <see langword="IVendorBuilder"/> interface.
    /// </summary>
    public class AndroidVendorBuilder : IVendorBuilder
    {
        private readonly AndroidJavaObject _vendorBuilder;
        private AndroidJavaObject _vendor;

        private AndroidJavaObject GetVendorBuilderJavaObject()
        {
            return _vendorBuilder;
        }

        public AndroidVendorBuilder(string name, string bundle, string policyUrl)
        {
            _vendorBuilder = new AndroidJavaObject("com.explorestack.consent.Vendor$Builder", name, bundle, policyUrl);
        }

        public IVendor Build()
        {
            _vendor = GetVendorBuilderJavaObject().Call<AndroidJavaObject>("build");
            return new AndroidVendor(_vendor);
        }

        public void SetPurposeIds(IEnumerable<int> purposeIds)
        {
            SetNativeList(purposeIds, "setPurposeIds");
        }

        public void SetFeatureIds(IEnumerable<int> featureIds)
        {
            SetNativeList(featureIds, "setFeatureIds");
        }

        public void SetLegitimateInterestPurposeIds(IEnumerable<int> legitimateInterestPurposeIds)
        {
            SetNativeList(legitimateInterestPurposeIds, "setLegitimateInterestPurposeIds");
        }

        private void SetNativeList(IEnumerable<int> list, string methodName)
        {
            var joList = new AndroidJavaObject("java.util.ArrayList");
            foreach (var obj in list)
            {
                joList.Call<bool>("add", Helper.GetJavaObject(obj));
            }

            GetVendorBuilderJavaObject().Call<AndroidJavaObject>(methodName, joList);
        }
    }
}
