// ReSharper disable CheckNamespace

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using AppodealInc.Mediation.Analytics.Editor;
using AppodealInc.Mediation.PluginSettings.Editor;
using AppodealInc.Mediation.Utils.Editor;

namespace AppodealInc.Mediation.SettingsWindow.Editor
{
    public class AppodealSettingsWindow : EditorWindow
    {
        public static void ShowAppodealSettingsWindow()
        {
            AnalyticsService.TrackClickEvent(ActionType.OpenAppodealSettings);

            if (AppodealSettings.Instance == null) return;

            GetWindowWithRect(typeof(AppodealSettingsWindow), new Rect(0, 0, 650, 760), true, "Appodeal Settings");
        }

        private async void OnEnable()
        {
            var ids = await GetSkAdNetworkIds();

            if (AppodealSettings.Instance == null) return;

            if (ids?.Count > 0 && !ids.SequenceEqual(AppodealSettings.Instance.IosSkAdNetworkItemsList))
            {
                AppodealSettings.Instance.IosSkAdNetworkItemsList = ids;
            }
        }

        private static async Task<List<string>> GetSkAdNetworkIds()
        {
            try
            {
                var request = UnityWebRequest.Get("https://mw-backend-new.appodeal.com/v3/skadn/ids");
                _ = request.SendWebRequest();
                while (!request.isDone) await Task.Yield();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    request.Dispose();
                    return new List<string>();
                }

                string json = request.downloadHandler.text;
                request.Dispose();

                if (String.IsNullOrWhiteSpace(json) || !json.StartsWith('[') || !json.EndsWith(']')) return new List<string>();

                string[] ids = JsonHelper.FromJson<string>(JsonHelper.FixJson(json));
                if ((ids?.Length ?? 0) < 1) return new List<string>();

                var regex = new Regex("^([a-z]|[0-9]){10}.skadnetwork$");
                return ids!.Where(id => regex.IsMatch(id)).Distinct().OrderBy(id => id).ToList();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return new List<string>();
            }
        }

        private void OnDestroy()
        {
            AnalyticsService.TrackClickEvent(ActionType.CloseAppodealSettings);
            AppodealSettings.SaveAsync();
        }

        private void OnGUI()
        {
            if (AppodealSettings.Instance == null) return;

            LabelHeaderField("Mediation Settings");

            #region Admob App Id Setting

            GUILayout.BeginHorizontal();

            using (new EditorGUILayout.VerticalScope("box"))
            {
                if (GUILayout.Button("AdMob App Ids", new GUIStyle(EditorStyles.label)
                {
                    fontSize = 15,
                    fontStyle = FontStyle.Bold,
                    fixedHeight = 25
                }, GUILayout.Width(200)))
                {
                    Application.OpenURL("https://docs.appodeal.com/unity/get-started#admob-configuration");
                }

                GUILayout.Space(2);

                AppodealSettings.Instance.AdMobAndroidAppId = AppIdPlatformRow("AdMob App ID (Android)",
                    AppodealSettings.Instance.AdMobAndroidAppId, GUILayout.Width(200));
                GUILayout.Space(5);
                AppodealSettings.Instance.AdMobIosAppId = AppIdPlatformRow("AdMob App ID (iOS)",
                    AppodealSettings.Instance.AdMobIosAppId, GUILayout.Width(200));
                GUILayout.Space(10);
            }

            GUILayout.EndHorizontal();

            #endregion

            GUILayout.Space(5);

            #region Android Settings

            GUILayout.BeginHorizontal();

            using (new EditorGUILayout.VerticalScope("box", GUILayout.Width(200), GUILayout.Height(200)))
            {
                LabelField("Android Settings");
                HeaderField("Add optional permissions",
                    "https://docs.appodeal.com/unity/get-started?settings=auto#configure-androidmanifestxml");

                AppodealSettings.Instance.AccessCoarseLocationPermission = KeyRow("ACCESS_COARSE_LOCATION",
                    AppodealSettings.Instance.AccessCoarseLocationPermission);
                AppodealSettings.Instance.AccessFineLocationPermission = KeyRow("ACCESS_FINE_LOCATION",
                    AppodealSettings.Instance.AccessFineLocationPermission);
                AppodealSettings.Instance.WriteExternalStoragePermission = KeyRow("WRITE_EXTERNAL_STORAGE",
                    AppodealSettings.Instance.WriteExternalStoragePermission);
                AppodealSettings.Instance.AccessWifiStatePermission = KeyRow("ACCESS_WIFI_STATE",
                    AppodealSettings.Instance.AccessWifiStatePermission);
                AppodealSettings.Instance.VibratePermission = KeyRow("VIBRATE",
                    AppodealSettings.Instance.VibratePermission);

                GUILayout.Space(67);
            }

            #endregion

            GUILayout.Space(3);

            #region iOS Settings

            using (new EditorGUILayout.VerticalScope("box", GUILayout.Width(200), GUILayout.Height(200)))
            {
                LabelField("iOS Settings");
                HeaderField("Add keys to info.plist",
                    "https://docs.appodeal.com/unity/get-started?settings=auto#other-feature-usage-descriptions");

                AppodealSettings.Instance.NsUserTrackingUsageDescription = KeyRow("NSUserTrackingUsageDescription",
                    AppodealSettings.Instance.NsUserTrackingUsageDescription);
                AppodealSettings.Instance.NsLocationWhenInUseUsageDescription = KeyRow("NSLocationWhenInUseUsageDescription",
                    AppodealSettings.Instance.NsLocationWhenInUseUsageDescription);
                AppodealSettings.Instance.NsCalendarsUsageDescription = KeyRow("NSCalendarsUsageDescription",
                    AppodealSettings.Instance.NsCalendarsUsageDescription);
                AppodealSettings.Instance.NsAppTransportSecurity = KeyRow("NSAppTransportSecurity",
                    AppodealSettings.Instance.NsAppTransportSecurity);

                GUILayout.Space(35);
                if (GUILayout.Button("SKAdNetwork", new GUIStyle(EditorStyles.label)
                {
                    fontSize = 12,
                    fontStyle = FontStyle.Bold,
                    fixedHeight = 18
                }, GUILayout.ExpandWidth(true)))
                {
                    Application.OpenURL("https://developer.apple.com/documentation/storekit/skadnetwork");
                }

                AppodealSettings.Instance.IosSkAdNetworkItems = KeyRow("Add SKAdNetworkItems",
                    AppodealSettings.Instance.IosSkAdNetworkItems);

                GUILayout.Space(12);
            }

            GUILayout.EndHorizontal();

            #endregion

            GUILayout.Space(10);
            LabelHeaderField("Attribution Settings");

            #region Attribution Services

            GUILayout.BeginHorizontal();

            using (new EditorGUILayout.VerticalScope("box"))
            {
                LabelField("Meta Settings");

                AppodealSettings.Instance.FacebookAutoConfiguration = KeyRow("Enable auto configuration",
                    AppodealSettings.Instance.FacebookAutoConfiguration);

                GUILayout.Space(10);

                AppodealSettings.Instance.FacebookAndroidAppId = AppIdPlatformRow("Meta App ID (Android)",
                    AppodealSettings.Instance.FacebookAndroidAppId, GUILayout.Width(200));

                GUILayout.Space(5);

                AppodealSettings.Instance.FacebookIosAppId = AppIdPlatformRow("Meta App ID (iOS)",
                    AppodealSettings.Instance.FacebookIosAppId, GUILayout.Width(200));

                GUILayout.Space(15);

                AppodealSettings.Instance.FacebookAndroidClientToken = AppIdPlatformRow("Meta Client Token (Android)",
                    AppodealSettings.Instance.FacebookAndroidClientToken, GUILayout.Width(200));

                GUILayout.Space(5);

                AppodealSettings.Instance.FacebookIosClientToken = AppIdPlatformRow("Meta Client Token (iOS)",
                    AppodealSettings.Instance.FacebookIosClientToken, GUILayout.Width(200));

                GUILayout.Space(10);

                HeaderField("Optional Settings", "https://docs.appodeal.com/unity/services/meta");

                AppodealSettings.Instance.FacebookAutoLogAppEvents = KeyRow("Enable FacebookAutoLogAppEvents",
                    AppodealSettings.Instance.FacebookAutoLogAppEvents);
                AppodealSettings.Instance.FacebookAdvertiserIDCollection = KeyRow("Enable FacebookAdvertiserIDCollection",
                    AppodealSettings.Instance.FacebookAdvertiserIDCollection);

                GUILayout.Space(5);
            }

            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            GUILayout.BeginHorizontal();

            using (new EditorGUILayout.VerticalScope("box"))
            {
                LabelField("Firebase Settings");

                AppodealSettings.Instance.FirebaseAutoConfiguration = KeyRow("Enable auto configuration",
                    AppodealSettings.Instance.FirebaseAutoConfiguration);

                GUILayout.Space(5);
            }

            GUILayout.EndHorizontal();

            #endregion

        }

        private static void LabelField(string label)
        {
            EditorGUILayout.LabelField(label, new GUIStyle(EditorStyles.label)
                {
                    fontSize = 15,
                    fontStyle = FontStyle.Bold
                },
                GUILayout.Height(20), GUILayout.Width(311));
            GUILayout.Space(2);
        }

        private static void HeaderField(string header, string url)
        {
            if (GUILayout.Button(header, new GUIStyle(EditorStyles.label)
            {
                fontSize = 12,
                fontStyle = FontStyle.Bold,
                fixedHeight = 18
            }, GUILayout.Width(200)))
            {
                Application.OpenURL(url);
            }

            GUILayout.Space(2);
        }

        private static string AppIdPlatformRow(string fieldTitle, string text, GUILayoutOption labelWidth, GUILayoutOption textFieldWidthOption = null)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(new GUIContent(fieldTitle), labelWidth);
            text = textFieldWidthOption == null
                ? GUILayout.TextField(text)
                : GUILayout.TextField(text, textFieldWidthOption);
            GUILayout.EndHorizontal();
            return text;
        }

        private static bool KeyRow(string fieldTitle, bool value)
        {
            GUILayout.Space(5);
            float originalValue = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 235;
            value = EditorGUILayout.Toggle(fieldTitle, value);
            EditorGUIUtility.labelWidth = originalValue;
            return value;
        }

        private static void HorizontalLine()
        {
            var separatorLineStyle = new GUIStyle
            {
                normal = new GUIStyleState { background = EditorGUIUtility.whiteTexture },
                margin = new RectOffset(0, 0, 10, 5),
                fixedHeight = 2
            };

            var c = GUI.color;
            GUI.color = Color.grey;
            GUILayout.Box( GUIContent.none, separatorLineStyle );
            GUI.color = c;
        }

        private static void LabelHeaderField(string header)
        {
            EditorGUILayout.LabelField(header, new GUIStyle(EditorStyles.label)
            {
                fontSize = 16,
                fontStyle = FontStyle.Bold,
                normal = new GUIStyleState { textColor = new Color(0.7f,0.6f,0.1f) },
                alignment = TextAnchor.MiddleCenter

            }, GUILayout.Height(20));

            HorizontalLine();
        }
    }
}
