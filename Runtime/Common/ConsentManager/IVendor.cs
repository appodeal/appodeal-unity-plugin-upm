using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;

namespace AppodealCM.Unity.Common
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public interface IVendor
    {
        string getName();
        string getBundle();
        string getPolicyUrl();
        List<int> getPurposeIds();
        List<int> getFeatureIds();
        List<int> getLegitimateInterestPurposeIds();
    }
}