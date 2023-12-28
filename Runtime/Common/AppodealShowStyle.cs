// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Common
{
    /// <summary>
    /// <para>
    /// Static class containing all possible styles for <see langword="Appodeal.Show()"/> method.
    /// </para>
    /// See <see href="https://docs.appodeal.com/unity/get-started"/> for more details.
    /// </summary>
    public static class AppodealShowStyle
    {
        /// <summary>
        /// <para>
        /// It is used in <see langword="Appodeal.Show()"/> method to display <see langword="AppodealAdType.Interstitial"/> ad type.
        /// </para>
        /// See <see href="https://docs.appodeal.com/unity/ad-types/interstitial?distribution=upm#displaying"/> for more details.
        /// </summary>
        public const int Interstitial = 1;

        /// <summary>
        /// <para>
        /// It is used in <see langword="Appodeal.Show()"/> method to display a horizontal <see langword="AppodealAdType.Banner"/> at the <see langword="bottom"/> of the screen.
        /// </para>
        /// See <see href="https://docs.appodeal.com/unity/ad-types/banner?distribution=upm#displaying"/> for more details.
        /// </summary>
        public const int BannerBottom = 2;

        /// <summary>
        /// <para>
        /// It is used in <see langword="Appodeal.Show()"/> method to display a horizontal <see langword="AppodealAdType.Banner"/> at the <see langword="top"/> of the screen.
        /// </para>
        /// See <see href="https://docs.appodeal.com/unity/ad-types/banner?distribution=upm#displaying"/> for more details.
        /// </summary>
        public const int BannerTop = 4;

        /// <summary>
        /// <para>
        /// It is used in <see langword="Appodeal.Show()"/> method to display a vertical <see langword="AppodealAdType.Banner"/> at the <see langword="left edge"/> of the screen.
        /// </para>
        /// See <see href="https://docs.appodeal.com/unity/ad-types/banner?distribution=upm#displaying"/> for more details.
        /// </summary>
        public const int BannerLeft = 8;

        /// <summary>
        /// <para>
        /// It is used in <see langword="Appodeal.Show()"/> method to display a vertical <see langword="AppodealAdType.Banner"/> at the <see langword="right edge"/> of the screen.
        /// </para>
        /// See <see href="https://docs.appodeal.com/unity/ad-types/banner?distribution=upm#displaying"/> for more details.
        /// </summary>
        public const int BannerRight = 16;

        /// <summary>
        /// <para>
        /// It is used in <see langword="Appodeal.Show()"/> method to display <see langword="AppodealAdType.RewardedVideo"/> ad type.
        /// </para>
        /// See <see href="https://docs.appodeal.com/unity/ad-types/rewarded-video?distribution=upm#displaying"/> for more details.
        /// </summary>
        public const int RewardedVideo = 32;
    }
}
