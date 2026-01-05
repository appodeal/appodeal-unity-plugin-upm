using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal static class DataLoader
    {
        public static Outcome<XmlDependencies> TryReadLocalDependencies()
        {
            try
            {
                var fileInfo = new FileInfo(DmConstants.Validation.DependenciesFilePath);

                var deserializationOutcome = XmlHandler.TryDeserializeDependencies(fileInfo);
                return deserializationOutcome.IsSuccess ? deserializationOutcome.Value : deserializationOutcome.Failure;
            }
            catch (Exception ex)
            {
                return Failure.Create(ex.GetType().ToString(), ex.Message);
            }
        }

        public static async Task<Outcome<List<PluginDto>>> FetchAvailablePluginsAsync(CancellationToken cancellationToken = default)
        {
            var fetchOutcome = await DmHttpClient.GetAsync(DmConstants.Endpoints.Plugins, cancellationToken);
            if (!fetchOutcome.IsSuccess) return fetchOutcome.Failure;

            try
            {
                var responseDto = JsonUtility.FromJson<AvailablePluginsResponseDto>(fetchOutcome.Value);
                if (responseDto?.plugins == null) return Failure.Create("DeserializationError", "Available plugins response is null");
                return responseDto.plugins;
            }
            catch (Exception ex)
            {
                return Failure.Create("JsonDeserializationError", $"Failed to deserialize available plugins: {ex.Message}");
            }
        }

        public static async Task<Outcome<WizardResponse>> FetchMediationsAsync(string version, CancellationToken cancellationToken)
        {
            var urlBuildOutcome = UrlBuilder.TryBuildMediationsUrl(version);
            if (!urlBuildOutcome.IsSuccess) return urlBuildOutcome.Failure;

            if (cancellationToken.IsCancellationRequested) return Failure.Create("Cancelled", "Request was cancelled");

            var fetchOutcome = await DmHttpClient.GetAsync(urlBuildOutcome.Value, cancellationToken);
            if (!fetchOutcome.IsSuccess) return fetchOutcome.Failure;

            try
            {
                var response = JsonUtility.FromJson<WizardResponseDto>(fetchOutcome.Value)?.ToDomain();
                if (response == null) return Failure.Create("DeserializationError", "Mediations response is null");
                return response;
            }
            catch (Exception ex)
            {
                return Failure.Create("JsonDeserializationError", $"Failed to deserialize mediations: {ex.Message}");
            }
        }

        public static async Task<Outcome<WizardResponse>> FetchComponentsAsync(WizardRequestDto requestDto, string version, CancellationToken cancellationToken)
        {
            var urlBuildOutcome = UrlBuilder.TryBuildComponentsUrl(version);
            if (!urlBuildOutcome.IsSuccess) return urlBuildOutcome.Failure;

            if (cancellationToken.IsCancellationRequested) return Failure.Create("Cancelled", "Request was cancelled");

            try
            {
                string jsonBody = JsonUtility.ToJson(requestDto);
                var fetchOutcome = await DmHttpClient.PostJsonAsync(urlBuildOutcome.Value, jsonBody, cancellationToken);
                if (!fetchOutcome.IsSuccess) return fetchOutcome.Failure;

                var response = JsonUtility.FromJson<WizardResponseDto>(fetchOutcome.Value)?.ToDomain();
                if (response == null) return Failure.Create("DeserializationError", "Components response is null");
                return response;
            }
            catch (Exception ex)
            {
                return Failure.Create("JsonProcessingError", $"Failed to process components: {ex.Message}");
            }
        }

        public static async Task<Outcome<string>> FetchDependenciesAsync(WizardRequestDto requestDto, string version, CancellationToken cancellationToken)
        {
            var urlBuildOutcome = UrlBuilder.TryBuildDependenciesUrl(version);
            if (!urlBuildOutcome.IsSuccess) return urlBuildOutcome.Failure;

            if (cancellationToken.IsCancellationRequested) return Failure.Create("Cancelled", "Request was cancelled");

            try
            {
                string jsonBody = JsonUtility.ToJson(requestDto);
                return await DmHttpClient.PostJsonAsync(urlBuildOutcome.Value, jsonBody, cancellationToken);
            }
            catch (Exception ex)
            {
                return Failure.Create("JsonSerializationError", $"Failed to serialize dependencies request: {ex.Message}");
            }
        }

        public static async Task<Outcome<ValidationResponse>> ValidateDependenciesAsync(string xmlContent)
        {
            var fetchOutcome = await DmHttpClient.PostXmlAsync(DmConstants.Endpoints.Validate, xmlContent);
            if (!fetchOutcome.IsSuccess)
            {
                if (fetchOutcome.Failure.Type != "HttpError") return fetchOutcome.Failure;

                try
                {
                    var errorDto = JsonUtility.FromJson<ValidationErrorResponseDto>(fetchOutcome.Failure.Message);
                    if (errorDto != null && !String.IsNullOrWhiteSpace(errorDto.error))
                    {
                        return Failure.Create("ValidationError", errorDto.error);
                    }
                }
                catch
                {
                    // Fall through to return original error
                }

                return fetchOutcome.Failure;
            }

            try
            {
                var response = JsonUtility.FromJson<ValidationResponseDto>(fetchOutcome.Value)?.ToDomain();
                if (response == null) return Failure.Create("DeserializationError", "Validation response is null");
                return response;
            }
            catch (Exception ex)
            {
                return Failure.Create("JsonDeserializationError", $"Failed to deserialize validation response: {ex.Message}");
            }
        }
    }
}
