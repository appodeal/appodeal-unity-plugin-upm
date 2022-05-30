#if UNITY_IOS
using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.iOS.Xcode;
using AppodealStack.UnityEditor.InternalResources;

// ReSharper Disable CheckNamespace
namespace AppodealStack.UnityEditor.PostProcess
{
    public static class IosPostProcessServices
    {
        private const string CfBundleURLTypes = "CFBundleURLTypes";
        private const string CfBundleURLSchemes = "CFBundleURLSchemes";
        private const string CfBundleURLName = "CFBundleURLName";
        private const string FacebookUrlName = "facebook-unity-sdk";
        private const string FacebookAppID = "FacebookAppID";
        private const string FacebookAutoLogAppEventsEnabled = "FacebookAutoLogAppEventsEnabled";
        private const string FacebookAdvertiserIDCollectionEnabled = "FacebookAdvertiserIDCollectionEnabled";

        public static void AddFacebookKeys(string plistPath)
        {
            if (!AppodealSettings.Instance.FacebookAutoConfiguration) return;

            string fbKey = AppodealSettings.Instance.FacebookIosAppId;
            bool areFbEventsEnabled = AppodealSettings.Instance.FacebookAutoLogAppEvents;
            bool isFbIdsCollectionEnabled = AppodealSettings.Instance.FacebookAdvertiserIDCollection;

            if (String.IsNullOrEmpty(fbKey))
            {
                Debug.LogWarning("Facebook App ID is empty (Appodeal > Appodeal Settings). This service won't be initialized properly.");
                return;
            }

            var plist = new PlistDocument();
            plist.ReadFromFile(plistPath);

        #region FacebookAppID

            if (plist.root[FacebookAppID] == null) plist.root.SetString(FacebookAppID, fbKey);
        
        #endregion

        #region FacebookAutoLogAppEventsEnabled

            if (plist.root[FacebookAutoLogAppEventsEnabled] == null) plist.root.SetBoolean(FacebookAutoLogAppEventsEnabled, areFbEventsEnabled);

        #endregion

        #region FacebookAdvertiserIDCollectionEnabled

            if (plist.root[FacebookAdvertiserIDCollectionEnabled] == null) plist.root.SetBoolean(FacebookAdvertiserIDCollectionEnabled, isFbIdsCollectionEnabled);

        #endregion

        #region CFBundleURLTypes

            var typesArray = plist.root[CfBundleURLTypes]?.AsArray() ?? plist.root.CreateArray(CfBundleURLTypes);

            var schemesDict = typesArray.values.Find(el => el.AsDict()[CfBundleURLSchemes] != null)?.AsDict() ?? typesArray.AddDict();

            if (schemesDict[CfBundleURLName]?.AsString() == null) schemesDict.SetString(CfBundleURLName, FacebookUrlName);
            
            var schemesArray = schemesDict[CfBundleURLSchemes]?.AsArray() ?? schemesDict.CreateArray(CfBundleURLSchemes);

            if (schemesArray.values.Find(el => el.AsString() == fbKey) == null) schemesArray.AddString($"fb{fbKey}");
        
        #endregion

            File.WriteAllText(plistPath, plist.WriteToString());
        }

        public static bool AddFirebasePlistFile(string buildPath)
        {
            if (!AppodealSettings.Instance.FirebaseAutoConfiguration) return false;

            string FirebasePlistFile = "GoogleService-Info.plist";

            if (File.Exists(Path.Combine(Application.dataPath, FirebasePlistFile)))
            {
                FileUtil.CopyFileOrDirectory(Path.Combine(Application.dataPath, FirebasePlistFile), Path.Combine(buildPath, FirebasePlistFile));
                return true;
            }
            else
            {
                Debug.LogWarning($"Firebase Plist file was not found at {Path.Combine(Application.dataPath, FirebasePlistFile)}. This service won't be initialized.");
                return false;
            }
        }
    }
}

#endif
