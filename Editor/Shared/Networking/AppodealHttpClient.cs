using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace AppodealInc.Mediation.Shared.Editor
{
    // Every await uses ConfigureAwait(false), so a caller on the main thread may block on the result
    // with GetAwaiter().GetResult(), as the build callbacks do.
    internal static class AppodealHttpClient
    {
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);

        private static readonly Lazy<HttpClient> LazyHttpClient = new(CreateHttpClient);

        private static HttpClient Client => LazyHttpClient.Value;

        public static Task<Outcome<string>> GetAsync(string url, TimeSpan? timeout = null, CancellationToken cancellationToken = default)
        {
            return SendAsync(HttpMethod.Get, url, null, null, timeout, cancellationToken);
        }

        public static Task<Outcome<string>> PostJsonAsync(string url, string jsonBody, TimeSpan? timeout = null, CancellationToken cancellationToken = default)
        {
            return SendAsync(HttpMethod.Post, url, jsonBody, "application/json", timeout, cancellationToken);
        }

        public static Task<Outcome<string>> PostXmlAsync(string url, string xmlBody, TimeSpan? timeout = null, CancellationToken cancellationToken = default)
        {
            return SendAsync(HttpMethod.Post, url, xmlBody, "application/xml", timeout, cancellationToken);
        }

        private static async Task<Outcome<string>> SendAsync(HttpMethod method, string url, string body, string contentType, TimeSpan? timeout, CancellationToken cancellationToken)
        {
            using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            timeoutCts.CancelAfter(timeout ?? DefaultTimeout);

            try
            {
                using var request = new HttpRequestMessage(method, new Uri(url));
                if (body != null) request.Content = CreateContent(body, contentType);

                using var response = await Client.SendAsync(request, timeoutCts.Token).ConfigureAwait(false);

                string responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!response.IsSuccessStatusCode) return Failure.Create("HttpError", responseBody);

                return responseBody;
            }
            catch (OperationCanceledException)
            {
                return cancellationToken.IsCancellationRequested ? Failure.Create("Cancelled", "Request was cancelled") : Failure.Create("Timeout", "Request timed out");
            }
            catch (HttpRequestException hre)
            {
                return Failure.Create("HttpRequestException", hre.Message);
            }
            catch (Exception ex)
            {
                return Failure.Create(ex.GetType().ToString(), ex.Message);
            }
        }

        private static HttpContent CreateContent(string body, string contentType)
        {
            var content = new StringContent(body, Encoding.UTF8);
            content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            content.Headers.Add("X-Unity-Version", Application.unityVersion);
            return content;
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

            // Each request gets its own timeout through a CancellationToken
            client.Timeout = Timeout.InfiniteTimeSpan;

            client.DefaultRequestHeaders.UserAgent.Clear();
            client.DefaultRequestHeaders.UserAgent.ParseAdd($"UnityPlayer/{Application.unityVersion}");

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.ParseAdd("*/*");

            client.DefaultRequestHeaders.AcceptEncoding.Clear();
            client.DefaultRequestHeaders.AcceptEncoding.ParseAdd("deflate, gzip");

            return client;
        }
    }
}
