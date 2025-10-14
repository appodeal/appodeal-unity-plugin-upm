// ReSharper disable CheckNamespace

using System.IO;
using UnityEditor;
using UnityEngine;
using AppodealInc.Mediation.Utils.Editor;

namespace AppodealInc.Mediation.AssetExtractors.Editor
{
    internal static class AndroidLibraryInstaller
    {
        public static bool InstallAndroidLibrary()
        {
            if (Directory.Exists(AppodealEditorConstants.AppodealAndroidLibDir)) return false;

            string source = $"{AppodealEditorConstants.PackageDir}/Runtime/Plugins/Android/appodeal.androidlib~";
            if (!Directory.Exists(source))
            {
                Debug.LogError($"[Appodeal] Directory was not found: '{source}'. Please, contact support@apppodeal.com about this issue.");
                return false;
            }

            Directory.CreateDirectory("Assets/Plugins/Android");
            FileUtil.CopyFileOrDirectory(source, AppodealEditorConstants.AppodealAndroidLibDir);

            return true;
        }
    }
}
