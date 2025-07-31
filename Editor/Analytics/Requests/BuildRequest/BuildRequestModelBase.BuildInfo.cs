// ReSharper disable CheckNamespace

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace AppodealInc.Mediation.Analytics.Editor
{
    internal abstract partial class BuildRequestModelBase
    {
        [Serializable]
        [SuppressMessage("ReSharper", "NotAccessedField.Global")]
        internal sealed class BuildInfo
        {
            private const int MaxErrorLength = 10000;

            public string buildId;
            public string buildPlatform;
            public bool isDebugBuild;
            public string buildResult;
            public ulong buildSize;
            public double buildTime;
            public int errorsCount;
            public List<string> errors = new();

            public BuildInfo(BuildReport report)
            {
                if (report == null) return;

                buildId = report.summary.guid.ToString();
                buildPlatform = report.summary.platform.ToString();
                isDebugBuild = Debug.isDebugBuild;
                buildResult = report.summary.result.ToString();
                buildSize = report.summary.totalSize;
                buildTime = report.summary.totalTime.TotalMilliseconds;
                errorsCount = report.summary.totalErrors;

                foreach (var step in report.steps)
                {
                    errors.AddRange(step.messages
                        .Where(msg => msg.type == LogType.Error)
                        .Select(msg => msg.content[..Math.Min(MaxErrorLength, msg.content.Length)].CompressAndConvertToBase64())
                    );
                }
            }
        }
    }
}
