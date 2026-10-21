#if UNITY_IOS || APPODEAL_DEV
using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;
using AppodealInc.Mediation.PluginSettings.Editor;
using AppodealInc.Mediation.Shared.Editor;

namespace AppodealInc.Mediation.MaxAdReview.Editor
{
    internal static class IosBuildPostProcessor
    {
        private static readonly AppodealLogger Log = MaxAdReviewLogging.Log;

        private const string MaxPod = "AppodealAppLovinMAXAdapter";
        private const string ScriptName = "AppLovinQualityServiceSetup.rb";
        private const string IosSetupUrl = ServiceHosts.SafeDk + "/v1/build/ios_setup2";

        private static readonly TimeSpan RequestTimeout = TimeSpan.FromSeconds(10);

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
                Log.Warning("SDK key is missing --> Ad Review installation skipped");
                return;
            }

            try
            {
                if (!IsMaxAdapterLinked(buildOutputPath))
                {
                    Log.Warning("AppLovin MAX dependency not found --> Ad Review installation skipped");
                    return;
                }
            }
            catch (Exception e)
            {
                Log.Error($"Failed to check for AppLovin MAX dependency presence --> Ad Review installation skipped. Error: '{e.Message}'");
                return;
            }

            string setupScriptPath = Path.GetFullPath(Path.Combine(buildOutputPath, ScriptName));
            if (File.Exists(setupScriptPath)) return;

            if (!DownloadIosSetupScript(setupScriptPath)) return;

            (int exitCode, _, _) = ShellUtil.RunInBash("ruby", "--version", buildOutputPath);
            if (exitCode != 0)
            {
                Log.Error("iOS installation requires Ruby. Please install Ruby, export it to your system PATH and re-export the project");
                return;
            }

            (int setupScriptExitCode, string output, string error) = ShellUtil.RunInBash("ruby", setupScriptPath, buildOutputPath);
            if (setupScriptExitCode != 0)
            {
                Log.Error($"Installation of Ad Review failed. Error: '{error}'");
            }
            else
            {
                Log.Info($"Ad Review was successfully installed. Output: '{output}'");
            }
        }

        private static bool IsMaxAdapterLinked(string buildOutputPath)
        {
            string pbxProjectPath = PBXProject.GetPBXProjectPath(buildOutputPath);
            if (Regex.IsMatch(File.ReadAllText(pbxProjectPath), $@"productName\s*=\s*""?{MaxPod}""?\s*;")) return true;

            string podfilePath = $"{buildOutputPath}/Podfile";
            return File.Exists(podfilePath) && Regex.IsMatch(File.ReadAllText(podfilePath), $@"^\s*pod\s+['""]{MaxPod}['""]", RegexOptions.Multiline);
        }

        private static bool DownloadIosSetupScript(string setupScriptPath)
        {
            try
            {
                string body = JsonUtility.ToJson(new MaxSdkKeyRequestDto { sdk_key = AppodealSettings.Instance.MaxSdkKey });

                // The post-build callback is synchronous, so it waits for the response
                var scriptOutcome = AppodealHttpClient.PostJsonAsync(IosSetupUrl, body, RequestTimeout).GetAwaiter().GetResult();
                if (!scriptOutcome.IsSuccess)
                {
                    Log.Error($"Setup script downloading failed --> Ad Review installation skipped. Error: '{scriptOutcome.Failure.Type}: {scriptOutcome.Failure.Message}'");
                    return false;
                }

                File.WriteAllText(setupScriptPath, scriptOutcome.Value);
                return File.Exists(setupScriptPath);
            }
            catch (Exception e)
            {
                Log.Error($"Failed to download the setup script --> Ad Review installation skipped. Error: '{e.Message}'");
                return false;
            }
        }
    }
}
#endif
