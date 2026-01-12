using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal static class DmHttpClient
    {
        private const int WebRequestTimeoutMs = 30_000;

        private static readonly Lazy<HttpClient> LazyHttpClient = new(CreateHttpClient);

        private static HttpClient Client => LazyHttpClient.Value;

        public static async Task<Outcome<string>> GetAsync(string url, CancellationToken cancellationToken = default)
        {
            try
            {
                var uri = new Uri(url);

                using var response = await Client.GetAsync(uri, cancellationToken).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    string errorBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return Failure.Create("HttpError", errorBody);
                }

                string responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return responseBody;
            }
            catch (Exception ex)
            {
                return CreateFailureFromHttpException(ex, url, cancellationToken);
            }
        }

        public static Task<Outcome<string>> PostJsonAsync(string url, string jsonBody, CancellationToken cancellationToken = default)
        {
            return PostAsync(url, jsonBody, "application/json", cancellationToken);
        }

        public static Task<Outcome<string>> PostXmlAsync(string url, string xmlBody, CancellationToken cancellationToken = default)
        {
            return PostAsync(url, xmlBody, "application/xml", cancellationToken);
        }

        private static async Task<Outcome<string>> PostAsync(string url, string body, string contentType, CancellationToken cancellationToken)
        {
            try
            {
                using var content = new StringContent(body, Encoding.UTF8);

                content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                content.Headers.Add("X-Unity-Version", Application.unityVersion);

                var uri = new Uri(url);
                using var response = await Client.PostAsync(uri, content, cancellationToken).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    string errorBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return Failure.Create("HttpError", errorBody);
                }

                string responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return responseBody;
            }
            catch (Exception ex)
            {
                return CreateFailureFromHttpException(ex, url, cancellationToken);
            }
        }

        private static HttpClient CreateHttpClient()
        {
            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip,

#if APPODEAL_DEV
                UseProxy = true,
                Proxy = new WebProxy("127.0.0.1", 8888), // Charles default proxy
#endif
            };

            var client = new HttpClient(handler);

            client.Timeout = TimeSpan.FromMilliseconds(WebRequestTimeoutMs);

            client.DefaultRequestHeaders.UserAgent.Clear();
            client.DefaultRequestHeaders.UserAgent.ParseAdd($"UnityPlayer/{Application.unityVersion}");

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.ParseAdd("*/*");

            client.DefaultRequestHeaders.AcceptEncoding.Clear();
            client.DefaultRequestHeaders.AcceptEncoding.ParseAdd("deflate, gzip");

            return client;
        }

        private static Outcome<string> CreateFailureFromHttpException(Exception ex, string url, CancellationToken cancellationToken)
        {
            switch (ex)
            {
                case OperationCanceledException oce:
                    if (cancellationToken.IsCancellationRequested) { return Failure.Create("Cancelled", "Request was cancelled"); }
                    LogHelper.LogError($"Request timeout for {url}: {oce.Message}");
                    return Failure.Create("Timeout", "Request timed out");
                case HttpRequestException hre:
                    LogHelper.LogError($"HTTP request error for {url}: {hre.Message}");
                    return Failure.Create("HttpRequestException", hre.Message);
                default:
                    LogHelper.LogError($"Unexpected error for {url}: {ex.Message}");
                    return Failure.Create(ex.GetType().ToString(), ex.Message);
            }
        }
    }
}
