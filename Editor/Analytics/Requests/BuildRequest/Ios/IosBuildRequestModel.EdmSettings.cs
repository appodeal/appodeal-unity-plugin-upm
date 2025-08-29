// ReSharper disable CheckNamespace

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace AppodealInc.Mediation.Analytics.Editor
{
    internal sealed partial class IosBuildRequestModel
    {
        [Serializable]
        [SuppressMessage("ReSharper", "NotAccessedField.Global")]
        internal sealed class EdmSettings
        {
            public string pluginVersion;
            public bool podfileGenerationEnabled;
            public string cocoapodsIntegrationMethod;
            public bool addUseFrameworksToPodfile;
            public bool linkFrameworksStatically;
            public bool alwaysAddMainTargetToPodfile;
            public bool swiftFrameworkSupportWorkaroundEnabled;
            public string swiftLanguageVersion;

            public EdmSettings()
            {
                var iosResolverVersionNumberType = Type.GetType("Google.IOSResolverVersionNumber, Google.IOSResolver");
                if (iosResolverVersionNumberType != null)
                {
                    pluginVersion = iosResolverVersionNumberType.GetPropertyValue<Version>("Value", BindingFlags.Static | BindingFlags.Public)?.ToString();
                }

                var iosResolverType = Type.GetType("Google.IOSResolver, Google.IOSResolver");
                if (iosResolverType == null) return;

                podfileGenerationEnabled = iosResolverType.GetPropertyValue<bool>("PodfileGenerationEnabled", BindingFlags.Static | BindingFlags.Public);

                cocoapodsIntegrationMethod = iosResolverType.GetPropertyValue<int>("CocoapodsIntegrationMethodPref", BindingFlags.Static | BindingFlags.Public) switch
                {
                    0 => "none",
                    1 => "project",
                    2 => "workspace",
                    _ => "unknown"
                };

                addUseFrameworksToPodfile = iosResolverType.GetPropertyValue<bool>("PodfileAddUseFrameworks", BindingFlags.Static | BindingFlags.Public);
                linkFrameworksStatically = iosResolverType.GetPropertyValue<bool>("PodfileStaticLinkFrameworks", BindingFlags.Static | BindingFlags.Public);
                alwaysAddMainTargetToPodfile = iosResolverType.GetPropertyValue<bool>("PodfileAlwaysAddMainTarget", BindingFlags.Static | BindingFlags.Public);
                swiftFrameworkSupportWorkaroundEnabled = iosResolverType.GetPropertyValue<bool>("SwiftFrameworkSupportWorkaroundEnabled", BindingFlags.Static | BindingFlags.Public);
                swiftLanguageVersion = iosResolverType.GetPropertyValue<string>("SwiftLanguageVersion", BindingFlags.Static | BindingFlags.Public);
            }
        }
    }
}
