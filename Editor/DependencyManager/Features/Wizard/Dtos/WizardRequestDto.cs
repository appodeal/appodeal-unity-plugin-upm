using System;
using UnityEngine;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    [Serializable]
    internal class WizardRequestDto
    {
        public PlatformSelectionDto android;
        public PlatformSelectionDto ios;

        public override string ToString() => JsonUtility.ToJson(this);
    }
}
