using System;
using UnityEngine;

namespace AppodealInc.Mediation.Shared.Editor
{
    internal sealed class AppodealLogger
    {
        private readonly string _prefix;
        private readonly Func<bool> _isVerboseEnabled;

        public AppodealLogger(string tag, Func<bool> isVerboseEnabled = null)
        {
            _prefix = $"[Appodeal] [{tag}]";
            _isVerboseEnabled = isVerboseEnabled;
        }

        private bool IsVerboseEnabled => DevMode.IsEnabled || (_isVerboseEnabled?.Invoke() ?? false);

        public void Verbose(string message)
        {
            if (!IsVerboseEnabled) return;
            Debug.Log(AddPrefix(message));
        }

        public void Info(string message) => Debug.Log(AddPrefix(message));

        public void Warning(string message) => Debug.LogWarning(AddPrefix(message));

        public void Error(string message) => Debug.LogError(AddPrefix(message));

        public void Exception(Exception ex) => Debug.LogException(ex);

        // Rich text tags reach batch mode logs as plain text
        public static string Colorize(Color color, string text) => Application.isBatchMode ? text : $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{text}</color>";

        private string AddPrefix(string message) => $"{_prefix} {message}";
    }
}
