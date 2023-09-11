// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Common
{
    /// <summary>
    /// Interface containing signature of Appodeal Ad Revenue callback method.
    /// </summary>
    public interface IAdRevenueListener
    {
        /// <summary>
        /// <para>
        /// Raised when Appodeal SDK tracks ad impression.
        /// </para>
        /// See <see href="https://docs.appodeal.com/unity/advanced/ad-revenue-callback"/> for more details.
        /// </summary>
        /// <param name="ad">contains info about the tracked impression.</param>
        void OnAdRevenueReceived(AppodealAdRevenue ad);
    }
}
