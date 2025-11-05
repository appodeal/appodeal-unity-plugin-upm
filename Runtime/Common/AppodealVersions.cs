// ReSharper disable CheckNamespace

using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace AppodealStack.Monetization.Common
{
    /// <summary>
    /// Static class containing information about Appodeal Plugin and Unity Editor versions.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static class AppodealVersions
    {
        private const string AppodealPluginVersion = "3.11.0";

        /// <summary>
        /// Gets the current version of the Appodeal Unity Plugin.
        /// </summary>
        /// <returns>Appodeal Unity Plugin version as string.</returns>
        public static string GetPluginVersion()
        {
            return AppodealPluginVersion;
        }

        /// <summary>
        /// Gets Unity Editor version.
        /// </summary>
        /// <returns>Current version of Unity Editor.</returns>
        public static string GetUnityVersion()
        {
            return Application.unityVersion;
        }
    }
}
