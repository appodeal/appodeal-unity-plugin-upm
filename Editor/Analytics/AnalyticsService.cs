using System;
using System.Threading.Tasks;
using AppodealInc.Mediation.PluginSettings.Editor;
using AppodealInc.Mediation.Shared.Editor;

namespace AppodealInc.Mediation.Analytics.Editor
{
    public static class AnalyticsService
    {
        private static readonly AppodealLogger Log = AnalyticsLogging.Log;

        private const int RetryAttemptsCount = 3;
        private const int MaxCriticalEventBlockTimeMs = 1_500;
        private const string Url = ServiceHosts.Analytics + "/unity_plugin_events/v1";

        private static readonly TimeSpan RequestTimeout = TimeSpan.FromSeconds(10);

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
                    var sendOutcome = await AppodealHttpClient.PostJsonAsync(Url, req.ToString(), RequestTimeout).ConfigureAwait(false);
                    if (sendOutcome.IsSuccess) break;
                    if (sendOutcome.Failure.Type != "HttpError") Log.Verbose($"Error sending analytics data to {Url}: {sendOutcome.Failure.Message}");
                    Log.Verbose($"Failed to send '{req.EventType}' analytics event ({i + 1})");
                    if (i < RetryAttemptsCount - 1) await Task.Delay(500).ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                Log.Verbose($"Error sending '{req?.EventType}' analytics event: {e.Message}");
            }
        }
    }
}
