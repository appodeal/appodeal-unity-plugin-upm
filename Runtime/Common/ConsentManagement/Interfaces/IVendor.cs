using System.Collections.Generic;

// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Common
{
    /// <summary>
    /// <para>
    /// Interface containing method signatures of the <see langword="Vendor"/> class.
    /// </para>
    /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
    /// </summary>
    public interface IVendor
    {
        string GetName();
        string GetBundle();
        string GetPolicyUrl();
        List<int> GetPurposeIds();
        List<int> GetFeatureIds();
        List<int> GetLegitimateInterestPurposeIds();
        IVendor NativeVendor { get; }
    }
}
