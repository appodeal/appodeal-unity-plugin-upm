using AppodealInc.Mediation.PluginSettings.Editor;
using AppodealInc.Mediation.Shared.Editor;

namespace AppodealInc.Mediation.Analytics.Editor
{
    internal static class AnalyticsLogging
    {
        public static readonly AppodealLogger Log = new("Analytics", () => AppodealSettings.Instance?.IsAnalyticsLoggingEnabled ?? false);
    }
}
