// ReSharper disable CheckNamespace

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AppodealInc.Mediation.Analytics.Editor
{
    internal static class AnalyticsHttpClient
    {
        private const int WebRequestTimeoutMs = 10_000;

        public static async Task<bool> PostAsync(string url, string json)
        {
            try
            {
                // var handler = new HttpClientHandler
                // {
                //     UseProxy = true,
                //     Proxy = new System.Net.WebProxy("127.0.0.1", 8888) // Charles default proxy
                // };
                //
                // using var client = new HttpClient(handler);

                using var client = new HttpClient();
                client.Timeout = TimeSpan.FromMilliseconds(WebRequestTimeoutMs);

                client.DefaultRequestHeaders.UserAgent.Clear();
                client.DefaultRequestHeaders.UserAgent.ParseAdd($"UnityPlayer/{Application.unityVersion}");

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.ParseAdd("*/*");

                client.DefaultRequestHeaders.AcceptEncoding.Clear();
                client.DefaultRequestHeaders.AcceptEncoding.ParseAdd("deflate, gzip");

                using var content = new StringContent(json, Encoding.UTF8);

                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                content.Headers.Add("X-Unity-Version", Application.unityVersion);

                var uri = new Uri(url);
                var response = await client.PostAsync(uri, content).ConfigureAwait(false);
                // string httpResponseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return response.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                Logger.Log($"Error sending analytics data to {url}: {e.Message}");
                return false;
            }
        }
    }
}
