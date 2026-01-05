using System;
using System.Collections.Generic;
using UnityEngine;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    [Serializable]
    internal class SdkDto
    {
        public string name;
        public string display_name;
        public int category;
        public int requirement;
        public int ad_types;
        public List<VersionInfoDto> versions;
        public string warning;

        public override string ToString() => JsonUtility.ToJson(this);
    }
}
