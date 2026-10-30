using System;
using System.Xml.Linq;
using UnityEditor.Build;
using AppodealInc.Mediation.Shared.Editor;

namespace AppodealInc.Mediation.PreProcess.Editor
{
    internal class AndroidManifest
    {
        private static readonly AppodealLogger Log = PreProcessLogging.Log;

        private static readonly XNamespace Android = "http://schemas.android.com/apk/res/android";

        private readonly XElement _manifest;
        private readonly XElement _application;

        public AndroidManifest(XDocument document)
        {
            Document = document ?? throw new ArgumentNullException(nameof(document));

            _manifest = document.Root;
            if (_manifest?.Name != "manifest")
            {
                Log.Error("Manifest file has no <manifest> root element. The build can't be configured.");
                throw new BuildFailedException("Appodeal Android manifest file has no <manifest> root element.");
            }

            _application = _manifest.Element("application");
            if (_application == null)
            {
                Log.Error("Manifest file has no <application> element. The build can't be configured.");
                throw new BuildFailedException("Appodeal Android manifest file has no <application> element.");
            }
        }

        public XDocument Document { get; }

        public void AddPermission(string name)
        {
            _manifest.Add(new XElement("uses-permission", new XAttribute(Android + "name", name)));
        }

        public void AddMetaData(string name, string value)
        {
            _application.Add(new XElement("meta-data", new XAttribute(Android + "name", name), new XAttribute(Android + "value", value)));
        }
    }
}
