using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using AppodealInc.Mediation.Shared.Editor;

namespace AppodealInc.Mediation.PluginSettings.Editor
{
    /// <summary>
    /// Workaround for Unity's issue where <c>Resources.Load</c> fails in late build callbacks
    /// (PostProcessBuild on iOS, IPostGenerateGradleAndroidProject on Android) on CI environments
    /// due to asset database not being fully indexed.
    /// By accessing <c>AppodealSettings.Instance</c> during PreprocessBuild, we force Unity
    /// to load and cache the settings before Appodeal's late callbacks run.
    /// </summary>
    internal class AppodealSettingsInitializer : IPreprocessBuildWithReport
    {
        private static readonly AppodealLogger Log = PluginSettingsLogging.Log;

        public int callbackOrder => -9999;

        public void OnPreprocessBuild(BuildReport report)
        {
            if (report.summary.platform is not BuildTarget.iOS and not BuildTarget.Android) return;

            var instance = AppodealSettings.Instance;

            if (instance == null)
            {
                Log.Error($"Failed to load {nameof(AppodealSettings)} instance during build preprocess. Appodeal SDK configuration may not be applied to the build.");
            }
        }
    }
}
