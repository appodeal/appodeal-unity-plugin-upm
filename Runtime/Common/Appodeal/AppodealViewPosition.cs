using System.Diagnostics.CodeAnalysis;

namespace AppodealAds.Unity.Common
{
    /// <summary>
    /// <para>
    /// Static class containing all currently supported view positions.
    /// </para>
    /// Its variables can be used as arguments for <see langword="Appodeal.showBannerView()"/> and <see langword="Appodeal.showMrecView()"/> methods.
    /// </summary>
    /// <remarks>See <see href="https://wiki.appodeal.com/en/unity/get-started/ad-types/banner#id-[Development]UnitySDK.Banner-DisplayBanneratCustomPosition"/> for more details.</remarks>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    public static class AppodealViewPosition
    {
        public const int HORIZONTAL_SMART =     -1;
        public const int HORIZONTAL_CENTER =    -2;
        public const int HORIZONTAL_RIGHT =     -3;
        public const int HORIZONTAL_LEFT =      -4;
        public const int VERTICAL_BOTTOM =      AppodealAdType.BANNER_BOTTOM;
        public const int VERTICAL_TOP =         AppodealAdType.BANNER_TOP;
    }
}
