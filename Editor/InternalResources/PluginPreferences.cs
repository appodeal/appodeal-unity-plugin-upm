using System;
using System.IO;
using UnityEngine;
using UnityEditor;

// ReSharper Disable CheckNamespace
namespace AppodealStack.UnityEditor.InternalResources
{
    public class PluginPreferences : ScriptableObject
    {
        private const string PluginPreferencesExportPath = "Appodeal/Editor/InternalResources/PluginPreferences.asset";
        private static PluginPreferences _instance;

        [SerializeField] private bool shouldIgnoreEdmInstallation;
        [SerializeField] private bool areNetworkConfigsImported;
        [SerializeField] private bool isAndroidLibraryImported;

        public static PluginPreferences Instance
        {
            get
            {
                if (_instance != null) return _instance;
                string preferencesFilePath = Path.Combine("Assets", PluginPreferencesExportPath);
                string preferencesDirectory = Path.GetDirectoryName(preferencesFilePath);
                if (!Directory.Exists(preferencesDirectory))
                {
                    Directory.CreateDirectory(preferencesDirectory ?? String.Empty);
                }

                _instance = AssetDatabase.LoadAssetAtPath<PluginPreferences>(preferencesFilePath);
                if (_instance != null) return _instance;
                _instance = CreateInstance<PluginPreferences>();
                AssetDatabase.CreateAsset(_instance, preferencesFilePath);

                return _instance;
            }
        }

        public bool ShouldIgnoreEdmInstallation
        {
            get => shouldIgnoreEdmInstallation;
            set => Instance.shouldIgnoreEdmInstallation = value;
        }

        public bool AreNetworkConfigsImported
        {
            get => areNetworkConfigsImported;
            set => Instance.areNetworkConfigsImported = value;
        }

        public bool IsAndroidLibraryImported
        {
            get => isAndroidLibraryImported;
            set => Instance.isAndroidLibraryImported = value;
        }

        public void SaveAsync()
        {
            EditorUtility.SetDirty(_instance);
        }
    }
}
