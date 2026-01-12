using System;
using System.Xml.Serialization;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    [Serializable]
    [XmlRoot(ElementName = "iosPod")]
    public class PodNode
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "version")]
        public string Version { get; set; }

        [XmlAttribute(AttributeName = "minTargetSdk")]
        public string MinTargetSdk { get; set; }
    }
}
