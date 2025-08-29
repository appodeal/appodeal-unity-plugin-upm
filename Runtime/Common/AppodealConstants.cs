// ReSharper disable CheckNamespace

using System.Diagnostics.CodeAnalysis;

namespace AppodealStack.Monetization.Common
{
    /// <summary>
    /// Static class containing predefined string constants.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static class AppodealConstants
    {
        /// <summary>
        /// PlayerPrefs key for storing Appodeal App Key.
        /// Used by demo scripts and external configuration tools.
        /// </summary>
        public const string AppKeyPlayerPrefsKey = "Appodeal.AppKey";
    }
}
