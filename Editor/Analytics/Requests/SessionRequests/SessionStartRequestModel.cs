// ReSharper disable CheckNamespace

using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace AppodealInc.Mediation.Analytics.Editor
{
    [Serializable]
    [SuppressMessage("ReSharper", "NotAccessedField.Global")]
    internal sealed class SessionStartRequestModel : IAnalyticsRequest
    {
        [SerializeField] private string eventType = "sessionStart";
        public string EventType => eventType;

        public string userId;
        public string deviceId;
        public string projectId;
        public string sessionId;

        public string unityVersion;
        public string pluginVersion;

        public long timestamp;

        public string deviceModel;
        public string processorType;
        public int physicalMemory;
        public int videoMemory;
        public string operatingSystem;
        public float batteryLevel;

        [SuppressMessage("ReSharper", "ConvertConstructorToMemberInitializers")]
        public SessionStartRequestModel()
        {
            userId = AnalyticsContextProvider.UserId;
            deviceId = AnalyticsContextProvider.DeviceId;
            projectId = AnalyticsContextProvider.ProjectId;
            sessionId = AnalyticsContextProvider.SessionId;
            unityVersion = AnalyticsContextProvider.UnityVersion;
            pluginVersion = AnalyticsContextProvider.PluginVersion;
            timestamp = AnalyticsContextProvider.Timestamp;
            deviceModel = AnalyticsContextProvider.DeviceModel;
            processorType = AnalyticsContextProvider.ProcessorType;
            physicalMemory = SystemInfo.systemMemorySize;
            videoMemory = SystemInfo.graphicsMemorySize;
            operatingSystem = AnalyticsContextProvider.OperatingSystem;
            batteryLevel = SystemInfo.batteryLevel;
        }

        public override string ToString() => JsonUtility.ToJson(this, false);
    }
}
