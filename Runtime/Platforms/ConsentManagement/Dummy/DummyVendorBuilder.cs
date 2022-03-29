using UnityEngine;
using System.Collections.Generic;
using AppodealStack.ConsentManagement.Common;

// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Platforms.Dummy
{
    /// <summary>
    /// Unity Editor implementation of <see langword="IVendorBuilder"/> interface.
    /// </summary>
    public class DummyVendorBuilder : IVendorBuilder
    {
        public IVendor Build()
        {
            Debug.Log(Utils.GetDummyMessage());
            return null;
        }

        public void SetPurposeIds(IEnumerable<int> purposeIds)
        {
            Debug.Log(Utils.GetDummyMessage());
        }

        public void SetFeatureIds(IEnumerable<int> featureIds)
        {
            Debug.Log(Utils.GetDummyMessage());
        }

        public void SetLegitimateInterestPurposeIds(IEnumerable<int> legitimateInterestPurposeIds)
        {
            Debug.Log(Utils.GetDummyMessage());
        }
    }
}
