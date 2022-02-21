using UnityEngine;
using UnityEditor;
using System.IO;
using System.Diagnostics.CodeAnalysis;

namespace AppodealStack.UnityEditor.InternalResources
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class PluginPreferences : ScriptableObject
    {
        private static PluginPreferences    instance;

        private const string    PluginPreferencesExportPath = "Appodeal/Editor/InternalResources/PluginPreferences.asset";

        [SerializeField] private bool   shouldIgnoreEDMInstallation;
        [SerializeField] private bool   areNetworkConfigsImported;
        [SerializeField] private bool   isAndroidLibraryImported;
        
        public static PluginPreferences Instance
        {
            get
            {
                if (instance != null) return instance;
                var preferencesFilePath = Path.Combine("Assets", PluginPreferencesExportPath);
                var preferencesDirectory = Path.GetDirectoryName(preferencesFilePath);
                if (!Directory.Exists(preferencesDirectory))
                {
                    Directory.CreateDirectory(preferencesDirectory ?? string.Empty);
                }

                instance = AssetDatabase.LoadAssetAtPath<PluginPreferences>(preferencesFilePath);
                if (instance != null) return instance;
                instance = CreateInstance<PluginPreferences>();
                AssetDatabase.CreateAsset(instance, preferencesFilePath);

                return instance;
            }
        }

        public bool ShouldIgnoreEDMInstallation
        {
            get { return shouldIgnoreEDMInstallation; }
            set { Instance.shouldIgnoreEDMInstallation = value; }
        }

        public bool AreNetworkConfigsImported
        {
            get { return areNetworkConfigsImported; }
            set { Instance.areNetworkConfigsImported = value; }
        }

        public bool IsAndroidLibraryImported
        {
            get { return isAndroidLibraryImported; }
            set { Instance.isAndroidLibraryImported = value; }
        }

        public void SaveAsync()
        {
            EditorUtility.SetDirty(instance);
        }

    }
}
