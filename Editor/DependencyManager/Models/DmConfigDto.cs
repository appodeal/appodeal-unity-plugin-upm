// ReSharper disable CheckNamespace

using System;
using System.Collections.Generic;
using UnityEngine;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    [Serializable]
    internal class DmConfigDto
    {
        public XmlDependenciesModel LocalDeps { get; set; }
        public List<Plugin> Plugins { get; set; }
        public PluginConfigResponseModel RemotePluginConfig { get; set; }

        public override string ToString() => JsonUtility.ToJson(this);
    }
}
