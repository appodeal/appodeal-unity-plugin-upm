using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace AppodealInc.Mediation.DependencyManager.Editor
{
    [Serializable]
    internal partial class PluginConfigResponseModel
    {
        public PlatformConfig ios;
        public PlatformConfig android;

        public override string ToString() => JsonUtility.ToJson(this);
    }

    [Serializable]
    internal class PlatformConfig
    {
        public List<Sdk> sdks;
        public List<Adapter> adapters;

        public override string ToString() => JsonUtility.ToJson(this);
    }

    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal class Sdk
    {
        public int id;
        public string name;
        public string status;
        public string version;
        public string created_at;
        public string updated_at;
        public Platform platform;
        public SdkCategory category;
        public SdkStability stability;
        public Requirement requirement;

        public override string ToString() => JsonUtility.ToJson(this);
    }

    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal class Adapter
    {
        public int id;
        public bool bidding;
        public string status;
        public string version;
        public string created_at;
        public string updated_at;
        public Platform platform;
        public List<AdType> ad_types;

        public override string ToString() => JsonUtility.ToJson(this);

        public bool ShouldAdd()
        {
            const int adapterStatusPartsCount = 2;
            const char adapterPartsSeparator = '_';

            string[] sdks = status?.Split(adapterPartsSeparator, adapterStatusPartsCount);
            if (sdks?.Length != adapterStatusPartsCount) return false;
            return DmChoicesScriptableObject.IsSdkSelected(sdks[0]) && DmChoicesScriptableObject.IsSdkSelected(sdks[1]);
        }
    }
}
