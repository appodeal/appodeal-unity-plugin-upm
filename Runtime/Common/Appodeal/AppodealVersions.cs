using UnityEngine;
using System.Reflection;

namespace AppodealAds.Unity.Common {

    public static class AppodealVersions {
        /// <summary>
        /// The version for the Appodeal Unity SDK, which includes specific versions of the Appodeal Android and iOS SDKs.
        /// </summary>
        public const string APPODEAL_PLUGIN_VERSION = "2.14.5";

        /// <summary>
        /// Get Unity plugin version
        /// See <see cref="Appodeal.getPluginVersion"/> for resulting triggered event.
        /// </summary> 
        public static string getPluginVersion()
        {
            return APPODEAL_PLUGIN_VERSION;
        }

        /// <summary>
        /// Get Unity version
        /// See <see cref="Appodeal.getUnityVersion"/> for resulting triggered event.
        /// </summary> 
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
