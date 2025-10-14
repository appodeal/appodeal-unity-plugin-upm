// ReSharper disable CheckNamespace

using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using AppodealInc.Mediation.PluginSettings.Editor;

namespace AppodealInc.Mediation.PostProcess.Editor
{
    public class AppodealPostProcess : MonoBehaviour
    {
        [PostProcessBuild(100)]
        public static void OnPostProcessBuild(BuildTarget target, string path)
        {
            if (target.ToString() != "iOS") return;
            if (AppodealSettings.Instance == null) return;

            IosPostprocessUtils.PrepareProject(path);
        }
    }
}
