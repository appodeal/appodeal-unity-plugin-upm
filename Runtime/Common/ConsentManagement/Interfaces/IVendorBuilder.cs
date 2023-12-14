using System.Collections.Generic;

// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Common
{
    /// <summary>
    /// <para>
    /// Interface containing method signatures of the <see langword="VendorBuilder"/> class.
    /// </para>
    /// See <see href="https://docs.appodeal.com/unity/data-protection/gdpr-and-ccpa"/> for more details.
    /// </summary>
    public interface IVendorBuilder
    {
        IVendor Build();
        void SetPurposeIds(IEnumerable<int> purposeIds);
        void SetFeatureIds(IEnumerable<int> featureIds);
        void SetLegitimateInterestPurposeIds(IEnumerable<int> legitimateInterestPurposeIds);
    }
}
