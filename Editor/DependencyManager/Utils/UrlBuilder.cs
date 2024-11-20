// ReSharper disable CheckNamespace

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal static class UrlBuilder
    {
        public static async Task<Request<string>> GetPluginConfigUrlAsync(List<Plugin> plugins, string version = null)
        {
            if (version == null)
            {
                var versionRequest = await VersionHelper.GetCurrentVersionForPackage(DmConstants.AppodealPackageName);
                if (!versionRequest.IsSuccess) return versionRequest.Error;
                version = versionRequest.Value;
            }

            return VersionHelper.IsPluginVersionKnownAndValid(plugins, version)
                ? $"{DmConstants.BaseUrl}/{version}"
                : Error.Create("VersionIsInvalid", $"{nameof(version)} value has to be valid");
        }

        public static async Task<Request<string>> GetDependenciesUrlAsync(List<Plugin> plugins, PluginConfigResponseModel config, string version = null)
        {
            var urlRequest = await GetPluginConfigUrlAsync(plugins, version);
            if (!urlRequest.IsSuccess) return urlRequest.Error;
            return $"{urlRequest.Value}/dependencies{GetPayloadFromUserChoices(config)}";
        }

        public static async Task<Request<string>> GetLocalDependenciesUrlAsync(DmConfigDto config, string version = null)
        {
            var urlRequest = await GetPluginConfigUrlAsync(config.Plugins, version);
            if (!urlRequest.IsSuccess) return urlRequest.Error;
            return $"{urlRequest.Value}/dependencies{GetPayloadFromLocalDeps(config.LocalDeps, config.RemotePluginConfig)}";
        }

        private static string GetPayloadFromUserChoices(PluginConfigResponseModel config)
        {
            var adapters = new List<int>();

            config?.ios?.adapters?.ForEach(adapter =>
            {
                if (adapter.ShouldAdd()) adapters.Add(adapter.id);
            });

            config?.android?.adapters?.ForEach(adapter =>
            {
                if (adapter.ShouldAdd()) adapters.Add(adapter.id);
            });

            return adapters.Count > 0 ? $"?{String.Join('&', adapters.Distinct().OrderBy(el => el).Select(el => $"adapter={el}"))}" : String.Empty;
        }

        private static string GetPayloadFromLocalDeps(XmlDependenciesModel localDeps, PluginConfigResponseModel config)
        {
            var adapters = new List<int>();

            localDeps?.Ios?.Pods?.ForEach(pod =>
            {
                var remoteAdapter = config?.ios?.adapters?.FirstOrDefault(adapter => adapter.status == pod.Id);
                if (remoteAdapter != null) adapters.Add(remoteAdapter.id);
            });

            localDeps?.Android?.Packages?.ForEach(package =>
            {
                var remoteAdapter = config?.android?.adapters?.FirstOrDefault(adapter => adapter.status == package.Id);
                if (remoteAdapter != null) adapters.Add(remoteAdapter.id);
            });

            return adapters.Count > 0 ? $"?{String.Join('&', adapters.Distinct().OrderBy(el => el).Select(el => $"adapter={el}"))}" : String.Empty;
        }
    }
}
