// ReSharper disable CheckNamespace

using System;
using System.Diagnostics.CodeAnalysis;
using UnityEditor;
using UnityEngine;

namespace AppodealInc.Mediation.Analytics.Editor
{
    internal abstract partial class BuildRequestModelBase
    {
        [Serializable]
        [SuppressMessage("ReSharper", "NotAccessedField.Global")]
        internal sealed class ProjectInfo
        {
            public string projectId;
            public string projectName;
            public string appVersion;
            public string unityVersion;
            public string systemLanguage;
            public bool stripEngineCode;

            [SuppressMessage("ReSharper", "ConvertConstructorToMemberInitializers")]
            public ProjectInfo()
            {
                projectId = AnalyticsContextProvider.ProjectId;
                projectName = PlayerSettings.productName;
                appVersion = PlayerSettings.bundleVersion;
                unityVersion = AnalyticsContextProvider.UnityVersion;
                systemLanguage = Application.systemLanguage.ToString();
                stripEngineCode = PlayerSettings.stripEngineCode;
            }
        }
    }
}
