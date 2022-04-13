using UnityEditor;
using UnityEngine;
using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using AppodealStack.UnityEditor.Utils;
using AppodealStack.UnityEditor.SDKManager.Models;

// ReSharper Disable CheckNamespace
namespace AppodealStack.UnityEditor.SDKManager
{
    public static class AppodealDependencyUtils
    {
        public static FileInfo[] GetInternalDependencyPath()
        {
            if (String.IsNullOrEmpty(AppodealEditorConstants.PluginPath) ||
                String.IsNullOrEmpty(AppodealEditorConstants.DependenciesPath))
            {
                return null;
            }

            var path = Path.Combine(AppodealEditorConstants.PluginPath, AppodealEditorConstants.DependenciesPath);
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
            var configName = value.Replace($"{AppodealEditorConstants.PluginPath}/{AppodealEditorConstants.DependenciesPath}/", string.Empty);
            return configName.Replace("Dependencies.xml", string.Empty);
        }

        public static string GetIosContent(string path)
        {
            var iosContent = string.Empty;
            var lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                if (String.IsNullOrEmpty(line)) continue;

                if (line.Contains("<iosPods>"))
                {
                    iosContent += line + "\n";
                }

                if (line.Contains("<iosPod name="))
                {
                    iosContent += line + "\n";
                }

                if (line.Contains("</iosPods>"))
                {
                    iosContent += line;
                }
            }

            return iosContent;
        }

        public static string GetAndroidContent(string path)
        {
            var androidContent = string.Empty;
            var lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                if (String.IsNullOrEmpty(line)) continue;

                if (line.Contains("<androidPackages>"))
                {
                    androidContent += line + "\n";
                }

                if (line.Contains("<androidPackage spec="))
                {
                    androidContent += line + "\n";
                }

                if (line.Contains("<repositories>"))
                {
                    androidContent += line + "\n";
                }

                if (line.Contains("<repository>"))
                {
                    androidContent += line + "\n";
                }

                if (line.Contains("</repositories>"))
                {
                    androidContent += line + "\n";
                }

                if (line.Contains("</androidPackages>"))
                {
                    androidContent += line;
                }
            }

            return androidContent;
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
            //return value.Substring(0, 6).Remove(0, 5).Insert(0, string.Empty);
            return value[0].ToString();
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

        public static int CompareVersion(string current, string latest)
        {
            var xParts = current.Split('.');
            var yParts = latest.Split('.');
            var partsLength = Math.Max(xParts.Length, yParts.Length);
            if (partsLength <= 0) return string.Compare(current, latest, StringComparison.Ordinal);
            for (var i = 0; i < partsLength; i++)
            {
                if (xParts.Length <= i) return -1;
                if (yParts.Length <= i) return 1;
                var xPart = xParts[i];
                var yPart = yParts[i];
                if (String.IsNullOrEmpty(xPart)) xPart = "0";
                if (String.IsNullOrEmpty(yPart)) yPart = "0";
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

        public static AppodealDependency GetAppodealDependency(SortedDictionary<string, AppodealDependency> dependencies)
        {
            return dependencies.First(dep => dep.Key.Contains(AppodealEditorConstants.Appodeal) && dep.Value != null).Value;
        }
    }
}
