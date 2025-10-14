// ReSharper disable CheckNamespace

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using AppodealInc.Mediation.Utils.Editor;

namespace AppodealInc.Mediation.PluginSettings.Editor
{
    public class AppodealSettings : ScriptableObject
    {
        private const int SpaceHeight = 5;

        [Header("Appodeal Analytics")]
        [SerializeField] private bool analyticsEnabled = true;
        [SerializeField] private bool analyticsLoggingEnabled;
        [SerializeField] private bool analyticsConfigFileTransmissionEnabled = true;

        [Space(SpaceHeight)]
        [Header("AdMob App IDs")]
        [SerializeField] private string adMobAndroidAppId = AppodealEditorConstants.AdMobAppIdPlaceholder;
        [SerializeField] private string adMobIosAppId = AppodealEditorConstants.AdMobAppIdPlaceholder;

        [Space(SpaceHeight)]
        [Header("Android Permissions")]
        [SerializeField] private bool accessCoarseLocationPermission;
        [SerializeField] private bool writeExternalStoragePermission = true;
        [SerializeField] private bool accessWifiStatePermission = true;
        [SerializeField] private bool vibratePermission;
        [SerializeField] private bool accessFineLocationPermission;

        [Space(SpaceHeight)]
        [Header("iOS Permissions")]
        [SerializeField] private bool nSUserTrackingUsageDescription;
        [SerializeField] private bool nSLocationWhenInUseUsageDescription;
        [SerializeField] private bool nSCalendarsUsageDescription;
        [SerializeField] private bool nSAppTransportSecurity;

        [Space(SpaceHeight)]
        [Header("iOS SKAdNetwork Items")]
        [SerializeField] private bool iosSkAdNetworkItems = true;
        [SerializeField] private List<string> iosSkAdNetworkItemsList = new();

        [Space(SpaceHeight)]
        [Header("Facebook Service")]
        [SerializeField] private bool facebookAutoConfiguration;

        [SerializeField] private string facebookAndroidAppId = String.Empty;
        [SerializeField] private string facebookIosAppId = String.Empty;

        [SerializeField] private string facebookAndroidClientToken = String.Empty;
        [SerializeField] private string facebookIosClientToken = String.Empty;

        [SerializeField] private bool facebookAutoLogAppEvents = true;
        [SerializeField] private bool facebookAdvertiserIDCollection = true;

        [Space(SpaceHeight)]
        [Header("Firebase Service")]
        [SerializeField] private bool firebaseAutoConfiguration;

        [Space(SpaceHeight)]
        [Header("MAX AdReview")]
        [SerializeField] private bool isMaxAdReviewEnabled;
        [SerializeField] private string maxSdkKey;

        private static AppodealSettings _instance;

        public static AppodealSettings Instance
        {
            get
            {
                if (_instance) return _instance;

                _instance = Resources.Load<AppodealSettings>(AppodealEditorConstants.AppodealSettingsResourcePath);

                if (_instance) return _instance;

                if (File.Exists(AppodealEditorConstants.AppodealSettingsFilePath))
                {
                    Debug.LogError($"[Appodeal] Failed to load existing asset from '{AppodealEditorConstants.AppodealSettingsFilePath}'");
                    return null;
                }

                Directory.CreateDirectory(AppodealEditorConstants.EditorResourcesDir);
                _instance = CreateInstance<AppodealSettings>();
                AssetDatabase.CreateAsset(_instance, AppodealEditorConstants.AppodealSettingsFilePath);

                return _instance;
            }
        }

        public bool IsAnalyticsEnabled
        {
            get => analyticsEnabled;
            set => analyticsEnabled = value;
        }

        public bool IsAnalyticsLoggingEnabled
        {
            get => analyticsLoggingEnabled;
            set => analyticsLoggingEnabled = value;
        }

        public bool IsAnalyticsConfigFileTransmissionEnabled
        {
            get => analyticsConfigFileTransmissionEnabled;
            set => analyticsConfigFileTransmissionEnabled = value;
        }

        public string AdMobAndroidAppId
        {
            get => adMobAndroidAppId;
            set => adMobAndroidAppId = value.Trim();
        }

        public string AdMobIosAppId
        {
            get => adMobIosAppId;
            set => adMobIosAppId = value.Trim();
        }

        public bool AccessCoarseLocationPermission
        {
            get => accessCoarseLocationPermission;
            set => accessCoarseLocationPermission = value;
        }

        public bool WriteExternalStoragePermission
        {
            get => writeExternalStoragePermission;
            set => writeExternalStoragePermission = value;
        }

        public bool AccessWifiStatePermission
        {
            get => accessWifiStatePermission;
            set => accessWifiStatePermission = value;
        }

        public bool VibratePermission
        {
            get => vibratePermission;
            set => vibratePermission = value;
        }

        public bool AccessFineLocationPermission
        {
            get => accessFineLocationPermission;
            set => accessFineLocationPermission = value;
        }

        public bool NsUserTrackingUsageDescription
        {
            get => nSUserTrackingUsageDescription;
            set => nSUserTrackingUsageDescription = value;
        }

        public bool NsLocationWhenInUseUsageDescription
        {
            get => nSLocationWhenInUseUsageDescription;
            set => nSLocationWhenInUseUsageDescription = value;
        }

        public bool NsCalendarsUsageDescription
        {
            get => nSCalendarsUsageDescription;
            set => nSCalendarsUsageDescription = value;
        }

        public bool NsAppTransportSecurity
        {
            get => nSAppTransportSecurity;
            set
            {
                nSAppTransportSecurity = value;
#if UNITY_2022_1_OR_NEWER
                PlayerSettings.insecureHttpOption = value ? InsecureHttpOption.AlwaysAllowed : InsecureHttpOption.NotAllowed;
#else
                if (BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.iOS, BuildTarget.iOS)) PlayerSettings.iOS.allowHTTPDownload = value;
                else nSAppTransportSecurity = false;
#endif
            }
        }

        public bool IosSkAdNetworkItems
        {
            get => iosSkAdNetworkItems;
            set => iosSkAdNetworkItems = value;
        }

        public List<string> IosSkAdNetworkItemsList
        {
            get => iosSkAdNetworkItemsList;
            set => iosSkAdNetworkItemsList = value;
        }

        public bool FirebaseAutoConfiguration
        {
            get => firebaseAutoConfiguration;
            set => firebaseAutoConfiguration = value;
        }

        public bool FacebookAutoConfiguration
        {
            get => facebookAutoConfiguration;
            set => facebookAutoConfiguration = value;
        }

        public string FacebookAndroidAppId
        {
            get => facebookAndroidAppId;
            set => facebookAndroidAppId = value.Trim();
        }

        public string FacebookIosAppId
        {
            get => facebookIosAppId;
            set => facebookIosAppId = value.Trim();
        }

        public string FacebookAndroidClientToken
        {
            get => facebookAndroidClientToken;
            set => facebookAndroidClientToken = value.Trim();
        }

        public string FacebookIosClientToken
        {
            get => facebookIosClientToken;
            set => facebookIosClientToken = value.Trim();
        }

        public bool FacebookAutoLogAppEvents
        {
            get => facebookAutoLogAppEvents;
            set => facebookAutoLogAppEvents = value;
        }

        public bool FacebookAdvertiserIDCollection
        {
            get => facebookAdvertiserIDCollection;
            set => facebookAdvertiserIDCollection = value;
        }

        public bool IsMaxAdReviewEnabled
        {
            get => isMaxAdReviewEnabled;
            set => isMaxAdReviewEnabled = value;
        }

        public string MaxSdkKey
        {
            get => maxSdkKey;
            set => maxSdkKey = value.Trim();
        }

        public static void SaveAsync()
        {
            if (_instance == null) return;
            EditorUtility.SetDirty(_instance);
            AssetDatabase.SaveAssetIfDirty(_instance);
        }
    }
}
