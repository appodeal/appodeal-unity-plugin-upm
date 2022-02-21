using UnityEngine;
using System.Reflection;

namespace AppodealStack.Mediation.Common
{
    /// <summary>
    /// Static class containing information about Appodeal Plugin and Unity Editor versions.
    /// </summary>
    public static class AppodealVersions {
        /// <summary>
        /// Current version of the Appodeal Unity Plugin.
        /// </summary>
        public const string APPODEAL_PLUGIN_VERSION = "2.15.2";

        /// <summary>
        /// Gets the value of APPODEAL_PLUGIN_VERSION variable.
        /// </summary> 
        /// <returns>current version of Appodeal Unity Plugin.</returns>
        public static string getPluginVersion()
        {
            return APPODEAL_PLUGIN_VERSION;
        }

        /// <summary>
        /// Gets Unity Editor version.
        /// </summary>
        /// <returns>current version of Unity Editor.</returns>
        public static string getUnityVersion()
        {
            var unityVersion = Application.unityVersion;
            if (!string.IsNullOrEmpty(unityVersion)) return unityVersion;
            var appId =
                typeof(Application).GetProperty("identifier", BindingFlags.Public | BindingFlags.Static);
            unityVersion = appId != null ? "5.6+" : "5.5-";

            return unityVersion;
        }
    }
}
