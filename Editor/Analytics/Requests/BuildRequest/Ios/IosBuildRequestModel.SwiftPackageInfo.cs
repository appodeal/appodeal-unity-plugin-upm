// ReSharper disable CheckNamespace

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;

namespace AppodealInc.Mediation.Analytics.Editor
{
    internal sealed partial class IosBuildRequestModel
    {
        [Serializable]
        [SuppressMessage("ReSharper", "NotAccessedField.Global")]
        internal sealed class SwiftPackageInfo
        {
            private static readonly Regex PackageReferencePattern = new(
                @"(?<guid>[0-9A-F]{24}) /\*[^*]*\*/ = \{\s*isa = XCRemoteSwiftPackageReference;\s*repositoryURL = ""?(?<url>[^"";]+)""?;\s*requirement = \{\s*kind = (?<kind>\w+);\s*\w+ = ""?(?<version>[^"";]+)""?;",
                RegexOptions.Compiled);

            private static readonly Regex ProductDependencyPattern = new(
                @"isa = XCSwiftPackageProductDependency;\s*package = (?<guid>[0-9A-F]{24})[^;]*;\s*productName = ""?(?<product>[^"";]+)""?;",
                RegexOptions.Compiled);

            public string url;
            public string requirement;
            public string version;
            public List<string> products = new();

            internal static List<SwiftPackageInfo> ParseFromPbxProject(string content)
            {
                var packagesByGuid = new Dictionary<string, SwiftPackageInfo>();
                var packages = new List<SwiftPackageInfo>();

                foreach (Match match in PackageReferencePattern.Matches(content))
                {
                    var package = new SwiftPackageInfo
                    {
                        url = match.Groups["url"].Value.SanitizeSwiftPackageUrl(),
                        requirement = match.Groups["kind"].Value,
                        version = match.Groups["version"].Value
                    };
                    packagesByGuid[match.Groups["guid"].Value] = package;
                    packages.Add(package);
                }

                foreach (Match match in ProductDependencyPattern.Matches(content))
                {
                    if (packagesByGuid.TryGetValue(match.Groups["guid"].Value, out var package)) package.products.Add(match.Groups["product"].Value);
                }

                return packages.GroupBy(package => (package.url, package.requirement, package.version)).Select(group => group.First()).ToList();
            }
        }
    }
}
