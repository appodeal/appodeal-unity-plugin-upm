// ReSharper disable CheckNamespace
// ReSharper disable HeuristicUnreachableCode

#pragma warning disable CS0162

using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using AppodealInc.Mediation.PluginSettings.Editor;

namespace AppodealInc.Mediation.Analytics.Editor
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    internal static class Logger
    {
        private const string AnalyticsPrefix = "[Appodeal Analytics]";

        private static bool IsLoggingEnabled
        {
            get
            {
#if APPODEAL_DEV
                return true;
#endif
                return AppodealSettings.Instance?.IsAnalyticsLoggingEnabled ?? false;
            }
        }

        public static void Log(string message)
        {
            if (!IsLoggingEnabled) return;

#if APPODEAL_DEV
            LogError(message);
#else
            LogWarning(message);
#endif
        }

        private static void LogWarning(string message) => Debug.LogWarning(message.AddAnalyticsPrefix());

        private static void LogError(string message) => Debug.LogError(message.AddAnalyticsPrefix());

        private static string AddAnalyticsPrefix(this string message) => $"{AnalyticsPrefix.Colorize()} {message}";

        private static string Colorize(this string message) => $"<color=#FFD800>{message}</color>";
    }
}
