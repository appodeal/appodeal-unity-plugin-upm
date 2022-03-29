using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AppodealStack.ConsentManagement.Common;
using AppodealStack.ConsentManagement.Platforms;

// ReSharper disable CheckNamespace
namespace AppodealStack.ConsentManagement.Api
{
    /// <summary>
    /// <para>Vendor Unity API for developers, including documentation.</para>
    /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Vendor : IVendor
    {
        private readonly IVendor _vendor;

        private IVendor GetNativeVendor()
        {
            return _vendor;
        }

        /// <summary>
        /// Provides access to a native Vendor object.
        /// </summary>
        public object NativeVendorObject { get; }

        /// <summary>
        /// Public constructor of the <see langword="Vendor"/> class.
        /// </summary>
        /// <param name="builder">class which implements AppodealStack.ConsentManager.Common.IVendor interface.</param>
        public Vendor(IVendor builder)
        {
            _vendor = builder;
            NativeVendorObject = builder.NativeVendorObject;
        }

        /// <summary>
        /// <para>Gets the name of the vendor.</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <returns>Name of the vendor as string.</returns>
        public string GetName()
        {
            return GetNativeVendor().GetName();
        }

        /// <summary>
        /// <para>Gets the bundle identifier of the vendor.</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <returns>Bundle id as string.</returns>
        public string GetBundle()
        {
            return GetNativeVendor().GetBundle();
        }

        /// <summary>
        /// <para>Gets the policy URL of the vendor.</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <returns>Policy URL as string.</returns>
        public string GetPolicyUrl()
        {
            return GetNativeVendor().GetPolicyUrl();
        }

        /// <summary>
        /// <para>Gets the IAB purpose ids.</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <returns>IAB purpose ids as a list of integers.</returns>
        public List<int> GetPurposeIds()
        {
            return GetNativeVendor().GetPurposeIds();
        }

        /// <summary>
        /// <para>Gets the IAB features ids.</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <returns>IAB feature ids as a list of integers.</returns>
        public List<int> GetFeatureIds()
        {
            return GetNativeVendor().GetFeatureIds();
        }

        /// <summary>
        /// <para>Gets the IAB legitimate interest purpose ids.</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <returns>IAB legitimate interest purpose ids as a list of integers.</returns>
        public List<int> GetLegitimateInterestPurposeIds()
        {
            return GetNativeVendor().GetLegitimateInterestPurposeIds();
        }

        #region Deprecated Methods

        [Obsolete("It will be removed in the next release. Use the capitalized version (GetName) of this method instead.", false)]
        public string getName()
        {
            return GetNativeVendor().GetName();
        }

        [Obsolete("It will be removed in the next release. Use the capitalized version (GetBundle) of this method instead.", false)]
        public string getBundle()
        {
            return GetNativeVendor().GetBundle();
        }

        [Obsolete("It will be removed in the next release. Use the capitalized version (GetPolicyUrl) of this method instead.", false)]
        public string getPolicyUrl()
        {
            return GetNativeVendor().GetPolicyUrl();
        }

        [Obsolete("It will be removed in the next release. Use the capitalized version (GetPurposeIds) of this method instead.", false)]
        public List<int> getPurposeIds()
        {
            return GetNativeVendor().GetPurposeIds();
        }

        [Obsolete("It will be removed in the next release. Use the capitalized version (GetFeatureIds) of this method instead.", false)]
        public List<int> getFeatureIds()
        {
            return GetNativeVendor().GetFeatureIds();
        }

        [Obsolete("It will be removed in the next release. Use the capitalized version (GetLegitimateInterestPurposeIds) of this method instead.", false)]
        public List<int> getLegitimateInterestPurposeIds()
        {
            return GetNativeVendor().GetLegitimateInterestPurposeIds();
        }

        #endregion

        /// <summary>
        /// Builder class is responsible for creating an object of the <see langword="Vendor"/> class.
        /// </summary>
        public class Builder
        {
            private readonly IVendorBuilder _vendorBuilder;

            private IVendorBuilder GetNativeVendorBuilder()
            {
                return _vendorBuilder;
            }

            /// <summary>
            /// Public constructor of the <see langword="Builder"/> class.
            /// </summary>
            /// <param name="vendorName">name that will be displayed in the consent window.</param>
            /// <param name="vendorBundle">identifier that can be used to check the consent status for the vendor.</param>
            /// <param name="vendorURL">policy URL.</param>
            public Builder(string vendorName, string vendorBundle, string vendorURL)
            {
                _vendorBuilder = ConsentManagerClientFactory.GetVendorBuilder(vendorName, vendorBundle, vendorURL);
            }

            /// <summary>
            /// Builds the Vendor object using all data you have set via the other Builder's methods.
            /// </summary>
            /// <returns>Object of type <see langword="Vendor"/>.</returns>
            public Vendor Build()
            {
                return new Vendor(GetNativeVendorBuilder().Build());
            }

            /// <summary>
            /// Sets a list of IAB purpose ids.
            /// </summary>
            /// <param name="purposeIds">an IEnumerable of integers containing IAB purpose ids for your vendor.</param>
            /// <returns>An instance of the builder class.</returns>
            public Builder SetPurposeIds(IEnumerable<int> purposeIds)
            {
                GetNativeVendorBuilder().SetPurposeIds(purposeIds);
                return this;
            }

            /// <summary>
            /// Sets a list of IAB feature ids.
            /// </summary>
            /// <param name="featureIds">an IEnumerable of integers containing IAB feature ids for your vendor.</param>
            /// <returns>An instance of the builder class.</returns>
            public Builder SetFeatureId(IEnumerable<int> featureIds)
            {
                GetNativeVendorBuilder().SetFeatureIds(featureIds);
                return this;
            }

            /// <summary>
            /// Sets a list of IAB legitimate interest purpose ids.
            /// </summary>
            /// <param name="legitimateInterestPurposeIds">an IEnumerable of integers containing IAB legitimate interest purpose ids for your vendor.</param>
            /// <returns>An instance of the builder class.</returns>
            public Builder SetLegitimateInterestPurposeIds(IEnumerable<int> legitimateInterestPurposeIds)
            {
                GetNativeVendorBuilder().SetLegitimateInterestPurposeIds(legitimateInterestPurposeIds);
                return this;
            }

            #region Deprecated Methods

            [Obsolete("It will be removed in the next release. Use the capitalized version (Build) of this method instead.", false)]
            public Vendor build()
            {
                return new Vendor(GetNativeVendorBuilder().Build());
            }

            [Obsolete("It will be removed in the next release. Use the capitalized version (SetPurposeIds) of this method instead.", false)]
            public Builder setPurposeIds(IEnumerable<int> purposeIds)
            {
                GetNativeVendorBuilder().SetPurposeIds(purposeIds);
                return this;
            }

            [Obsolete("It will be removed in the next release. Use the capitalized version (SetFeatureId) of this method instead.", false)]
            public Builder setFeatureId(IEnumerable<int> featureIds)
            {
                GetNativeVendorBuilder().SetFeatureIds(featureIds);
                return this;
            }

            [Obsolete("It will be removed in the next release. Use the capitalized version (SetLegitimateInterestPurposeIds) of this method instead.", false)]
            public Builder setLegitimateInterestPurposeIds(IEnumerable<int> legitimateInterestPurposeIds)
            {
                GetNativeVendorBuilder().SetLegitimateInterestPurposeIds(legitimateInterestPurposeIds);
                return this;
            }

            #endregion

        }
    }
}