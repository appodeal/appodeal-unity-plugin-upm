using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using AppodealInc.Mediation.PluginSettings.Editor;
using AppodealInc.Mediation.Shared.Editor;

namespace AppodealInc.Mediation.Analytics.Editor
{
    internal sealed partial class AndroidBuildRequestModel
    {
        [Serializable]
        [SuppressMessage("ReSharper", "NotAccessedField.Global")]
        internal sealed class AndroidInfoWrapper
        {
            private static readonly AppodealLogger Log = AnalyticsLogging.Log;

            private const string MainGradleFileName = "mainTemplate.gradle";
            private const string SettingsGradleFileName = "settingsTemplate.gradle";
            private const string GradlePropertiesFileName = "gradleTemplate.properties";

            public PlayerSettings playerSettings;
            public EdmSettings edmSettings;
            public string mainGradleContent;
            public string settingsGradleContent;
            public string gradlePropertiesContent;

            public AndroidInfoWrapper()
            {
                playerSettings = new PlayerSettings();
                edmSettings = new EdmSettings();

                mainGradleContent = LoadGradleTemplateFileAsCompressedBase64(MainGradleFileName);
                settingsGradleContent = LoadGradleTemplateFileAsCompressedBase64(SettingsGradleFileName);
                gradlePropertiesContent = LoadGradleTemplateFileAsCompressedBase64(GradlePropertiesFileName);
            }

            private string LoadGradleTemplateFileAsCompressedBase64(string fileName)
            {
                try
                {
                    if (!AppodealSettings.Instance?.IsAnalyticsConfigFileTransmissionEnabled ?? false) return null;

                    string path = $"{PluginPaths.AndroidPluginsDir}/{fileName}";
                    return File.Exists(path) ? File.ReadAllText(path).SanitizeGradleContent().CompressAndConvertToBase64() : null;
                }
                catch (Exception e)
                {
                    Log.Verbose($"Error loading Gradle template '{fileName}' file: {e.Message}");
                    return null;
                }
            }
        }
    }
}
