// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Common
{
    /// <summary>
    /// <para>
    /// Static class containing all currently supported view positions.
    /// </para>
    /// Its variables can be used as arguments for the <see langword="Appodeal.ShowBannerView()"/> and <see langword="Appodeal.ShowMrecView()"/> methods.
    /// </summary>
    /// <remarks>See <see href="https://docs.appodeal.com/unity/ad-types/banner?distribution=upm#displaying-banner-at-custom-position"/> for more details.</remarks>
    public static class AppodealViewPosition
    {
        /// <summary>
        /// <para>
        /// XAxis constant. Forces banner to use the full-screen width if possible.
        /// </para>
        /// See <see href="https://docs.appodeal.com/unity/ad-types/banner?distribution=upm#displaying-banner-at-custom-position"/> for more details.
        /// </summary>
        public const int HorizontalSmart = -1;

        /// <summary>
        /// <para>
        /// XAxis constant. Aligns banner to the center of the screen.
        /// </para>
        /// See <see href="https://docs.appodeal.com/unity/ad-types/banner?distribution=upm#displaying-banner-at-custom-position"/> for more details.
        /// </summary>
        public const int HorizontalCenter = -2;

        /// <summary>
        /// <para>
        /// XAxis constant. Aligns banner to the right edge of the screen.
        /// </para>
        /// See <see href="https://docs.appodeal.com/unity/ad-types/banner?distribution=upm#displaying-banner-at-custom-position"/> for more details.
        /// </summary>
        public const int HorizontalRight = -3;

        /// <summary>
        /// <para>
        /// XAxis constant. Aligns banner to the left edge of the screen.
        /// </para>
        /// See <see href="https://docs.appodeal.com/unity/ad-types/banner?distribution=upm#displaying-banner-at-custom-position"/> for more details.
        /// </summary>
        public const int HorizontalLeft = -4;

        /// <summary>
        /// <para>
        /// YAxis constant. Aligns banner to the bottom edge of the screen.
        /// </para>
        /// See <see href="https://docs.appodeal.com/unity/ad-types/banner?distribution=upm#displaying-banner-at-custom-position"/> for more details.
        /// </summary>
        public const int VerticalBottom = -1;

        /// <summary>
        /// <para>
        /// YAxis constant. Aligns banner to the top edge of the screen.
        /// </para>
        /// See <see href="https://docs.appodeal.com/unity/ad-types/banner?distribution=upm#displaying-banner-at-custom-position"/> for more details.
        /// </summary>
        public const int VerticalTop = -2;
    }
}
