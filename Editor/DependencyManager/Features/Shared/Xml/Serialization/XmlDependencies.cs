using System;
using System.Xml.Serialization;
using UnityEngine;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    [Serializable]
    [XmlRoot(ElementName = "dependencies")]
    public class XmlDependencies
    {
        [XmlAttribute(AttributeName = "pluginVersion")]
        public string PluginVersion { get; set; }

        [XmlElement(ElementName = "iosPods")]
        public IosNode Ios { get; set; }

        [XmlElement(ElementName = "androidPackages")]
        public AndroidNode Android { get; set; }

        public override string ToString() => JsonUtility.ToJson(this);

        public bool IsDifferentFromRemote(XmlDependencies remote) => DependenciesDiff.Get(this, remote).Any();
    }
}
