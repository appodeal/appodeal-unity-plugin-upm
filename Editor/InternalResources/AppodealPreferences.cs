using UnityEngine;
using UnityEditor;
using System.IO;
using System.Diagnostics.CodeAnalysis;

namespace AppodealAds.Unity.Editor.InternalResources
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class AppodealPreferences : ScriptableObject
    {
        private static AppodealPreferences      instance;

        private const string                    AppodealPreferencesExportPath = "Appodeal/Editor/InternalResources/AppodealPreferences.asset";

        [SerializeField] private bool           shouldIgnoreEDMInstallation;
        [SerializeField] private bool           wereNetworkConfigsImported;
        
        public static AppodealPreferences Instance
        {
            get
            {
                if (instance != null) return instance;
                var preferencesFilePath = Path.Combine("Assets", AppodealPreferencesExportPath);
                var preferencesDirectory = Path.GetDirectoryName(preferencesFilePath);
                if (!Directory.Exists(preferencesDirectory))
                {
                    Directory.CreateDirectory(preferencesDirectory ?? string.Empty);
                }

                instance = AssetDatabase.LoadAssetAtPath<AppodealPreferences>(preferencesFilePath);
                if (instance != null) return instance;
                instance = CreateInstance<AppodealPreferences>();
                AssetDatabase.CreateAsset(instance, preferencesFilePath);

                return instance;
            }
        }

        public bool ShouldIgnoreEDMInstallation
        {
            get { return shouldIgnoreEDMInstallation; }
            set { Instance.shouldIgnoreEDMInstallation = value; }
        }

        public bool WereNetworkConfigsImported
        {
            get { return wereNetworkConfigsImported; }
            set { Instance.wereNetworkConfigsImported = value; }
        }

        public void SaveAsync()
        {
            EditorUtility.SetDirty(instance);
        }

    }
}
