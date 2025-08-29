// ReSharper disable CheckNamespace

using System;
using System.Diagnostics.CodeAnalysis;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace AppodealInc.Mediation.Analytics.Editor
{
    [Serializable]
    [SuppressMessage("ReSharper", "NotAccessedField.Global")]
    internal sealed partial class IosBuildRequestModel : BuildRequestModelBase, IAnalyticsRequest
    {
        public IosInfoWrapper ios;

        public IosBuildRequestModel(BuildReport report) : base(report)
        {
            ios = new IosInfoWrapper(report);
        }

        public override string ToString() => JsonUtility.ToJson(this, false);
    }
}
