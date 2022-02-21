using UnityEditor;
using UnityEngine;
using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Diagnostics.CodeAnalysis;
using AppodealStack.UnityEditor.Utils;
using AppodealStack.UnityEditor.SDKManager.Models;

// ReSharper disable All

namespace AppodealStack.UnityEditor.SDKManager
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "ReturnValueOfPureMethodIsNotUsed")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static class AppodealDependencyUtils
    {
        public static FileInfo[] GetInternalDependencyPath()
        {
            if (string.IsNullOrEmpty(AppodealEditorConstants.PluginPath) ||
                string.IsNullOrEmpty(AppodealEditorConstants.NetworkDepsPath))
            {
                return null;
            }

            var path = Path.Combine(AppodealEditorConstants.PluginPath, AppodealEditorConstants.NetworkDepsPath);
            if (!Directory.Exists(path))
            {
                return null;
            }
            
            var info = new DirectoryInfo(path);
            var fileInfo = info.GetFiles();
            return fileInfo.Length <= 0 ? null : fileInfo.Where(val => !val.Name.Contains("meta")).ToArray();
        }

        public static void ShowInternalErrorDialog(EditorWindow editorWindow, string message, string debugLog)
        {
            EditorUtility.ClearProgressBar();
            Debug.LogError(message);
            var option = EditorUtility.DisplayDialog("Internal error",
                $"{message}. Please contact Appodeal support.",
                "Ok");
            if (option)
            {
                editorWindow.Close();
            }
        }

        public static void ShowInternalErrorDialog(EditorWindow editorWindow, string message)
        {
            EditorUtility.ClearProgressBar();
            Debug.LogError(message);
            var option = EditorUtility.DisplayDialog("Internal error",
                $"{message}.",
                "Ok");
            if (option)
            {
                editorWindow.Close();
            }
        }

        public static void FormatXml(string inputXml)
        {
            var document = new XmlDocument();
            document.Load(new StringReader(inputXml));
            var builder = new StringBuilder();
            using (var writer = new XmlTextWriter(new StringWriter(builder)))
            {
                writer.Formatting = Formatting.Indented;
                document.Save(writer);
            }
        }

        public static string GetConfigName(string value)
        {
            var configName = value.Replace($"{AppodealEditorConstants.PluginPath}/{AppodealEditorConstants.NetworkDepsPath}/", string.Empty);
            return configName.Replace("Dependencies.xml", string.Empty);
        }

        public static string GetiOSContent(string path)
        {
            var iOSContent = string.Empty;
            var lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line)) continue;

                if (line.Contains("<iosPods>"))
                {
                    iOSContent += line + "\n";
                }

                if (line.Contains("<iosPod name="))
                {
                    iOSContent += line + "\n";
                }

                if (line.Contains("</iosPods>"))
                {
                    iOSContent += line;
                }
            }

            return iOSContent;
        }

        public static string GetAndroidContent(string path)
        {
            var iOSContent = string.Empty;
            var lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line)) continue;

                if (line.Contains("<androidPackages>"))
                {
                    iOSContent += line + "\n";
                }

                if (line.Contains("<androidPackage spec="))
                {
                    iOSContent += line + "\n";
                }

                if (line.Contains("<repositories>"))
                {
                    iOSContent += line + "\n";
                }

                if (line.Contains("<repository>"))
                {
                    iOSContent += line + "\n";
                }

                if (line.Contains("</repositories>"))
                {
                    iOSContent += line + "\n";
                }

                if (line.Contains("</androidPackages>"))
                {
                    iOSContent += line;
                }
            }

            return iOSContent;
        }

        public static string GetAndroidDependencyName(string value)
        {
            var dependencyName = value.Replace(AppodealEditorConstants.ReplaceDepValue, string.Empty);
            var sub = dependencyName.Substring(0,
                dependencyName.LastIndexOf(":", StringComparison.Ordinal));
            return sub.Contains("@aar") ? sub.Substring(0, sub.LastIndexOf("@", StringComparison.Ordinal)) : sub;
        }

        public static string GetAndroidDependencyVersion(string value)
        {
            var androidDependencyVersion =
                value.Replace(AppodealEditorConstants.ReplaceDepValue + GetAndroidDependencyName(value) + ":", string.Empty);
            if (androidDependencyVersion.Contains("@aar"))
            {
                androidDependencyVersion = androidDependencyVersion.Substring(0,
                    androidDependencyVersion.LastIndexOf("@", StringComparison.Ordinal));
            }

            return androidDependencyVersion;
        }

        public static string GetMajorVersion(string value)
        {
            return value.Substring(0, 6).Remove(0, 5).Insert(0, string.Empty);
        }

        public static string GetAndroidDependencyCoreVersion(string value)
        {
            var androidDependencyVersion =
                value.Replace(AppodealEditorConstants.ReplaceDepCore, string.Empty);
            if (androidDependencyVersion.Contains("@aar"))
            {
                androidDependencyVersion = androidDependencyVersion.Substring(0,
                    androidDependencyVersion.LastIndexOf("@", StringComparison.Ordinal));
            }

            return androidDependencyVersion;
        }

        public static string ReplaceBetaVersion(string value)
        {
            return Regex.Replace(value, "-Beta", string.Empty);
        }

        public static void ReplaceInFile(
            string filePath, string searchText, string replaceText)
        {
            string contentString;
            using (var reader = new StreamReader(filePath))
            {
                contentString = reader.ReadToEnd();
                reader.Close();
            }

            contentString = Regex.Replace(contentString, searchText, replaceText);

            using (var writer = new StreamWriter(filePath))
            {
                writer.Write(contentString);
                writer.Close();
            }
        }

        public static int CompareVersion(string interal, string latest)
        {
            var xParts = interal.Split('.');
            var yParts = latest.Split('.');
            var partsLength = Math.Max(xParts.Length, yParts.Length);
            if (partsLength <= 0) return string.Compare(interal, latest, StringComparison.Ordinal);
            for (var i = 0; i < partsLength; i++)
            {
                if (xParts.Length <= i) return -1;
                if (yParts.Length <= i) return 1;
                var xPart = xParts[i];
                var yPart = yParts[i];
                if (string.IsNullOrEmpty(xPart)) xPart = "0";
                if (string.IsNullOrEmpty(yPart)) yPart = "0";
                if (!int.TryParse(xPart, out var xInt) || !int.TryParse(yPart, out var yInt))
                {
                    var abcCompare = string.Compare(xPart, yPart, StringComparison.Ordinal);
                    if (abcCompare != 0)
                        return abcCompare;
                    continue;
                }

                if (xInt != yInt) return xInt < yInt ? -1 : 1;
            }

            return 0;
        }

        public static void GuiHeaders(GUIStyle headerInfoStyle, GUILayoutOption btnFieldWidth)
        {
            using (new EditorGUILayout.HorizontalScope(GUILayout.ExpandWidth(false)))
            {
                GUILayout.Button(AppodealEditorConstants.PackageName, headerInfoStyle, GUILayout.Width(150));
                GUILayout.Space(25);
                GUILayout.Button(AppodealEditorConstants.CurrentVersionHeader, headerInfoStyle, GUILayout.Width(110));
                GUILayout.Space(90);
                GUILayout.Button(AppodealEditorConstants.LatestVersionHeader, headerInfoStyle);
                GUILayout.Button(AppodealEditorConstants.ActionHeader, headerInfoStyle, btnFieldWidth);
                GUILayout.Button(string.Empty, headerInfoStyle, GUILayout.Width(5));
            }
        }

        public static NetworkDependency GetAppodealDependency(
            SortedDictionary<string, NetworkDependency> networkDependencies)
        {
            NetworkDependency networkDependency = null;
            foreach (var dependency
                in networkDependencies.Where(dependency
                        => dependency.Key.Contains(AppodealEditorConstants.Appodeal))
                    .Where(dependency => dependency.Value != null))
            {
                networkDependency = dependency.Value;
            }

            return networkDependency;
        }
    }
}
