// ReSharper disable CheckNamespace

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEditor.PackageManager;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal static class VersionHelper
    {
        private static readonly Regex PluginVersionRegex = new(DmConstants.Regex.PluginVersionPattern);
        private static readonly Regex AdapterVersionRegex = new(DmConstants.Regex.AdapterVersionPattern);

        public static async Task<Request<string>> GetCurrentVersionForPackage(string packageName)
        {
            if (String.IsNullOrWhiteSpace(packageName)) return Error.Create("ParamIsInvalid", $"{nameof(packageName)} parameter is invalid");

            try
            {
                var request = Client.List(true);
                while(!request.IsCompleted) await Task.Yield();
                string version = request.Result?.FirstOrDefault(el => el.name == packageName)?.version;
                return version == null ? Error.Create("VersionNotFound", $"{nameof(version)} variable value cannot be null") : version;
            }
            catch (Exception e)
            {
                return Error.Create(e.GetType().ToString(), e.Message);
            }
        }

        public static Request<Plugin> GetLatestPlugin(List<Plugin> plugins, bool isBetaIncluded = false)
        {
            if (plugins == null) return Error.Create("ParamIsNull", $"{nameof(plugins)} param cannot be null");

            plugins = isBetaIncluded
                ? plugins.Where(plugin => IsValidPluginVersion(plugin)).OrderBy(plugin => plugin.id).ToList()
                : plugins.Where(plugin => IsValidPluginVersion(plugin) && !plugin.version.Contains("beta")).OrderBy(plugin => plugin.id).ToList();

            var latest = plugins.FirstOrDefault();
            if (latest == null) return Error.Create("NoValidVersions", $"{nameof(plugins)} param has no elements after filtering");

            plugins.ForEach(plugin =>
            {
                if (plugin.CompareTo(latest) == ComparisonResult.Subsequent) latest = plugin;
            });

            return latest;
        }

        public static Request<string> GetVersionFromAndroidSpec(string spec)
        {
            if (String.IsNullOrWhiteSpace(spec)) return Error.Create("WrongAndroidSpecString", $"'{spec}' is an invalid android spec format");
            const char specPartsSeparator = ':';
            string[] specParts = spec.Split(specPartsSeparator);
            if (specParts.Length != 3) return Error.Create("WrongAndroidSpecString", $"'{spec}' is an invalid android spec format");
            return specParts[^1];
        }

        public static bool IsPluginVersionKnownAndValid(List<Plugin> plugins, string version)
        {
            if (String.IsNullOrEmpty(version) || !IsValidPluginVersion(version)) return false;
            return plugins?.Any(plugin => plugin.version == version) ?? false;
        }

        public static ComparisonResult CompareTo(this Plugin pluginA, Plugin pluginB) => pluginA.CompareTo(pluginB.version);

        public static ComparisonResult CompareTo(this Plugin pluginA, string pluginBVersion)
        {
            if (!IsValidPluginVersion(pluginA) || !IsValidPluginVersion(pluginBVersion)) return ComparisonResult.WrongInput;

            var aMatch = PluginVersionRegex.Match(pluginA.version);
            var bMatch = PluginVersionRegex.Match(pluginBVersion);

            var aVersion = new Version(aMatch.Groups[DmConstants.Regex.VersionGroupName].Value);
            var bVersion = new Version(bMatch.Groups[DmConstants.Regex.VersionGroupName].Value);

            int res = aVersion.CompareTo(bVersion);

            if (res < 0) return ComparisonResult.Previous;
            if (res > 0) return ComparisonResult.Subsequent;

            string aIdentifier = aMatch.Groups[DmConstants.Regex.IdentifierGroupName].Value;
            string bIdentifier = bMatch.Groups[DmConstants.Regex.IdentifierGroupName].Value;

            if (aIdentifier == bIdentifier) return ComparisonResult.Equal;
            if (String.IsNullOrEmpty(aIdentifier)) return ComparisonResult.Subsequent;
            if (String.IsNullOrEmpty(bIdentifier)) return ComparisonResult.Previous;

            aIdentifier = aIdentifier.Replace("-beta.", String.Empty);
            bIdentifier = bIdentifier.Replace("-beta.", String.Empty);

            if (!Int32.TryParse(aIdentifier, out int aIdInt) || !Int32.TryParse(bIdentifier, out int bIdInt)) return ComparisonResult.WrongInput;

            return aIdInt > bIdInt ? ComparisonResult.Subsequent : ComparisonResult.Previous;
        }

        public static ComparisonResult CompareAdapterVersionTo(this string adapterAVersion, string adapterBVersion)
        {
            if (adapterAVersion != null && adapterBVersion != null && adapterAVersion == adapterBVersion) return ComparisonResult.Equal;
            if (!IsValidAdapterVersion(adapterAVersion) || !IsValidAdapterVersion(adapterBVersion)) return ComparisonResult.WrongInput;

            var aMatch = AdapterVersionRegex.Match(adapterAVersion!);
            var bMatch = AdapterVersionRegex.Match(adapterBVersion!);

            var aVersion = new Version(aMatch.Groups[DmConstants.Regex.VersionGroupName].Value);
            var bVersion = new Version(bMatch.Groups[DmConstants.Regex.VersionGroupName].Value);

            int res = aVersion.CompareTo(bVersion);

            if (res < 0) return ComparisonResult.Previous;
            if (res > 0) return ComparisonResult.Subsequent;

            string aIdentifier = aMatch.Groups[DmConstants.Regex.IdentifierGroupName].Value;
            string bIdentifier = bMatch.Groups[DmConstants.Regex.IdentifierGroupName].Value;

            if (aIdentifier == bIdentifier) return ComparisonResult.Equal;
            if (String.IsNullOrEmpty(aIdentifier)) return ComparisonResult.Subsequent;
            if (String.IsNullOrEmpty(bIdentifier)) return ComparisonResult.Previous;

            aIdentifier = aIdentifier.Replace("-beta.", String.Empty);
            bIdentifier = bIdentifier.Replace("-beta.", String.Empty);

            if (!Int32.TryParse(aIdentifier, out int aIdInt) || !Int32.TryParse(bIdentifier, out int bIdInt)) return ComparisonResult.WrongInput;

            return aIdInt > bIdInt ? ComparisonResult.Subsequent : ComparisonResult.Previous;
        }

        private static bool IsValidPluginVersion(Plugin plugin)
        {
            if (String.IsNullOrEmpty(plugin?.version)) return false;
            return PluginVersionRegex.IsMatch(plugin.version);
        }

        private static bool IsValidPluginVersion(string pluginVersion)
        {
            if (String.IsNullOrEmpty(pluginVersion)) return false;
            return PluginVersionRegex.IsMatch(pluginVersion);
        }

        private static bool IsValidAdapterVersion(string adapterVersion)
        {
            if (String.IsNullOrEmpty(adapterVersion)) return false;
            return AdapterVersionRegex.IsMatch(adapterVersion);
        }
    }
}
