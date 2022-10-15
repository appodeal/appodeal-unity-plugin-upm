using UnityEditor;
using UnityEngine;
using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

// ReSharper Disable CheckNamespace
namespace AppodealStack.UnityEditor.Utils
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
    [SuppressMessage("ReSharper", "InlineOutVariableDeclaration")]
    [SuppressMessage("ReSharper", "IdentifierTypo")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static class AppodealUnityUtils
    {
        private const string UnityAndroidVersionEnumPrefix = "AndroidApiLevel";
        private const BindingFlags PublicStaticFlags = BindingFlags.Public | BindingFlags.Static;
        public const string KeySkAdNetworkItems = "SKAdNetworkItems";
        public const string KeySkAdNetworkID = "SKAdNetworkIdentifier";
        public const string GadApplicationIdentifier = "GADApplicationIdentifier";
        public const string NsUserTrackingUsageDescriptionKey = "NSUserTrackingUsageDescription";
        public const string NsUserTrackingUsageDescription = "This identifier will be used to deliver personalized ads to you";

        public const string GadApplicationIdentifierDefaultKey = "ca-app-pub-3940256099942544~1458002511";

        #region Optional Android Permissions

        public const string CoarseLocation = "android.permission.ACCESS_COARSE_LOCATION";
        public const string FineLocation = "android.permission.ACCESS_FINE_LOCATION";
        public const string ExternalStorageWrite = "android.permission.WRITE_EXTERNAL_STORAGE";
        public const string AccessWifiState = "android.permission.ACCESS_WIFI_STATE";
        public const string Vibrate = "android.permission.VIBRATE";

        #endregion

        [Flags]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        [SuppressMessage("ReSharper", "IdentifierTypo")]
        public enum AndroidArchitecture
        {
            Invalid = 0,
            ARmv7 = 1 << 0,
            ARM64 = 1 << 1,
            X86 = 1 << 2,
        }

        public static string GetApplicationId()
        {
            var appId = typeof(PlayerSettings).GetProperty("applicationIdentifier", PublicStaticFlags);
            if (appId == null) appId = typeof(PlayerSettings).GetProperty("bundleIdentifier", PublicStaticFlags);
            var bundleId = (string) appId?.GetValue(null, null);
            return bundleId;
        }

        public static bool IsGradleEnabled()
        {
            var isGradleEnabledVal = false;
            var androidBuildSystem =
                typeof(EditorUserBuildSettings).GetProperty("androidBuildSystem", PublicStaticFlags);
            if (androidBuildSystem == null) return isGradleEnabledVal;
            var gradle = Enum.Parse(androidBuildSystem.PropertyType, "Gradle");
            isGradleEnabledVal = androidBuildSystem.GetValue(null, null).Equals(gradle);

            return isGradleEnabledVal;
        }

        public static bool IsGradleAvailable()
        {
            var androidBuildSystem =
                typeof(EditorUserBuildSettings).GetProperty("androidBuildSystem", PublicStaticFlags);
            return androidBuildSystem != null;
        }

        public static void EnableGradleBuildSystem()
        {
            var androidBuildSystem =
                typeof(EditorUserBuildSettings).GetProperty("androidBuildSystem", PublicStaticFlags);
            if (androidBuildSystem == null) return;
            var gradle = Enum.Parse(androidBuildSystem.PropertyType, "Gradle");
            androidBuildSystem.SetValue(null, gradle, null);
        }

        public static string Absolute2Relative(string absolutePath)
        {
            string relativePath = absolutePath;
            if (absolutePath.StartsWith(Application.dataPath, StringComparison.Ordinal))
            {
                relativePath = "Assets" + absolutePath.Substring(Application.dataPath.Length);
            }

            return relativePath;
        }

        public static string Relative2Absolute(string relativePath)
        {
            string absolutePath = relativePath;
            if (!relativePath.StartsWith(Application.dataPath, StringComparison.Ordinal))
            {
                absolutePath = Application.dataPath + absolutePath.Substring("Assets".Length);
            }

            return absolutePath;
        }

        public static int GetAndroidMinSDK()
        {
            var minSdkVersion = VersionFromAndroidSDKVersionsEnum(
                PlayerSettings.Android.minSdkVersion.ToString());
            return minSdkVersion;
        }

        public static int GetAndroidTargetSDK()
        {
            var property = typeof(PlayerSettings.Android).GetProperty("targetSdkVersion");
            var target = -1;
            if (property != null)
                target = VersionFromAndroidSDKVersionsEnum(Enum.GetName(property.PropertyType,
                    property.GetValue(null, null)));
            if (target == -1)
                target = GetLatestInstalledAndroidPlatformVersion();
            return target;
        }

        private static int VersionFromAndroidSDKVersionsEnum(string enumName)
        {
            if (enumName.StartsWith(UnityAndroidVersionEnumPrefix))
            {
                enumName = enumName.Substring(UnityAndroidVersionEnumPrefix.Length);
            }

            if (enumName == "Auto")
            {
                return -1;
            }

            int versionVal;
            int.TryParse(enumName, out versionVal);
            return versionVal;
        }

        private static int GetLatestInstalledAndroidPlatformVersion()
        {
            var androidSDKPath = EditorPrefs.GetString("AndroidSdkRoot");
            if (string.IsNullOrEmpty(androidSDKPath)) return -1;

            var platforms = Directory.GetDirectories(Path.Combine(androidSDKPath, "platforms"), "*",
                SearchOption.TopDirectoryOnly);
            var buildToolsRegex = new Regex(@"android-(\d+)$", RegexOptions.Compiled);

            return platforms
                .Select(platform => buildToolsRegex.Match(platform))
                .Select(match => int.Parse(match.Groups[1].Value))
                .Concat(new[] {0}).Max();
        }

        public static AndroidArchitecture GetAndroidArchitecture()
        {
            var targetArchitectures =
                typeof(PlayerSettings.Android).GetProperty("targetArchitectures");
            var arch = AndroidArchitecture.Invalid;
            if (targetArchitectures != null)
            {
                var armv7 = Enum.Parse(targetArchitectures.PropertyType, "ARMv7");
                var armv7_int = (int) Convert.ChangeType(armv7, typeof(int));
                var arm64 = Enum.Parse(targetArchitectures.PropertyType, "ARM64");
                var arm64_int = (int) Convert.ChangeType(arm64, typeof(int));
                var x64 = Enum.Parse(targetArchitectures.PropertyType, "X86");
                var x64_int = (int) Convert.ChangeType(x64, typeof(int));
                var currentArch = targetArchitectures.GetValue(null, null);
                var currentArch_int = (int) Convert.ChangeType(currentArch, typeof(int));
                if ((currentArch_int & armv7_int) == armv7_int) arch |= AndroidArchitecture.ARmv7;
                if ((currentArch_int & arm64_int) == arm64_int) arch |= AndroidArchitecture.ARM64;
                if ((currentArch_int & x64_int) == x64_int) arch |= AndroidArchitecture.X86;
            }
            else
            {
                targetArchitectures = typeof(PlayerSettings.Android).GetProperty("targetDevice");
                if (targetArchitectures == null) return arch;
                var currentDevice = targetArchitectures.GetValue(null, null);
                var armv7 = Enum.Parse(targetArchitectures.PropertyType, "ARMv7");
                var x64 = Enum.Parse(targetArchitectures.PropertyType, "x86");
                var fat = Enum.Parse(targetArchitectures.PropertyType, "FAT");
                if (currentDevice.Equals(armv7)) arch = AndroidArchitecture.ARmv7;
                else if (currentDevice.Equals(x64)) arch = AndroidArchitecture.X86;
                else if (currentDevice.Equals(fat)) arch = AndroidArchitecture.ARmv7 | AndroidArchitecture.X86;
            }

            return arch;
        }

        public static string CombinePaths(params string[] paths)
        {
            var result = paths[0];
            for (var i = 1; i < paths.Length; i++)
            {
                result = Path.Combine(result, paths[i]);
            }

            return result;
        }

        public static int CompareVersions(string v1, string v2)
        {
            var re = new Regex(@"\d+(\.\d+)+");
            var match1 = re.Match(v1);
            var match2 = re.Match(v2);
            return new Version(match1.ToString()).CompareTo(new Version(match2.ToString()));
        }

        public static Texture2D MakeColorTexture(int width, int height, Color color)
        {
            var colors = new Color [width * height];
            for (var i = 0; i < colors.Length; ++i)
                colors[i] = color;

            var retVal = new Texture2D(width, height);
            retVal.SetPixels(colors);
            retVal.Apply();
            return retVal;
        }

        public static XmlNode XmlFindChildNode(XmlNode parent, string name)
        {
            var curr = parent.FirstChild;
            while (curr != null)
            {
                if (curr.Name.Equals(name))
                {
                    return curr;
                }

                curr = curr.NextSibling;
            }

            return null;
        }

        public static XmlElement XmlCreateTag(XmlDocument doc, string tag)
        {
            var permissionElement = doc.CreateElement(tag);
            return permissionElement;
        }

        public static string FixSlashesInPath(string path)
        {
            return path?.Replace('\\', '/');
        }
    }
}
