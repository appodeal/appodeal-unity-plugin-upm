// ReSharper disable CheckNamespace

using UnityEngine;

namespace AppodealInc.Mediation.MaxAdReview.Editor
{
    internal static class AdReviewHelper
    {
        private const string Tag = "[Appodeal] [MaxAdReview]";

        public static void Log(string message)
        {
            if (!Debug.isDebugBuild) return;
            Debug.Log(message.AddPrefix());
        }

        public static void LogWarning(string message) => Debug.LogWarning(message.AddPrefix());

        public static void LogError(string message) => Debug.LogError(message.AddPrefix());

        private static string AddPrefix(this string message) => $"{Tag} {message}";
    }
}
