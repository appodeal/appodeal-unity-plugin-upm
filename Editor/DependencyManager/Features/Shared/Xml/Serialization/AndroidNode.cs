using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    [Serializable]
    [XmlRoot(ElementName = "androidPackages")]
    public class AndroidNode
    {
        [XmlElement(ElementName = "androidPackage")]
        public List<PackageNode> Packages { get; set; }

        [XmlArray("repositories")]
        [XmlArrayItem("repository")]
        public List<string> Repositories { get; set; }
    }
}
