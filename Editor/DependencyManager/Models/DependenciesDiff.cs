using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal class DependenciesDiff
    {
        private List<PodNode> AddedIosPods { get; set; }
        private List<PodNode> RemovedIosPods  { get; set; }
        private List<List<PodNode>> UpdatedIosPods { get; set; }
        private List<List<PodNode>> DowngradedIosPods { get; set; }

        private List<PackageNode> AddedAndroidPackages { get; set; }
        private List<PackageNode> RemovedAndroidPackages  { get; set; }
        private List<List<PackageNode>> UpdatedAndroidPackages { get; set; }
        private List<List<PackageNode>> DowngradedAndroidPackages { get; set; }

        private DependenciesDiff() { }

        public static DependenciesDiff Get(XmlDependenciesModel local, XmlDependenciesModel remote)
        {
            var addedIosPods = remote?.Ios?.Pods?
                .Where(rPod => local?.Ios?.Pods?.Any(lPod => lPod.Id == rPod.Id) == false).ToList() ?? new List<PodNode>();
            var removedIosPods = local?.Ios?.Pods?
                .Where(lPod => remote?.Ios?.Pods?.Any(rPod => rPod.Id == lPod.Id) == false).ToList() ?? new List<PodNode>();
            var updatedIosPods = new List<List<PodNode>>();
            var downgradedIosPods = new List<List<PodNode>>();
            remote?.Ios?.Pods?.ForEach(rPod =>
            {
                var lPod = local?.Ios?.Pods?.FirstOrDefault(pod => pod.Id == rPod.Id);
                if (lPod == null) return;
                var comparisonResult = lPod.Version.CompareAdapterVersionTo(rPod.Version);
                if (comparisonResult == ComparisonResult.Previous) updatedIosPods.Add(new List<PodNode> { lPod, rPod });
                else if (comparisonResult == ComparisonResult.Subsequent) downgradedIosPods.Add(new List<PodNode> { lPod, rPod });
            });

            var addedAndroidPackages = remote?.Android?.Packages?
                .Where(rPkg => local?.Android?.Packages?.Any(lPkg => lPkg.Id == rPkg.Id) == false).ToList() ?? new List<PackageNode>();
            var removedAndroidPackages = local?.Android?.Packages?
                .Where(lPkg => remote?.Android?.Packages?.Any(rPkg => rPkg.Id == lPkg.Id) == false).ToList() ?? new List<PackageNode>();
            var updatedAndroidPackages = new List<List<PackageNode>>();
            var downgradedAndroidPackages = new List<List<PackageNode>>();
            remote?.Android?.Packages?.ForEach(rPkg =>
            {
                var lPkg = local?.Android?.Packages?.FirstOrDefault(pkg => pkg.Id == rPkg.Id);
                if (lPkg == null) return;
                var lPkgVersionRequest = VersionHelper.GetVersionFromAndroidSpec(lPkg.Spec);
                var rPkgVersionRequest = VersionHelper.GetVersionFromAndroidSpec(rPkg.Spec);
                if (!lPkgVersionRequest.IsSuccess || !rPkgVersionRequest.IsSuccess) return;
                var comparisonResult = lPkgVersionRequest.Value.CompareAdapterVersionTo(rPkgVersionRequest.Value);
                if (comparisonResult == ComparisonResult.Previous) updatedAndroidPackages.Add(new List<PackageNode> { lPkg, rPkg });
                else if (comparisonResult == ComparisonResult.Subsequent) downgradedAndroidPackages.Add(new List<PackageNode> { lPkg, rPkg });
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
            if (AddedIosPods?.Count > 0) changes.Add($"Added iOS adapters:\n{String.Join("\n", AddedIosPods.Select(pod => pod.Name))}");
            if (RemovedIosPods?.Count > 0) changes.Add($"Removed iOS adapters:\n{String.Join("\n", RemovedIosPods!.Select(pod => pod.Name))}");
            if (UpdatedIosPods?.Count > 0)
            {
                string updatedIosAdapters = String.Join("\n", UpdatedIosPods.Select(gr => $"{gr[0].Name} v{gr[0].Version} --> v{gr[1].Version}"));
                changes.Add($"Updated iOS adapters:\n{updatedIosAdapters}");
            }
            if (DowngradedIosPods?.Count > 0)
            {
                string downgradedIosAdapters = String.Join("\n", DowngradedIosPods.Select(gr => $"{gr[0].Name} v{gr[0].Version} --> v{gr[1].Version}"));
                changes.Add($"Downgraded iOS adapters:\n{downgradedIosAdapters}");
            }
            if (AddedAndroidPackages?.Count > 0)
            {
                string addedAndroidPackages = String.Join("\n", AddedAndroidPackages.Select(pkg => pkg.Spec[..pkg.Spec.LastIndexOf(':')]));
                changes.Add($"Added Android adapters:\n{addedAndroidPackages}");
            }
            if (RemovedAndroidPackages?.Count > 0)
            {
                string removedAndroidPackages = String.Join("\n", RemovedAndroidPackages!.Select(pkg => pkg.Spec[..pkg.Spec.LastIndexOf(':')]));
                changes.Add($"Removed Android adapters:\n{removedAndroidPackages}");
            }
            if (UpdatedAndroidPackages?.Count > 0)
            {
                string updatedAndroidPackages = String.Join("\n", UpdatedAndroidPackages.Select(gr =>
                    $"{gr[0].Spec[..gr[0].Spec.LastIndexOf(':')]} v{gr[0].Spec[(gr[0].Spec.LastIndexOf(':') + 1)..]} --> v{gr[1].Spec[(gr[1].Spec.LastIndexOf(':') + 1)..]}"));
                changes.Add($"Updated Android adapters:\n{updatedAndroidPackages}");
            }
            if (DowngradedAndroidPackages?.Count > 0)
            {
                string downgradedAndroidPackages = String.Join("\n", DowngradedAndroidPackages.Select(gr =>
                    $"{gr[0].Spec[..gr[0].Spec.LastIndexOf(':')]} v{gr[0].Spec[(gr[0].Spec.LastIndexOf(':') + 1)..]} --> v{gr[1].Spec[(gr[1].Spec.LastIndexOf(':') + 1)..]}"));
                changes.Add($"Downgraded android adapters:\n{downgradedAndroidPackages}");
            }

            return changes.Count > 0 ? String.Join("\n\n", changes) : String.Empty;
        }
    }
}
