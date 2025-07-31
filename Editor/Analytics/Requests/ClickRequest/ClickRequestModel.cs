// ReSharper disable CheckNamespace

using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace AppodealInc.Mediation.Analytics.Editor
{
    [Serializable]
    [SuppressMessage("ReSharper", "NotAccessedField.Global")]
    internal sealed class ClickRequestModel : IAnalyticsRequest
    {
        [SerializeField] private string eventType = "click";
        public string EventType => eventType;

        public string userId;
        public string deviceId;
        public string projectId;
        public string sessionId;

        public long timestamp;

        public ActionType actionType;

        public ClickRequestModel(ActionType type)
        {
            userId = AnalyticsContextProvider.UserId;
            deviceId = AnalyticsContextProvider.DeviceId;
            projectId = AnalyticsContextProvider.ProjectId;
            sessionId = AnalyticsContextProvider.SessionId;
            timestamp = AnalyticsContextProvider.Timestamp;
            actionType = type;
        }

        public override string ToString() => JsonUtility.ToJson(this, false);
    }
}
