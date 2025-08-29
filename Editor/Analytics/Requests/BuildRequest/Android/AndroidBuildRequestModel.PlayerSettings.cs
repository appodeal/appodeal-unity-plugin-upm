// ReSharper disable CheckNamespace

using System;
using System.Diagnostics.CodeAnalysis;
using UnityEditor;
using UnityEditor.Build;

namespace AppodealInc.Mediation.Analytics.Editor
{
    internal sealed partial class AndroidBuildRequestModel
    {
        [Serializable]
        [SuppressMessage("ReSharper", "NotAccessedField.Global")]
        internal sealed class PlayerSettings
        {
            public string packageName;
            public int bundleVersionCode;
            public string minSdkVersion;
            public string targetSdkVersion;
            public string targetArchitectures;
            public string scriptingBackend;
            public string graphicsApis;
            public string managedStrippingLevel;
            public string applicationEntry;

            public PlayerSettings()
            {
                packageName = UnityEditor.PlayerSettings.applicationIdentifier;
                bundleVersionCode = UnityEditor.PlayerSettings.Android.bundleVersionCode;
                minSdkVersion = UnityEditor.PlayerSettings.Android.minSdkVersion.ToString();
                targetSdkVersion = UnityEditor.PlayerSettings.Android.targetSdkVersion.ToString();
                targetArchitectures = UnityEditor.PlayerSettings.Android.targetArchitectures.ToString();
                scriptingBackend = UnityEditor.PlayerSettings.GetScriptingBackend(NamedBuildTarget.Android).ToString();
                graphicsApis = String.Join(", ", UnityEditor.PlayerSettings.GetGraphicsAPIs(BuildTarget.Android));
                managedStrippingLevel = UnityEditor.PlayerSettings.GetManagedStrippingLevel(NamedBuildTarget.Android).ToString();
#if UNITY_2023_1_OR_NEWER
                applicationEntry = UnityEditor.PlayerSettings.Android.applicationEntry.ToString();
#endif
            }
        }
    }
}
