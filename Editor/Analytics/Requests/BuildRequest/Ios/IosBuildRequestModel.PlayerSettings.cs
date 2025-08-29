// ReSharper disable CheckNamespace

using System;
using System.Diagnostics.CodeAnalysis;
using UnityEditor;
using UnityEditor.Build;

namespace AppodealInc.Mediation.Analytics.Editor
{
    internal sealed partial class IosBuildRequestModel
    {
        [Serializable]
        [SuppressMessage("ReSharper", "NotAccessedField.Global")]
        internal sealed class PlayerSettings
        {
            public string bundleId;
            public string buildNumber;
            public string targetDevice;
            public string sdkVersion;
            public string targetOSVersion;
            public string graphicsApis;
            public string managedStrippingLevel;

            [SuppressMessage("ReSharper", "ConvertConstructorToMemberInitializers")]
            public PlayerSettings()
            {
                bundleId = UnityEditor.PlayerSettings.applicationIdentifier;
                buildNumber = UnityEditor.PlayerSettings.iOS.buildNumber;
                targetDevice = UnityEditor.PlayerSettings.iOS.targetDevice.ToString();
                sdkVersion = UnityEditor.PlayerSettings.iOS.sdkVersion.ToString();
                targetOSVersion = UnityEditor.PlayerSettings.iOS.targetOSVersionString;
                graphicsApis = String.Join(", ", UnityEditor.PlayerSettings.GetGraphicsAPIs(BuildTarget.iOS));
                managedStrippingLevel = UnityEditor.PlayerSettings.GetManagedStrippingLevel(NamedBuildTarget.iOS).ToString();
            }
        }
    }
}
