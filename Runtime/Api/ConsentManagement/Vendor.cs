using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AppodealStack.ConsentManagement.Common;
using AppodealStack.ConsentManagement.Platforms;

// ReSharper disable CheckNamespace
namespace AppodealStack.ConsentManagement.Api
{
    /// <summary>
    /// <para>Vendor Unity API for developers, including documentation.</para>
    /// See <see href="https://docs.appodeal.com/unity/data-protection/gdpr-and-ccpa"/> for more details.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class Vendor : IVendor
    {
        /// <summary>
        /// Provides access to a native Vendor object.
        /// </summary>
        public IVendor NativeVendor { get; }

        /// <summary>
        /// Public constructor of the <see langword="Vendor"/> class.
        /// </summary>
        /// <param name="builder">class which implements AppodealStack.ConsentManager.Common.IVendor interface.</param>
        public Vendor(IVendor builder)
        {
            NativeVendor = builder.NativeVendor;
        }

        /// <summary>
        /// <para>Gets the name of the vendor.</para>
        /// See <see href="https://docs.appodeal.com/unity/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <returns>Name of the vendor as string.</returns>
        public string GetName()
        {
            return NativeVendor.GetName();
        }

        /// <summary>
        /// <para>Gets the bundle identifier of the vendor.</para>
        /// See <see href="https://docs.appodeal.com/unity/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <returns>Bundle id as string.</returns>
        public string GetBundle()
        {
            return NativeVendor.GetBundle();
        }

        /// <summary>
        /// <para>Gets the policy URL of the vendor.</para>
        /// See <see href="https://docs.appodeal.com/unity/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <returns>Policy URL as string.</returns>
        public string GetPolicyUrl()
        {
            return NativeVendor.GetPolicyUrl();
        }

        /// <summary>
        /// <para>Gets the IAB purpose ids.</para>
        /// See <see href="https://docs.appodeal.com/unity/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <returns>IAB purpose ids as a list of integers.</returns>
        public List<int> GetPurposeIds()
        {
            return NativeVendor.GetPurposeIds();
        }

        /// <summary>
        /// <para>Gets the IAB features ids.</para>
        /// See <see href="https://docs.appodeal.com/unity/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <returns>IAB feature ids as a list of integers.</returns>
        public List<int> GetFeatureIds()
        {
            return NativeVendor.GetFeatureIds();
        }

        /// <summary>
        /// <para>Gets the IAB legitimate interest purpose ids.</para>
        /// See <see href="https://docs.appodeal.com/unity/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <returns>IAB legitimate interest purpose ids as a list of integers.</returns>
        public List<int> GetLegitimateInterestPurposeIds()
        {
            return NativeVendor.GetLegitimateInterestPurposeIds();
        }

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
            /// <para> Sets a list of IAB purpose ids. </para>
            /// See <see href="https://iabeurope.eu/iab-europe-transparency-consent-framework-policies/"/> for more details.
            /// </summary>
            /// <param name="purposeIds">an IEnumerable of integers containing IAB purpose ids for your vendor.</param>
            /// <returns>An instance of the builder class.</returns>
            public Builder SetPurposeIds(IEnumerable<int> purposeIds)
            {
                GetNativeVendorBuilder().SetPurposeIds(purposeIds);
                return this;
            }

            /// <summary>
            /// <para> Sets a list of IAB feature ids. </para>
            /// See <see href="https://iabeurope.eu/iab-europe-transparency-consent-framework-policies/"/> for more details.
            /// </summary>
            /// <param name="featureIds">an IEnumerable of integers containing IAB feature ids for your vendor.</param>
            /// <returns>An instance of the builder class.</returns>
            public Builder SetFeatureIds(IEnumerable<int> featureIds)
            {
                GetNativeVendorBuilder().SetFeatureIds(featureIds);
                return this;
            }

            /// <summary>
            /// <para> Sets a list of IAB legitimate interest purpose ids. </para>
            /// See <see href="https://iabeurope.eu/iab-europe-transparency-consent-framework-policies/"/> for more details.
            /// </summary>
            /// <param name="legitimateInterestPurposeIds">an IEnumerable of integers containing IAB legitimate interest purpose ids for your vendor.</param>
            /// <returns>An instance of the builder class.</returns>
            public Builder SetLegitimateInterestPurposeIds(IEnumerable<int> legitimateInterestPurposeIds)
            {
                GetNativeVendorBuilder().SetLegitimateInterestPurposeIds(legitimateInterestPurposeIds);
                return this;
            }
        }
    }
}
