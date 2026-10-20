using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Xml;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using AppodealInc.Mediation.PluginSettings.Editor;
using AppodealInc.Mediation.Shared.Editor;

namespace AppodealInc.Mediation.PreProcess.Editor
{
    internal class AppodealPreProcess : IPreprocessBuildWithReport
    {
        private static readonly AppodealLogger Log = PreProcessLogging.Log;

        private const string ManifestTemplateName = "AndroidManifest.xml";

        private const string CoarseLocation = "android.permission.ACCESS_COARSE_LOCATION";
        private const string FineLocation = "android.permission.ACCESS_FINE_LOCATION";
        private const string ExternalStorageWrite = "android.permission.WRITE_EXTERNAL_STORAGE";
        private const string AccessWifiState = "android.permission.ACCESS_WIFI_STATE";
        private const string Vibrate = "android.permission.VIBRATE";

        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            if (report.summary.platform != BuildTarget.Android) return;
            if (AppodealSettings.Instance == null) return;

            string manifestPath = Path.GetFullPath(Path.Combine(PluginPaths.AndroidLibDir, ManifestTemplateName));

            if (!File.Exists(manifestPath))
            {
                Log.Error($"Appodeal Android Manifest file was not found at {manifestPath}. The app cannot be set up correctly and may crash on startup.");
                throw new BuildFailedException("Couldn't find Appodeal Android Manifest file");
            }

            var androidManifest = new AndroidManifest(manifestPath);

            AddOptionalPermissions(manifestPath, androidManifest);
            AddAdmobAppId(manifestPath, androidManifest);

            androidManifest.Save();

            AndroidPreProcessServices.SetupManifestForFacebook();
            AndroidPreProcessServices.GenerateXMLForFirebase();
        }

        private void AddAdmobAppId(string path, AndroidManifest androidManifest)
        {
            string depsFilePath = PluginPaths.DependenciesFile;
            if (!File.Exists(depsFilePath) || !CheckContainsAdmobAdapter(depsFilePath))
            {
                if (File.Exists(path) && CheckContainsAppId(path))
                {
                    androidManifest.RemoveAdmobAppId();
                }
                Log.Info($"Missing Network config at {depsFilePath}. AdMob App Id won't be added");
                return;
            }

            string admobAppId = AppodealSettings.Instance.AdMobAndroidAppId;

            if (String.IsNullOrEmpty(admobAppId))
            {
                if (CheckContainsAppId(path))
                {
                    androidManifest.RemoveAdmobAppId();
                }
                Log.Error(
                    "AdMob App ID is not set via 'Appodeal/Appodeal Settings' tool." +
                    "\nThe app may crash on startup!");
            }
            else
            {
                if (!admobAppId.StartsWith("ca-app-pub-") || admobAppId == AppodealSettings.AdMobAppIdPlaceholder)
                {
                    Log.Error(
                        $"Incorrect AdMob App ID: '{admobAppId}'. The app may crash on startup." +
                        "\nPlease enter a valid AdMob App ID via 'Appodeal/Appodeal Settings' tool.");
                }

                if (CheckContainsAppId(path))
                {
                    androidManifest.ChangeAdmobAppId(admobAppId);
                }
                else
                {
                    androidManifest.AddAdmobAppId(admobAppId);
                }
            }
        }

        private void AddOptionalPermissions(string manifestPath, AndroidManifest androidManifest)
        {
            if (AppodealSettings.Instance.AccessCoarseLocationPermission)
            {
                if (!CheckContainsPermission(manifestPath, CoarseLocation))
                {
                    androidManifest.SetPermission(CoarseLocation);
                }
            }
            else
            {
                if (CheckContainsPermission(manifestPath, CoarseLocation))
                {
                    androidManifest.RemovePermission(CoarseLocation);
                }
            }

            if (AppodealSettings.Instance.AccessFineLocationPermission)
            {
                if (!CheckContainsPermission(manifestPath, FineLocation))
                {
                    androidManifest.SetPermission(FineLocation);
                }
            }
            else
            {
                if (CheckContainsPermission(manifestPath, FineLocation))
                {
                    androidManifest.RemovePermission(FineLocation);
                }
            }

            if (AppodealSettings.Instance.WriteExternalStoragePermission)
            {
                if (!CheckContainsPermission(manifestPath, ExternalStorageWrite))
                {
                    androidManifest.SetPermission(ExternalStorageWrite);
                }
            }
            else
            {
                if (CheckContainsPermission(manifestPath, ExternalStorageWrite))
                {
                    androidManifest.RemovePermission(ExternalStorageWrite);
                }
            }

            if (AppodealSettings.Instance.AccessWifiStatePermission)
            {
                if (!CheckContainsPermission(manifestPath, AccessWifiState))
                {
                    androidManifest.SetPermission(AccessWifiState);
                }
            }
            else
            {
                if (CheckContainsPermission(manifestPath, AccessWifiState))
                {
                    androidManifest.RemovePermission(AccessWifiState);
                }
            }

            if (AppodealSettings.Instance.VibratePermission)
            {
                if (!CheckContainsPermission(manifestPath, Vibrate))
                {
                    androidManifest.SetPermission(Vibrate);
                }
            }
            else
            {
                if (CheckContainsPermission(manifestPath, Vibrate))
                {
                    androidManifest.RemovePermission(Vibrate);
                }
            }
        }

        private bool CheckContainsAdmobAdapter(string depsFilePath)
        {
            return GetContentString(depsFilePath).Contains("com.appodeal.ads.sdk.adapters:admob");
        }

        private bool CheckContainsAppId(string manifestPath)
        {
            return GetContentString(manifestPath).Contains("APPLICATION_ID");
        }

        private bool CheckContainsPermission(string manifestPath, string permission)
        {
            return GetContentString(manifestPath).Contains(permission);
        }

        private static string GetContentString(string path)
        {
            using var reader = new StreamReader(path);
            string contentString = reader.ReadToEnd();
            reader.Close();

            return contentString;
        }
    }

    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    internal class AndroidXmlDocument : XmlDocument
    {
        private readonly string _mPath;
        protected const string AndroidXmlNamespace = "http://schemas.android.com/apk/res/android";

        protected AndroidXmlDocument(string path)
        {
            _mPath = path;
            using var reader = new XmlTextReader(_mPath);
            reader.Read();
            Load(reader);
        }

        public void Save()
        {
            SaveAs(_mPath);
        }

        private void SaveAs(string path)
        {
            using var writer = new XmlTextWriter(path, new UTF8Encoding(false));
            writer.Formatting = Formatting.Indented;
            Save(writer);
        }
    }

    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    internal class AndroidManifest : AndroidXmlDocument
    {
        public readonly XmlElement ApplicationElement;

        public AndroidManifest(string path) : base(path)
        {
            ApplicationElement = SelectSingleNode("/manifest/application") as XmlElement;
        }

        private XmlAttribute CreateAndroidAttribute(string key, string value)
        {
            var attr = CreateAttribute("android", key, AndroidXmlNamespace);
            attr.Value = value;
            return attr;
        }

        internal void SetPermission(string permission)
        {
            var manifest = SelectSingleNode("/manifest");
            if (manifest == null) return;

            var child = CreateElement("uses-permission");
            manifest.AppendChild(child);

            var newAttribute = CreateAndroidAttribute("name", permission);
            child.Attributes.Append(newAttribute);
        }

        internal void RemovePermission(string permission)
        {
            var manifest = SelectSingleNode("/manifest");
            if (manifest == null) return;

            foreach (XmlNode child in manifest.SelectNodes("uses-permission"))
            {
                for (int i = 0; i < child.Attributes?.Count; i++)
                {
                    if (child.Attributes[i].Value.Equals(permission))
                    {
                        manifest.RemoveChild(child);
                    }
                }
            }
        }

        internal void ChangeAdmobAppId(string id)
        {
            RemoveAdmobAppId();

            var manifest = SelectSingleNode("/manifest/application");
            var childMetaData = CreateElement("meta-data");
            manifest?.AppendChild(childMetaData);

            childMetaData.Attributes.Append(CreateAndroidAttribute("name", "com.google.android.gms.ads.APPLICATION_ID"));
            childMetaData.Attributes.Append(CreateAndroidAttribute("value", id));
        }

        internal void RemoveAdmobAppId()
        {
            var manifest = SelectSingleNode("/manifest/application");
            if (manifest == null) return;

            foreach (XmlNode child in manifest.SelectNodes("meta-data"))
            {
                manifest.RemoveChild(child);
            }
        }

        internal void AddAdmobAppId(string id)
        {
            var manifest = SelectSingleNode("/manifest/application");
            if (manifest == null) return;

            var child = CreateElement("meta-data");
            manifest.AppendChild(child);

            var androidAttribute = CreateAndroidAttribute("name", "com.google.android.gms.ads.APPLICATION_ID");
            var valueAttribute = CreateAndroidAttribute("value", id);
            child.Attributes.Append(androidAttribute);
            child.Attributes.Append(valueAttribute);
        }
    }
}
