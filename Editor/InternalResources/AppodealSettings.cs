using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

// ReSharper Disable CheckNamespace
namespace AppodealStack.UnityEditor.InternalResources
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class AppodealSettings : ScriptableObject
    {
        private const string AppodealSettingsExportPath = "Appodeal/Editor/InternalResources/AppodealSettings.asset";
        private static AppodealSettings instance;

        [SerializeField] private string adMobAndroidAppId = string.Empty;
        [SerializeField] private string adMobIosAppId = string.Empty;

        [SerializeField] private bool accessCoarseLocationPermission;
        [SerializeField] private bool writeExternalStoragePermission;
        [SerializeField] private bool accessWifiStatePermission;
        [SerializeField] private bool vibratePermission;
        [SerializeField] private bool accessFineLocationPermission;

        [SerializeField] private bool androidMultidex;

        [SerializeField] private bool nSUserTrackingUsageDescription;
        [SerializeField] private bool nSLocationWhenInUseUsageDescription;
        [SerializeField] private bool nSCalendarsUsageDescription;
        [SerializeField] private bool nSAppTransportSecurity;

        [SerializeField] private bool iosSKAdNetworkItems;
        [SerializeField] private List<string> iosSkAdNetworkItemsList;


        [SerializeField] private bool firebaseAutoConfiguration;
        [SerializeField] private bool facebookAutoConfiguration;
        [SerializeField] private string facebookAndroidAppId = string.Empty;
        [SerializeField] private string facebookIosAppId = string.Empty;
        [SerializeField] private bool facebookAutoLogAppEvents;
        [SerializeField] private bool facebookAdvertiserIDCollection;

        public static AppodealSettings Instance
        {
            get
            {
                if (instance != null) return instance;
                var settingsFilePath = Path.Combine("Assets", AppodealSettingsExportPath);
                var settingsDir = Path.GetDirectoryName(settingsFilePath);
                if (!Directory.Exists(settingsDir))
                {
                    Directory.CreateDirectory(settingsDir ?? string.Empty);
                }

                instance = AssetDatabase.LoadAssetAtPath<AppodealSettings>(settingsFilePath);
                if (instance != null) return instance;
                instance = CreateInstance<AppodealSettings>();
                AssetDatabase.CreateAsset(instance, settingsFilePath);

                return instance;
            }
        }

        public string AdMobAndroidAppId
        {
            get { return Instance.adMobAndroidAppId; }
            set { Instance.adMobAndroidAppId = value.Trim(); }
        }

        public string AdMobIosAppId
        {
            get { return Instance.adMobIosAppId; }
            set { Instance.adMobIosAppId = value.Trim(); }
        }

        public bool AccessCoarseLocationPermission
        {
            get { return accessCoarseLocationPermission; }
            set { Instance.accessCoarseLocationPermission = value; }
        }

        public bool WriteExternalStoragePermission
        {
            get { return writeExternalStoragePermission; }
            set { Instance.writeExternalStoragePermission = value; }
        }

        public bool AccessWifiStatePermission
        {
            get { return accessWifiStatePermission; }
            set { Instance.accessWifiStatePermission = value; }
        }

        public bool VibratePermission
        {
            get { return vibratePermission; }
            set { Instance.vibratePermission = value; }
        }

        public bool AccessFineLocationPermission
        {
            get { return accessFineLocationPermission; }
            set { Instance.accessFineLocationPermission = value; }
        }

        public bool AndroidMultidex
        {
            get { return androidMultidex; }
            set { Instance.androidMultidex = value; }
        }

        public bool NSUserTrackingUsageDescription
        {
            get { return nSUserTrackingUsageDescription; }
            set { Instance.nSUserTrackingUsageDescription = value; }
        }

        public bool NSLocationWhenInUseUsageDescription
        {
            get { return nSLocationWhenInUseUsageDescription; }
            set { Instance.nSLocationWhenInUseUsageDescription = value; }
        }

        public bool NSCalendarsUsageDescription
        {
            get { return nSCalendarsUsageDescription; }
            set { Instance.nSCalendarsUsageDescription = value; }
        }

        public bool NSAppTransportSecurity
        {
            get { return nSAppTransportSecurity; }
            set
            {
                Instance.nSAppTransportSecurity = value;
                if (BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.iOS, BuildTarget.iOS)) PlayerSettings.iOS.allowHTTPDownload = value;
                else Instance.nSAppTransportSecurity = false;
            }
        }

        public bool IosSkAdNetworkItems
        {
            get { return iosSKAdNetworkItems; }
            set { Instance.iosSKAdNetworkItems = value; }
        }

        public List<string> IosSkAdNetworkItemsList
        {
            get { return iosSkAdNetworkItemsList; }
            set { Instance.iosSkAdNetworkItemsList = value; }
        }

        public bool FirebaseAutoConfiguration
        {
            get { return firebaseAutoConfiguration; }
            set { Instance.firebaseAutoConfiguration = value; }
        }

        public bool FacebookAutoConfiguration
        {
            get { return facebookAutoConfiguration; }
            set { Instance.facebookAutoConfiguration = value; }
        }

        public string FacebookAndroidAppId
        {
            get { return Instance.facebookAndroidAppId; }
            set { Instance.facebookAndroidAppId = value.Trim(); }
        }

        public string FacebookIosAppId
        {
            get { return Instance.facebookIosAppId; }
            set { Instance.facebookIosAppId = value.Trim(); }
        }

        public bool FacebookAutoLogAppEvents
        {
            get { return facebookAutoLogAppEvents; }
            set { Instance.facebookAutoLogAppEvents = value; }
        }

        public bool FacebookAdvertiserIDCollection
        {
            get { return facebookAdvertiserIDCollection; }
            set { Instance.facebookAdvertiserIDCollection = value; }
        }

        public void SaveAsync()
        {
            EditorUtility.SetDirty(instance);
        }
    }
}
