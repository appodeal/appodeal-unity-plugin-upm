#if UNITY_IOS
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

// ReSharper Disable CheckNamespace
namespace AppodealStack.UnityEditor.PostProcess
{
    public class AppodealPostProcess : MonoBehaviour
    {
        [PostProcessBuild(100)]
        public static void OnPostProcessBuild(BuildTarget target, string path)
        {
            if (target.ToString() != "iOS") return;

            IosPostprocessUtils.PrepareProject(path);
        }
    }
}

#endif
