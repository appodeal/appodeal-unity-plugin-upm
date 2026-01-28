using System;
using System.Collections.Generic;
using UnityEngine;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    [Serializable]
    internal class WizardResponseDto
    {
        public List<SdkDto> android;
        public List<SdkDto> ios;

        public override string ToString() => JsonUtility.ToJson(this);
    }
}
