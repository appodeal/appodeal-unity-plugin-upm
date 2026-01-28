using System;
using UnityEngine;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    [Serializable]
    internal class ValidationErrorResponseDto
    {
        public string error;

        public override string ToString() => JsonUtility.ToJson(this);
    }
}
