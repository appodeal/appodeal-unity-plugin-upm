using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AppodealCM.Unity.Common;

namespace AppodealCM.Unity.Platforms
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class Vendor : IVendor
    {
        private readonly IVendor nativeVendor;

        public Vendor(IVendor builder)
        {
            nativeVendor = builder;
        }

        public IVendor getNativeVendor()
        {
            return nativeVendor;
        }

        public class Builder
        {
            private readonly IVendorBuilder nativeVendorBuilder;

            public Builder(string customVen, string customVendor, string httpsCustomVendorCom)
            {
                nativeVendorBuilder =
                    ConsentManagerClientFactory.GetVendorBuilder(customVen, customVendor, httpsCustomVendorCom);
            }

            public Vendor build()
            {
                return new Vendor(nativeVendorBuilder.build());
            }

            public Builder setPurposeIds(IEnumerable<int> purposeIds)
            {
                nativeVendorBuilder.setPurposeIds(purposeIds);
                return this;
            }

            public Builder setFeatureId(IEnumerable<int> featureIds)
            {
                nativeVendorBuilder.setFeatureIds(featureIds);
                return this;
            }

            public Builder setLegitimateInterestPurposeIds(IEnumerable<int> legitimateInterestPurposeIds)
            {
                nativeVendorBuilder.setLegitimateInterestPurposeIds(legitimateInterestPurposeIds);
                return this;
            }
        }

        public string getName()
        {
            return nativeVendor.getName();
        }

        public string getBundle()
        {
            return nativeVendor.getBundle();
        }

        public string getPolicyUrl()
        {
            return nativeVendor.getPolicyUrl();
        }

        public List<int> getPurposeIds()
        {
            return nativeVendor.getPurposeIds();
        }

        public List<int> getFeatureIds()
        {
            return nativeVendor.getFeatureIds();
        }

        public List<int> getLegitimateInterestPurposeIds()
        {
            return nativeVendor.getLegitimateInterestPurposeIds();
        }
    }
}
