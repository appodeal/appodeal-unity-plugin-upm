// ReSharper disable CheckNamespace

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using UnityEngine;

namespace AppodealInc.Mediation.Analytics.Editor
{
    internal sealed partial class AndroidBuildRequestModel
    {
        [Serializable]
        [SuppressMessage("ReSharper", "NotAccessedField.Global")]
        internal sealed class AndroidInfoWrapper
        {
            private const string AndroidPluginsDir = "Plugins/Android";
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
                    string path = $"{Application.dataPath}/{AndroidPluginsDir}/{fileName}";
                    return File.Exists(path) ? File.ReadAllText(path).CompressAndConvertToBase64() : null;
                }
                catch (Exception e)
                {
                    Logger.Log($"Error loading Gradle template '{fileName}' file: {e.Message}");
                    return null;
                }
            }
        }
    }
}
