using System;
using System.Collections.Generic;
using UnityEngine;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    [Serializable]
    internal class VersionInfoDto
    {
        public int id;
        public string name;
        public string message;
        public List<BadgeDto> badges;
        public List<string> mediations;

        public override string ToString() => JsonUtility.ToJson(this);
    }
}
