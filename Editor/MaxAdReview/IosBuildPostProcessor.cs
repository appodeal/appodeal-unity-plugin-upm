#if UNITY_IOS || APPODEAL_DEV
// ReSharper disable CheckNamespace

using System;
using System.IO;
using System.Net;
using System.Text;
using UnityEditor;
using UnityEditor.Callbacks;
using AppodealInc.Mediation.PluginSettings.Editor;

namespace AppodealInc.Mediation.MaxAdReview.Editor
{
    internal static class IosBuildPostProcessor
    {
        private const string MaxPod = "APDAppLovinMAXAdapter";
        private const string ScriptName = "AppLovinQualityServiceSetup.rb";
        private const string IosSetupUrl = "https://api2.safedk.com/v1/build/ios_setup2";

        [PostProcessBuild(Int32.MaxValue)]
        public static void OnPostProcessBuild(BuildTarget target, string path)
        {
            if (target != BuildTarget.iOS) return;
            if (AppodealSettings.Instance == null) return;

            SetupMaxAdReview(path);
        }

        private static void SetupMaxAdReview(string buildOutputPath)
        {
            if (!AppodealSettings.Instance.IsMaxAdReviewEnabled) return;

            if (String.IsNullOrWhiteSpace(AppodealSettings.Instance.MaxSdkKey))
            {
                AdReviewHelper.LogWarning("SDK key is missing --> Ad Review installation skipped");
                return;
            }

            string podfilePath = Path.GetFullPath(Path.Combine(buildOutputPath, "Podfile"));
            try
            {
                string podfileContents = File.ReadAllText(podfilePath);
                if (!podfileContents.Contains(MaxPod))
                {
                    AdReviewHelper.LogWarning("AppLovin MAX dependency not found --> Ad Review installation skipped");
                    return;
                }
            }
            catch (Exception e)
            {
                AdReviewHelper.LogError($"Failed to read '{podfilePath}' to check for AppLovin MAX dependency presence --> Ad Review installation skipped. Error: '{e.Message}'");
                return;
            }

            string setupScriptPath = Path.GetFullPath(Path.Combine(buildOutputPath, ScriptName));
            if (File.Exists(setupScriptPath)) return;

            if (!DownloadIosSetupScript(setupScriptPath)) return;

            (int exitCode, _, _) = ShellUtil.RunInBash("ruby", "--version", buildOutputPath);
            if (exitCode != 0)
            {
                AdReviewHelper.LogError("iOS installation requires Ruby. Please install Ruby, export it to your system PATH and re-export the project");
                return;
            }

            (int setupScriptExitCode, string output, string error) = ShellUtil.RunInBash("ruby", setupScriptPath, buildOutputPath);
            if (setupScriptExitCode != 0)
            {
                AdReviewHelper.LogError($"Installation of Ad Review failed. Error: '{error}'");
            }
            else
            {
                AdReviewHelper.Log($"Ad Review was successfully installed. Output: '{output}'");
            }
        }

        private static bool DownloadIosSetupScript(string setupScriptPath)
        {
            string postJson = $"{{\"sdk_key\":\"{AppodealSettings.Instance.MaxSdkKey}\"}}";
            byte[] bodyRaw = Encoding.UTF8.GetBytes(postJson);

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(IosSetupUrl);
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
                    AdReviewHelper.LogError($"Setup script downloading failed --> Ad Review installation skipped. StatusCode: '{response.StatusCode}'");
                    return false;
                }

                using var responseStream = response.GetResponseStream() ?? throw new InvalidOperationException("No response stream");
                using var fileStream = new FileStream(setupScriptPath, FileMode.Create, FileAccess.Write);
                responseStream.CopyTo(fileStream);

                return File.Exists(setupScriptPath);
            }
            catch (Exception e)
            {
                AdReviewHelper.LogError($"Failed to download the setup script --> Ad Review installation skipped. Error: '{e.Message}'");
                return false;
            }
        }
    }
}
#endif
