// ReSharper disable CheckNamespace

using System;
using UnityEditor;
using UnityEngine;
using AppodealStack.Monetization.Common;

namespace AppodealInc.Mediation.Analytics.Editor
{
    internal static class AnalyticsContextProvider
    {
        public static string UserId => AnalyticsController.UserId;
        public static string DeviceId => SystemInfo.deviceUniqueIdentifier.ToLowerInvariant();
        public static string ProjectId => PlayerSettings.productGUID.ToString();
        public static string SessionId => AnalyticsController.SessionId;
        public static long Timestamp => DateTimeOffset.UtcNow.ToUnixTimeSeconds(); // (long)DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalSeconds;
        public static string UnityVersion => Application.unityVersion;
        public static string PluginVersion => AppodealVersions.GetPluginVersion();
        public static long ElapsedTime => EditorAnalyticsSessionInfo.elapsedTime;
        public static long FocusedElapsedTime => EditorAnalyticsSessionInfo.focusedElapsedTime;
        public static long ActiveElapsedTime => EditorAnalyticsSessionInfo.activeElapsedTime;
        public static string DeviceModel => SystemInfo.deviceModel;
        public static string ProcessorType => SystemInfo.processorType;
        public static string OperatingSystem => SystemInfo.operatingSystem;
    }
}
