using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

// ReSharper Disable CheckNamespace
namespace AppodealStack.UnityEditor.InternalResources
{
    public class AppodealSettings : ScriptableObject
    {
        private const string AppodealSettingsExportPath = "Appodeal/Editor/InternalResources/AppodealSettings.asset";
        private static AppodealSettings _instance;

        [SerializeField] private string adMobAndroidAppId = String.Empty;
        [SerializeField] private string adMobIosAppId = String.Empty;

        [SerializeField] private bool accessCoarseLocationPermission;
        [SerializeField] private bool writeExternalStoragePermission;
        [SerializeField] private bool accessWifiStatePermission;
        [SerializeField] private bool vibratePermission;
        [SerializeField] private bool accessFineLocationPermission;

        [SerializeField] private bool nSUserTrackingUsageDescription;
        [SerializeField] private bool nSLocationWhenInUseUsageDescription;
        [SerializeField] private bool nSCalendarsUsageDescription;
        [SerializeField] private bool nSAppTransportSecurity;

        [SerializeField] private bool iosSkAdNetworkItems;
        [SerializeField] private List<string> iosSkAdNetworkItemsList;

        [SerializeField] private string facebookAndroidAppId = String.Empty;
        [SerializeField] private string facebookIosAppId = String.Empty;

        [SerializeField] private string facebookAndroidClientToken = String.Empty;

        [SerializeField] private bool firebaseAutoConfiguration;
        [SerializeField] private bool facebookAutoConfiguration;
        [SerializeField] private bool facebookAutoLogAppEvents;
        [SerializeField] private bool facebookAdvertiserIDCollection;

        public static AppodealSettings Instance
        {
            get
            {
                if (_instance != null) return _instance;
                string settingsFilePath = Path.Combine("Assets", AppodealSettingsExportPath);
                string settingsDir = Path.GetDirectoryName(settingsFilePath);
                if (!Directory.Exists(settingsDir))
                {
                    Directory.CreateDirectory(settingsDir ?? String.Empty);
                }

                _instance = AssetDatabase.LoadAssetAtPath<AppodealSettings>(settingsFilePath);
                if (_instance != null) return _instance;
                _instance = CreateInstance<AppodealSettings>();
                AssetDatabase.CreateAsset(_instance, settingsFilePath);

                return _instance;
            }
        }

        public string AdMobAndroidAppId
        {
            get => Instance.adMobAndroidAppId;
            set => Instance.adMobAndroidAppId = value.Trim();
        }

        public string AdMobIosAppId
        {
            get => Instance.adMobIosAppId;
            set => Instance.adMobIosAppId = value.Trim();
        }

        public bool AccessCoarseLocationPermission
        {
            get => accessCoarseLocationPermission;
            set => Instance.accessCoarseLocationPermission = value;
        }

        public bool WriteExternalStoragePermission
        {
            get => writeExternalStoragePermission;
            set => Instance.writeExternalStoragePermission = value;
        }

        public bool AccessWifiStatePermission
        {
            get => accessWifiStatePermission;
            set => Instance.accessWifiStatePermission = value;
        }

        public bool VibratePermission
        {
            get => vibratePermission;
            set => Instance.vibratePermission = value;
        }

        public bool AccessFineLocationPermission
        {
            get => accessFineLocationPermission;
            set => Instance.accessFineLocationPermission = value;
        }

        public bool NsUserTrackingUsageDescription
        {
            get => nSUserTrackingUsageDescription;
            set => Instance.nSUserTrackingUsageDescription = value;
        }

        public bool NsLocationWhenInUseUsageDescription
        {
            get => nSLocationWhenInUseUsageDescription;
            set => Instance.nSLocationWhenInUseUsageDescription = value;
        }

        public bool NsCalendarsUsageDescription
        {
            get => nSCalendarsUsageDescription;
            set => Instance.nSCalendarsUsageDescription = value;
        }

        public bool NsAppTransportSecurity
        {
            get => nSAppTransportSecurity;
            set
            {
                Instance.nSAppTransportSecurity = value;
#if UNITY_2022_1_OR_NEWER
                PlayerSettings.insecureHttpOption = value ? InsecureHttpOption.AlwaysAllowed : InsecureHttpOption.NotAllowed;
#else
                if (BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.iOS, BuildTarget.iOS)) PlayerSettings.iOS.allowHTTPDownload = value;
                else Instance.nSAppTransportSecurity = false;
#endif
            }
        }

        public bool IosSkAdNetworkItems
        {
            get => iosSkAdNetworkItems;
            set => Instance.iosSkAdNetworkItems = value;
        }

        public List<string> IosSkAdNetworkItemsList
        {
            get => iosSkAdNetworkItemsList;
            set => Instance.iosSkAdNetworkItemsList = value;
        }

        public bool FirebaseAutoConfiguration
        {
            get => firebaseAutoConfiguration;
            set => Instance.firebaseAutoConfiguration = value;
        }

        public bool FacebookAutoConfiguration
        {
            get => facebookAutoConfiguration;
            set => Instance.facebookAutoConfiguration = value;
        }

        public string FacebookAndroidAppId
        {
            get => Instance.facebookAndroidAppId;
            set => Instance.facebookAndroidAppId = value.Trim();
        }

        public string FacebookIosAppId
        {
            get => Instance.facebookIosAppId;
            set => Instance.facebookIosAppId = value.Trim();
        }

        public string FacebookAndroidClientToken
        {
            get => Instance.facebookAndroidClientToken;
            set => Instance.facebookAndroidClientToken = value.Trim();
        }

        public bool FacebookAutoLogAppEvents
        {
            get => facebookAutoLogAppEvents;
            set => Instance.facebookAutoLogAppEvents = value;
        }

        public bool FacebookAdvertiserIDCollection
        {
            get => facebookAdvertiserIDCollection;
            set => Instance.facebookAdvertiserIDCollection = value;
        }

        public void SaveAsync()
        {
            EditorUtility.SetDirty(_instance);
        }
    }
}
