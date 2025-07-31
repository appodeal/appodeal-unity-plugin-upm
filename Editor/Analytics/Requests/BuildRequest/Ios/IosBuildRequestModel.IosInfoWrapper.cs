// ReSharper disable CheckNamespace

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using UnityEditor.Build.Reporting;

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

            private string LoadFileAsCompressedBase64(string path)
            {
                try
                {
                    return File.Exists(path) ? File.ReadAllText(path).CompressAndConvertToBase64() : null;
                }
                catch (Exception e)
                {
                    Logger.Log($"Error loading Podfile from path '{path}': {e.Message}");
                    return null;
                }
            }
        }
    }
}
