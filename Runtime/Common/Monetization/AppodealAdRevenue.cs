// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Common
{
    /// <summary>
    /// <para>
    /// This class is designed to store ad revenue information.
    /// </para>
    /// See <see href="https://docs.appodeal.com/unity/advanced/ad-revenue-callback"/> for more details.
    /// </summary>
    public class AppodealAdRevenue
    {
        /// <summary>
        /// <para>
        /// Appodeal Ad Type as a not-null string.
        /// </para>
        /// See <see href="https://docs.appodeal.com/unity/advanced/ad-revenue-callback#appodeal-ad-revenue-description"/> for more details.
        /// </summary>
        public string AdType;

        /// <summary>
        /// <para>
        /// Ad Network Name as a not-null string.
        /// </para>
        /// See <see href="https://docs.appodeal.com/unity/advanced/ad-revenue-callback#appodeal-ad-revenue-description"/> for more details.
        /// </summary>
        public string NetworkName;

        /// <summary>
        /// <para>
        /// Appodeal Ad Unit Name as a not-null string.
        /// </para>
        /// See <see href="https://docs.appodeal.com/unity/advanced/ad-revenue-callback#appodeal-ad-revenue-description"/> for more details.
        /// </summary>
        public string AdUnitName;

        /// <summary>
        /// <para>
        /// Demand Source as a not-null string.
        /// </para>
        /// See <see href="https://docs.appodeal.com/unity/advanced/ad-revenue-callback#appodeal-ad-revenue-description"/> for more details.
        /// </summary>
        /// <remarks>Bidder name in case of RTB, otherwise - the same as ad network name.</remarks>
        public string DemandSource;

        /// <summary>
        /// <para>
        /// Appodeal Placement as a not-null string.
        /// </para>
        /// See <see href="https://docs.appodeal.com/unity/advanced/ad-revenue-callback#appodeal-ad-revenue-description"/> for more details.
        /// </summary>
        public string Placement;

        /// <summary>
        /// <para>
        /// The amount of revenue for an ad.
        /// </para>
        /// See <see href="https://docs.appodeal.com/unity/advanced/ad-revenue-callback#appodeal-ad-revenue-description"/> for more details.
        /// </summary>
        /// <remarks>It can be zero in case of an invalid impression.</remarks>
        public double Revenue;

        /// <summary>
        /// <para>
        /// The Revenue Currency as a not-null string.
        /// </para>
        /// See <see href="https://docs.appodeal.com/unity/advanced/ad-revenue-callback#appodeal-ad-revenue-description"/> for more details.
        /// </summary>
        /// <remarks>At the moment the only supported currency is USD.</remarks>
        public string Currency;

        /// <summary>
        /// <para>
        /// Ad Revenue Precision as a not-null string.
        /// </para>
        /// See <see href="https://docs.appodeal.com/unity/advanced/ad-revenue-callback#appodeal-ad-revenue-description"/> for more details.
        /// </summary>
        /// <remarks><para>'exact' - programmatic revenue is the resulting price of an auction.</para><para>'publisher_defined' - revenue from cross-promo campaigns.</para><para>'estimated' - revenue based on ad unit price floor or historical eCPM.</para><para>'undefined' - revenue amount is not defined.</para></remarks>
        public string RevenuePrecision;

        /// <summary>
        /// Returns ad revenue information as a json-formatted string.
        /// </summary>
        /// <param name="isPretty">If true, format the output for readability. If false, format the output for minimum size. Default is false.</param>
        public string ToJsonString(bool isPretty = false) => UnityEngine.JsonUtility.ToJson(this, isPretty);
    }
}
