#if UNITY_IPHONE
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

namespace AppodealAds.Unity.Editor.Utils
{
    public class AppodealPostProcess : MonoBehaviour
    {
        [PostProcessBuild(100)]
        public static void OnPostProcessBuild(BuildTarget target, string path)
        {
            if (target.ToString() == "iOS" || target.ToString() == "iPhone")
            {
                iOSPostprocessUtils.PrepareProject(path);
            }
        }
    }
}
#endif
