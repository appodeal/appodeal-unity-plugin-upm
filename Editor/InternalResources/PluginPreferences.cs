using System.IO;
using UnityEngine;
using UnityEditor;

// ReSharper Disable CheckNamespace
namespace AppodealStack.UnityEditor.InternalResources
{
    public class PluginPreferences : ScriptableObject
    {
        private const string PluginPreferencesExportPath = "Assets/Appodeal/Editor/InternalResources";
        private const string PluginPreferencesFileName = "PluginPreferences.asset";

        [HideInInspector] [SerializeField] private bool areAdaptersImported;
        [HideInInspector] [SerializeField] private bool isAndroidLibraryImported;

        private static PluginPreferences _instance;
        public static PluginPreferences Instance
        {
            get
            {
                if (_instance) return _instance;

                Directory.CreateDirectory(PluginPreferencesExportPath);

                string preferencesFilePath = $"{PluginPreferencesExportPath}/{PluginPreferencesFileName}";

                _instance = AssetDatabase.LoadAssetAtPath<PluginPreferences>(preferencesFilePath);
                if (_instance) return _instance;
                _instance = CreateInstance<PluginPreferences>();
                AssetDatabase.CreateAsset(_instance, preferencesFilePath);

                return _instance;
            }
        }

        public bool AreAdaptersImported
        {
            get => areAdaptersImported;
            set => areAdaptersImported = value;
        }

        public bool IsAndroidLibraryImported
        {
            get => isAndroidLibraryImported;
            set => isAndroidLibraryImported = value;
        }

        public static void SaveAsync()
        {
            EditorUtility.SetDirty(_instance);
        }
    }
}
