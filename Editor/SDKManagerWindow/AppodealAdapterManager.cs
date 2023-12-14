using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Networking;
using File = UnityEngine.Windows.File;
using marijnz.EditorCoroutines;
using AppodealStack.Monetization.Common;
using AppodealStack.UnityEditor.Utils;
using AppodealStack.UnityEditor.PluginRemover;
using AppodealStack.UnityEditor.SDKManager.Models;

#pragma warning disable 612

// ReSharper Disable CheckNamespace
namespace AppodealStack.UnityEditor.SDKManager
{
    public enum PlatformSdk
    {
        Android,
        Ios
    }

    public class AppodealAdapterManager : EditorWindow
    {
        #region Dictionaries

        private SortedDictionary<string, AppodealDependency> _internalDependencies = new SortedDictionary<string, AppodealDependency>();
        private SortedDictionary<string, AppodealDependency> _latestDependencies = new SortedDictionary<string, AppodealDependency>();

        #endregion

        #region GUIStyles

        private GUIStyle _labelStyle;
        private GUIStyle _headerInfoStyle;
        private GUIStyle _subHeaderInfoStyle;
        private GUIStyle _packageInfoStyle;
        private GUIStyle _separatorLineStyle;
        private readonly GUILayoutOption _btnFieldWidth = GUILayout.Width(60);

        #endregion

        #region Coroutines

        private static EditorCoroutines.EditorCoroutine _coroutine;
        private float _loading;
        private float _progress;
        private bool _isPluginInfoReady;

        #endregion

        private Vector2 _scrollPosition;

        private AppodealUnityPlugin _appodealUnityPlugin;

        public static void ShowSdkManager()
        {
            GetWindow(typeof(AppodealAdapterManager),
                true, AppodealEditorConstants.AppodealSdkManager);
        }

        private void HorizontalLine ()
        {
            var temp = GUI.color;
            GUI.color = Color.grey;
            GUILayout.Box(GUIContent.none, _separatorLineStyle);
            GUI.color = temp;
        }

        private void Awake()
        {
            _labelStyle = new GUIStyle(EditorStyles.label)
            {
                fontSize = 18,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter
            };

            _packageInfoStyle = new GUIStyle(EditorStyles.label)
            {
                fontSize = 12,
                fontStyle = FontStyle.Normal,
                fixedHeight = 18
            };

            _headerInfoStyle = new GUIStyle(EditorStyles.label)
            {
                fontSize = 13,
                fontStyle = FontStyle.Bold,
                fixedHeight = 18
            };

            _subHeaderInfoStyle = new GUIStyle(EditorStyles.label)
            {
                fontSize = 16,
                fontStyle = FontStyle.Bold,
                normal = new GUIStyleState() { textColor = new Color(0.7f,0.6f,0.1f) },
                alignment = TextAnchor.MiddleCenter

            };

            _separatorLineStyle = new GUIStyle()
            {
                normal = new GUIStyleState() { background = EditorGUIUtility.whiteTexture },
                margin = new RectOffset(0, 0, 10, 5),
                fixedHeight = 2
            };

            Reset();
        }

        public void Reset()
        {
            _internalDependencies = new SortedDictionary<string, AppodealDependency>();
            _latestDependencies = new SortedDictionary<string, AppodealDependency>();

            if (_coroutine != null)
                this.StopCoroutine(_coroutine.routine);
            if (_progress > 0)
                EditorUtility.ClearProgressBar();
            if (_loading > 0)
                EditorUtility.ClearProgressBar();

            _coroutine = null;

            _loading = 0f;
            _progress = 0f;
        }

        private void OnEnable()
        {
            _loading = 0f;
            _coroutine = this.StartCoroutine(GetAppodealSDKData());
        }

        private void OnGUI()
        {
            this.minSize = new Vector2(650, 650);
            this.maxSize = new Vector2(2000, 2000);

            if (_isPluginInfoReady && _internalDependencies.Count > 0 && _latestDependencies.Count > 0 && _appodealUnityPlugin != null)
                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, false, false);
            else EditorGUILayout.BeginScrollView(_scrollPosition, false, false);

            GUILayout.BeginVertical();

            if (_isPluginInfoReady)
            {
                #region Plugin

                GUILayout.Space(10);
                EditorGUILayout.LabelField(AppodealEditorConstants.AppodealUnityPlugin, _labelStyle,
                    GUILayout.Height(20));

                if (_appodealUnityPlugin != null)
                {
                    GUILayout.Space(5);
                    using (new EditorGUILayout.VerticalScope(AppodealEditorConstants.BoxStyle, GUILayout.Height(45)))
                    {
                        AppodealDependencyUtils.GuiHeaders(_headerInfoStyle, _btnFieldWidth);
                        if (!String.IsNullOrEmpty(AppodealVersions.GetPluginVersion()) &&
                            !String.IsNullOrEmpty(_appodealUnityPlugin.version) &&
                            !String.IsNullOrEmpty(_appodealUnityPlugin.source))
                        {
                            GuiPluginRow(_appodealUnityPlugin);
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

                if (_internalDependencies.Count > 0 && _latestDependencies.Count > 0)
                {
                    GUILayout.Space(10);
                    EditorGUILayout.LabelField(AppodealEditorConstants.AppodealCoreDependencies, _labelStyle,
                        GUILayout.Height(20));
                    GUILayout.Space(5);
                    EditorGUILayout.LabelField(AppodealEditorConstants.Ios, _subHeaderInfoStyle, GUILayout.Height(20));
                    GUILayout.Space(5);
                    using (new EditorGUILayout.VerticalScope(AppodealEditorConstants.BoxStyle, GUILayout.Height(45)))
                    {
                        AppodealDependencyUtils.GuiHeaders(_headerInfoStyle, _btnFieldWidth);
                        GuiCoreRow(AppodealDependencyUtils.GetAppodealDependency(_internalDependencies),
                            AppodealDependencyUtils.GetAppodealDependency(_latestDependencies), PlatformSdk.Ios);
                    }
                    GUILayout.Space(5);
                    EditorGUILayout.LabelField(AppodealEditorConstants.Android, _subHeaderInfoStyle, GUILayout.Height(20));
                    GUILayout.Space(5);
                    using (new EditorGUILayout.VerticalScope(AppodealEditorConstants.BoxStyle, GUILayout.Height(45)))
                    {
                        AppodealDependencyUtils.GuiHeaders(_headerInfoStyle, _btnFieldWidth);
                        GuiCoreRow(AppodealDependencyUtils.GetAppodealDependency(_internalDependencies),
                            AppodealDependencyUtils.GetAppodealDependency(_latestDependencies), PlatformSdk.Android);
                    }
                    HorizontalLine();
                }

                #endregion

                #region NetworksAdaptersInfo

                if (_internalDependencies.Count > 0)
                {
                    GUILayout.Space(10);
                    EditorGUILayout.LabelField(AppodealEditorConstants.AppodealNetworkDependencies, _labelStyle,
                        GUILayout.Height(20));
                    GUILayout.Space(5);
                    EditorGUILayout.LabelField(AppodealEditorConstants.Ios, _subHeaderInfoStyle, GUILayout.Height(20));
                    GUILayout.Space(5);
                    using (new EditorGUILayout.VerticalScope(AppodealEditorConstants.BoxStyle, GUILayout.Height(45)))
                    {
                        AppodealDependencyUtils.GuiHeaders(_headerInfoStyle, _btnFieldWidth);
                        GuiAdaptersRows(PlatformSdk.Ios, DependencyType.AdNetwork);
                    }
                    GUILayout.Space(5);
                    EditorGUILayout.LabelField(AppodealEditorConstants.Android, _subHeaderInfoStyle, GUILayout.Height(20));
                    GUILayout.Space(5);
                    using (new EditorGUILayout.VerticalScope(AppodealEditorConstants.BoxStyle, GUILayout.Height(45)))
                    {
                        AppodealDependencyUtils.GuiHeaders(_headerInfoStyle, _btnFieldWidth);
                        GuiAdaptersRows(PlatformSdk.Android, DependencyType.AdNetwork);
                    }
                    HorizontalLine();
                }

                #endregion

                #region ServicesAdaptersInfo

                if (_internalDependencies.Count > 0)
                {
                    GUILayout.Space(10);
                    EditorGUILayout.LabelField(AppodealEditorConstants.AppodealServiceDependencies, _labelStyle, GUILayout.Height(20));
                    GUILayout.Space(5);
                    EditorGUILayout.LabelField(AppodealEditorConstants.Ios, _subHeaderInfoStyle, GUILayout.Height(20));
                    GUILayout.Space(5);
                    using (new EditorGUILayout.VerticalScope(AppodealEditorConstants.BoxStyle, GUILayout.Height(45)))
                    {
                        AppodealDependencyUtils.GuiHeaders(_headerInfoStyle, _btnFieldWidth);
                        GuiAdaptersRows(PlatformSdk.Ios, DependencyType.Service);
                    }
                    GUILayout.Space(5);
                    EditorGUILayout.LabelField(AppodealEditorConstants.Android, _subHeaderInfoStyle, GUILayout.Height(20));
                    GUILayout.Space(5);
                    using (new EditorGUILayout.VerticalScope(AppodealEditorConstants.BoxStyle, GUILayout.Height(45)))
                    {
                        AppodealDependencyUtils.GuiHeaders(_headerInfoStyle, _btnFieldWidth);
                        GuiAdaptersRows(PlatformSdk.Android, DependencyType.Service);
                    }
                    HorizontalLine();
                }

                #endregion

            }

            GUILayout.Space(5);
            GUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }

        private void GuiAdaptersRows(PlatformSdk platformSdk, DependencyType type)
        {
            foreach (var key in _latestDependencies.Keys)
            {
                if (_internalDependencies.ContainsKey(key))
                {
                    if (key.Equals(AppodealEditorConstants.Appodeal)) continue;

                    if (_latestDependencies.TryGetValue(key, out var latestDependency) &&
                        _internalDependencies.TryGetValue(key, out var internalDependency))
                    {
                        switch (platformSdk)
                        {
                            case PlatformSdk.Android:
                                if (latestDependency.type == type)
                                {
                                    if (internalDependency.android_info != null && latestDependency.android_info != null)
                                    {
                                        if (!String.IsNullOrEmpty(internalDependency.android_info.name)
                                            && !String.IsNullOrEmpty(internalDependency.android_info.version)
                                            && !String.IsNullOrEmpty(internalDependency.android_info.unity_content))
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
                                        if (latestDependency.android_info?.name != null)
                                        {
                                            GUILayout.Space(5);
                                            SetAdapterInformationForImport(latestDependency, platformSdk);
                                        }
                                    }
                                }
                                break;
                            case PlatformSdk.Ios:
                                if (latestDependency.type == type)
                                {
                                    if (internalDependency.ios_info != null && latestDependency.ios_info != null)
                                    {
                                        if (!String.IsNullOrEmpty(internalDependency.ios_info.name)
                                            && !String.IsNullOrEmpty(internalDependency.ios_info.version)
                                            && !String.IsNullOrEmpty(internalDependency.ios_info.unity_content))
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
                                }
                                break;
                        }
                    }
                }
                else
                {
                    if (_latestDependencies.TryGetValue(key, out var dependency))
                    {
                        switch (platformSdk)
                        {
                            case PlatformSdk.Android:
                                if (dependency.type == type && dependency.android_info?.name != null)
                                {
                                    GUILayout.Space(5);
                                    SetAdapterInformationForImport(dependency, PlatformSdk.Android);
                                }

                                break;
                            case PlatformSdk.Ios:
                                if (dependency.type == type && dependency.ios_info?.name != null)
                                {
                                    GUILayout.Space(5);
                                    SetAdapterInformationForImport(dependency, PlatformSdk.Ios);
                                }

                                break;
                            default:
                                throw new ArgumentOutOfRangeException(nameof(platformSdk), platformSdk, null);
                        }
                    }
                }
            }
        }

        private void SetAdapterInformationForImport(AppodealDependency latestDependency, PlatformSdk platformSdk)
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
                case PlatformSdk.Ios:
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
                AppodealEditorConstants.DependenciesPath,
                $"{nameDep}{AppodealEditorConstants.Dependencies}{AppodealEditorConstants.XmlFileExtension}");

            if (File.Exists(path))
            {
                UpdateDependency(nameDep, AppodealEditorConstants.SpecCloseDependencies,
                    content + "\n" + AppodealEditorConstants.SpecCloseDependencies);
            }
            else
            {
                using (TextWriter writer = new StreamWriter(path, false))
                {
                    writer.WriteLine(AppodealEditorConstants.SpecOpenDependencies
                                     + content + "\n" + AppodealEditorConstants.SpecCloseDependencies);
                    writer.Close();
                }

                AppodealDependencyUtils.FormatXml(path);
            }

            UpdateWindow();
        }

        private void GuiCoreRow(AppodealDependency internalDependency, AppodealDependency latestDependency,
            PlatformSdk platform)
        {
            if (internalDependency == null || latestDependency == null) return;
            using (new EditorGUILayout.HorizontalScope(GUILayout.Height(20)))
            {
                if (!String.IsNullOrEmpty(internalDependency.name))
                {
                    GUILayout.Space(2);
                    GUILayout.Button(internalDependency.name, _packageInfoStyle, GUILayout.Width(150));
                    if (platform == PlatformSdk.Ios)
                    {
                        if (internalDependency.ios_info != null
                            && !String.IsNullOrEmpty(internalDependency.ios_info.version))
                        {
                            GUILayout.Space(56);
                            GUILayout.Button(
                                AppodealDependencyUtils.ReplaceBetaVersion(internalDependency.ios_info.version),
                                _packageInfoStyle, GUILayout.Width(110));
                            if (latestDependency.ios_info != null &&
                                !String.IsNullOrEmpty(latestDependency.ios_info.version))
                            {
                                GUILayout.Space(85);
                                GUILayout.Button(
                                    AppodealDependencyUtils.ReplaceBetaVersion(latestDependency.ios_info.version),
                                    _packageInfoStyle);
                                GUILayout.Space(15);

                                if (AppodealDependencyUtils.CompareVersion(internalDependency.ios_info.version,
                                    latestDependency.ios_info.version) == 0)
                                {
                                    GUI.enabled = false;
                                    GUILayout.Button(
                                        new GUIContent { text = AppodealEditorConstants.ActionUpdate },
                                        _btnFieldWidth);
                                    GUI.enabled = true;
                                }
                                else if (AppodealDependencyUtils.CompareVersion(internalDependency.ios_info.version,
                                    latestDependency.ios_info.version) > 0)
                                {
                                    GUI.enabled = false;
                                    GUILayout.Button(
                                        new GUIContent { text = AppodealEditorConstants.ActionUpdate },
                                        _btnFieldWidth);
                                    GUI.enabled = true;
                                }
                                else
                                {
                                    Color defaultColor = GUI.backgroundColor;
                                    GUI.backgroundColor = Color.red;
                                    UpdateCoreProcess(internalDependency.name,
                                        internalDependency.ios_info.unity_content,
                                        latestDependency.ios_info.unity_content, PlatformSdk.Ios);
                                    GUI.backgroundColor = defaultColor;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (internalDependency.android_info != null
                            && !String.IsNullOrEmpty(internalDependency.android_info.version))
                        {
                            GUILayout.Space(56);
                            GUILayout.Button(
                                AppodealDependencyUtils.ReplaceBetaVersion(internalDependency.android_info.version),
                                _packageInfoStyle, GUILayout.Width(110));
                            if (latestDependency.android_info != null &&
                                !String.IsNullOrEmpty(latestDependency.android_info.version))
                            {
                                GUILayout.Space(85);
                                GUILayout.Button(
                                    AppodealDependencyUtils.ReplaceBetaVersion(
                                        latestDependency.android_info.version),
                                    _packageInfoStyle);
                                GUILayout.Space(15);

                                if (AppodealDependencyUtils.CompareVersion(
                                    internalDependency.android_info.version,
                                    latestDependency.android_info.version) == 0)
                                {
                                    GUI.enabled = false;
                                    GUILayout.Button(
                                        new GUIContent { text = AppodealEditorConstants.ActionUpdate },
                                        _btnFieldWidth);
                                    GUI.enabled = true;
                                }
                                else if (AppodealDependencyUtils.CompareVersion(
                                    internalDependency.android_info.version,
                                    latestDependency.android_info.version) > 0)
                                {
                                    GUI.enabled = false;
                                    GUILayout.Button(
                                        new GUIContent { text = AppodealEditorConstants.ActionUpdate },
                                        _btnFieldWidth);
                                    GUI.enabled = true;
                                }
                                else
                                {
                                    Color defaultColor = GUI.backgroundColor;
                                    GUI.backgroundColor = Color.red;
                                    UpdateCoreProcess(internalDependency.name,
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

        private void UpdateCoreProcess(string internalDependencyName, string internalDependencyUnityContent,
            string latestDependencyUnityContent, PlatformSdk platformSdk)
        {
            if (GUILayout.Button(
                new GUIContent { text = AppodealEditorConstants.ActionUpdate },
                _btnFieldWidth))
            {
                var option = EditorUtility.DisplayDialog("Update dependencies",
                    "If you will update core, all adapters for this platform will be updated automatically. " +
                    "Do you want to update core?",
                    "Ok",
                    "Cancel");
                if (!option) return;
                switch (platformSdk)
                {
                    case PlatformSdk.Ios:
                        if (_internalDependencies.Count <= 0 || _latestDependencies.Count <= 0) return;
                        foreach (var key in _internalDependencies.Keys.Where(key =>
                            _latestDependencies.ContainsKey(key)))
                        {
                            if (_internalDependencies.TryGetValue(key, out var internalDep) &&
                                _latestDependencies.TryGetValue(key, out var latestDep))
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
                        if (_internalDependencies.Count <= 0 || _latestDependencies.Count <= 0) return;
                        foreach (var key in _internalDependencies.Keys.Where(key =>
                            _latestDependencies.ContainsKey(key)))
                        {
                            if (_internalDependencies.TryGetValue(key, out var internalDep) &&
                                _latestDependencies.TryGetValue(key, out var latestDep))
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

                UpdateDependency(internalDependencyName, internalDependencyUnityContent, latestDependencyUnityContent);

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
                    if (String.IsNullOrEmpty(nameDep) || String.IsNullOrEmpty(currentVersion) ||
                        String.IsNullOrEmpty(latestVersion)) return;
                    GUILayout.Button(nameDep, _packageInfoStyle,
                        GUILayout.Width(145));
                    GUILayout.Space(56);
                    GUILayout.Button(currentVersion,
                        _packageInfoStyle,
                        GUILayout.Width(110));
                    GUILayout.Space(85);
                    GUILayout.Button(
                        AppodealDependencyUtils.ReplaceBetaVersion(latestVersion),
                        _packageInfoStyle);
                    GUILayout.Space(15);
                    Color defaultColor = GUI.backgroundColor;
                    GUI.backgroundColor = Color.green;
                    if (GUILayout.Button(
                        new GUIContent { text = AppodealEditorConstants.ActionImport },
                        _btnFieldWidth))
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

        private static string SDKInfo(IEnumerable<AppodealDependency.Dependency> dependencies)
        {
            string content = null;
            var enumerable = dependencies as AppodealDependency.Dependency[] ?? dependencies.ToArray();
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

            return String.IsNullOrEmpty(content) ? " " : content;
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
                    if (String.IsNullOrEmpty(nameDep) || String.IsNullOrEmpty(currentVersion) ||
                        String.IsNullOrEmpty(latestVersion)) return;
                    EditorGUILayout.LabelField(new GUIContent
                    {
                        text = nameDep,
                        tooltip = String.IsNullOrEmpty(sdkInfoDependencies) ? "-" : sdkInfoDependencies
                    }, _packageInfoStyle, GUILayout.Width(145));
                    GUILayout.Space(56);
                    GUILayout.Button(
                        AppodealDependencyUtils.ReplaceBetaVersion(currentVersion),
                        _packageInfoStyle,
                        GUILayout.Width(110));
                    GUILayout.Space(85);
                    GUILayout.Button(
                        AppodealDependencyUtils.ReplaceBetaVersion(latestVersion),
                        _packageInfoStyle);
                    GUILayout.Space(15);

                    if (GUILayout.Button(
                        new GUIContent { text = AppodealEditorConstants.ActionRemove },
                        _btnFieldWidth))
                    {
                        UpdateDependency(nameDep, internalContent, String.Empty);
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
                    _btnFieldWidth))
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
                    _btnFieldWidth);
                GUI.enabled = true;
            }

            GUILayout.Space(5);
            GUILayout.Space(5);
            GUILayout.Space(5);
        }

        private void UpdateDependency(string nameDependency, string previous, string latest)
        {
            var path = Path.Combine(AppodealEditorConstants.PluginPath,
                AppodealEditorConstants.DependenciesPath,
                $"{nameDependency}{AppodealEditorConstants.Dependencies}{AppodealEditorConstants.XmlFileExtension}");

            if (!File.Exists(path))
            {
                AppodealDependencyUtils.ShowInternalErrorDialog(this,
                    "Can't find config with path " + path, $"path - {nameDependency}");
            }
            else
            {
                AppodealDependencyUtils.ReplaceInFile(path, previous, latest);
                AppodealDependencyUtils.FormatXml(path);
            }
        }

        private void GuiPluginRow(AppodealUnityPlugin plugin)
        {
            using (new EditorGUILayout.HorizontalScope(GUILayout.Height(20)))
            {
                GUILayout.Space(2);
                GUILayout.Button(AppodealEditorConstants.AppodealUnityPlugin, _packageInfoStyle,
                    GUILayout.Width(150));
                GUILayout.Space(56);
                GUILayout.Button(AppodealDependencyUtils.ReplaceBetaVersion(AppodealVersions.GetPluginVersion()), _packageInfoStyle, GUILayout.Width(110));
                GUILayout.Space(85);
                GUILayout.Button(AppodealDependencyUtils.ReplaceBetaVersion(plugin.version), _packageInfoStyle);
                GUILayout.Space(15);

                if (AppodealDependencyUtils.CompareVersion(
                    AppodealVersions.GetPluginVersion(),
                    plugin.version) == 0)
                {
                    GUI.enabled = false;
                    GUILayout.Button(new GUIContent { text = AppodealEditorConstants.ActionUpdate },
                        _btnFieldWidth);
                    GUI.enabled = true;
                }
                else if (AppodealDependencyUtils.CompareVersion(
                    AppodealVersions.GetPluginVersion(),
                    plugin.version) > 0)
                {
                    GUI.enabled = false;
                    GUILayout.Button(new GUIContent { text = AppodealEditorConstants.ActionUpdate },
                        _btnFieldWidth);
                    GUI.enabled = true;
                }
                else
                {
                    Color defaultColor = GUI.backgroundColor;
                    GUI.backgroundColor = Color.red;
                    if (GUILayout.Button(new GUIContent { text = AppodealEditorConstants.ActionUpdate },
                        _btnFieldWidth))
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
                            AppodealEditorConstants.DependenciesPath, fileInfo.Name)))
                    {
                        AppodealDependencyUtils.ShowInternalErrorDialog(this,
                            $"File doesn't exist - {Path.Combine(AppodealEditorConstants.PluginPath, AppodealEditorConstants.DependenciesPath, fileInfo.Name)}",
                            string.Empty);
                    }
                    else
                    {
                        GetInternalDependencies(Path.Combine(AppodealEditorConstants.PluginPath,
                            AppodealEditorConstants.DependenciesPath, fileInfo.Name));
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

                if (webRequest.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.Log(pages[page] + ": Error: " + webRequest.error);
                    AppodealDependencyUtils.ShowInternalErrorDialog(this, webRequest.error, string.Empty);
                }
                else
                {
                    if (String.IsNullOrEmpty(webRequest.downloadHandler.text))
                    {
                        AppodealDependencyUtils.ShowInternalErrorDialog(this, "Can't find appodeal plugin information",
                            string.Empty);
                        yield break;
                    }

                    var root = JsonUtility.FromJson<Root>(webRequest.downloadHandler.text);
                    _appodealUnityPlugin = root.items.ToList().FirstOrDefault(x => x.build_type.Equals("stable"));

                    if (_appodealUnityPlugin == null)
                    {
                        AppodealDependencyUtils.ShowInternalErrorDialog(this, "Can't find appodeal plugin information",
                            string.Empty);
                        yield break;
                    }
                }
            }

            #endregion

            #region Adapters

            string adaptersUri = AppodealEditorConstants.AdaptersRequest + AppodealVersions.GetPluginVersion();

            using (var webRequest = UnityWebRequest.Get(adaptersUri))
            {
                yield return webRequest.SendWebRequest();
                var pages = adaptersUri.Split('/');
                var page = pages.Length - 1;

                if (webRequest.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.Log(pages[page] + ": Error: " + webRequest.error);
                    AppodealDependencyUtils.ShowInternalErrorDialog(this, webRequest.error, string.Empty);
                }
                else
                {
                    if (String.IsNullOrEmpty(webRequest.downloadHandler.text))
                    {
                        AppodealDependencyUtils.ShowInternalErrorDialog(this,
                            "Can't find appodeal adapters information",
                            string.Empty);
                        yield break;
                    }

                    var serverConfig = JsonUtility.FromJson<ServerConfig>(webRequest.downloadHandler.text);

                    serverConfig.core.type = DependencyType.Core;
                    serverConfig.ad_networks.ForEach(network => network.type = DependencyType.AdNetwork);
                    serverConfig.services.ForEach(service => service.type = DependencyType.Service);

                    var tempDeps = new List<AppodealDependency> {serverConfig.core};
                    serverConfig.ad_networks.ForEach(dep => tempDeps.Add(dep));
                    serverConfig.services.ForEach(dep => tempDeps.Add(dep));

                    tempDeps.Where(dep => !String.IsNullOrEmpty(dep.name) && !dep.name.Equals(AppodealEditorConstants.TwitterMoPub))
                               .ToList().ForEach(dep => _latestDependencies.Add(dep.name, dep));

                    if (_latestDependencies.Count > 0)
                    {
                        var missingAdapters = _internalDependencies.Keys.Where(key => !_latestDependencies.ContainsKey(key)).ToList();
                        if (missingAdapters.Count > 0) {
                            AppodealDependencyUtils.ShowInternalErrorDialog(this,
                                $"Out-of-use appodeal adapters were found: {string.Join(", ", missingAdapters)}",
                            string.Empty);
                        }
                    }
                }
            }

            #endregion

            _coroutine = null;

            _isPluginInfoReady = true;

            EditorUtility.ClearProgressBar();
        }

        private void GetInternalDependencies(string dependencyPath)
        {
            var networkDependency = new AppodealDependency
            {
                name = AppodealDependencyUtils.GetConfigName(dependencyPath)
            };

            #region IosInternalDependencies

            var sourcesIos = new List<string>();
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

                            sourcesIos = new List<string>();
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
                                if ((podName.Equals("Appodeal") || podName.StartsWith("APD")) && !podName.Contains(AppodealEditorConstants.ApdAppodealAdExchangeAdapter))
                                {
                                    networkDependency.ios_info = new AppodealDependency.IosDependency(podName,
                                        version,
                                        AppodealDependencyUtils.GetIosContent(dependencyPath));
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
                            sourcesIos = new List<string>();
                        }
                        else
                        {
                            using var enumerator = sourcesIos.GetEnumerator();
                            while (enumerator.MoveNext())
                            {
                                var current = enumerator.Current;
                                Debug.Log(current);
                            }
                        }

                        return true;
                    }

                    if (elementName != "source" || parentElementName != "sources")
                        return false;
                    if (isStart && reader.Read() && reader.NodeType == XmlNodeType.Text)
                        sourcesIos.Add(reader.ReadContentAsString());
                    return true;
                });

            #endregion

            #region AndroidInternalDependencies

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
                            if (specName == null)
                            {
                                Debug.Log(
                                    $"Spec attribute was not found at {dependencyPath}:{reader.LineNumber}\n");
                                return false;
                            }

                            if (networkDependency.name == AppodealEditorConstants.Appodeal &&
                                !specName.Contains(AppodealEditorConstants.ReplaceDepCore)) return true;

                            if (networkDependency.name != AppodealEditorConstants.Appodeal &&
                                specName.Contains(AppodealEditorConstants.ReplaceDepCore)) return true;

                            if (networkDependency.name == AppodealEditorConstants.GoogleAdMob &&
                                !specName.Contains(AppodealEditorConstants.ReplaceAdmobDepValue)) return true;

                            if (specName.Contains(AppodealEditorConstants.ReplaceNetworkDepValue) || specName.Contains(AppodealEditorConstants.ReplaceServiceDepValue))
                            {
                                networkDependency.android_info = new AppodealDependency.AndroidDependency(
                                    AppodealDependencyUtils.GetAndroidDependencyName(specName),
                                    AppodealDependencyUtils.GetAndroidDependencyVersion(specName),
                                    AppodealDependencyUtils.GetAndroidContent(dependencyPath));

                                return false;
                            }
                            else if (specName.Contains(AppodealEditorConstants.ReplaceDepCore))
                            {
                                networkDependency.android_info = new AppodealDependency.AndroidDependency(
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

            if (!String.IsNullOrEmpty(networkDependency.name))
            {
                _internalDependencies.Add(networkDependency.name, networkDependency);
            }
        }

        private void UpdateWindow()
        {
            Reset();
            _coroutine = this.StartCoroutine(GetAppodealSDKData());
            GUI.enabled = true;
            AssetDatabase.Refresh();
        }

        private void OnInspectorUpdate()
        {
            Repaint();
        }
    }
}
