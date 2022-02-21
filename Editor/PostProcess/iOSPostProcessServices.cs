#if UNITY_IPHONE
using UnityEngine;
using UnityEditor;
using UnityEditor.iOS.Xcode;
using System.IO;
using AppodealStack.UnityEditor.InternalResources;

namespace AppodealStack.UnityEditor.PostProcess
{
    public class iOSPostProcessServices
    {
        private const string CFBundleURLTypes = "CFBundleURLTypes";
        private const string CFBundleURLSchemes = "CFBundleURLSchemes";
        private const string CFBundleURLName = "CFBundleURLName";
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

            if (string.IsNullOrEmpty(fbKey))
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

            var typesArray = plist.root[CFBundleURLTypes]?.AsArray() ?? plist.root.CreateArray(CFBundleURLTypes);

            var schemesDict = typesArray.values.Find(el => el.AsDict()[CFBundleURLSchemes] != null)?.AsDict() ?? typesArray.AddDict();

            if (schemesDict[CFBundleURLName]?.AsString() == null) schemesDict.SetString(CFBundleURLName, FacebookUrlName);
            
            var schemesArray = schemesDict[CFBundleURLSchemes]?.AsArray() ?? schemesDict.CreateArray(CFBundleURLSchemes);

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
