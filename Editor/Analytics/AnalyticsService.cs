// ReSharper disable CheckNamespace

using System;
using System.Threading.Tasks;
using AppodealInc.Mediation.PluginSettings.Editor;

namespace AppodealInc.Mediation.Analytics.Editor
{
    public static class AnalyticsService
    {
        private const int RetryAttemptsCount = 3;
        private const int MaxCriticalEventBlockTimeMs = 1_500;
        private const string Url = "https://api-services.appodeal.com/unity_plugin_events/v1";

        public static void TrackClickEvent(ActionType actionType, bool waitForCompletion = false)
        {
            var request = new ClickRequestModel(actionType);
            var task = SendEvent(request);

            if (waitForCompletion)
            {
                task.Wait(TimeSpan.FromMilliseconds(MaxCriticalEventBlockTimeMs));
            }
        }

        internal static async Task SendEvent(IAnalyticsRequest req)
        {
            try
            {
                if (req == null || !(AppodealSettings.Instance?.IsAnalyticsEnabled ?? false)) return;

                for (int i = 0; i < RetryAttemptsCount; i++)
                {
                    bool isSuccess = await AnalyticsHttpClient.PostAsync(Url, req.ToString()).ConfigureAwait(false);
                    if (isSuccess) break;
                    Logger.Log($"Failed to send '{req.EventType}' analytics event ({i + 1})");
                    if (i < RetryAttemptsCount - 1) await Task.Delay(500).ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                Logger.Log($"Error sending '{req?.EventType}' analytics event: {e.Message}");
            }
        }
    }
}
