using System;
using UnityEngine;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    [Serializable]
    internal class ProblematicSdkDto
    {
        public string sdk_name;
        public string message;

        public override string ToString() => JsonUtility.ToJson(this);
    }
}
