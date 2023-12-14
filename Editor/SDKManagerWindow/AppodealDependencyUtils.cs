using UnityEditor;
using UnityEngine;
using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using AppodealStack.UnityEditor.Utils;
using AppodealStack.UnityEditor.SDKManager.Models;

// ReSharper Disable CheckNamespace
namespace AppodealStack.UnityEditor.SDKManager
{
    public static class AppodealDependencyUtils
    {
        public static FileInfo[] GetInternalDependencyPath()
        {
            string path = Path.Combine(AppodealEditorConstants.PluginPath, AppodealEditorConstants.DependenciesPath);
            if (!Directory.Exists(path)) return null;

            var info = new DirectoryInfo(path);
            var fileInfo = info.GetFiles();
            return fileInfo.Length <= 0 ? null : fileInfo.Where(val => val.Name.EndsWith("Dependencies.xml")).ToArray();
        }

        public static void ShowInternalErrorDialog(EditorWindow editorWindow, string message, string debugLog)
        {
            EditorUtility.ClearProgressBar();
            Debug.LogError(message);
            bool option = EditorUtility.DisplayDialog("Internal error", $"{message}. Please contact Appodeal support.", "Ok");
            if (option) editorWindow.Close();
        }

        public static void ShowInternalErrorDialog(EditorWindow editorWindow, string message)
        {
            EditorUtility.ClearProgressBar();
            Debug.LogError(message);
            bool option = EditorUtility.DisplayDialog("Internal error", $"{message}.", "Ok");
            if (option) editorWindow.Close();
        }

        public static void FormatXml(string inputXml)
        {
            var document = new XmlDocument();
            document.Load(inputXml);

            using var writer = new XmlTextWriter(inputXml, Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 4;
            document.Save(writer);
        }

        public static string GetConfigName(string value)
        {
            return Regex.Match(value, @"[\\/](?!.*[\\/])(.*)Dependencies.xml").Groups[1].Value;
        }

        public static string GetIosContent(string path)
        {
            string iosContent = String.Empty;
            var lines = File.ReadAllLines(path);
            foreach (string line in lines)
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

                if (line.Contains("<sources>"))
                {
                    iosContent += line + "\n";
                }

                if (line.Contains("<source>"))
                {
                    iosContent += line + "\n";
                }

                if (line.Contains("</sources>"))
                {
                    iosContent += line + "\n";
                }

                if (line.Contains("</iosPod>"))
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
            string androidContent = String.Empty;
            var lines = File.ReadAllLines(path);
            foreach (string line in lines)
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
            return value.Substring(value.IndexOf(':') + 1, value.LastIndexOf(':') - value.IndexOf(':') - 1);
        }

        public static string GetAndroidDependencyVersion(string value)
        {
            string androidDependencyVersion = value.Substring(value.LastIndexOf(':') + 1);
            if (androidDependencyVersion.Contains("@aar"))
            {
                androidDependencyVersion = androidDependencyVersion.Substring(0, androidDependencyVersion.IndexOf('@'));
            }

            return androidDependencyVersion;
        }

        public static string GetMajorVersion(string value)
        {
            return value[0].ToString();
        }

        public static string GetAndroidDependencyCoreVersion(string value)
        {
            string androidDependencyVersion = value.Replace(AppodealEditorConstants.ReplaceDepCore, String.Empty);
            if (androidDependencyVersion.Contains("@aar"))
            {
                androidDependencyVersion = androidDependencyVersion.Substring(0, androidDependencyVersion.IndexOf('@'));
            }

            return androidDependencyVersion;
        }

        public static string ReplaceBetaVersion(string value)
        {
            return Regex.Replace(value, "-Beta", String.Empty);
        }

        public static void ReplaceInFile(string filePath, string searchText, string replaceText)
        {
            string contentString;
            using (var reader = new StreamReader(filePath))
            {
                contentString = reader.ReadToEnd();
                reader.Close();
            }

            contentString = Regex.Replace(contentString.Replace("\r", ""), searchText, replaceText);

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
            return dependencies.FirstOrDefault(dep => dep.Key.Contains(AppodealEditorConstants.Appodeal) && dep.Value != null).Value;
        }
    }
}
