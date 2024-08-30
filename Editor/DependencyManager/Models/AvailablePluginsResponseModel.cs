// ReSharper disable CheckNamespace

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    [Serializable]
    internal class AvailablePluginsResponseModel
    {
        public List<Plugin> plugins;

        public override string ToString() => JsonUtility.ToJson(this);
    }

    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "NotAccessedField.Global")]
    internal class Plugin
    {
        public long id;
        public string name;
        public string status;
        public string version;
        public string created_at;
        public string updated_at;
        public Framework framework;

        public override string ToString() => JsonUtility.ToJson(this);
    }
}
