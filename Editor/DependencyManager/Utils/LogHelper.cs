// ReSharper disable CheckNamespace

using System;
using UnityEngine;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal static class LogHelper
    {
        private static bool IsVerbose => DmChoicesScriptableObject.Instance?.EnableVerboseLogging ?? false;

        public static void Log(string message)
        {
            if (!IsVerbose) return;
            Debug.Log(message.AddAppodealPrefix());
        }

        public static void LogDepsUpdate(string message, DependenciesDiff diff)
        {
            string changes = diff.ToString();
            if (!String.IsNullOrEmpty(changes)) message += $"\n\n{changes}\n";
            Debug.Log(message.AddAppodealPrefix());
        }

        public static void LogWarning(string message) => Debug.LogWarning(message.AddAppodealPrefix());

        public static void LogError(string message) => Debug.LogError(message.AddAppodealPrefix());

        public static void LogException(Exception e) => Debug.LogException(e);

        public static string Colorize(Color color, string message) => $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{message}</color>";

        private static string AddAppodealPrefix(this string message) => $"[Appodeal] {message}";
    }
}
