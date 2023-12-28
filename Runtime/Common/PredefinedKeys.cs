using System.Diagnostics.CodeAnalysis;

// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Common
{
    /// <summary>
    /// <para>
    /// Static class containing predefined string constants.
    /// </para>
    /// Its fields can be used as keys for the <see langword="Appodeal.SetCustomFilter()"/> and <see langword="Appodeal.SetExtraData()"/> methods.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static class PredefinedKeys
    {
        public const string AppsFlyerId = "appsflyer_id";
        public const string UserAge = "appodeal_user_age";
        public const string UserGender = "appodeal_user_gender";
    }
}
