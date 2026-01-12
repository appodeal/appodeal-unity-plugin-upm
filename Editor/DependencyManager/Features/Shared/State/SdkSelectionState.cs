using System;
using UnityEngine;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    [Serializable]
    internal class SdkSelectionState
    {
        public string sdkId;
        public bool isSelected;

        public override string ToString() => JsonUtility.ToJson(this);
    }
}
