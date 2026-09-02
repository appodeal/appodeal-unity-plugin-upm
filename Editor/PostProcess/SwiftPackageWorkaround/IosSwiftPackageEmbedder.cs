// ReSharper disable CheckNamespace

using System.IO;
using UnityEditor.iOS.Xcode;
using AppodealInc.Mediation.Utils.Editor;

namespace AppodealInc.Mediation.PostProcess.Editor
{
    /// <summary>
    /// <para>
    /// EDM4U links Swift Package products into UnityFramework and leaves two things undone: the
    /// '-ObjC' flag on that target, and copying dynamic frameworks and resource bundles into the
    /// '.app'. Xcode copies them on its own only for products linked against an application target,
    /// and linking them there as well would put every static class into both binaries.
    /// </para>
    /// <para>
    /// Upstream: https://github.com/googlesamples/unity-jar-resolver/issues/779. Once fixed there,
    /// delete this folder and the call in AppodealPostProcess.
    /// </para>
    /// </summary>
    internal static class IosSwiftPackageEmbedder
    {
        private const string BuildPhaseName = "[Appodeal] Embed Swift Package Artifacts";
        private const string ScriptName = "appodeal_embed_swift_packages.sh";
        private const string BundledScriptPath = AppodealEditorConstants.PackageDir + "/Editor/PostProcess/SwiftPackageWorkaround/" + ScriptName;

        internal static void Apply(string buildPath)
        {
            string projectPath = PBXProject.GetPBXProjectPath(buildPath);
            string contents = File.ReadAllText(projectPath);

            // EDM4U adds packages at build order 35, ahead of AppodealPostProcess. A CocoaPods
            // project gets the equivalent from 'pod install'.
            if (!contents.Contains("XCRemoteSwiftPackageReference")) return;

            // An append build keeps the previous copy.
            File.Copy(BundledScriptPath, Path.Combine(buildPath, ScriptName), overwrite: true);

            var project = new PBXProject();
            project.ReadFromString(contents);

            AddObjCLinkerFlag(project);
            AddEmbedBuildPhase(project);

            project.WriteToFile(projectPath);
        }

        /// <summary>
        /// <para>
        /// Static libraries only contribute the object files someone references, so Objective-C
        /// categories are dropped unless the linker is told to load them. The SDK is linked into
        /// UnityFramework, so the flag goes there. A pod brings it in its xcconfig; a Swift Package
        /// has no equivalent.
        /// </para>
        /// <para>
        /// Unity already sets OTHER_LDFLAGS on UnityFramework, so the flag is appended, not set.
        /// </para>
        /// </summary>
        private static void AddObjCLinkerFlag(PBXProject project)
        {
            project.AddBuildProperty(project.GetUnityFrameworkTargetGuid(), "OTHER_LDFLAGS", "-ObjC");
        }

        /// <summary>
        /// <para>
        /// Appended after Unity's phases. Position does not matter: the '.app' is signed after every
        /// phase, and that is the only step the copied artifacts have to precede.
        /// </para>
        /// <para>
        /// The artifacts are only known at build time, so the phase declares no outputs and Xcode
        /// warns that it runs on every build. Unity's own il2cpp phase carries the same warning.
        /// </para>
        /// </summary>
        private static void AddEmbedBuildPhase(PBXProject project)
        {
            project.AddShellScriptBuildPhase(project.GetUnityMainTargetGuid(), BuildPhaseName, "/bin/sh",
                                             $"sh \"${{PROJECT_DIR}}/{ScriptName}\"\n");
        }
    }
}
