using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using File = UnityEngine.Windows.File;
using UnityEngine.Networking;
using marijnz.EditorCoroutines;
using AppodealAds.Unity.Common;
using AppodealAds.Unity.Editor.Utils;
using AppodealAds.Unity.Editor.PluginRemover;
using UnityEditor.PackageManager;
using AppodealAds.Unity.Editor.SDKManager.Models;

#pragma warning disable 618

#pragma warning disable 612

namespace AppodealAds.Unity.Editor.SDKManager
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum PlatformSdk
    {
        Android,
        iOS
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "NotAccessedVariable")]
    [SuppressMessage("ReSharper", "CollectionNeverQueried.Local")]
    public class AppodealAdapterManager : EditorWindow
    {
        #region Dictionaries

        private SortedDictionary<string, NetworkDependency> internalDependencies =
            new SortedDictionary<string, NetworkDependency>();

        private SortedDictionary<string, NetworkDependency> latestDependencies =
            new SortedDictionary<string, NetworkDependency>();

        #endregion

        #region GUIStyles

        private GUIStyle labelStyle;
        private GUIStyle headerInfoStyle;
        private GUIStyle subHeaderInfoStyle;
        private GUIStyle packageInfoStyle;
        private GUIStyle separatorLineStyle;
        private readonly GUILayoutOption btnFieldWidth = GUILayout.Width(60);

        #endregion

        private static EditorCoroutines.EditorCoroutine coroutine;
        private static EditorCoroutines.EditorCoroutine coroutinePB;
        private float progress;
        private float loading;
        private Vector2 scrollPosition;
        private bool isPluginInfoReady;
        private AppodealUnityPlugin appodealUnityPlugin;
        
        public static void ShowSdkManager()
        {
            GetWindow(typeof(AppodealAdapterManager),
                true, AppodealEditorConstants.AppodealSdkManager);
        }

        private void HorizontalLine ()
        {
            var c = GUI.color;
            GUI.color = Color.grey;
            GUILayout.Box( GUIContent.none, separatorLineStyle );
            GUI.color = c;
        }

        private void Awake()
        {
            labelStyle = new GUIStyle(EditorStyles.label)
            {
                fontSize = 18,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter
            };

            packageInfoStyle = new GUIStyle(EditorStyles.label)
            {
                fontSize = 12,
                fontStyle = FontStyle.Normal,
                fixedHeight = 18
            };

            headerInfoStyle = new GUIStyle(EditorStyles.label)
            {
                fontSize = 13,
                fontStyle = FontStyle.Bold,
                fixedHeight = 18
            };

            subHeaderInfoStyle = new GUIStyle(EditorStyles.label)
            {
                fontSize = 16,
                fontStyle = FontStyle.Bold,
                normal = new GUIStyleState() { textColor = new Color(0.7f,0.6f,0.1f) },
                alignment = TextAnchor.MiddleCenter

            };

            separatorLineStyle = new GUIStyle()
            {
                normal = new GUIStyleState() { background = EditorGUIUtility.whiteTexture },
                margin = new RectOffset(0, 0, 10, 5),
                fixedHeight = 2
            };

            Reset();
        }

        public void Reset()
        {
            internalDependencies =
                new SortedDictionary<string, NetworkDependency>();
            latestDependencies =
                new SortedDictionary<string, NetworkDependency>();

            if (coroutine != null)
                this.StopCoroutine(coroutine.routine);
            if (progress > 0)
                EditorUtility.ClearProgressBar();
            if (loading > 0)
                EditorUtility.ClearProgressBar();

            coroutine = null;

            loading = 0f;
            progress = 0f;
        }

        private void OnEnable()
        {
            loading = 0f;
            coroutine = this.StartCoroutine(GetAppodealSDKData());
        }

        private void OnGUI()
        {
            this.minSize = new Vector2(700, 900);
            this.maxSize = new Vector2(700, 2000);

            if (isPluginInfoReady && internalDependencies.Count > 0 && latestDependencies.Count > 0 && appodealUnityPlugin != null)
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, false, false);
            else EditorGUILayout.BeginScrollView(scrollPosition, false, false);

            GUILayout.BeginVertical();

            if (isPluginInfoReady)
            {
                #region Plugin

                GUILayout.Space(10);
                EditorGUILayout.LabelField(AppodealEditorConstants.AppodealUnityPlugin, labelStyle,
                    GUILayout.Height(20));

                if (appodealUnityPlugin != null)
                {
                    GUILayout.Space(5);
                    using (new EditorGUILayout.VerticalScope(AppodealEditorConstants.BoxStyle, GUILayout.Height(45)))
                    {
                        AppodealDependencyUtils.GuiHeaders(headerInfoStyle, btnFieldWidth);
                        if (!string.IsNullOrEmpty(AppodealVersions.APPODEAL_PLUGIN_VERSION) &&
                            !string.IsNullOrEmpty(appodealUnityPlugin.version) &&
                            !string.IsNullOrEmpty(appodealUnityPlugin.source))
                        {
                            GuiPluginRow(appodealUnityPlugin);
                        }
                        else
                        {
                            AppodealDependencyUtils.ShowInternalErrorDialog(this, "Can't find plugin information.",
                                "Can't find plugin information. - {180}");
                        }
                    }
                    HorizontalLine();
                }
                else
                {
                    AppodealDependencyUtils.ShowInternalErrorDialog(this, "Can't find plugin information.",
                        "appodealUnityPlugin != null - {175}");
                }

                #endregion

                #region CoreInfo

                if (internalDependencies.Count > 0 && latestDependencies.Count > 0)
                {
                    GUILayout.Space(10);
                    EditorGUILayout.LabelField(AppodealEditorConstants.AppodealCoreDependencies, labelStyle,
                        GUILayout.Height(20));
                    GUILayout.Space(5);
                    EditorGUILayout.LabelField(AppodealEditorConstants.IOS, subHeaderInfoStyle, GUILayout.Height(20));
                    GUILayout.Space(5);
                    using (new EditorGUILayout.VerticalScope(AppodealEditorConstants.BoxStyle, GUILayout.Height(45)))
                    {
                        AppodealDependencyUtils.GuiHeaders(headerInfoStyle, btnFieldWidth);
                        GuiCoreRow(AppodealDependencyUtils.GetAppodealDependency(internalDependencies),
                            AppodealDependencyUtils.GetAppodealDependency(latestDependencies), PlatformSdk.iOS);
                    }
                    GUILayout.Space(5);
                    EditorGUILayout.LabelField(AppodealEditorConstants.Android, subHeaderInfoStyle, GUILayout.Height(20));
                    GUILayout.Space(5);
                    using (new EditorGUILayout.VerticalScope(AppodealEditorConstants.BoxStyle, GUILayout.Height(45)))
                    {
                        AppodealDependencyUtils.GuiHeaders(headerInfoStyle, btnFieldWidth);
                        GuiCoreRow(AppodealDependencyUtils.GetAppodealDependency(internalDependencies),
                            AppodealDependencyUtils.GetAppodealDependency(latestDependencies), PlatformSdk.Android);
                    }
                    HorizontalLine();
                }

                #endregion

                #region NetworksAdaptersInfo

                if (internalDependencies.Count > 0)
                {
                    GUILayout.Space(10);
                    EditorGUILayout.LabelField(AppodealEditorConstants.AppodealNetworkDependencies, labelStyle,
                        GUILayout.Height(20));
                    GUILayout.Space(5);
                    EditorGUILayout.LabelField(AppodealEditorConstants.IOS, subHeaderInfoStyle, GUILayout.Height(20));
                    GUILayout.Space(5);
                    using (new EditorGUILayout.VerticalScope(AppodealEditorConstants.BoxStyle, GUILayout.Height(45)))
                    {
                        AppodealDependencyUtils.GuiHeaders(headerInfoStyle, btnFieldWidth);
                        GuiAdaptersRows(PlatformSdk.iOS);
                    }
                    GUILayout.Space(5);
                    EditorGUILayout.LabelField(AppodealEditorConstants.Android, subHeaderInfoStyle, GUILayout.Height(20));
                    GUILayout.Space(5);
                    using (new EditorGUILayout.VerticalScope(AppodealEditorConstants.BoxStyle, GUILayout.Height(45)))
                    {
                        AppodealDependencyUtils.GuiHeaders(headerInfoStyle, btnFieldWidth);
                        GuiAdaptersRows(PlatformSdk.Android);
                    }
                    HorizontalLine();
                }

                #endregion
            }

            GUILayout.Space(5);
            GUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }

        private void GuiAdaptersRows(PlatformSdk platformSdk)
        {
            foreach (var key in latestDependencies.Keys)
            {
                if (internalDependencies.ContainsKey(key))
                {
                    if (key.Equals(AppodealEditorConstants.Appodeal)) continue;

                    if (latestDependencies.TryGetValue(key, out var latestDependency) &&
                        internalDependencies.TryGetValue(key, out var internalDependency))
                    {
                        switch (platformSdk)
                        {
                            case PlatformSdk.Android:
                                if (internalDependency.android_info != null)
                                {
                                    if (!string.IsNullOrEmpty(internalDependency.android_info.name)
                                        && !string.IsNullOrEmpty(internalDependency.android_info.version)
                                        && !string.IsNullOrEmpty(internalDependency.android_info.unity_content))
                                    {
                                        GUILayout.Space(5);
                                        SetAdapterUpdateInfo(latestDependency.name,
                                            internalDependency.android_info.version,
                                            latestDependency.android_info.version,
                                            internalDependency.android_info.unity_content,
                                            latestDependency.android_info.unity_content,
                                            SDKInfo(latestDependency.android_info.dependencies));
                                    }
                                }
                                else
                                {
                                    if (latestDependency.android_info.name != null)
                                    {
                                        GUILayout.Space(5);
                                        SetAdapterInformationForImport(latestDependency, platformSdk);
                                    }
                                }

                                break;
                            case PlatformSdk.iOS:
                                if (internalDependency.ios_info != null && latestDependency.ios_info != null)
                                {
                                    if (!string.IsNullOrEmpty(internalDependency.ios_info.name)
                                        && !string.IsNullOrEmpty(internalDependency.ios_info.version)
                                        && !string.IsNullOrEmpty(internalDependency.ios_info.unity_content))
                                    {
                                        GUILayout.Space(5);
                                        SetAdapterUpdateInfo(latestDependency.name,
                                            internalDependency.ios_info.version,
                                            latestDependency.ios_info.version,
                                            internalDependency.ios_info.unity_content,
                                            latestDependency.ios_info.unity_content,
                                            SDKInfo(latestDependency.ios_info.dependencies));
                                    }
                                }
                                else
                                {
                                    if (latestDependency.ios_info?.name != null)
                                    {
                                        GUILayout.Space(5);
                                        SetAdapterInformationForImport(latestDependency, platformSdk);
                                    }
                                }

                                break;
                        }
                    }
                }
                else
                {
                    if (latestDependencies.TryGetValue(key, out var networkDependency))
                    {
                        switch (platformSdk)
                        {
                            case PlatformSdk.Android:
                                if (networkDependency.android_info?.name != null)
                                {
                                    GUILayout.Space(5);
                                    SetAdapterInformationForImport(networkDependency, PlatformSdk.Android);
                                }

                                break;
                            case PlatformSdk.iOS:
                                if (networkDependency.ios_info?.name != null)
                                {
                                    GUILayout.Space(5);
                                    SetAdapterInformationForImport(networkDependency, PlatformSdk.iOS);
                                }

                                break;
                            default:
                                throw new ArgumentOutOfRangeException(nameof(platformSdk), platformSdk, null);
                        }
                    }
                }
            }
        }

        private void SetAdapterInformationForImport(NetworkDependency latestDependency, PlatformSdk platformSdk)
        {
            switch (platformSdk)
            {
                case PlatformSdk.Android:
                    if (latestDependency.android_info != null)
                    {
                        GUILayout.Space(5);
                        SetAdapterImportInfo(latestDependency.name, AppodealEditorConstants.EmptyCurrentVersion,
                            latestDependency.android_info.version, latestDependency.android_info.unity_content);
                    }

                    break;
                case PlatformSdk.iOS:
                    if (latestDependency.ios_info != null)
                    {
                        GUILayout.Space(5);
                        SetAdapterImportInfo(latestDependency.name, AppodealEditorConstants.EmptyCurrentVersion,
                            latestDependency.ios_info.version, latestDependency.ios_info.unity_content);
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(platformSdk), platformSdk, null);
            }
        }

        private void ImportConfig(string nameDep, string content)
        {
            var path = Path.Combine(AppodealEditorConstants.PluginPath,
                AppodealEditorConstants.NetworkDepsPath,
                $"{nameDep}{AppodealEditorConstants.Dependencies}{AppodealEditorConstants.XmlFileExtension}");

            if (File.Exists(path))
            {
                UpdateDependency(nameDep, AppodealEditorConstants.SpecCloseDependencies,
                    content + "\n" + AppodealEditorConstants.SpecCloseDependencies);
                AppodealDependencyUtils.FormatXml(System.IO.File.ReadAllText(path));
            }
            else
            {
                using (TextWriter writer = new StreamWriter(path, false))
                {
                    writer.WriteLine(AppodealEditorConstants.SpecOpenDependencies
                                     + content + "\n" + AppodealEditorConstants.SpecCloseDependencies);
                    writer.Close();
                }

                AppodealDependencyUtils.FormatXml(System.IO.File.ReadAllText(path));
            }

            UpdateWindow();
        }

        private void GuiCoreRow(NetworkDependency internalDependency, NetworkDependency latestDependency,
            PlatformSdk platform)
        {
            if (internalDependency == null || latestDependency == null) return;
            using (new EditorGUILayout.HorizontalScope(GUILayout.Height(20)))
            {
                if (!string.IsNullOrEmpty(internalDependency.name))
                {
                    GUILayout.Space(2);
                    GUILayout.Button(internalDependency.name, packageInfoStyle, GUILayout.Width(150));
                    if (platform == PlatformSdk.iOS)
                    {
                        if (internalDependency.ios_info != null
                            && !string.IsNullOrEmpty(internalDependency.ios_info.version))
                        {
                            GUILayout.Space(56);
                            GUILayout.Button(
                                AppodealDependencyUtils.ReplaceBetaVersion(internalDependency.ios_info.version),
                                packageInfoStyle, GUILayout.Width(110));
                            if (latestDependency.ios_info != null &&
                                !string.IsNullOrEmpty(latestDependency.ios_info.version))
                            {
                                GUILayout.Space(85);
                                GUILayout.Button(
                                    AppodealDependencyUtils.ReplaceBetaVersion(latestDependency.ios_info.version),
                                    packageInfoStyle);
                                GUILayout.Space(15);

                                if (AppodealDependencyUtils.CompareVersion(internalDependency.ios_info.version,
                                    latestDependency.ios_info.version) == 0)
                                {
                                    GUI.enabled = false;
                                    GUILayout.Button(
                                        new GUIContent { text = AppodealEditorConstants.ActionUpdate },
                                        btnFieldWidth);
                                    GUI.enabled = true;
                                }
                                else if (AppodealDependencyUtils.CompareVersion(internalDependency.ios_info.version,
                                    latestDependency.ios_info.version) > 0)
                                {
                                    GUI.enabled = false;
                                    GUILayout.Button(
                                        new GUIContent { text = AppodealEditorConstants.ActionUpdate },
                                        btnFieldWidth);
                                    GUI.enabled = true;
                                }
                                else
                                {
                                    Color defaultColor = GUI.backgroundColor;
                                    GUI.backgroundColor = Color.red;
                                    UpdateCoreProccess(internalDependency.name,
                                        internalDependency.ios_info.unity_content,
                                        latestDependency.ios_info.unity_content, PlatformSdk.iOS);
                                    GUI.backgroundColor = defaultColor;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (internalDependency.android_info != null
                            && !string.IsNullOrEmpty(internalDependency.android_info.version))
                        {
                            GUILayout.Space(56);
                            GUILayout.Button(
                                AppodealDependencyUtils.ReplaceBetaVersion(internalDependency.android_info.version),
                                packageInfoStyle, GUILayout.Width(110));
                            if (latestDependency.android_info != null &&
                                !string.IsNullOrEmpty(latestDependency.android_info.version))
                            {
                                GUILayout.Space(85);
                                GUILayout.Button(
                                    AppodealDependencyUtils.ReplaceBetaVersion(
                                        latestDependency.android_info.version),
                                    packageInfoStyle);
                                GUILayout.Space(15);

                                if (AppodealDependencyUtils.CompareVersion(
                                    internalDependency.android_info.version,
                                    latestDependency.android_info.version) == 0)
                                {
                                    GUI.enabled = false;
                                    GUILayout.Button(
                                        new GUIContent { text = AppodealEditorConstants.ActionUpdate },
                                        btnFieldWidth);
                                    GUI.enabled = true;
                                }
                                else if (AppodealDependencyUtils.CompareVersion(
                                    internalDependency.android_info.version,
                                    latestDependency.android_info.version) > 0)
                                {
                                    GUI.enabled = false;
                                    GUILayout.Button(
                                        new GUIContent { text = AppodealEditorConstants.ActionUpdate },
                                        btnFieldWidth);
                                    GUI.enabled = true;
                                }
                                else
                                {
                                    Color defaultColor = GUI.backgroundColor;
                                    GUI.backgroundColor = Color.red;
                                    UpdateCoreProccess(internalDependency.name,
                                        internalDependency.android_info.unity_content,
                                        latestDependency.android_info.unity_content,
                                        PlatformSdk.Android);
                                    GUI.backgroundColor = defaultColor;
                                }
                            }
                        }
                    }
                }

                GUILayout.Space(5);
                GUILayout.Space(5);
                GUILayout.Space(5);
            }
        }

        private void UpdateCoreProccess(string internalDependencyName, string internalDependencyUnityContent,
            string latestDependencyUnityContent, PlatformSdk platformSdk)
        {
            if (GUILayout.Button(
                new GUIContent { text = AppodealEditorConstants.ActionUpdate },
                btnFieldWidth))
            {
                var option = EditorUtility.DisplayDialog("Update dependencies",
                    "If you will update core, all adapters this platform will be updated automatically. " +
                    "Do you want to update core?",
                    "Ok",
                    "Cancel");
                if (!option) return;
                switch (platformSdk)
                {
                    case PlatformSdk.iOS:
                        if (internalDependencies.Count <= 0 || latestDependencies.Count <= 0) return;
                        foreach (var key in internalDependencies.Keys.Where(key =>
                            latestDependencies.ContainsKey(key)))
                        {
                            if (internalDependencies.TryGetValue(key, out var internalDep) &&
                                latestDependencies.TryGetValue(key, out var latestDep))
                            {
                                if (internalDep.ios_info != null)
                                {
                                    UpdateDependency(internalDep.name,
                                        internalDep.ios_info.unity_content,
                                        latestDep.ios_info.unity_content);
                                }
                            }
                        }

                        break;
                    case PlatformSdk.Android:
                        if (internalDependencies.Count <= 0 || latestDependencies.Count <= 0) return;
                        foreach (var key in internalDependencies.Keys.Where(key =>
                            latestDependencies.ContainsKey(key)))
                        {
                            if (internalDependencies.TryGetValue(key, out var internalDep) &&
                                latestDependencies.TryGetValue(key, out var latestDep))
                            {
                                if (internalDep.android_info != null)
                                {
                                    UpdateDependency(internalDep.name,
                                        internalDep.android_info.unity_content,
                                        latestDep.android_info.unity_content);
                                }
                            }
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(platformSdk), platformSdk, null);
                }

                UpdateDependency(internalDependencyName,
                    internalDependencyUnityContent,
                    latestDependencyUnityContent);

                UpdateWindow();
            }
        }

        private void SetAdapterImportInfo(string nameDep, string currentVersion, string latestVersion, string content)
        {
            using (new EditorGUILayout.VerticalScope(AppodealEditorConstants.BoxStyle))
            {
                GUILayout.Space(5);
                using (new EditorGUILayout.HorizontalScope(GUILayout.Height(20)))
                {
                    GUILayout.Space(2);
                    if (string.IsNullOrEmpty(nameDep) || string.IsNullOrEmpty(currentVersion) ||
                        string.IsNullOrEmpty(latestVersion)) return;
                    GUILayout.Button(nameDep, packageInfoStyle,
                        GUILayout.Width(145));
                    GUILayout.Space(56);
                    GUILayout.Button(currentVersion,
                        packageInfoStyle,
                        GUILayout.Width(110));
                    GUILayout.Space(85);
                    GUILayout.Button(
                        AppodealDependencyUtils.ReplaceBetaVersion(latestVersion),
                        packageInfoStyle);
                    GUILayout.Space(15);
                    Color defaultColor = GUI.backgroundColor;
                    GUI.backgroundColor = Color.green;
                    if (GUILayout.Button(
                        new GUIContent { text = AppodealEditorConstants.ActionImport },
                        btnFieldWidth))
                    {
                        ImportConfig(nameDep, content);
                    }
                    GUI.backgroundColor = defaultColor;

                    GUILayout.Space(5);
                    GUILayout.Space(5);
                    GUILayout.Space(5);
                }
                GUILayout.Space(5);
            }
        }

        private static string SDKInfo(IEnumerable<NetworkDependency.Dependency> dependencies)
        {
            string content = null;
            var enumerable = dependencies as NetworkDependency.Dependency[] ?? dependencies.ToArray();
            foreach (var dependency in enumerable)
            {
                if (dependency.Equals(enumerable.Last()))
                {
                    content += dependency.name + " - " + dependency.version;
                }
                else
                {
                    content += dependency.name + " - " + dependency.version + "\n";
                }
            }

            return string.IsNullOrEmpty(content) ? " " : content;
        }

        private void SetAdapterUpdateInfo(string nameDep, string currentVersion, string latestVersion,
            string internalContent, string latestContent, string sdkInfoDependencies)
        {
            using (new EditorGUILayout.VerticalScope(AppodealEditorConstants.BoxStyle))
            {
                GUILayout.Space(5);
                using (new EditorGUILayout.HorizontalScope(GUILayout.Height(20)))
                {
                    GUILayout.Space(2);
                    if (string.IsNullOrEmpty(nameDep) || string.IsNullOrEmpty(currentVersion) ||
                        string.IsNullOrEmpty(latestVersion)) return;
                    EditorGUILayout.LabelField(new GUIContent
                    {
                        text = nameDep,
                        tooltip = string.IsNullOrEmpty(sdkInfoDependencies) ? "-" : sdkInfoDependencies
                    }, packageInfoStyle, GUILayout.Width(145));
                    GUILayout.Space(56);
                    GUILayout.Button(
                        AppodealDependencyUtils.ReplaceBetaVersion(currentVersion),
                        packageInfoStyle,
                        GUILayout.Width(110));
                    GUILayout.Space(85);
                    GUILayout.Button(
                        AppodealDependencyUtils.ReplaceBetaVersion(latestVersion),
                        packageInfoStyle);
                    GUILayout.Space(15);

                    if (GUILayout.Button(
                        new GUIContent { text = AppodealEditorConstants.ActionRemove },
                        btnFieldWidth))
                    {
                        var path = Path.Combine(AppodealEditorConstants.PluginPath,
                            AppodealEditorConstants.NetworkDepsPath,
                            $"{nameDep}{AppodealEditorConstants.Dependencies}{AppodealEditorConstants.XmlFileExtension}");

                        AppodealDependencyUtils.ReplaceInFile(path, internalContent, "");
                        var text = System.IO.File.ReadAllLines(path).Where(s => s.Trim() != string.Empty).ToArray();
                        File.Delete(path);
                        System.IO.File.WriteAllLines(path, text);
                        AppodealDependencyUtils.FormatXml(System.IO.File.ReadAllText(path));

                        UpdateWindow();
                    }

                    var current = AppodealDependencyUtils.GetMajorVersion(
                        AppodealDependencyUtils.ReplaceBetaVersion(currentVersion));
                    var last = AppodealDependencyUtils.GetMajorVersion(
                        AppodealDependencyUtils.ReplaceBetaVersion(latestVersion));

                    if (AppodealDependencyUtils.CompareVersion(current, last) == -1)
                    {
                        CompareForAction(0,
                            nameDep, internalContent, latestContent);
                    }
                    else
                    {
                        if (AppodealDependencyUtils.CompareVersion(currentVersion, latestVersion) == -1)
                        {
                            CompareForAction(AppodealDependencyUtils.CompareVersion(
                                    AppodealDependencyUtils.ReplaceBetaVersion(currentVersion),
                                    AppodealDependencyUtils.ReplaceBetaVersion(latestVersion)),
                                nameDep, internalContent, latestContent);
                        }
                        else
                        {
                            CompareForAction(0,
                                nameDep, internalContent, latestContent);
                        }
                    }
                }
                GUILayout.Space(5);
            }
        }

        private void CompareForAction(int action, string nameDependency, string previous, string latest)
        {
            if (action == -1)
            {
                Color defaultColor = GUI.backgroundColor;
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button(
                    new GUIContent { text = AppodealEditorConstants.ActionUpdate },
                    btnFieldWidth))
                {
                    UpdateDependency(nameDependency, previous, latest);
                    UpdateWindow();
                }
                GUI.backgroundColor = defaultColor;
            }
            else
            {
                GUI.enabled = false;
                GUILayout.Button(
                    new GUIContent { text = AppodealEditorConstants.ActionUpdate },
                    btnFieldWidth);
                GUI.enabled = true;
            }

            GUILayout.Space(5);
            GUILayout.Space(5);
            GUILayout.Space(5);
        }

        private void UpdateDependency(string nameDependency, string previous, string latest)
        {
            var path = Path.Combine(AppodealEditorConstants.PluginPath,
                AppodealEditorConstants.NetworkDepsPath,
                $"{nameDependency}{AppodealEditorConstants.Dependencies}{AppodealEditorConstants.XmlFileExtension}");

            if (!File.Exists(path))
            {
                AppodealDependencyUtils.ShowInternalErrorDialog(this,
                    "Can't find config with path " + path, $"path - {nameDependency}");
            }
            else
            {
                string contentString;
                using (var reader = new StreamReader(path))
                {
                    contentString = reader.ReadToEnd();
                    reader.Close();
                }

                contentString = Regex.Replace(contentString, previous, latest);

                using (var writer = new StreamWriter(path))
                {
                    writer.Write(contentString);
                    writer.Close();
                }
            }
        }

        private void GuiPluginRow(AppodealUnityPlugin plugin)
        {
            using (new EditorGUILayout.HorizontalScope(GUILayout.Height(20)))
            {
                GUILayout.Space(2);
                GUILayout.Button(AppodealEditorConstants.AppodealUnityPlugin, packageInfoStyle,
                    GUILayout.Width(150));
                GUILayout.Space(56);
                GUILayout.Button(AppodealDependencyUtils.ReplaceBetaVersion(AppodealVersions.APPODEAL_PLUGIN_VERSION), packageInfoStyle, GUILayout.Width(110));
                GUILayout.Space(85);
                GUILayout.Button(AppodealDependencyUtils.ReplaceBetaVersion(plugin.version), packageInfoStyle);
                GUILayout.Space(15);

                if (AppodealDependencyUtils.CompareVersion(
                    AppodealVersions.APPODEAL_PLUGIN_VERSION,
                    plugin.version) == 0)
                {
                    GUI.enabled = false;
                    GUILayout.Button(new GUIContent { text = AppodealEditorConstants.ActionUpdate },
                        btnFieldWidth);
                    GUI.enabled = true;
                }
                else if (AppodealDependencyUtils.CompareVersion(
                    AppodealVersions.APPODEAL_PLUGIN_VERSION,
                    plugin.version) > 0)
                {
                    GUI.enabled = false;
                    GUILayout.Button(new GUIContent { text = AppodealEditorConstants.ActionUpdate },
                        btnFieldWidth);
                    GUI.enabled = true;
                }
                else
                {
                    Color defaultColor = GUI.backgroundColor;
                    GUI.backgroundColor = Color.red;
                    if (GUILayout.Button(new GUIContent { text = AppodealEditorConstants.ActionUpdate },
                        btnFieldWidth))
                    {
                        bool decision = RemoveHelper.RemovePlugin();
                        if (decision)
                        {
                            Client.Add($"{AppodealEditorConstants.GitRepoAddress}#v{plugin.version}");
                            this.Close();
                        }
                    }
                    GUI.backgroundColor = defaultColor;
                }

                GUILayout.Space(15);
            }
        }

        private IEnumerator GetAppodealSDKData()
        {
            yield return null;


            if (!EditorUtility.DisplayCancelableProgressBar(
                AppodealEditorConstants.AppodealSdkManager,
                AppodealEditorConstants.Loading,
                80f))
            {
            }

            #region Internal

            if (AppodealDependencyUtils.GetInternalDependencyPath() != null)
            {
                foreach (var fileInfo in AppodealDependencyUtils.GetInternalDependencyPath())
                {
                    if (!File.Exists(Path.Combine(AppodealEditorConstants.PluginPath,
                            AppodealEditorConstants.NetworkDepsPath, fileInfo.Name)))
                    {
                        AppodealDependencyUtils.ShowInternalErrorDialog(this,
                            $"File doesn't exist - {Path.Combine(AppodealEditorConstants.PluginPath, AppodealEditorConstants.NetworkDepsPath, fileInfo.Name)}",
                            string.Empty);
                    }
                    else
                    {
                        GetInternalDependencies(Path.Combine(AppodealEditorConstants.PluginPath,
                            AppodealEditorConstants.NetworkDepsPath, fileInfo.Name));
                    }
                }
            }
            else
            {
                AppodealDependencyUtils.ShowInternalErrorDialog(this,
                    "Can't find internal dependencies. Make sure to import them at 'Appodeal/Appodeal Settings' tab first");
            }

            #endregion

            #region Plugin

            using (var webRequest = UnityWebRequest.Get(AppodealEditorConstants.PluginRequest))
            {
                yield return webRequest.SendWebRequest();
                var pages = AppodealEditorConstants.PluginRequest.Split('/');
                var page = pages.Length - 1;
                if (webRequest.isNetworkError)
                {
                    Debug.Log(pages[page] + ": Error: " + webRequest.error);
                    AppodealDependencyUtils.ShowInternalErrorDialog(this, webRequest.error, string.Empty);
                }
                else
                {
                    if (string.IsNullOrEmpty(webRequest.downloadHandler.text))
                    {
                        AppodealDependencyUtils.ShowInternalErrorDialog(this, "Can't find appodeal plugin information",
                            string.Empty);
                        yield break;
                    }

                    var root = JsonUtility.FromJson<Root>(webRequest.downloadHandler.text);
                    appodealUnityPlugin = root.items.ToList().FirstOrDefault(x => x.build_type.Equals("stable"));

                    if (appodealUnityPlugin == null)
                    {
                        AppodealDependencyUtils.ShowInternalErrorDialog(this, "Can't find appodeal plugin information",
                            string.Empty);
                        yield break;
                    }
                }
            }

            #endregion

            #region Adapters

            const string adaptersUri = AppodealEditorConstants.AdaptersRequest +
                                       AppodealVersions.APPODEAL_PLUGIN_VERSION;
            using (var webRequest = UnityWebRequest.Get(adaptersUri))
            {
                yield return webRequest.SendWebRequest();
                var pages = adaptersUri.Split('/');
                var page = pages.Length - 1;
                if (webRequest.isNetworkError)
                {
                    Debug.Log(pages[page] + ": Error: " + webRequest.error);
                    AppodealDependencyUtils.ShowInternalErrorDialog(this, webRequest.error, string.Empty);
                }
                else
                {
                    if (string.IsNullOrEmpty(webRequest.downloadHandler.text))
                    {
                        AppodealDependencyUtils.ShowInternalErrorDialog(this,
                            "Can't find appodeal adapters information",
                            string.Empty);
                        yield break;
                    }

                    var networkDependencies = JsonHelper.FromJson<NetworkDependency>(
                        JsonHelper.fixJson(webRequest.downloadHandler.text));
                    if (networkDependencies.Length > 0)
                    {
                        foreach (var networkDependency in networkDependencies)
                        {
                            if (!string.IsNullOrEmpty(networkDependency.name)
                                && !networkDependency.name.Equals(AppodealEditorConstants.TwitterMoPub))
                            {
                                latestDependencies.Add(networkDependency.name, networkDependency);
                            }
                        }
                        
                        var missingAdapters = internalDependencies.Keys.Where(key => !latestDependencies.ContainsKey(key)).ToList();
                        if (missingAdapters.Count > 0) {
                            AppodealDependencyUtils.ShowInternalErrorDialog(this,
                                $"Out-of-use appodeal adapters were found: {string.Join(", ", missingAdapters)}",
                            string.Empty);
                        }
                    }
                }
            }
            
            #endregion

            coroutine = null;

            isPluginInfoReady = true;

            EditorUtility.ClearProgressBar();
        }

        private void GetInternalDependencies(string dependencyPath)
        {
            var networkDependency = new NetworkDependency
            {
                name = AppodealDependencyUtils.GetConfigName(dependencyPath)
            };

            #region iOSInternalDependencies

            var sourcesiOS = new List<string>();
            string podName = null;
            string version = null;
            string minTargetSdk = null;

            XmlUtilities.ParseXmlTextFileElements(dependencyPath,
                (reader, elementName, isStart, parentElementName, elementNameStack) =>
                {
                    if (elementName == "dependencies" &&
                        parentElementName == "" || elementName == "iosPods" &&
                        (parentElementName == "dependencies" || parentElementName == ""))
                        return true;

                    if (elementName == "iosPod" && parentElementName == "iosPods")
                    {
                        if (isStart)
                        {
                            podName = reader.GetAttribute("name");
                            version = reader.GetAttribute("version");
                            minTargetSdk = reader.GetAttribute("minTargetSdk");

                            sourcesiOS = new List<string>();
                            if (podName == null)
                            {
                                Debug.Log(
                                    $"Pod name not specified while reading {dependencyPath}:{reader.LineNumber}\n");
                                return false;
                            }
                        }
                        else
                        {
                            if (podName != null && version != null && minTargetSdk != null)
                            {
                                if (!podName.Contains(AppodealEditorConstants.APDAppodealAdExchangeAdapter))
                                {
                                    networkDependency.ios_info = new NetworkDependency.iOSDependency(podName,
                                        version,
                                        AppodealDependencyUtils.GetiOSContent(dependencyPath));
                                }
                            }
                        }

                        return true;
                    }

                    if (elementName == "sources" && parentElementName == "iosPod")
                        return true;
                    if (elementName == "sources" && parentElementName == "iosPods")
                    {
                        if (isStart)
                        {
                            sourcesiOS = new List<string>();
                        }
                        else
                        {
                            using (var enumerator = sourcesiOS.GetEnumerator())
                            {
                                while (enumerator.MoveNext())
                                {
                                    var current = enumerator.Current;
                                    Debug.Log(current);
                                }
                            }
                        }

                        return true;
                    }

                    if (!(elementName == "source") || !(parentElementName == "sources"))
                        return false;
                    if (isStart && reader.Read() && reader.NodeType == XmlNodeType.Text)
                        sourcesiOS.Add(reader.ReadContentAsString());
                    return true;
                });

            #endregion

            #region AndroidInternalDependencies

            var sources = new List<string>();
            string specName;

            XmlUtilities.ParseXmlTextFileElements(dependencyPath,
                (reader, elementName, isStart, parentElementName, elementNameStack) =>
                {
                    if (elementName == "dependencies" &&
                        parentElementName == "" || elementName == "androidPackages" &&
                        (parentElementName == "dependencies" || parentElementName == ""))
                    {
                        return true;
                    }

                    if (elementName == "androidPackage" && parentElementName == "androidPackages")
                    {
                        if (isStart)
                        {
                            specName = reader.GetAttribute("spec");
                            sources = new List<string>();
                            if (specName == null)
                            {
                                Debug.Log(
                                    $"Spec attribute was not found at {dependencyPath}:{reader.LineNumber}\n");
                                return false;
                            }

                            if (networkDependency.name == AppodealEditorConstants.Appodeal && 
                                !specName.Contains(AppodealEditorConstants.ReplaceDepCore)) return true;
                            
                            if (networkDependency.name == AppodealEditorConstants.GoogleAdMob && 
                                !specName.Contains(AppodealEditorConstants.ReplaceAdmobDepValue)) return true;

                            if (specName.Contains(AppodealEditorConstants.ReplaceDepValue))
                            {
                                networkDependency.android_info = new NetworkDependency.AndroidDependency(
                                    AppodealDependencyUtils.GetAndroidDependencyName(specName),
                                    AppodealDependencyUtils.GetAndroidDependencyVersion(specName),
                                    AppodealDependencyUtils.GetAndroidContent(dependencyPath));
                                
                                return false;
                            }
                            else if (specName.Contains(AppodealEditorConstants.ReplaceDepCore))
                            {
                                networkDependency.android_info = new NetworkDependency.AndroidDependency(
                                    "appodeal",
                                    AppodealDependencyUtils.GetAndroidDependencyCoreVersion(specName),
                                    AppodealDependencyUtils.GetAndroidContent(dependencyPath));
                                
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                    }
                    return false;
                });

            #endregion

            if (!string.IsNullOrEmpty(networkDependency.name))
            {
                internalDependencies.Add(networkDependency.name, networkDependency);
            }
        }

        private void UpdateWindow()
        {
            Reset();
            coroutine = this.StartCoroutine(GetAppodealSDKData());
            GUI.enabled = true;
            AssetDatabase.Refresh();
        }

        private void OnInspectorUpdate()
        {
            Repaint();
        }
    }
}