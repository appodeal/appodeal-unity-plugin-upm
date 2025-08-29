// ReSharper disable CheckNamespace

using System;
using System.Diagnostics.CodeAnalysis;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace AppodealInc.Mediation.Analytics.Editor
{
    [Serializable]
    [SuppressMessage("ReSharper", "NotAccessedField.Global")]
    internal abstract partial class BuildRequestModelBase
    {
        [SerializeField] private string eventType = "build";
        public string EventType => eventType;

        public string userId;
        public string deviceId;
        public string sessionId;

        public long timestamp;
        public long elapsedTime;
        public long focusedElapsedTime;
        public long activeElapsedTime;

        public string deviceModel;
        public string processorType;
        public string operatingSystem;

        public ProjectInfo projectInfo;
        public BuildInfo buildInfo;
        public PluginSettings pluginSettings;

        protected BuildRequestModelBase(BuildReport report)
        {
            userId = AnalyticsContextProvider.UserId;
            deviceId = AnalyticsContextProvider.DeviceId;
            sessionId = AnalyticsContextProvider.SessionId;
            timestamp = AnalyticsContextProvider.Timestamp;
            elapsedTime = AnalyticsContextProvider.ElapsedTime;
            focusedElapsedTime = AnalyticsContextProvider.FocusedElapsedTime;
            activeElapsedTime = AnalyticsContextProvider.ActiveElapsedTime;
            deviceModel = AnalyticsContextProvider.DeviceModel;
            processorType = AnalyticsContextProvider.ProcessorType;
            operatingSystem = AnalyticsContextProvider.OperatingSystem;
            projectInfo = new ProjectInfo();
            buildInfo = new BuildInfo(report);
            pluginSettings = new PluginSettings();
        }
    }
}
