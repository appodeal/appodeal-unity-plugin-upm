using System;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal static class UrlBuilder
    {
        public static Outcome<string> TryBuildMediationsUrl(string version) => TryBuildUrl(version, "mediations");

        public static Outcome<string> TryBuildComponentsUrl(string version) => TryBuildUrl(version, "sdks");

        public static Outcome<string> TryBuildDependenciesUrl(string version) => TryBuildUrl(version, "dependencies");

        private static Outcome<string> TryBuildUrl(string version, string endpoint)
        {
            if (!VersionComparer.IsValidPluginVersion(version))
            {
                return Failure.Create("WrongVersionFormat", $"'{version}' is an invalid plugin version format");
            }

            if (String.IsNullOrWhiteSpace(endpoint))
            {
                return Failure.Create("WrongEndpoint", $"'{endpoint}' is an invalid endpoint");
            }

            return $"{DmConstants.Endpoints.BaseUrl}/{version}/{endpoint}";
        }
    }
}
