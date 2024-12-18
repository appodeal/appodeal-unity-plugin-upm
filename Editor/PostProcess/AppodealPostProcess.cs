﻿#if UNITY_IOS
// ReSharper Disable CheckNamespace

using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace AppodealInc.Mediation.PostProcess.Editor
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
