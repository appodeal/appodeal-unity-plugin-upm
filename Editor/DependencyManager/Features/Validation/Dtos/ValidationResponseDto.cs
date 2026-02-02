using System;
using System.Collections.Generic;
using UnityEngine;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    [Serializable]
    internal class ValidationResponseDto
    {
        public List<ProblematicSdkDto> problematic_sdk_versions;

        public override string ToString() => JsonUtility.ToJson(this);
    }
}
