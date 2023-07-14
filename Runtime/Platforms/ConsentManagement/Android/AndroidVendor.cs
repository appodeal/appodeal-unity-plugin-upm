using UnityEngine;
using System;
using System.Collections.Generic;
using AppodealStack.ConsentManagement.Common;

// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Platforms.Android
{
    /// <summary>
    /// Android implementation of <see langword="IVendor"/> interface.
    /// </summary>
    public class AndroidVendor : IVendor
    {
        private readonly AndroidJavaObject _vendor;

        public IVendor NativeVendor { get; }

        public AndroidVendor(AndroidJavaObject vendor)
        {
            _vendor = vendor;
            NativeVendor = this;
        }

        public AndroidJavaObject GetVendorJavaObject()
        {
            return _vendor;
        }

        public string GetName()
        {
            return GetVendorJavaObject().Call<string>("getName");
        }

        public string GetBundle()
        {
            return GetVendorJavaObject().Call<string>("getBundle");
        }

        public string GetPolicyUrl()
        {
            return GetVendorJavaObject().Call<string>("getPolicyUrl");
        }

        public List<int> GetPurposeIds()
        {
            return GetNativeList("getPurposeIds", GetVendorJavaObject());
        }

        public List<int> GetFeatureIds()
        {
            return GetNativeList("getFeatureIds", GetVendorJavaObject());
        }

        public List<int> GetLegitimateInterestPurposeIds()
        {
            return GetNativeList("getLegitimateInterestPurposeIds", GetVendorJavaObject());
        }

        private static List<int> GetNativeList(string methodName, AndroidJavaObject androidJavaObject)
        {
            var csList = new List<int>();

            AndroidJNI.PushLocalFrame(100);
            using (var joList = androidJavaObject.Call<AndroidJavaObject>(methodName))
            {
                for (var i = 0; i < joList.Call<int>("size"); i++)
                {
                    using var joElement = joList.Call<AndroidJavaObject>("get", i);
                    csList.Add(joElement.Call<int>("intValue"));
                }
            }
            AndroidJNI.PopLocalFrame(IntPtr.Zero);

            return csList;
        }
    }
}
