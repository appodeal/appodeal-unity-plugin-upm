using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    [Serializable]
    [XmlRoot(ElementName = "remoteSwiftPackage")]
    public class RemoteSwiftPackageNode
    {
        [XmlAttribute(AttributeName = "url")]
        public string Url { get; set; }

        [XmlAttribute(AttributeName = "version")]
        public string Version { get; set; }

        [XmlElement(ElementName = "swiftPackage")]
        public List<SwiftPackageNode> Packages { get; set; }
    }
}
