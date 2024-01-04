using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using AppodealStack.UnityEditor.Utils;
using AppodealStack.UnityEditor.InternalResources;

// ReSharper Disable CheckNamespace
namespace AppodealStack.UnityEditor.PreProcess
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public class AppodealPreProcess : IPreprocessBuildWithReport
    {
        #region Constants

        //Templates in Unity Editor Data folder
        public const string ManifestDefaultTemplatePath = "PlaybackEngines/AndroidPlayer/Apk/AndroidManifest.xml";

        private const string GradleDefaultTemplatePath = "PlaybackEngines/AndroidPlayer/Tools/GradleTemplates";

        //Paths without leading Assets folder
        public const string AppodealTemplatesPath = "Appodeal/InternalResources";

        private const string AndroidPluginsPath = "Plugins/Android";
        private const string GradleTemplateName = "mainTemplate.gradle";
        private const string ManifestTemplateName = "AndroidManifest.xml";
        private const string AppodealAndroidLibDirPath = "Plugins/Android/appodeal.androidlib";

        //Gradle search lines
        public const string GradleGoogleRepository = "google()";
        public const string GradleGoogleRepositoryCompat = "maven { url \"https://maven.google.com\" }";
        public const string GradleDependencies = "**DEPS**";
        public const string GradleAppID = "**APPLICATIONID**";
        public const string GradleUseProguard = "useProguard";
        public const string GradleMultidexDependencyWoVersion = "androidx.multidex:multidex:";
        public const string GradleDefaultConfig = "defaultConfig";
        public const string CompileOptions = "compileOptions {";
        public const string GradleJavaVersion18 = "JavaVersion.VERSION_1_8";
        public const string GradleSourceCapability = "sourceCompatibility ";
        public const string GradleTargetCapability = "targetCompatibility ";

        //Gradle add lines
        public const string GradleMultidexEnable = "multiDexEnabled true";

        private const string GradleImplementation = "implementation ";
        private const string GradleMultidexDependency = "'androidx.multidex:multidex:2.0.1'";

        //Manifest add lines
        private const string ManifestMultidexApp = "androidx.multidex.MultiDexApplication";

        #endregion

        public void OnPreprocessBuild(BuildReport report)
        {
            if (report.summary.platform.ToString() != "Android") return;

            var manifestPath = Path.Combine(Application.dataPath, AppodealAndroidLibDirPath, ManifestTemplateName);

            if (!File.Exists(manifestPath))
            {
                Debug.LogError($"Appodeal Android Manifest file was not found at {manifestPath}. The app cannot be set up correctly and may crash on startup.");
                throw new BuildFailedException("Couldn't find Appodeal Android Manifest file");
            }

            var androidManifest = new AndroidManifest(manifestPath);

            AddOptionalPermissions(manifestPath, androidManifest);
            AddAdmobAppId(manifestPath, androidManifest);
            EnableMultidex(manifestPath, androidManifest);

            androidManifest.Save();

            AndroidPreProcessServices.SetupManifestForFacebook();
            AndroidPreProcessServices.GenerateXMLForFirebase();
        }

        private void EnableMultidex(string manifestPath, AndroidManifest androidManifest)
        {
            if(CheckContainsMultidex(manifestPath, ManifestMultidexApp))
            {
                androidManifest.RemoveMultiDexApplication();
            }
        }

        private void AddAdmobAppId(string path, AndroidManifest androidManifest)
        {
            string admobDepPath = Path.Combine(AppodealEditorConstants.PluginPath, AppodealEditorConstants.DependenciesPath,
                                                $"{AppodealEditorConstants.GoogleAdMob}{AppodealEditorConstants.Dependencies}{AppodealEditorConstants.XmlFileExtension}");
            if (!File.Exists(admobDepPath))
            {
                if (File.Exists(path) && CheckContainsAppId(path))
                {
                    androidManifest.RemoveAdmobAppId();
                }
                Debug.Log($"Missing Network config at {admobDepPath}. Admob App Id won't be added.");
                return;
            }

            string admobAppId = AppodealSettings.Instance.AdMobAndroidAppId;

            if (String.IsNullOrEmpty(admobAppId))
            {
                if (CheckContainsAppId(path))
                {
                    androidManifest.RemoveAdmobAppId();
                }
                Debug.LogError(
                    $"Admob App ID is not set via 'Appodeal/Appodeal Settings' tool." +
                    "\nThe app may crash on startup!");
            }
            else
            {
                if (!admobAppId.StartsWith("ca-app-pub-") || admobAppId == AppodealEditorConstants.AdMobAppIdPlaceholder)
                {
                    Debug.LogError(
                        "Incorrect value. The app may crash on startup." +
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
                if (!CheckContainsPermission(manifestPath, AppodealUnityUtils.CoarseLocation))
                {
                    androidManifest.SetPermission(AppodealUnityUtils.CoarseLocation);
                }
            }
            else
            {
                if (CheckContainsPermission(manifestPath, AppodealUnityUtils.CoarseLocation))
                {
                    androidManifest.RemovePermission(AppodealUnityUtils.CoarseLocation);
                }
            }

            if (AppodealSettings.Instance.AccessFineLocationPermission)
            {
                if (!CheckContainsPermission(manifestPath, AppodealUnityUtils.FineLocation))
                {
                    androidManifest.SetPermission(AppodealUnityUtils.FineLocation);
                }
            }
            else
            {
                if (CheckContainsPermission(manifestPath, AppodealUnityUtils.FineLocation))
                {
                    androidManifest.RemovePermission(AppodealUnityUtils.FineLocation);
                }
            }

            if (AppodealSettings.Instance.WriteExternalStoragePermission)
            {
                if (!CheckContainsPermission(manifestPath, AppodealUnityUtils.ExternalStorageWrite))
                {
                    androidManifest.SetPermission(AppodealUnityUtils.ExternalStorageWrite);
                }
            }
            else
            {
                if (CheckContainsPermission(manifestPath, AppodealUnityUtils.ExternalStorageWrite))
                {
                    androidManifest.RemovePermission(AppodealUnityUtils.ExternalStorageWrite);
                }
            }

            if (AppodealSettings.Instance.AccessWifiStatePermission)
            {
                if (!CheckContainsPermission(manifestPath, AppodealUnityUtils.AccessWifiState))
                {
                    androidManifest.SetPermission(AppodealUnityUtils.AccessWifiState);
                }
            }
            else
            {
                if (CheckContainsPermission(manifestPath, AppodealUnityUtils.AccessWifiState))
                {
                    androidManifest.RemovePermission(AppodealUnityUtils.AccessWifiState);
                }
            }

            if (AppodealSettings.Instance.VibratePermission)
            {
                if (!CheckContainsPermission(manifestPath, AppodealUnityUtils.Vibrate))
                {
                    androidManifest.SetPermission(AppodealUnityUtils.Vibrate);
                }
            }
            else
            {
                if (CheckContainsPermission(manifestPath, AppodealUnityUtils.Vibrate))
                {
                    androidManifest.RemovePermission(AppodealUnityUtils.Vibrate);
                }
            }
        }

        private bool CheckContainsAppId(string manifestPath)
        {
            return GetContentString(manifestPath).Contains("APPLICATION_ID");
        }

        private bool CheckContainsPermission(string manifestPath, string permission)
        {
            return GetContentString(manifestPath).Contains(permission);
        }

        private bool CheckContainsMultidex(string manifestPath, string multidex)
        {
            return GetContentString(manifestPath).Contains(multidex);
        }

        private bool CheckContainsMultidexDependency()
        {
            return GetContentString(GetDefaultGradleTemplate())
                .Contains(GradleImplementation + GradleMultidexDependency);
        }

        private void RemoveMultidexDependency()
        {
            var contentString = GetContentString(GetDefaultGradleTemplate());
            contentString = Regex.Replace(contentString, GradleImplementation + GradleMultidexDependency,
                String.Empty);

            using var writer = new StreamWriter(GetDefaultGradleTemplate());
            writer.Write(contentString);
            writer.Close();
        }

        private static string GetDefaultGradleTemplate()
        {
            var defaultGradleTemplateFullName = AppodealUnityUtils.CombinePaths(
                EditorApplication.applicationContentsPath,
                GradleDefaultTemplatePath,
                GradleTemplateName);
            if (File.Exists(defaultGradleTemplateFullName)) return defaultGradleTemplateFullName;
            var unixAppContentsPath =
                Path.GetDirectoryName(Path.GetDirectoryName(EditorApplication.applicationContentsPath));
            defaultGradleTemplateFullName = AppodealUnityUtils.CombinePaths(unixAppContentsPath,
                GradleDefaultTemplatePath,
                GradleTemplateName);

            return defaultGradleTemplateFullName;
        }

        private static string GetContentString(string path)
        {
            using var reader = new StreamReader(path);
            string contentString = reader.ReadToEnd();
            reader.Close();

            return contentString;
        }

        private static string GetCustomGradleScriptPath()
        {
            var androidDirectory = new DirectoryInfo(Path.Combine("Assets", AndroidPluginsPath));
            var filePaths = androidDirectory.GetFiles("*.gradle");
            return filePaths.Length > 0
                ? Path.Combine(Path.Combine(Application.dataPath, AndroidPluginsPath), filePaths[0].Name)
                : null;
        }

        public int callbackOrder => 0;
    }

    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    internal class AndroidXmlDocument : XmlDocument
    {
        private readonly string _mPath;
        protected const string AndroidXmlNamespace = "http://schemas.android.com/apk/res/android";

        protected AndroidXmlDocument(string path)
        {
            _mPath = path;
            using (var reader = new XmlTextReader(_mPath))
            {
                reader.Read();
                Load(reader);
            }

            var nsMgr = new XmlNamespaceManager(NameTable);
            nsMgr.AddNamespace("android", AndroidXmlNamespace);
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
            var manifest = SelectSingleNode("/manifest/application");
            RemoveAdmobAppId();
            var childMetaData = CreateElement("meta-data");
            manifest?.AppendChild(childMetaData);
            childMetaData.Attributes.Append(CreateAndroidAttribute("name",
                "com.google.android.gms.ads.APPLICATION_ID"));
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

        internal void RemoveMultiDexApplication()
        {
            var manifest = SelectSingleNode("/manifest/application");
            if (manifest == null) return;
            for (int i = 0; i < manifest.Attributes?.Count; i++)
            {
                if (manifest.Attributes[i].Value.Equals("androidx.multidex.MultiDexApplication"))
                {
                    manifest.Attributes.Remove(manifest.Attributes[i]);
                }
            }
        }
    }
}
