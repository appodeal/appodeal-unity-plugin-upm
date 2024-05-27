using System;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace AppodealInc.Mediation.DependencyManager.Editor
{
    [Serializable]
    internal class SdkSelectionStateModel
    {
        public string sdkId;
        public bool isSelected;

        public override string ToString() => JsonUtility.ToJson(this);
    }
}
