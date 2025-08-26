// ReSharper disable CheckNamespace

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using UnityEditor.Build.Reporting;
using AppodealInc.Mediation.PluginSettings.Editor;

namespace AppodealInc.Mediation.Analytics.Editor
{
    internal sealed partial class IosBuildRequestModel
    {
        [Serializable]
        [SuppressMessage("ReSharper", "NotAccessedField.Global")]
        internal sealed class IosInfoWrapper
        {
            public PlayerSettings playerSettings;
            public EdmSettings edmSettings;
            public string podfileContent;

            public IosInfoWrapper(BuildReport report)
            {
                playerSettings = new PlayerSettings();
                edmSettings = new EdmSettings();

                if (report == null) return;
                string podfilePath = $"{report.summary.outputPath}/Podfile";
                podfileContent = LoadFileAsCompressedBase64(podfilePath);
            }

            private string LoadFileAsCompressedBase64(string filePath)
            {
                try
                {
                    if (!AppodealSettings.Instance?.IsAnalyticsConfigFileTransmissionEnabled ?? false) return null;

                    return File.Exists(filePath) ? File.ReadAllText(filePath).SanitizePodfileContent().CompressAndConvertToBase64() : null;
                }
                catch (Exception e)
                {
                    Logger.Log($"Error loading Podfile from path '{filePath}': {e.Message}");
                    return null;
                }
            }
        }
    }
}
