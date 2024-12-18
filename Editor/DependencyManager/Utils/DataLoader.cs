// ReSharper disable CheckNamespace

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal static class DataLoader
    {
        public static async Task<(Guid sessionId, Request<DmConfigDto> configRequest)> GetConfigAsync(Guid sessionId, string version = null)
        {
            XmlDependenciesModel localDeps = null;
            var localDepsRequest = GetLocalDependenciesSync();
            if (localDepsRequest.IsSuccess) localDeps = localDepsRequest.Value;
            else LogHelper.LogWarning($"Local dependencies fetching failed. Reason - '{localDepsRequest.Error.Message}'.");

            List<Plugin> availablePlugins;
            var availablePluginsRequest = await GetAvailablePluginsAsync();
            if (availablePluginsRequest.IsSuccess) availablePlugins = availablePluginsRequest.Value;
            else return (sessionId, availablePluginsRequest.Error);

            PluginConfigResponseModel pluginConfig;
            var pluginConfigUrlRequest = await UrlBuilder.GetPluginConfigUrlAsync(availablePlugins, version);
            if (!pluginConfigUrlRequest.IsSuccess) return (sessionId, pluginConfigUrlRequest.Error);
            var pluginConfigRequest = await GetPluginConfigAsync(pluginConfigUrlRequest.Value);
            if (pluginConfigRequest.IsSuccess) pluginConfig = pluginConfigRequest.Value;
            else return (sessionId, pluginConfigRequest.Error);

            var config = new DmConfigDto
            {
                LocalDeps = localDeps,
                Plugins = availablePlugins,
                RemotePluginConfig = pluginConfig
            };

            return (sessionId, config);
        }

        public static async Task<Request<string>> GetRemoteDependenciesAsync(string url)
        {
            try
            {
                var request = await SendWebRequestAsync(url);
                if (request.result != UnityWebRequest.Result.Success) return Error.Create("WebRequestError", request.error);
                return request.downloadHandler.text;
            }
            catch (Exception e)
            {
                return Error.Create(e.GetType().ToString(), e.Message);
            }
        }

        public static Request<FileInfo> GetDependenciesFileInfo()
        {
            try
            {
                return new FileInfo(DmConstants.DependenciesFilePath);
            }
            catch (Exception e)
            {
                return Error.Create(e.GetType().ToString(), e.Message);
            }
        }

        private static Request<XmlDependenciesModel> GetLocalDependenciesSync()
        {
            var fileInfoRequest = GetDependenciesFileInfo();
            if (!fileInfoRequest.IsSuccess) return fileInfoRequest.Error;

            var xmlDepsModelRequest = XmlHandler.DeserializeDependencies(fileInfoRequest.Value);
            return xmlDepsModelRequest.IsSuccess ? xmlDepsModelRequest.Value : xmlDepsModelRequest.Error;
        }

        private static async Task<Request<List<Plugin>>> GetAvailablePluginsAsync()
        {
            try
            {
                var request = await SendWebRequestAsync(DmConstants.BaseUrl);
                if (request.result != UnityWebRequest.Result.Success) return Error.Create("WebRequestError", request.error);
                return JsonUtility.FromJson<AvailablePluginsResponseModel>(request.downloadHandler.text).plugins;
            }
            catch (Exception e)
            {
                return Error.Create(e.GetType().ToString(), e.Message);
            }
        }

        private static async Task<Request<PluginConfigResponseModel>> GetPluginConfigAsync(string url)
        {
            try
            {
                var request = await SendWebRequestAsync(url);
                if (request.result != UnityWebRequest.Result.Success) return Error.Create("WebRequestError", request.error);
                return JsonUtility.FromJson<PluginConfigResponseModel>(request.downloadHandler.text);
            }
            catch (Exception e)
            {
                return Error.Create(e.GetType().ToString(), e.Message);
            }
        }

        private static async Task<UnityWebRequest> SendWebRequestAsync(string url)
        {
            var request = UnityWebRequest.Get(url);
            _ = request.SendWebRequest();
            while (!request.isDone) await Task.Yield();
            return request;
        }
    }
}
