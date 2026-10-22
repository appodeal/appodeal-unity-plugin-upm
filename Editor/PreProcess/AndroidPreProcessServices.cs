using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using UnityEngine;
using AppodealInc.Mediation.PluginSettings.Editor;
using AppodealInc.Mediation.Shared.Editor;

namespace AppodealInc.Mediation.PreProcess.Editor
{
    internal static class AndroidPreProcessServices
    {
        private static readonly AppodealLogger Log = PreProcessLogging.Log;

        private const string AndroidManifestFile = "AndroidManifest.xml";
        private const string FirebaseAndroidConfigPath = "res/values";
        private const string FirebaseAndroidConfigFile = "google-services.xml";
        private const string FirebaseAndroidJsonFile = "google-services.json";

        private const string FacebookApplicationId = "com.facebook.sdk.ApplicationId";
        private const string FacebookClientToken = "com.facebook.sdk.ClientToken";
        private const string FacebookAutoLogAppEventsEnabled = "com.facebook.sdk.AutoLogAppEventsEnabled";
        private const string FacebookAdvertiserIDCollectionEnabled = "com.facebook.sdk.AdvertiserIDCollectionEnabled";

        #region Firebase

        public static void GenerateXMLForFirebase(string libDir)
        {
            string valuesDir = $"{libDir}/{FirebaseAndroidConfigPath}";
            string xmlFilePath = $"{valuesDir}/{FirebaseAndroidConfigFile}";

            // The file isn't in the package template, so Unity never resets it: remove it, and its folder if empty,
            // so the build doesn't get the one from the previous build
            if (File.Exists(xmlFilePath)) File.Delete(xmlFilePath);
            if (Directory.Exists(valuesDir) && !Directory.EnumerateFileSystemEntries(valuesDir).Any()) Directory.Delete(valuesDir);

            if (!AppodealSettings.Instance.FirebaseAutoConfiguration) return;

            string jsonFilePath = $"{PluginPaths.AssetsDir}/{FirebaseAndroidJsonFile}";

            if (!File.Exists(jsonFilePath))
            {
                Log.Error($"Firebase Android Configuration file was not found on path '{jsonFilePath}'. This service cannot be configured correctly.");
                return;
            }

            var firebaseStrings = ParseFirebaseJson(jsonFilePath);

            if (firebaseStrings == null)
            {
                Log.Error($"Couldn't find a valid Firebase configuration for package name: {Application.identifier} in '{jsonFilePath}' file. This service cannot be configured correctly.");
                return;
            }

            Directory.CreateDirectory(valuesDir);

            CreateOrReplaceFirebaseXml(xmlFilePath, firebaseStrings);
        }

        private static Dictionary<string, string> ParseFirebaseJson(string path)
        {
            string jsonString = new StreamReader(path).ReadToEnd();
            var model = JsonUtility.FromJson<GoogleServicesJsonDto>(jsonString);
            var projectInfo = model?.project_info;

            if (projectInfo?.project_id == null || (model.client?.Count ?? 0) <= 0) return null;

            foreach (var client in model.client)
            {
                var clientInfo = client.client_info;
                var androidClientInfo = clientInfo?.android_client_info;

                if (androidClientInfo?.package_name != Application.identifier) continue;

                if ((client.api_key?.Count ?? 0) <= 0 || client.api_key[0].current_key == null) return null;
                if (clientInfo?.mobilesdk_app_id == null) return null;

                var outputDict = new Dictionary<string, string>
                {
                    {"project_id", projectInfo.project_id},
                    {"google_api_key", client.api_key[0].current_key},
                    {"google_crash_reporting_api_key", client.api_key[0].current_key},
                    {"google_app_id", clientInfo.mobilesdk_app_id},
                };

                if (projectInfo.firebase_url != null) outputDict.Add("firebase_database_url", projectInfo.firebase_url);
                if (projectInfo.project_number != null) outputDict.Add("gcm_defaultSenderId", projectInfo.project_number);
                if (projectInfo.storage_bucket != null) outputDict.Add("google_storage_bucket", projectInfo.storage_bucket);
                if ((client.oauth_client?.Count ?? 0) > 0 && client.oauth_client[0].client_id != null) outputDict.Add("default_web_client_id", client.oauth_client[0].client_id);

                return outputDict;
            }
            return null;
        }

        private static void CreateOrReplaceFirebaseXml(string path, Dictionary<string, string> firebaseStrings)
        {
            var xmlDocument = new XmlDocument();
            var root = xmlDocument.DocumentElement;

            var xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDocument.InsertBefore(xmlDeclaration, root);

            var resourcesElement = xmlDocument.CreateElement("resources");

            var attribute = xmlDocument.CreateAttribute("tools", "keep", "http://schemas.android.com/tools");
            string toolsKeep = String.Join(",", firebaseStrings.Keys.Select(key => $"@string/{key}").ToArray());
            attribute.Value = toolsKeep;
            resourcesElement.Attributes.Append(attribute);

            firebaseStrings.Keys.ToList().ForEach(key =>
                resourcesElement.AppendChild(CreateFirebaseStringElement(key, firebaseStrings[key], xmlDocument)));

            xmlDocument.AppendChild(resourcesElement);
            xmlDocument.Save(path);
        }

        private static XmlElement CreateFirebaseStringElement(string elName, string elValue, XmlDocument xmlDocument)
        {
            var xmlElement = xmlDocument.CreateElement("string");
            xmlElement.SetAttribute("name", elName);
            xmlElement.SetAttribute("translatable", "false");
            var xmlText = xmlDocument.CreateTextNode(elValue);
            xmlElement.AppendChild(xmlText);

            return xmlElement;
        }

        #endregion

        #region Facebook
        public static void SetupManifestForFacebook(string libDir)
        {
            string path = $"{libDir}/{AndroidManifestFile}";

            if (!File.Exists(path))
            {
                Log.Warning($"Missing AndroidManifest.xml file at {path}." +
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
            string clientToken = AppodealSettings.Instance.FacebookAndroidClientToken;

            if (String.IsNullOrEmpty(appId) | String.IsNullOrEmpty(clientToken))
            {
                Log.Warning("Facebook App ID / Client Token is empty (Appodeal > Appodeal Settings). This service won't be initialized properly!");
                return;
            }

            var xmlDocument = new XmlDocument();
            xmlDocument.Load(fullPath);

            var xmlNode = FindChildNode(FindChildNode(xmlDocument, "manifest"), "application");
            if (xmlNode == null)
            {
                Log.Error("Error parsing " + fullPath);
                return;
            }

            string namespaceOfPrefix = xmlNode.GetNamespaceOfPrefix("android");
            var xmlElement = xmlDocument.CreateElement("meta-data");
            xmlElement.SetAttribute("name", namespaceOfPrefix, FacebookApplicationId);
            xmlElement.SetAttribute("value", namespaceOfPrefix, "fb" + appId);
            SetOrReplaceXmlElement(xmlNode, xmlElement);

            var xmlElement2 = xmlDocument.CreateElement("meta-data");
            xmlElement2.SetAttribute("name", namespaceOfPrefix, FacebookClientToken);
            xmlElement2.SetAttribute("value", namespaceOfPrefix, clientToken);
            SetOrReplaceXmlElement(xmlNode, xmlElement2);

            string value = AppodealSettings.Instance.FacebookAutoLogAppEvents.ToString().ToLower();
            var xmlElement3 = xmlDocument.CreateElement("meta-data");
            xmlElement3.SetAttribute("name", namespaceOfPrefix, FacebookAutoLogAppEventsEnabled);
            xmlElement3.SetAttribute("value", namespaceOfPrefix, value);
            SetOrReplaceXmlElement(xmlNode, xmlElement3);

            string value2 = AppodealSettings.Instance.FacebookAdvertiserIDCollection.ToString().ToLower();
            var xmlElement4 = xmlDocument.CreateElement("meta-data");
            xmlElement4.SetAttribute("name", namespaceOfPrefix, FacebookAdvertiserIDCollectionEnabled);
            xmlElement4.SetAttribute("value", namespaceOfPrefix, value2);
            SetOrReplaceXmlElement(xmlNode, xmlElement4);

            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace
            };

            using var w = XmlWriter.Create(fullPath, settings);
            xmlDocument.Save(w);
        }

        private static XmlNode FindChildNode(XmlNode parent, string name)
        {
            for (var xmlNode = parent.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
            {
                if (xmlNode.Name.Equals(name))
                {
                    return xmlNode;
                }
            }
            return null;
        }

        private static void SetOrReplaceXmlElement(XmlNode parent, XmlElement newElement)
        {
            string attribute = newElement.GetAttribute("name");
            string name = newElement.Name;
            if (TryFindElementWithAndroidName(parent, attribute, out XmlElement element, name))
            {
                parent.ReplaceChild(newElement, element);
            }
            else
            {
                parent.AppendChild(newElement);
            }
        }

        private static bool TryFindElementWithAndroidName(XmlNode parent, string attrNameValue, out XmlElement element, string elementType = "activity")
        {
            string namespaceOfPrefix = parent.GetNamespaceOfPrefix("android");
            for (var xmlNode = parent.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
            {
                var xmlElement = xmlNode as XmlElement;
                if (xmlElement != null && xmlElement.Name == elementType && xmlElement.GetAttribute("name", namespaceOfPrefix) == attrNameValue)
                {
                    element = xmlElement;
                    return true;
                }
            }
            element = null;
            return false;
        }

        #endregion

    }
}
