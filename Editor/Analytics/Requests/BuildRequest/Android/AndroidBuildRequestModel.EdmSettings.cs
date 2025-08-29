// ReSharper disable CheckNamespace

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace AppodealInc.Mediation.Analytics.Editor
{
    internal sealed partial class AndroidBuildRequestModel
    {
        [Serializable]
        [SuppressMessage("ReSharper", "NotAccessedField.Global")]
        internal sealed class EdmSettings
        {
            public string pluginVersion;
            public bool autoResolutionEnabled;
            public bool autoResolveOnBuild;
            public bool patchAndroidManifest;
            public bool useJetifier;
            public bool patchMainGradle;
            public bool patchGradleProperties;
            public bool patchSettingsGradle;

            public EdmSettings()
            {
                var androidResolverVersionNumberType = Type.GetType("Google.AndroidResolverVersionNumber, Google.JarResolver");
                if (androidResolverVersionNumberType != null)
                {
                    pluginVersion = androidResolverVersionNumberType.GetPropertyValue<Version>("Value", BindingFlags.Static | BindingFlags.Public)?.ToString();
                }

                var androidResolverType = Type.GetType("GooglePlayServices.SettingsDialog, Google.JarResolver");
                if (androidResolverType == null) return;

                autoResolutionEnabled = androidResolverType.GetPropertyValue<bool>("EnableAutoResolution", BindingFlags.Static | BindingFlags.NonPublic);
                autoResolveOnBuild = androidResolverType.GetPropertyValue<bool>("AutoResolveOnBuild", BindingFlags.Static | BindingFlags.NonPublic);
                patchAndroidManifest = androidResolverType.GetPropertyValue<bool>("PatchAndroidManifest", BindingFlags.Static | BindingFlags.NonPublic);
                useJetifier = androidResolverType.GetPropertyValue<bool>("UseJetifier", BindingFlags.Static | BindingFlags.NonPublic);
                patchMainGradle = androidResolverType.GetPropertyValue<bool>("PatchMainTemplateGradle", BindingFlags.Static | BindingFlags.NonPublic);
                patchGradleProperties = androidResolverType.GetPropertyValue<bool>("PatchPropertiesTemplateGradle", BindingFlags.Static | BindingFlags.NonPublic);
                patchSettingsGradle = androidResolverType.GetPropertyValue<bool>("PatchSettingsTemplateGradle", BindingFlags.Static | BindingFlags.NonPublic);
            }
        }
    }
}
