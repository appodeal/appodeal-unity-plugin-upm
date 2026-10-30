using System;
using System.Linq;
using System.Xml.Linq;

namespace AppodealInc.Mediation.PreProcess.Editor
{
    internal class StringResources
    {
        private static readonly XNamespace Tools = "http://schemas.android.com/tools";

        private readonly XElement _resources = new("resources", new XAttribute(XNamespace.Xmlns + "tools", Tools));

        public StringResources()
        {
            Document = new XDocument(_resources);
        }

        public XDocument Document { get; }

        public bool IsEmpty => !_resources.HasElements;

        public void AddString(string name, string value)
        {
            _resources.Add(new XElement("string", new XAttribute("name", name), new XAttribute("translatable", "false"), value));
            _resources.SetAttributeValue(Tools + "keep", String.Join(",", _resources.Elements("string").Select(e => $"@string/{(string)e.Attribute("name")}")));
        }
    }
}
