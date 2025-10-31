// ReSharper disable CheckNamespace

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AppodealInc.Mediation.PluginSettings.Editor;

namespace AppodealInc.Mediation.Analytics.Editor
{
    internal abstract partial class BuildRequestModelBase
    {
        [Serializable]
        [SuppressMessage("ReSharper", "NotAccessedField.Global")]
        internal sealed class PluginSettings
        {
            public string pluginVersion;

            public string adMobIosAppId;
            public string adMobAndroidAppId;

            public bool coarseLocation;
            public bool fineLocation;

            public bool userTracking;
            public bool locationWhenInUse;
            public bool appTransportSecurity;

            public bool addSkAdNetworkItems;
            public List<string> skAdNetworkItems;

            public bool configureFirebase;

            public bool configureFacebook;
            public string facebookIosAppId;
            public string facebookAndroidAppId;
            public string facebookIosClientToken;
            public string facebookAndroidClientToken;

            public PluginSettings()
            {
                pluginVersion = AnalyticsContextProvider.PluginVersion;

                var settings = AppodealSettings.Instance;
                if (settings == null) return;

                adMobIosAppId = settings.AdMobIosAppId;
                adMobAndroidAppId = settings.AdMobAndroidAppId;

                coarseLocation = settings.AccessCoarseLocationPermission;
                fineLocation = settings.AccessFineLocationPermission;

                userTracking = settings.NsUserTrackingUsageDescription;
                locationWhenInUse = settings.NsLocationWhenInUseUsageDescription;
                appTransportSecurity = settings.NsAppTransportSecurity;

                addSkAdNetworkItems = settings.IosSkAdNetworkItems;
                skAdNetworkItems = settings.IosSkAdNetworkItemsList;

                configureFirebase = settings.FirebaseAutoConfiguration;

                configureFacebook = settings.FacebookAutoConfiguration;
                facebookIosAppId = settings.FacebookIosAppId;
                facebookAndroidAppId = settings.FacebookAndroidAppId;
                facebookIosClientToken = settings.FacebookIosClientToken;
                facebookAndroidClientToken = settings.FacebookAndroidClientToken;
            }
        }
    }
}
