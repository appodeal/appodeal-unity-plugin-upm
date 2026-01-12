using System;
using UnityEngine;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    [Serializable]
    internal class PluginDto
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
