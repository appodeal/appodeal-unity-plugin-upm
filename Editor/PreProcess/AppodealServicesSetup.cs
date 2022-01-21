using System.IO;
using System.Xml;
using UnityEditor;
using UnityEngine;
using AppodealAds.Unity.Editor.InternalResources;

namespace AppodealAds.Unity.Editor.PreProcess
{
    public class AppodealServicesSetup
    {
        public const string ApplicationIdMetaDataName = "com.facebook.sdk.ApplicationId";
        public const string AutoLogAppEventsEnabled = "com.facebook.sdk.AutoLogAppEventsEnabled";
        public const string AdvertiserIDCollectionEnabled = "com.facebook.sdk.AdvertiserIDCollectionEnabled";
        public const string AndroidManifestPath = "Plugins/Android/AndroidManifest.xml";

        [MenuItem("Appodeal/Facebook Android")]
        public static void FacebookAndroid()
        {
            SetupManifestForFacebook();
        }

        public static void SetupManifestForFacebook()
        {
            string path = Path.Combine(Application.dataPath, AndroidManifestPath);

            if(!File.Exists(path))
            {
                Debug.LogWarning(
                    $"Missing AndroidManifest {path}." +
                    "\nFacebook App ID can't be added. This service won't be initialized properly!");
                return;
            }

            UpdateManifest(path);
        }

        public static void UpdateManifest(string fullPath)
        {
            string appId = AppodealSettings.Instance.FacebookAndroidAppId;

            if(string.IsNullOrEmpty(appId)) {
                Debug.LogWarning("Facebook App ID is empty (Appodeal > Appodeal Setings). This service won't be initialized properly!");
                return;
            }

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(fullPath);
            if(xmlDocument == null) {
                Debug.LogError((object)("Couldn't load " + fullPath));
                return;
            }
            XmlNode xmlNode = FindChildNode(FindChildNode(xmlDocument, "manifest"), "application");
            if(xmlNode == null) {
                Debug.LogError((object)("Error parsing " + fullPath));
                return;
            }
            string namespaceOfPrefix = xmlNode.GetNamespaceOfPrefix("android");
            XmlElement xmlElement = xmlDocument.CreateElement("meta-data");
            xmlElement.SetAttribute("name", namespaceOfPrefix, ApplicationIdMetaDataName);
            xmlElement.SetAttribute("value", namespaceOfPrefix, "fb" + appId);
            SetOrReplaceXmlElement(xmlNode, xmlElement);

            string value = AppodealSettings.Instance.FacebookAutoLogAppEvents.ToString().ToLower();
            XmlElement xmlElement2 = xmlDocument.CreateElement("meta-data");
            xmlElement2.SetAttribute("name", namespaceOfPrefix, AutoLogAppEventsEnabled);
            xmlElement2.SetAttribute("value", namespaceOfPrefix, value);
            SetOrReplaceXmlElement(xmlNode, xmlElement2);

            string value2 = AppodealSettings.Instance.FacebookAdvertiserIDCollection.ToString().ToLower();
            XmlElement xmlElement3 = xmlDocument.CreateElement("meta-data");
            xmlElement3.SetAttribute("name", namespaceOfPrefix, AdvertiserIDCollectionEnabled);
            xmlElement3.SetAttribute("value", namespaceOfPrefix, value2);
            SetOrReplaceXmlElement(xmlNode, xmlElement3);

            XmlWriterSettings settings = new XmlWriterSettings {
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace
            };
            using(XmlWriter w = XmlWriter.Create(fullPath, settings)) {
                xmlDocument.Save(w);
            }
        }

        private static XmlNode FindChildNode(XmlNode parent, string name)
        {
            for(XmlNode xmlNode = parent.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling) {
                if(xmlNode.Name.Equals(name)) {
                    return xmlNode;
                }
            }
            return null;
        }

        private static void SetOrReplaceXmlElement(XmlNode parent, XmlElement newElement)
        {
            string attribute = newElement.GetAttribute("name");
            string name = newElement.Name;
            if(TryFindElementWithAndroidName(parent, attribute, out XmlElement element, name)) {
                parent.ReplaceChild(newElement, element);
            } else {
                parent.AppendChild(newElement);
            }
        }

        private static bool TryFindElementWithAndroidName(XmlNode parent, string attrNameValue, out XmlElement element, string elementType = "activity")
        {
            string namespaceOfPrefix = parent.GetNamespaceOfPrefix("android");
            for(XmlNode xmlNode = parent.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling) {
                XmlElement xmlElement = xmlNode as XmlElement;
                if(xmlElement != null && xmlElement.Name == elementType && xmlElement.GetAttribute("name", namespaceOfPrefix) == attrNameValue) {
                    element = xmlElement;
                    return true;
                }
            }
            element = null;
            return false;
        }

        private static XmlElement CreateActivityElement(XmlDocument doc, string ns, string activityName, bool exported = false)
        {
            XmlElement xmlElement = doc.CreateElement("activity");
            xmlElement.SetAttribute("name", ns, activityName);
            if(exported) {
                xmlElement.SetAttribute("exported", ns, "true");
            }
            return xmlElement;
        }
    }
}
