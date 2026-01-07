using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal static class VersionComparer
    {
        private static readonly Regex PluginVersionRegex = new(DmConstants.Regex.PluginVersionPattern);
        private static readonly Regex AdapterVersionRegex = new(DmConstants.Regex.AdapterVersionPattern);

        public static async Task<bool> IsLocalDependenciesVersionMatchingPackageVersionAsync()
        {
            var depsReadOutcome = DataLoader.TryReadLocalDependencies();
            if (!depsReadOutcome.IsSuccess)
            {
                LogHelper.LogWarning($"Failed to read local dependencies. Reason: '{depsReadOutcome.Failure.Message}'");
                return false;
            }

            var localDeps = depsReadOutcome.Value;
            if (String.IsNullOrWhiteSpace(localDeps.PluginVersion))
            {
                LogHelper.Log("Local dependencies file has no plugin version specified");
                return false;
            }

            var packageVersionLookupOutcome = await PackageVersionProvider.TryLookupVersionAsync();
            if (!packageVersionLookupOutcome.IsSuccess)
            {
                LogHelper.LogWarning($"Failed to get current plugin version. Reason: '{packageVersionLookupOutcome.Failure.Message}'");
                return false;
            }

            string currentPluginVersion = packageVersionLookupOutcome.Value;

            if (localDeps.PluginVersion == currentPluginVersion) return true;

            LogHelper.LogWarning($"Plugin version mismatch: config has '{localDeps.PluginVersion}', current plugin is '{currentPluginVersion}'");
            return false;
        }

        public static async Task<Outcome<bool>> TryCheckVersionAvailabilityAsync(string pluginVersion, CancellationToken cancellationToken)
        {
            var pluginsFetchOutcome = await DataLoader.FetchAvailablePluginsAsync(cancellationToken);
            if (!pluginsFetchOutcome.IsSuccess) return pluginsFetchOutcome.Failure;

            return IsPluginVersionKnownAndValid(pluginsFetchOutcome.Value, pluginVersion);
        }

        public static Outcome<PluginDto> TrySelectLatestPlugin(List<PluginDto> plugins, bool includeBeta = false)
        {
            if (plugins == null) return Failure.Create("ParamIsNull", $"{nameof(plugins)} param cannot be null");

            plugins = includeBeta
                ? plugins.Where(IsValidPluginVersion).OrderBy(plugin => plugin.id).ToList()
                : plugins.Where(plugin => IsValidPluginVersion(plugin) && !plugin.version.Contains("beta")).OrderBy(plugin => plugin.id).ToList();

            var latest = plugins.FirstOrDefault();
            if (latest == null) return Failure.Create("NoValidVersions", $"{nameof(plugins)} param has no elements after filtering");

            plugins.ForEach(plugin =>
            {
                if (plugin.CompareTo(latest) == VersionComparisonResult.Subsequent) latest = plugin;
            });

            return latest;
        }

        public static Outcome<string> TryExtractVersionFromAndroidSpec(string spec)
        {
            if (String.IsNullOrWhiteSpace(spec)) return Failure.Create("WrongAndroidSpecString", $"'{spec}' is an invalid android spec format");
            const char specPartsSeparator = ':';
            string[] specParts = spec.Split(specPartsSeparator);
            if (specParts.Length != 3) return Failure.Create("WrongAndroidSpecString", $"'{spec}' is an invalid android spec format");
            return specParts[^1];
        }

        public static bool IsValidPluginVersion(string pluginVersion)
        {
            if (String.IsNullOrEmpty(pluginVersion)) return false;
            return PluginVersionRegex.IsMatch(pluginVersion);
        }

        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        public static VersionComparisonResult CompareTo(this PluginDto pluginA, PluginDto pluginB) => pluginA.CompareTo(pluginB.version);

        public static VersionComparisonResult CompareTo(this PluginDto pluginA, string pluginBVersion)
        {
            if (!IsValidPluginVersion(pluginA) || !IsValidPluginVersion(pluginBVersion)) return VersionComparisonResult.WrongInput;

            var aMatch = PluginVersionRegex.Match(pluginA.version);
            var bMatch = PluginVersionRegex.Match(pluginBVersion);

            var aVersion = new Version(aMatch.Groups[DmConstants.Regex.VersionGroupName].Value);
            var bVersion = new Version(bMatch.Groups[DmConstants.Regex.VersionGroupName].Value);

            int res = aVersion.CompareTo(bVersion);

            if (res < 0) return VersionComparisonResult.Previous;
            if (res > 0) return VersionComparisonResult.Subsequent;

            string aIdentifier = aMatch.Groups[DmConstants.Regex.IdentifierGroupName].Value;
            string bIdentifier = bMatch.Groups[DmConstants.Regex.IdentifierGroupName].Value;

            if (aIdentifier == bIdentifier) return VersionComparisonResult.Equal;
            if (String.IsNullOrEmpty(aIdentifier)) return VersionComparisonResult.Subsequent;
            if (String.IsNullOrEmpty(bIdentifier)) return VersionComparisonResult.Previous;

            aIdentifier = aIdentifier.Replace("-beta.", String.Empty);
            bIdentifier = bIdentifier.Replace("-beta.", String.Empty);

            if (!Int32.TryParse(aIdentifier, out int aIdInt) || !Int32.TryParse(bIdentifier, out int bIdInt)) return VersionComparisonResult.WrongInput;

            return aIdInt > bIdInt ? VersionComparisonResult.Subsequent : VersionComparisonResult.Previous;
        }

        public static VersionComparisonResult CompareAdapterVersionTo(this string adapterAVersion, string adapterBVersion)
        {
            if (adapterAVersion != null && adapterBVersion != null && adapterAVersion == adapterBVersion) return VersionComparisonResult.Equal;
            if (!IsValidAdapterVersion(adapterAVersion) || !IsValidAdapterVersion(adapterBVersion)) return VersionComparisonResult.WrongInput;

            var aMatch = AdapterVersionRegex.Match(adapterAVersion!);
            var bMatch = AdapterVersionRegex.Match(adapterBVersion!);

            var aVersion = new Version(aMatch.Groups[DmConstants.Regex.VersionGroupName].Value);
            var bVersion = new Version(bMatch.Groups[DmConstants.Regex.VersionGroupName].Value);

            int res = aVersion.CompareTo(bVersion);

            if (res < 0) return VersionComparisonResult.Previous;
            if (res > 0) return VersionComparisonResult.Subsequent;

            string aIdentifier = aMatch.Groups[DmConstants.Regex.IdentifierGroupName].Value;
            string bIdentifier = bMatch.Groups[DmConstants.Regex.IdentifierGroupName].Value;

            if (aIdentifier == bIdentifier) return VersionComparisonResult.Equal;
            if (String.IsNullOrEmpty(aIdentifier)) return VersionComparisonResult.Subsequent;
            if (String.IsNullOrEmpty(bIdentifier)) return VersionComparisonResult.Previous;

            aIdentifier = aIdentifier.Replace("-beta.", String.Empty);
            bIdentifier = bIdentifier.Replace("-beta.", String.Empty);

            if (!Int32.TryParse(aIdentifier, out int aIdInt) || !Int32.TryParse(bIdentifier, out int bIdInt)) return VersionComparisonResult.WrongInput;

            return aIdInt > bIdInt ? VersionComparisonResult.Subsequent : VersionComparisonResult.Previous;
        }

        private static bool IsPluginVersionKnownAndValid(List<PluginDto> plugins, string version)
        {
            if (String.IsNullOrEmpty(version) || !IsValidPluginVersion(version)) return false;
            return plugins?.Any(plugin => plugin.version == version) ?? false;
        }

        private static bool IsValidPluginVersion(PluginDto plugin)
        {
            if (String.IsNullOrEmpty(plugin?.version)) return false;
            return PluginVersionRegex.IsMatch(plugin.version);
        }

        private static bool IsValidAdapterVersion(string adapterVersion)
        {
            if (String.IsNullOrEmpty(adapterVersion)) return false;
            return AdapterVersionRegex.IsMatch(adapterVersion);
        }
    }
}
