using System;
using UnityEngine;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    [Serializable]
    internal class BadgeDto
    {
        public int type;
        public string message;

        public override string ToString() => JsonUtility.ToJson(this);
    }
}
