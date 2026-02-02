using System;
using System.Collections.Generic;
using System.Linq;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal class DependenciesDiff
    {
        private List<PodNode> AddedIosPods { get; set; }
        private List<PodNode> RemovedIosPods  { get; set; }
        private List<(PodNode Old, PodNode New)> UpdatedIosPods { get; set; }
        private List<(PodNode Old, PodNode New)> DowngradedIosPods { get; set; }

        private List<PackageNode> AddedAndroidPackages { get; set; }
        private List<PackageNode> RemovedAndroidPackages  { get; set; }
        private List<(PackageNode Old, PackageNode New)> UpdatedAndroidPackages { get; set; }
        private List<(PackageNode Old, PackageNode New)> DowngradedAndroidPackages { get; set; }

        private DependenciesDiff() { }

        public static DependenciesDiff Get(XmlDependencies local, XmlDependencies remote)
        {
            var localIosPodIds = local?.Ios?.Pods?.Select(p => p.Id).ToHashSet() ?? new HashSet<string>();
            var addedIosPods = remote?.Ios?.Pods?
                .Where(rPod => !localIosPodIds.Contains(rPod.Id)).ToList() ?? new List<PodNode>();

            var remoteIosPodIds = remote?.Ios?.Pods?.Select(p => p.Id).ToHashSet() ?? new HashSet<string>();
            var removedIosPods = local?.Ios?.Pods?
                .Where(lPod => !remoteIosPodIds.Contains(lPod.Id)).ToList() ?? new List<PodNode>();

            var updatedIosPods = new List<(PodNode Old, PodNode New)>();
            var downgradedIosPods = new List<(PodNode Old, PodNode New)>();
            remote?.Ios?.Pods?.ForEach(rPod =>
            {
                var lPod = local?.Ios?.Pods?.FirstOrDefault(pod => pod.Id == rPod.Id);
                if (lPod == null) return;
                var comparisonResult = lPod.Version.CompareAdapterVersionTo(rPod.Version);
                if (comparisonResult == VersionComparisonResult.WrongInput)
                {
                    LogHelper.LogWarning($"Invalid version format for iOS pod '{lPod.Id}': '{lPod.Version}' vs '{rPod.Version}'");
                    return;
                }
                if (comparisonResult == VersionComparisonResult.Previous) updatedIosPods.Add((lPod, rPod));
                else if (comparisonResult == VersionComparisonResult.Subsequent) downgradedIosPods.Add((lPod, rPod));
            });

            var localAndroidPackageIds = local?.Android?.Packages?.Select(p => p.Id).ToHashSet() ?? new HashSet<string>();
            var addedAndroidPackages = remote?.Android?.Packages?
                .Where(rPkg => !localAndroidPackageIds.Contains(rPkg.Id)).ToList() ?? new List<PackageNode>();

            var remoteAndroidPackageIds = remote?.Android?.Packages?.Select(p => p.Id).ToHashSet() ?? new HashSet<string>();
            var removedAndroidPackages = local?.Android?.Packages?
                .Where(lPkg => !remoteAndroidPackageIds.Contains(lPkg.Id)).ToList() ?? new List<PackageNode>();

            var updatedAndroidPackages = new List<(PackageNode Old, PackageNode New)>();
            var downgradedAndroidPackages = new List<(PackageNode Old, PackageNode New)>();
            remote?.Android?.Packages?.ForEach(rPkg =>
            {
                var lPkg = local?.Android?.Packages?.FirstOrDefault(pkg => pkg.Id == rPkg.Id);
                if (lPkg == null) return;
                var lSpecVersionExtractOutcome = VersionComparer.TryExtractVersionFromAndroidSpec(lPkg.Spec);
                var rSpecVersionExtractOutcome = VersionComparer.TryExtractVersionFromAndroidSpec(rPkg.Spec);
                if (!lSpecVersionExtractOutcome.IsSuccess || !rSpecVersionExtractOutcome.IsSuccess) return;
                var comparisonResult = lSpecVersionExtractOutcome.Value.CompareAdapterVersionTo(rSpecVersionExtractOutcome.Value);
                if (comparisonResult == VersionComparisonResult.WrongInput)
                {
                    LogHelper.LogWarning($"Invalid version format for Android package '{lPkg.Id}': '{lSpecVersionExtractOutcome.Value}' vs '{rSpecVersionExtractOutcome.Value}'");
                    return;
                }
                if (comparisonResult == VersionComparisonResult.Previous) updatedAndroidPackages.Add((lPkg, rPkg));
                else if (comparisonResult == VersionComparisonResult.Subsequent) downgradedAndroidPackages.Add((lPkg, rPkg));
            });

            return new DependenciesDiff
            {
                AddedIosPods = addedIosPods,
                RemovedIosPods = removedIosPods,
                UpdatedIosPods = updatedIosPods,
                DowngradedIosPods = downgradedIosPods,
                AddedAndroidPackages = addedAndroidPackages,
                RemovedAndroidPackages = removedAndroidPackages,
                UpdatedAndroidPackages = updatedAndroidPackages,
                DowngradedAndroidPackages = downgradedAndroidPackages
            };
        }

        public bool Any()
        {
            return AddedIosPods?.Count > 0 ||
                   RemovedIosPods?.Count > 0 ||
                   UpdatedIosPods?.Count > 0 ||
                   DowngradedIosPods?.Count > 0 ||
                   AddedAndroidPackages?.Count > 0 ||
                   RemovedAndroidPackages?.Count > 0 ||
                   UpdatedAndroidPackages?.Count > 0 ||
                   DowngradedAndroidPackages?.Count > 0;
        }

        public override string ToString()
        {
            var changes = new List<string>();

            if (AddedIosPods?.Count > 0)
            {
                string addedIosAdapters = String.Join("\n", AddedIosPods.Where(pod => pod != null).Select(pod => pod.Name));
                changes.Add($"Added iOS adapters:\n{addedIosAdapters}");
            }

            if (RemovedIosPods?.Count > 0)
            {
                string removedIosAdapters = String.Join("\n", RemovedIosPods.Where(pod => pod != null).Select(pod => pod.Name));
                changes.Add($"Removed iOS adapters:\n{removedIosAdapters}");
            }

            if (UpdatedIosPods?.Count > 0)
            {
                string updatedIosAdapters = String.Join("\n", UpdatedIosPods
                    .Where(tuple => tuple.Old != null && tuple.New != null)
                    .Select(tuple => $"{tuple.Old.Name} v{tuple.Old.Version} --> v{tuple.New.Version}"));
                changes.Add($"Updated iOS adapters:\n{updatedIosAdapters}");
            }

            if (DowngradedIosPods?.Count > 0)
            {
                string downgradedIosAdapters = String.Join("\n", DowngradedIosPods
                    .Where(tuple => tuple.Old != null && tuple.New != null)
                    .Select(tuple => $"{tuple.Old.Name} v{tuple.Old.Version} --> v{tuple.New.Version}"));
                changes.Add($"Downgraded iOS adapters:\n{downgradedIosAdapters}");
            }

            if (AddedAndroidPackages?.Count > 0)
            {
                string addedAndroidPackages = String.Join("\n", AddedAndroidPackages
                    .Where(pkg => pkg != null)
                    .Select(pkg => ExtractAndroidPackageName(pkg.Spec)));
                changes.Add($"Added Android adapters:\n{addedAndroidPackages}");
            }

            if (RemovedAndroidPackages?.Count > 0)
            {
                string removedAndroidPackages = String.Join("\n", RemovedAndroidPackages
                    .Where(pkg => pkg != null)
                    .Select(pkg => ExtractAndroidPackageName(pkg.Spec)));
                changes.Add($"Removed Android adapters:\n{removedAndroidPackages}");
            }

            if (UpdatedAndroidPackages?.Count > 0)
            {
                string updatedAndroidPackages = String.Join("\n", UpdatedAndroidPackages
                    .Where(tuple => tuple.Old != null && tuple.New != null)
                    .Select(tuple => FormatAndroidPackageUpdate(tuple.Old.Spec, tuple.New.Spec)));
                changes.Add($"Updated Android adapters:\n{updatedAndroidPackages}");
            }

            if (DowngradedAndroidPackages?.Count > 0)
            {
                string downgradedAndroidPackages = String.Join("\n", DowngradedAndroidPackages
                    .Where(tuple => tuple.Old != null && tuple.New != null)
                    .Select(tuple => FormatAndroidPackageUpdate(tuple.Old.Spec, tuple.New.Spec)));
                changes.Add($"Downgraded Android adapters:\n{downgradedAndroidPackages}");
            }

            return changes.Count > 0 ? String.Join("\n\n", changes) : String.Empty;
        }

        private static string ExtractAndroidPackageName(string spec)
        {
            if (String.IsNullOrWhiteSpace(spec)) return spec ?? String.Empty;
            int lastColonIndex = spec.LastIndexOf(':');
            return lastColonIndex > 0 ? spec[..lastColonIndex] : spec;
        }

        private static string FormatAndroidPackageUpdate(string oldSpec, string newSpec)
        {
            string packageName = ExtractAndroidPackageName(oldSpec);
            string oldVersion = ExtractAndroidPackageVersion(oldSpec);
            string newVersion = ExtractAndroidPackageVersion(newSpec);
            return $"{packageName} v{oldVersion} --> v{newVersion}";
        }

        private static string ExtractAndroidPackageVersion(string spec)
        {
            if (String.IsNullOrWhiteSpace(spec)) return String.Empty;
            int lastColonIndex = spec.LastIndexOf(':');
            return lastColonIndex >= 0 && lastColonIndex < spec.Length - 1
                ? spec[(lastColonIndex + 1)..]
                : String.Empty;
        }
    }
}
