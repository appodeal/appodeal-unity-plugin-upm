// ReSharper disable CheckNamespace

using System;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace AppodealInc.Mediation.Analytics.Editor
{
    internal static class AnalyticsHttpClient
    {
        private const int WebRequestTimeoutMs = 10_000;

        public static async Task<bool> PostAsync(string url, string json)
        {
            UnityWebRequest request = null;

            try
            {
#if UNITY_2022_3_OR_NEWER
                request = UnityWebRequest.Post(url, json, "application/json");
#else
                byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(json);
                request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST)
                {
                    uploadHandler = new UploadHandlerRaw(jsonBytes),
                    downloadHandler = new DownloadHandlerBuffer()
                };
                request.SetRequestHeader("Content-Type", "application/json");
#endif
                request.timeout = WebRequestTimeoutMs / 1_000;

                _ = request.SendWebRequest();
                while (!request.isDone) await Task.Yield();
                return request.result == UnityWebRequest.Result.Success;
            }
            catch (Exception e)
            {
                Logger.Log($"Error sending analytics data to {url}: {e.Message}");
                return false;
            }
            finally
            {
                request?.Dispose();
            }
        }
    }
}
