// ReSharper disable CheckNamespace

using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using AppodealInc.Mediation.PluginSettings.Editor;

namespace AppodealInc.Mediation.Analytics.Editor
{
    [InitializeOnLoad]
    internal class AnalyticsController : IPreprocessBuildWithReport
    {
        private const int BuildCompletionTimeoutMs = 3_600_000;

        private const string UserIdKey = "Appodeal.Analytics.UserId";
        private const string SessionIdKey = "Appodeal.Analytics.SessionId";

        public static string UserId
        {
            get => EditorPrefs.GetString(UserIdKey, null);
            private set => EditorPrefs.SetString(UserIdKey, value);
        }

        public static string SessionId
        {
            get => SessionState.GetString(SessionIdKey, null);
            private set => SessionState.SetString(SessionIdKey, value);
        }

        public int callbackOrder => 0;

        // Triggered every time the scripts are recompiled thanks to the InitializeOnLoad attribute
        static AnalyticsController()
        {
            if (!AppodealSettings.Instance.IsAnalyticsEnabled) return;

            // We should subscribe to the quitting event every time the scripts are recompiled
            EditorApplication.quitting -= OnEditorQuitting;
            EditorApplication.quitting += OnEditorQuitting;

            if (String.IsNullOrEmpty(UserId)) UserId = Guid.NewGuid().ToString();

            if (!String.IsNullOrEmpty(SessionId)) return;
            // The code below is executed only once per Editor session

            SessionId = Guid.NewGuid().ToString();

            var sessionStartRequestModel = new SessionStartRequestModel();
            _ = AnalyticsService.SendEvent(sessionStartRequestModel);
        }

        private static void OnEditorQuitting()
        {
            var sessionEndRequestModel = new SessionEndRequestModel();
            _ = AnalyticsService.SendEvent(sessionEndRequestModel);
        }

        public void OnPreprocessBuild(BuildReport report)
        {
            if (report.summary.platform is not BuildTarget.iOS and not BuildTarget.Android) return;
            _ = HandleBuildEventAsync(report);
        }

        private static async Task HandleBuildEventAsync(BuildReport report)
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(BuildCompletionTimeoutMs));
            try
            {
                while ((BuildPipeline.isBuildingPlayer || report.summary.result == BuildResult.Unknown) && !cts.Token.IsCancellationRequested)
                {
                    await Task.Delay(500, cts.Token);
                }

                IAnalyticsRequest request = report.summary.platform switch
                {
                    BuildTarget.iOS => new IosBuildRequestModel(report),
                    BuildTarget.Android => new AndroidBuildRequestModel(report),
                    _ => null
                };

                _ = AnalyticsService.SendEvent(request);
            }
            catch (TaskCanceledException)
            {
                Logger.Log($"Build polling canceled after waiting {BuildCompletionTimeoutMs / 60_000} minutes for {report.summary.platform}");
            }
            catch (Exception e)
            {
                Logger.Log($"Error creating or sending 'build' analytics event for {report.summary.platform}: {e.Message}");
            }
        }
    }
}
