// ReSharper disable CheckNamespace

using System.IO;
using UnityEditor;
using AppodealInc.Mediation.Utils.Editor;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal static class AndroidLibraryInstaller
    {
        public static bool EnsureAndroidLibraryDirectory(bool forceReinstall = false)
        {
            if (Directory.Exists(AppodealEditorConstants.AppodealAndroidLibDir))
            {
                if (!forceReinstall) return false;
                FileUtil.DeleteFileOrDirectory(AppodealEditorConstants.AppodealAndroidLibDir);
            }

            string source = $"{AppodealEditorConstants.PackageDir}/Runtime/Plugins/Android/appodeal.androidlib~";
            if (!Directory.Exists(source))
            {
                LogHelper.LogError($"Directory was not found: '{source}'. Please, contact support@appodeal.com about this issue");
                return false;
            }

            Directory.CreateDirectory("Assets/Plugins/Android");
            FileUtil.CopyFileOrDirectory(source, AppodealEditorConstants.AppodealAndroidLibDir);

            return true;
        }
    }
}
