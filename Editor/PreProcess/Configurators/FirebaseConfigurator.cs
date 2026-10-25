using System.Collections.Generic;
using System.IO;
using UnityEngine;
using AppodealInc.Mediation.PluginSettings.Editor;
using AppodealInc.Mediation.Shared.Editor;

namespace AppodealInc.Mediation.PreProcess.Editor
{
    internal static class FirebaseConfigurator
    {
        private static readonly AppodealLogger Log = PreProcessLogging.Log;

        private const string FirebaseAndroidJsonFile = "google-services.json";

        public static void Configure(StringResources googleServices, AppodealSettings settings)
        {
            if (!settings.FirebaseAutoConfiguration) return;

            string jsonFilePath = $"{PluginPaths.AssetsDir}/{FirebaseAndroidJsonFile}";

            if (!File.Exists(jsonFilePath))
            {
                Log.Warning($"Firebase auto configuration is enabled in 'Appodeal > Appodeal Settings', but the config file '{jsonFilePath}' is not found. The SDK will likely fail to initialize.");
                return;
            }

            var firebaseStrings = ParseFirebaseJson(jsonFilePath);

            if (firebaseStrings == null)
            {
                Log.Warning($"Firebase config file '{jsonFilePath}' has no valid entry for package '{Application.identifier}'. The SDK will likely fail to initialize.");
                return;
            }

            foreach (var pair in firebaseStrings)
            {
                googleServices.AddString(pair.Key, pair.Value);
            }
        }

        private static Dictionary<string, string> ParseFirebaseJson(string path)
        {
            string jsonString = File.ReadAllText(path);
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
    }
}
