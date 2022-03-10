using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AppodealStack.ConsentManager.Common
{
    /// <summary>
    /// <para>
    /// Interface containing method signatures of the <see langword="VendorBuilder"/> class.
    /// </para>
    /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "InvalidXmlDocComment")]
    [SuppressMessage("ReSharper", "UnusedParameter.Global")]
    public interface IVendorBuilder
    {
        IVendor build();
        void setPurposeIds(IEnumerable<int> purposeIds);
        void setFeatureIds(IEnumerable<int> featureIds);
        void setLegitimateInterestPurposeIds(IEnumerable<int> legitimateInterestPurposeIds);
    }
}
