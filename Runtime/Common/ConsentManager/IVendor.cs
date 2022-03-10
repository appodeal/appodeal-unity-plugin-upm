using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AppodealStack.ConsentManager.Common
{
    /// <summary>
    /// <para>
    /// Interface containing method signatures of the <see langword="Vendor"/> class.
    /// </para>
    /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "InvalidXmlDocComment")]
    [SuppressMessage("ReSharper", "UnusedParameter.Global")]
    public interface IVendor
    {
        string getName();
        string getBundle();
        string getPolicyUrl();
        List<int> getPurposeIds();
        List<int> getFeatureIds();
        List<int> getLegitimateInterestPurposeIds();
        object nativeVendorObject { get; }
    }
}
