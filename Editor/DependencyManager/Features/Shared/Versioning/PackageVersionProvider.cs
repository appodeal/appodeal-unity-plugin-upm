using System;
using UnityEditor.PackageManager;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal static class PackageVersionProvider
    {
        public static Outcome<string> TryLookupVersion()
        {
            try
            {
                var package = PackageInfo.FindForAssembly(typeof(PackageVersionProvider).Assembly);
                if (package == null) return Failure.Create("PackageNotFound", "Assembly is not part of a UPM package");

                string version = package.version;
                return String.IsNullOrEmpty(version) ? Failure.Create("VersionNotFound", $"{nameof(version)} variable value cannot be null") : version;
            }
            catch (Exception ex)
            {
                return Failure.Create(ex.GetType().ToString(), ex.Message);
            }
        }
    }
}
