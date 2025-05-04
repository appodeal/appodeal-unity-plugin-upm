#if UNITY_IOS
// ReSharper disable CheckNamespace

using System;
using System.IO;
using System.Net;
using System.Text;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using AppodealInc.Mediation.PluginSettings.Editor;

namespace AppodealInc.Mediation.MaxAdReview.Editor
{
    internal static class IosBuildPostProcessor
    {
        [PostProcessBuild(Int32.MaxValue)]
        public static void OnPostProcessBuild(BuildTarget target, string path)
        {
            if (target != BuildTarget.iOS) return;
            SetupMaxAdReview(path);
        }

        private static void SetupMaxAdReview(string buildOutputPath)
        {
            if (!AppodealSettings.Instance.IsMaxAdReviewEnabled) return;
            if (String.IsNullOrWhiteSpace(AppodealSettings.Instance.MaxSdkKey)) return;

            string setupScriptPath = Path.Combine(buildOutputPath, "AppLovinQualityServiceSetup.rb");
            if (File.Exists(setupScriptPath)) return;

            if (!DownloadIosSetupScript(setupScriptPath)) return;

            (int exitCode, _, _) = ShellUtil.RunInBash("ruby", "--version", buildOutputPath);
            if (exitCode != 0)
            {
                Debug.LogError("[IosBuildPostProcessor] AppLovin MAX Ad Review installation requires Ruby. Please install Ruby, export it to your system PATH and re-export the project");
                return;
            }

            (int setupScriptExitCode, string output, string error) = ShellUtil.RunInBash("ruby", setupScriptPath, buildOutputPath);
            if (setupScriptExitCode != 0)
            {
                Debug.LogError($"[IosBuildPostProcessor] AppLovin MAX Ad Review installation failed: {error}");
            }
            else
            {
                Debug.Log($"AppLovin MAX Ad Review installation succeeded: {output}");
            }
        }

        private static bool DownloadIosSetupScript(string setupScriptPath)
        {
            const string url = "https://api2.safedk.com/v1/build/ios_setup2";

            string postJson = $"{{\"sdk_key\":\"{AppodealSettings.Instance.MaxSdkKey}\"}}";
            byte[] bodyRaw = Encoding.UTF8.GetBytes(postJson);

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = bodyRaw.Length;
                request.Timeout = 10_000;

                using (var requestStream = request.GetRequestStream())
                {
                    requestStream.Write(bodyRaw, 0, bodyRaw.Length);
                }

                using var response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    Debug.LogError($"[IosBuildPostProcessor] Download failed: {response.StatusCode}");
                    return false;
                }

                using var responseStream = response.GetResponseStream() ?? throw new InvalidOperationException("No response stream");
                using var fileStream = new FileStream(setupScriptPath, FileMode.Create, FileAccess.Write);
                responseStream.CopyTo(fileStream);

                return File.Exists(setupScriptPath);
            }
            catch (Exception e)
            {
                Debug.LogError($"[IosBuildPostProcessor] Failed to download the setup script: {e.Message}");
                return false;
            }
        }
    }
}
#endif
