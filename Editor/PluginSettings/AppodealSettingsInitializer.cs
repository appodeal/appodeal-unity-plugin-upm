// ReSharper disable CheckNamespace

using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace AppodealInc.Mediation.PluginSettings.Editor
{
    /// <summary>
    /// Workaround for Unity's issue where <c>Resources.Load</c> fails in PostProcessBuild
    /// on CI environments due to asset database not being fully indexed.
    /// By accessing <c>AppodealSettings.Instance</c> during PreprocessBuild, we force Unity
    /// to load and cache the settings before Appodeal's PostProcessBuild callback runs.
    /// </summary>
    internal class AppodealSettingsInitializer : IPreprocessBuildWithReport
    {
        public int callbackOrder => -9999;

        public void OnPreprocessBuild(BuildReport report)
        {
            if (report.summary.platform != BuildTarget.iOS) return;

            var instance = AppodealSettings.Instance;

            if (instance == null)
            {
                Debug.LogError($"Failed to load {nameof(AppodealSettings)} instance during build preprocess. Appodeal SDK configuration may not be applied to the Xcode project.");
            }
        }
    }
}
