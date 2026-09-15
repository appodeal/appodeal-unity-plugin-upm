using System;
using System.Xml.Serialization;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    [Serializable]
    [XmlRoot(ElementName = "swiftPackage")]
    public class SwiftPackageNode
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "replacesPod")]
        public string ReplacesPod { get; set; }
    }
}
