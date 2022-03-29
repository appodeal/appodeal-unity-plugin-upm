using UnityEngine;
using System.Collections.Generic;
using AppodealStack.ConsentManagement.Common;

// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Platforms.Dummy
{
    /// <summary>
    /// Unity Editor implementation of <see langword="IVendor"/> interface.
    /// </summary>
    public class DummyVendor : IVendor
    {
        public string GetName()
        {
            Debug.Log(Utils.GetDummyMessage());
            return Utils.GetDummyMessage();
        }

        public string GetBundle()
        {
            Debug.Log(Utils.GetDummyMessage());
            return Utils.GetDummyMessage();
        }

        public string GetPolicyUrl()
        {
            Debug.Log(Utils.GetDummyMessage());
            return Utils.GetDummyMessage();
        }

        public List<int> GetPurposeIds()
        {
            Debug.Log(Utils.GetDummyMessage());
            return new List<int>();
        }

        public List<int> GetFeatureIds()
        {
            Debug.Log(Utils.GetDummyMessage());
            return new List<int>();
        }

        public List<int> GetLegitimateInterestPurposeIds()
        {
            Debug.Log(Utils.GetDummyMessage());
            return new List<int>();
        }

        public object NativeVendorObject { get; } = null;
    }
}
