using System;
using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using AppodealStack.UnityEditor.Utils;

// ReSharper Disable CheckNamespace
namespace AppodealStack.UnityEditor.InternalResources
{
    public class AppodealSettings : ScriptableObject
    {
        private const string AppodealSettingsExportPath = "Assets/Appodeal/Editor/InternalResources";
        private const string AppodealSettingsFileName = "AppodealSettings.asset";

        [SerializeField] private string adMobAndroidAppId = AppodealEditorConstants.AdMobAppIdPlaceholder;
        [SerializeField] private string adMobIosAppId = AppodealEditorConstants.AdMobAppIdPlaceholder;

        [SerializeField] private bool accessCoarseLocationPermission;
        [SerializeField] private bool writeExternalStoragePermission = true;
        [SerializeField] private bool accessWifiStatePermission = true;
        [SerializeField] private bool vibratePermission;
        [SerializeField] private bool accessFineLocationPermission;

        [SerializeField] private bool nSUserTrackingUsageDescription;
        [SerializeField] private bool nSLocationWhenInUseUsageDescription;
        [SerializeField] private bool nSCalendarsUsageDescription;
        [SerializeField] private bool nSAppTransportSecurity;

        [SerializeField] private bool iosSkAdNetworkItems = true;
        [SerializeField] private List<string> iosSkAdNetworkItemsList;

        [SerializeField] private string facebookAndroidAppId = String.Empty;
        [SerializeField] private string facebookIosAppId = String.Empty;

        [SerializeField] private string facebookAndroidClientToken = String.Empty;
        [SerializeField] private string facebookIosClientToken = String.Empty;

        [SerializeField] private bool firebaseAutoConfiguration;
        [SerializeField] private bool facebookAutoConfiguration;

        [SerializeField] private bool facebookAutoLogAppEvents = true;
        [SerializeField] private bool facebookAdvertiserIDCollection = true;

        private static AppodealSettings _instance;
        public static AppodealSettings Instance
        {
            get
            {
                if (_instance) return _instance;

                Directory.CreateDirectory(AppodealSettingsExportPath);
                string settingsFilePath = $"{AppodealSettingsExportPath}/{AppodealSettingsFileName}";
                _instance = AssetDatabase.LoadAssetAtPath<AppodealSettings>(settingsFilePath);

                if (_instance) return _instance;

                _instance = CreateInstance<AppodealSettings>();
                AssetDatabase.CreateAsset(_instance, settingsFilePath);

                return _instance;
            }
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

        public static void SaveAsync()
        {
            EditorUtility.SetDirty(_instance);
        }
    }
}
