// ReSharper disable CheckNamespace

using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace AppodealInc.Mediation.Analytics.Editor
{
    [Serializable]
    [SuppressMessage("ReSharper", "NotAccessedField.Global")]
    internal sealed class SessionEndRequestModel : IAnalyticsRequest
    {
        [SerializeField] private string eventType = "sessionEnd";
        public string EventType => eventType;

        public string userId;
        public string deviceId;
        public string projectId;
        public string sessionId;

        public string unityVersion;
        public string pluginVersion;

        public long timestamp;
        public long elapsedTime;
        public long focusedElapsedTime;
        public long activeElapsedTime;

        [SuppressMessage("ReSharper", "ConvertConstructorToMemberInitializers")]
        public SessionEndRequestModel()
        {
            userId = AnalyticsContextProvider.UserId;
            deviceId = AnalyticsContextProvider.DeviceId;
            projectId = AnalyticsContextProvider.ProjectId;
            sessionId = AnalyticsContextProvider.SessionId;
            unityVersion = AnalyticsContextProvider.UnityVersion;
            pluginVersion = AnalyticsContextProvider.PluginVersion;
            timestamp = AnalyticsContextProvider.Timestamp;
            elapsedTime = AnalyticsContextProvider.ElapsedTime;
            focusedElapsedTime = AnalyticsContextProvider.FocusedElapsedTime;
            activeElapsedTime = AnalyticsContextProvider.ActiveElapsedTime;
        }

        public override string ToString() => JsonUtility.ToJson(this, false);
    }
}
