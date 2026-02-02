using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    [Serializable]
    [XmlRoot(ElementName = "iosPods")]
    public class IosNode
    {
        [XmlElement(ElementName = "iosPod")]
        public List<PodNode> Pods { get; set; }

        [XmlArray("sources")]
        [XmlArrayItem("source")]
        public List<string> Sources { get; set; }
    }
}
