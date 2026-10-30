using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using UnityEditor.Build;
using AppodealInc.Mediation.Shared.Editor;

namespace AppodealInc.Mediation.PreProcess.Editor
{
    internal class AndroidLibrary
    {
        private static readonly AppodealLogger Log = PreProcessLogging.Log;

        private const string Name = "appodeal.androidlib";
        private const string ManifestFile = "AndroidManifest.xml";
        private const string ValuesDir = "res/values";
        private const string GoogleServicesFile = ValuesDir + "/google-services.xml";

        private static readonly XmlWriterSettings WriterSettings = new()
        {
            Encoding = new UTF8Encoding(false),
            Indent = true,
            IndentChars = "  ",
            NewLineChars = "\n",
            NewLineHandling = NewLineHandling.Replace,
        };

        private readonly string _dir;

        public AndroidManifest Manifest { get; }
        public StringResources GoogleServices { get; } = new();

        private AndroidLibrary(string dir)
        {
            _dir = dir;
            Manifest = new AndroidManifest(XDocument.Load($"{dir}/{ManifestFile}"));
        }

        public static AndroidLibrary Open(string unityLibraryPath)
        {
            string dir = $"{unityLibraryPath}/{Name}";
            string manifestPath = $"{dir}/{ManifestFile}";

            // Check the manifest, not the folder, because the folder can outlive an excluded library
            if (!File.Exists(manifestPath))
            {
                Log.Error($"Manifest file '{manifestPath}' is not found. The build can't be configured.");
                throw new BuildFailedException("Appodeal Android manifest file is not found.");
            }

#if !UNITY_2023_1_OR_NEWER
            // Unity 2022.3 leaves the settings from the previous build in these files, so copy them from the package again
            string templateDir = $"{PluginPaths.PackageDir}/Runtime/Plugins/Android/{Name}";
            foreach (string file in Directory.GetFiles(templateDir, "*", SearchOption.AllDirectories))
            {
                string relativePath = Path.GetRelativePath(templateDir, file).Replace('\\', '/');
                File.Copy(file, $"{dir}/{relativePath}", true);
            }
#endif

            return new AndroidLibrary(dir);
        }

        public void Save()
        {
            Write(Manifest.Document, $"{_dir}/{ManifestFile}");

            string valuesDir = $"{_dir}/{ValuesDir}";
            string googleServicesPath = $"{_dir}/{GoogleServicesFile}";

            if (GoogleServices.IsEmpty)
            {
                // Not in the package template, so Unity never removes it: delete it and its folder, or a stale copy reaches the app
                if (File.Exists(googleServicesPath)) File.Delete(googleServicesPath);
                if (Directory.Exists(valuesDir) && !Directory.EnumerateFileSystemEntries(valuesDir).Any()) Directory.Delete(valuesDir);
            }
            else
            {
                Directory.CreateDirectory(valuesDir);
                Write(GoogleServices.Document, googleServicesPath);
            }
        }

        private static void Write(XDocument document, string path)
        {
            using var writer = XmlWriter.Create(path, WriterSettings);
            document.Save(writer);
        }
    }
}
