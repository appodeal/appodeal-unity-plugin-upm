using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal class DmChoicesScriptableObject : ScriptableObject
    {
        [HideInInspector] [SerializeField] private bool validateDependenciesPeriodically = true;

        [HideInInspector] [SerializeField] private bool checkPeriodicallyForPluginUpdates = true;
        [HideInInspector] [SerializeField] private bool includePluginBetaVersions;

        [HideInInspector] [SerializeField] private bool enableVerboseLogging;

        [HideInInspector] [SerializeField] private SdkSelectionMode selectionMode = SdkSelectionMode.Default;
        [HideInInspector] [SerializeField] private List<SdkSelectionState> customSelections = new();

        private static DmChoicesScriptableObject _instance;

        public static DmChoicesScriptableObject Instance
        {
            get
            {
                if (_instance) return _instance;

                _instance = Resources.Load<DmChoicesScriptableObject>(DmConstants.Choices.ResourcePath);

                if (_instance) return _instance;

                if (File.Exists(DmConstants.Choices.FilePath))
                {
                    LogHelper.LogError($"Failed to load existing asset from '{DmConstants.Choices.FilePath}'");
                    return null;
                }

                Directory.CreateDirectory(DmConstants.Choices.EditorResourcesDir);
                _instance = CreateInstance<DmChoicesScriptableObject>();
                AssetDatabase.CreateAsset(_instance, DmConstants.Choices.FilePath);

                return _instance;
            }
        }

        public static void SaveToDisk()
        {
            if (_instance == null) return;
            EditorUtility.SetDirty(_instance);
            AssetDatabase.SaveAssetIfDirty(_instance);
        }

        public static bool IsSdkSelectionStateKnown(string sdkId, out SdkSelectionState state)
        {
            if (String.IsNullOrWhiteSpace(sdkId))
            {
                state = null;
                return false;
            }

            state = Instance?.CustomSelections?.FirstOrDefault(el => el.sdkId == sdkId);
            return state != null;
        }

        public bool ValidateDependenciesPeriodically
        {
            get => validateDependenciesPeriodically;
            set => validateDependenciesPeriodically = value;
        }

        public bool CheckPeriodicallyForPluginUpdates
        {
            get => checkPeriodicallyForPluginUpdates;
            set => checkPeriodicallyForPluginUpdates = value;
        }

        public bool IncludePluginBetaVersions
        {
            get => includePluginBetaVersions;
            set => includePluginBetaVersions = value;
        }

        public bool EnableVerboseLogging
        {
            get => enableVerboseLogging;
            set => enableVerboseLogging = value;
        }

        public SdkSelectionMode SelectionMode
        {
            get => selectionMode;
            set => selectionMode = value;
        }

        public List<SdkSelectionState> CustomSelections
        {
            get => customSelections;
            set => customSelections = value;
        }
    }
}
