// ReSharper disable CheckNamespace

using System;
using System.IO;
using UnityEditor.iOS.Xcode;
using UnityEngine;
using AppodealInc.Mediation.PluginSettings.Editor;

namespace AppodealInc.Mediation.PostProcess.Editor
{
    public static class IosPostProcessServices
    {
        private const string CfBundleURLTypes = "CFBundleURLTypes";
        private const string CfBundleURLSchemes = "CFBundleURLSchemes";
        private const string CfBundleURLName = "CFBundleURLName";
        private const string FacebookUrlName = "facebook-unity-sdk";
        private const string FacebookAppID = "FacebookAppID";
        private const string FacebookClientToken = "FacebookClientToken";
        private const string FacebookAutoLogAppEventsEnabled = "FacebookAutoLogAppEventsEnabled";
        private const string FacebookAdvertiserIDCollectionEnabled = "FacebookAdvertiserIDCollectionEnabled";

        private const string FirPlistFileName = "GoogleService-Info.plist";
        private const string BundleIdPlistKey = "BUNDLE_ID";

        public static void AddFacebookKeys(string plistPath)
        {
            if (!AppodealSettings.Instance.FacebookAutoConfiguration) return;

            string fbAppId = AppodealSettings.Instance.FacebookIosAppId;
            string fbClientToken = AppodealSettings.Instance.FacebookIosClientToken;
            bool areFbEventsEnabled = AppodealSettings.Instance.FacebookAutoLogAppEvents;
            bool isFbIdsCollectionEnabled = AppodealSettings.Instance.FacebookAdvertiserIDCollection;

            if (String.IsNullOrEmpty(fbAppId))
            {
                Debug.LogWarning("Meta App ID (iOS) is empty (Appodeal > Appodeal Settings). This service won't be initialized properly.");
                return;
            }

            if (String.IsNullOrEmpty(fbClientToken))
            {
                Debug.LogWarning("Meta Client Token (iOS) is empty (Appodeal > Appodeal Settings). This service won't be initialized properly.");
                return;
            }

            var plist = new PlistDocument();
            plist.ReadFromFile(plistPath);

            if (plist.root[FacebookAppID] == null) plist.root.SetString(FacebookAppID, fbAppId);
            if (plist.root[FacebookClientToken] == null) plist.root.SetString(FacebookClientToken, fbClientToken);
            if (plist.root[FacebookAutoLogAppEventsEnabled] == null) plist.root.SetBoolean(FacebookAutoLogAppEventsEnabled, areFbEventsEnabled);
            if (plist.root[FacebookAdvertiserIDCollectionEnabled] == null) plist.root.SetBoolean(FacebookAdvertiserIDCollectionEnabled, isFbIdsCollectionEnabled);

            string fbScheme = $"fb{fbAppId}";
            var typesArray = plist.root[CfBundleURLTypes]?.AsArray() ?? plist.root.CreateArray(CfBundleURLTypes);
            var fbType = typesArray.values.Find(el => el.AsDict()[CfBundleURLName]?.AsString() == FacebookUrlName)?.AsDict();
            if (fbType == null)
            {
                fbType = typesArray.AddDict();
                fbType.SetString(CfBundleURLName, FacebookUrlName);
            }
            var schemesArray = fbType[CfBundleURLSchemes]?.AsArray() ?? fbType.CreateArray(CfBundleURLSchemes);
            if (schemesArray.values.Find(el => el.AsString() == fbScheme) == null) schemesArray.AddString(fbScheme);

            File.WriteAllText(plistPath, plist.WriteToString());
        }

        public static bool AddFirebasePlistFile(string buildPath)
        {
            if (!AppodealSettings.Instance.FirebaseAutoConfiguration) return false;

            string plistFileUnityPath = Application.dataPath + '/' + FirPlistFileName;
            string plistFileXcodePath = buildPath + '/' + FirPlistFileName;

            if (File.Exists(plistFileUnityPath))
            {
                var plist = new PlistDocument();
                plist.ReadFromFile(plistFileUnityPath);
                plist.root.values.TryGetValue(BundleIdPlistKey, out var bundle);
                if (bundle?.AsString() == Application.identifier)
                {
                    File.Copy(plistFileUnityPath, plistFileXcodePath, overwrite: true);
                    return true;
                }
                Debug.LogWarning($"No valid Firebase Plist file was found for {Application.identifier} at {plistFileUnityPath}. This service won't be initialized properly.");
                return false;
            }
            Debug.LogWarning($"Firebase Plist file was not found at {plistFileUnityPath}. This service won't be initialized properly.");
            return false;
        }
    }
}
