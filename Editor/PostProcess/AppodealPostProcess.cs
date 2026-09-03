// ReSharper disable CheckNamespace

using UnityEditor;
using UnityEditor.Callbacks;
using AppodealInc.Mediation.PluginSettings.Editor;

namespace AppodealInc.Mediation.PostProcess.Editor
{
    internal static class AppodealPostProcess
    {
        [PostProcessBuild(100)]
        public static void OnPostProcessBuild(BuildTarget target, string path)
        {
            if (target.ToString() != "iOS") return;
            if (AppodealSettings.Instance == null) return;

            IosPostProcessUtils.PrepareProject(path);
            IosSwiftPackageEmbedder.Apply(path);
        }
    }
}
