// ReSharper disable CheckNamespace

using System;
using System.Diagnostics.CodeAnalysis;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace AppodealInc.Mediation.Analytics.Editor
{
    [Serializable]
    [SuppressMessage("ReSharper", "NotAccessedField.Global")]
    internal sealed partial class AndroidBuildRequestModel : BuildRequestModelBase, IAnalyticsRequest
    {
        public AndroidInfoWrapper android;

        public AndroidBuildRequestModel(BuildReport report) : base(report)
        {
            android = new AndroidInfoWrapper();
        }

        public override string ToString() => JsonUtility.ToJson(this, false);
    }
}
