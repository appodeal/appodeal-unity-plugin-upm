using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;

namespace AppodealCM.Unity.Common
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedParameter.Global")]
    public interface IVendorBuilder
    {
        IVendor build();
        void setPurposeIds(IEnumerable<int> purposeIds);
        void setFeatureIds(IEnumerable<int> featureIds);
        void setLegitimateInterestPurposeIds(IEnumerable<int> legitimateInterestPurposeIds);
    }
}