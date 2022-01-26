#if UNITY_ANDROID
using UnityEditor;
using UnityEngine;
using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Collections.Generic;
using AppodealAds.Unity.Editor.Utils;
using AppodealAds.Unity.Editor.InternalResources;
using SimpleJSON;

namespace AppodealAds.Unity.Editor.PreProcess
{
    public class AndroidPreProcessServices
    {
    
    #region Firebase

        public static void GenerateXMLForFirebase()
        {
            string xmlFilePath = Path.Combine(Application.dataPath,
                                        AppodealEditorConstants.AppodealAndroidLibPath,
                                        AppodealEditorConstants.FirebaseAndroidConfigPath,
                                        AppodealEditorConstants.FirebaseAndroidConfigFile);

            if (!AppodealSettings.Instance.FirebaseAutoConfiguration)
            {
                RemoveFirebaseXml(xmlFilePath);
                return;
            }

            string jsonFilePath = Path.Combine(Application.dataPath, AppodealEditorConstants.FirebaseAndroidJsonFile);

            if (!File.Exists(jsonFilePath))
            {
                Debug.LogError($"Firebase Android Configuration file was not found at {jsonFilePath}\nThis service cannot be configured correctly.");
                return;
            }

            var firebaseStrings = ParseFirebaseJson(jsonFilePath);

            if (firebaseStrings == null)
            {
                Debug.LogError($"Couldn't find a valid Firebase configuration for package name: {Application.identifier} in {jsonFilePath} file.\nThis service cannot be configured correctly.");
                return;
            }

            CreateOrReplaceFirebaseXml(xmlFilePath, firebaseStrings);

        }

        private static Dictionary<string,string> ParseFirebaseJson(string path)
        {
            StreamReader reader = new StreamReader(path); 
            string jsonString = reader.ReadToEnd();

            JSONNode parsedJSON = JSON.Parse(jsonString);
            var clientJsonArray = parsedJSON["client"];

            if (clientJsonArray == null) return null;

            foreach (JSONNode node in clientJsonArray)
            {
                var clientInfoJsonObject = node["client_info"].AsObject;
                var andrClientInfoJsonObject = clientInfoJsonObject["android_client_info"].AsObject;

                if (andrClientInfoJsonObject["package_name"].Value == Application.identifier ) {
                    var projectInfoJsonObject = parsedJSON["project_info"].AsObject;
                    var outputDict = new Dictionary<string, string>();
                    outputDict.Add("firebase_database_url", projectInfoJsonObject["firebase_url"].Value);
                    outputDict.Add("gcm_defaultSenderId", projectInfoJsonObject["project_number"].Value);
                    outputDict.Add("google_storage_bucket", projectInfoJsonObject["storage_bucket"].Value);
                    outputDict.Add("project_id", projectInfoJsonObject["project_id"].Value);
                    outputDict.Add("google_api_key", node["api_key"][0].AsObject["current_key"].Value);
                    outputDict.Add("google_crash_reporting_api_key", node["api_key"][0].AsObject["current_key"].Value);
                    outputDict.Add("google_app_id", clientInfoJsonObject["mobilesdk_app_id"].Value);
                    outputDict.Add("default_web_client_id", node["oauth_client"][0].AsObject["client_id"].Value);
                    return outputDict;
                }
            }
            return null;
        }

        private static void CreateOrReplaceFirebaseXml(string path, Dictionary<string,string> firebaseStrings)
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlElement root = xmlDocument.DocumentElement;

            XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDocument.InsertBefore(xmlDeclaration, root);

            XmlElement resourcesElement = xmlDocument.CreateElement("resources");

            XmlAttribute attribute = xmlDocument.CreateAttribute("tools", "keep", "http://schemas.android.com/tools");
            string toolsKeep = "@string/firebase_database_url,@string/gcm_defaultSenderId,@string/google_storage_bucket,@string/project_id,@string/google_api_key,@string/google_crash_reporting_api_key,@string/google_app_id,@string/default_web_client_id";
            attribute.Value = toolsKeep;
            resourcesElement.Attributes.Append(attribute);

            firebaseStrings.Keys.ToList().ForEach(key =>
                resourcesElement.AppendChild(CreateFirebaseStringElement(key, firebaseStrings[key], xmlDocument)));

            xmlDocument.AppendChild(resourcesElement);
            xmlDocument.Save(path);
        }

        private static XmlElement CreateFirebaseStringElement(string elName, string elValue, XmlDocument xmlDocument)
        {
            XmlElement xmlElement = xmlDocument.CreateElement("string");
            xmlElement.SetAttribute("name", elName);
            xmlElement.SetAttribute("translatable", "false");
            XmlText xmlText = xmlDocument.CreateTextNode(elValue);
            xmlElement.AppendChild(xmlText);
            
            return xmlElement;
        }

        private static void RemoveFirebaseXml(string path)
        {
            if (File.Exists(path))
            {
                FileUtil.DeleteFileOrDirectory(path);
                FileUtil.DeleteFileOrDirectory($"{path}.meta");
            }
        }
    
    #endregion

    #region Facebook
        public static void SetupManifestForFacebook()
        {
            string path = Path.Combine(Application.dataPath,
                                        AppodealEditorConstants.AppodealAndroidLibPath,
                                        AppodealEditorConstants.AndroidManifestFile);

            if (!File.Exists(path))
            {
                Debug.LogWarning(
                    $"Missing AndroidManifest.xml file at {path}." +
                    "\nFacebook App ID cannot be added. This service won't be initialized properly!");
                return;
            }

            if (!AppodealSettings.Instance.FacebookAutoConfiguration)
            {
                return;
            }

            UpdateManifest(path);
        }

        private static void UpdateManifest(string fullPath)
        {
            string appId = AppodealSettings.Instance.FacebookAndroidAppId;

            if (string.IsNullOrEmpty(appId)) {
                Debug.LogWarning("Facebook App ID is empty (Appodeal > Appodeal Setings). This service won't be initialized properly!");
                return;
            }

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(fullPath);
            if (xmlDocument == null) {
                Debug.LogError((object)("Couldn't load " + fullPath));
                return;
            }
            XmlNode xmlNode = FindChildNode(FindChildNode(xmlDocument, "manifest"), "application");
            if (xmlNode == null) {
                Debug.LogError((object)("Error parsing " + fullPath));
                return;
            }
            string namespaceOfPrefix = xmlNode.GetNamespaceOfPrefix("android");
            XmlElement xmlElement = xmlDocument.CreateElement("meta-data");
            xmlElement.SetAttribute("name", namespaceOfPrefix, AppodealEditorConstants.FacebookApplicationId);
            xmlElement.SetAttribute("value", namespaceOfPrefix, "fb" + appId);
            SetOrReplaceXmlElement(xmlNode, xmlElement);

            string value = AppodealSettings.Instance.FacebookAutoLogAppEvents.ToString().ToLower();
            XmlElement xmlElement2 = xmlDocument.CreateElement("meta-data");
            xmlElement2.SetAttribute("name", namespaceOfPrefix, AppodealEditorConstants.FacebookAutoLogAppEventsEnabled);
            xmlElement2.SetAttribute("value", namespaceOfPrefix, value);
            SetOrReplaceXmlElement(xmlNode, xmlElement2);

            string value2 = AppodealSettings.Instance.FacebookAdvertiserIDCollection.ToString().ToLower();
            XmlElement xmlElement3 = xmlDocument.CreateElement("meta-data");
            xmlElement3.SetAttribute("name", namespaceOfPrefix, AppodealEditorConstants.FacebookAdvertiserIDCollectionEnabled);
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
            for (XmlNode xmlNode = parent.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling) {
                if (xmlNode.Name.Equals(name)) {
                    return xmlNode;
                }
            }
            return null;
        }

        private static void SetOrReplaceXmlElement(XmlNode parent, XmlElement newElement)
        {
            string attribute = newElement.GetAttribute("name");
            string name = newElement.Name;
            if (TryFindElementWithAndroidName(parent, attribute, out XmlElement element, name)) {
                parent.ReplaceChild(newElement, element);
            } else {
                parent.AppendChild(newElement);
            }
        }

        private static bool TryFindElementWithAndroidName(XmlNode parent, string attrNameValue, out XmlElement element, string elementType = "activity")
        {
            string namespaceOfPrefix = parent.GetNamespaceOfPrefix("android");
            for (XmlNode xmlNode = parent.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling) {
                XmlElement xmlElement = xmlNode as XmlElement;
                if (xmlElement != null && xmlElement.Name == elementType && xmlElement.GetAttribute("name", namespaceOfPrefix) == attrNameValue) {
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
            if (exported) {
                xmlElement.SetAttribute("exported", ns, "true");
            }
            return xmlElement;
        }

    #endregion

    }
}
#endif
