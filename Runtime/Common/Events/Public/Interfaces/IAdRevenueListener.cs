// ReSharper disable CheckNamespace

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
        /// <remarks>
        /// <para>
        /// On Android, this callback is invoked on a background thread to ensure ad revenue data
        /// is delivered immediately without loss, reducing discrepancies when forwarding to MMPs.
        /// </para>
        /// If you need to interact with Unity Engine APIs (e.g., UI updates), make sure to dispatch those calls to the main thread.
        /// See <see href="https://docs.appodeal.com/unity/advanced/main-thread-callbacks"/> for more details.
        /// </remarks>
        void OnAdRevenueReceived(AppodealAdRevenue ad);
    }
}
