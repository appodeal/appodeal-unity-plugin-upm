using System;
using System.Collections.Generic;
using UnityEngine;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    [Serializable]
    internal class PlatformSelectionDto
    {
        public List<int> mediations;
        public List<int> networks;
        public List<int> services;

        public override string ToString() => JsonUtility.ToJson(this);
    }
}
