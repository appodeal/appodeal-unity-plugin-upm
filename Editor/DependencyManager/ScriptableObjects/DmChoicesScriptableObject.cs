// ReSharper disable CheckNamespace

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
        [HideInInspector] [SerializeField] private bool checkPeriodicallyForAdapterUpdates = true;
        [HideInInspector] [SerializeField] private bool updateAdaptersAutomatically = true;

        [HideInInspector] [SerializeField] private bool checkPeriodicallyForPluginUpdates = true;
        [HideInInspector] [SerializeField] private bool includePluginBetaVersions;

        [HideInInspector] [SerializeField] private bool enableVerboseLogging;

        [HideInInspector] [SerializeField] private List<SdkSelectionStateModel> lastSyncSdkStates = new();

        private static DmChoicesScriptableObject _instance;

        public static DmChoicesScriptableObject Instance
        {
            get
            {
                if (_instance) return _instance;

                _instance = Resources.Load<DmChoicesScriptableObject>(DmConstants.DmChoicesResourcePath);

                if (_instance) return _instance;

                if (File.Exists(DmConstants.DmChoicesSoFilePath))
                {
                    Debug.LogError($"[Appodeal] Failed to load existing asset from '{DmConstants.DmChoicesSoFilePath}'");
                    return null;
                }

                Directory.CreateDirectory(DmConstants.EditorResourcesDir);
                _instance = CreateInstance<DmChoicesScriptableObject>();
                AssetDatabase.CreateAsset(_instance, DmConstants.DmChoicesSoFilePath);

                return _instance;
            }
        }

        public static void SaveAsync()
        {
            if (_instance == null) return;
            EditorUtility.SetDirty(_instance);
            AssetDatabase.SaveAssetIfDirty(_instance);
        }

        public static bool IsSdkSelected(string sdkId)
        {
            var state = Instance?.LastSyncSdkStates?.FirstOrDefault(el => el.sdkId == sdkId);
            return state?.isSelected ?? false;
        }

        public static bool IsSdkSelectionStateKnown(string sdkId, out SdkSelectionStateModel state)
        {
            if (String.IsNullOrWhiteSpace(sdkId))
            {
                state = null;
                return false;
            }

            state = Instance?.LastSyncSdkStates?.FirstOrDefault(el => el.sdkId == sdkId);
            return state != null;
        }

        public bool CheckPeriodicallyForAdapterUpdates
        {
            get => checkPeriodicallyForAdapterUpdates;
            set => checkPeriodicallyForAdapterUpdates = value;
        }

        public bool UpdateAdaptersAutomatically
        {
            get => updateAdaptersAutomatically;
            set => updateAdaptersAutomatically = value;
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

        public List<SdkSelectionStateModel> LastSyncSdkStates
        {
            get => lastSyncSdkStates;
            set => lastSyncSdkStates = value;
        }
    }
}
