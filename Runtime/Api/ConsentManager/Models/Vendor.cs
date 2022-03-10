using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AppodealStack.ConsentManager.Common;
using AppodealStack.ConsentManager.Platforms;

namespace AppodealStack.ConsentManager.Api
{
    /// <summary>
    /// <para>Vendor Unity API for developers, including documentation.</para>
    /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "InvalidXmlDocComment")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class Vendor : IVendor
    {
        private readonly IVendor nativeVendor;

        /// <summary>
        /// Provides access to a native Vendor object.
        /// </summary>
        public object nativeVendorObject { get; }

        /// <summary>
        /// Public constructor of the <see langword="Vendor"/> class.
        /// </summary>
        /// <param name="builder">class which implements AppodealStack.ConsentManager.Common.IVendor interface.</param>
        public Vendor(IVendor builder)
        {
            nativeVendor = builder;
            nativeVendorObject = builder.nativeVendorObject;
        }

        /// <summary>
        /// <para>Gets the wrapper over native (Android or iOS) <see langword="Vendor"/> object.</para>
        /// </summary>
        /// <returns>Object of type that implements the <see langword="IVendor"/> interface.</returns>
        public IVendor getNativeVendor()
        {
            return nativeVendor;
        }

        /// <summary>
        /// Builder class is responsible for creating an object of the <see langword="Vendor"/> class.
        /// </summary>
        public class Builder
        {
            private readonly IVendorBuilder nativeVendorBuilder;

            /// <summary>
            /// Public constructor of the <see langword="Builder"/> class.
            /// </summary>
            /// <param name="vendorName">name that will be displayed in the consent window.</param>
            /// <param name="vendorBundle">identifier that can be used to check the consent status for the vendor.</param>
            /// <param name="vendorURL">policy URL.</param>
            public Builder(string vendorName, string vendorBundle, string vendorURL)
            {
                nativeVendorBuilder =
                    ConsentManagerClientFactory.GetVendorBuilder(vendorName, vendorBundle, vendorURL);
            }

            /// <summary>
            /// Builds the Vendor object using all data you have set via the other Builder's methods.
            /// </summary>
            /// <returns>Object of type <see langword="Vendor"/>.</returns>
            public Vendor build()
            {
                return new Vendor(nativeVendorBuilder.build());
            }

            /// <summary>
            /// Sets a list of IAB purpose ids.
            /// </summary>
            /// <param name="purposeIds">an IEnumerable of integers containing IAB purpose ids for your vendor.</param>
            /// <returns>An instance of the builder class.</returns>
            public Builder setPurposeIds(IEnumerable<int> purposeIds)
            {
                nativeVendorBuilder.setPurposeIds(purposeIds);
                return this;
            }

            /// <summary>
            /// Sets a list of IAB feature ids.
            /// </summary>
            /// <param name="featureIds">an IEnumerable of integers containing IAB feature ids for your vendor.</param>
            /// <returns>An instance of the builder class.</returns>
            public Builder setFeatureId(IEnumerable<int> featureIds)
            {
                nativeVendorBuilder.setFeatureIds(featureIds);
                return this;
            }

            /// <summary>
            /// Sets a list of IAB legitimate interest purpose ids.
            /// </summary>
            /// <param name="legitimateInterestPurposeIds">an IEnumerable of integers containing IAB legitimate interest purpose ids for your vendor.</param>
            /// <returns>An instance of the builder class.</returns>
            public Builder setLegitimateInterestPurposeIds(IEnumerable<int> legitimateInterestPurposeIds)
            {
                nativeVendorBuilder.setLegitimateInterestPurposeIds(legitimateInterestPurposeIds);
                return this;
            }
        }

        /// <summary>
        /// <para>Gets the name of the vendor.</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <returns>Name of the vendor as string.</returns>
        public string getName()
        {
            return nativeVendor.getName();
        }

        /// <summary>
        /// <para>Gets the bundle identifier of the vendor.</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <returns>Bundle id as string.</returns>
        public string getBundle()
        {
            return nativeVendor.getBundle();
        }

        /// <summary>
        /// <para>Gets the policy URL of the vendor.</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <returns>Policy URL as string.</returns>
        public string getPolicyUrl()
        {
            return nativeVendor.getPolicyUrl();
        }

        /// <summary>
        /// <para>Gets the IAB purpose ids.</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <returns>IAB purpose ids as a list of integers.</returns>
        public List<int> getPurposeIds()
        {
            return nativeVendor.getPurposeIds();
        }

        /// <summary>
        /// <para>Gets the IAB features ids.</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <returns>IAB feature ids as a list of integers.</returns>
        public List<int> getFeatureIds()
        {
            return nativeVendor.getFeatureIds();
        }

        /// <summary>
        /// <para>Gets the IAB legitimate interest purpose ids.</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <returns>IAB legitimate interest purpose ids as a list of integers.</returns>
        public List<int> getLegitimateInterestPurposeIds()
        {
            return nativeVendor.getLegitimateInterestPurposeIds();
        }
    }
}
