using System;
using System.Collections.Generic;
using UnityEngine;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    [Serializable]
    internal class AvailablePluginsResponseDto
    {
        public List<PluginDto> plugins;

        public override string ToString() => JsonUtility.ToJson(this);
    }
}
