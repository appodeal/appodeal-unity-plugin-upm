using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor.PackageManager;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal static class PackageVersionProvider
    {
        public static async Task<Outcome<string>> TryLookupVersionAsync(CancellationToken cancellationToken = default)
        {
            if (String.IsNullOrWhiteSpace(DmConstants.Plugin.PackageName))
            {
                return Failure.Create("WrongPackageName", $"'{DmConstants.Plugin.PackageName}' is an invalid package name");
            }

            try
            {
                var request = Client.List(true);
                while(!request.IsCompleted)
                {
                    if (cancellationToken.IsCancellationRequested) return Failure.Create("Cancelled", "Operation was cancelled");
                    await Task.Yield();
                }
                string version = request.Result?.FirstOrDefault(el => el.name == DmConstants.Plugin.PackageName)?.version;
                return version == null ? Failure.Create("VersionNotFound", $"{nameof(version)} variable value cannot be null") : version;
            }
            catch (Exception ex)
            {
                return Failure.Create(ex.GetType().ToString(), ex.Message);
            }
        }
    }
}
