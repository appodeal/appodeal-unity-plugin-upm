using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using AppodealInc.Mediation.Shared.Editor;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal static class DependencyValidationService
    {
        private static readonly AppodealLogger Log = DmLogging.Log;

        public static async Task<bool> ValidateAsync(bool isManual)
        {
            bool versionMatches = VersionComparer.IsLocalDependenciesVersionMatchingPackageVersion();
            if (!versionMatches)
            {
                Log.Warning("Skipping validation due to version mismatch or error");
                return false;
            }

            var depsReadOutcome = DataLoader.TryReadLocalDependencies();
            if (!depsReadOutcome.IsSuccess)
            {
                Log.Error($"Failed to read dependencies file. Reason - '{depsReadOutcome.Failure.Message}'");
                return false;
            }

            var serializationOutcome = XmlHandler.TrySerializeDependencies(depsReadOutcome.Value);
            if (!serializationOutcome.IsSuccess)
            {
                Log.Error($"Failed to serialize dependencies to XML. Reason - '{serializationOutcome.Failure.Message}'");
                return false;
            }

            var validationOutcome = await DataLoader.ValidateDependenciesAsync(serializationOutcome.Value);
            if (!validationOutcome.IsSuccess)
            {
                Log.Error($"Dependencies validation failed. Reason - '{validationOutcome.Failure.Message}'");
                if (isManual)
                {
                    EditorUtility.DisplayDialog(DmConstants.UI.DialogTitle, $"Validation failed: {validationOutcome.Failure.Message}", "OK");
                }
                return false;
            }

            var validationResponse = validationOutcome.Value;
            if (!validationResponse.HasProblematicSdks)
            {
                if (isManual)
                {
                    EditorUtility.DisplayDialog(DmConstants.UI.DialogTitle, "All dependencies are valid!", "OK");
                }
                return true;
            }

            LogValidationResults(validationResponse.ProblematicSdks);
            ShowValidationResultDialog(validationResponse.ProblematicSdks);
            return true;
        }

        private static void LogValidationResults(List<ProblematicSdk> problematicSdks)
        {
            Log.Warning("Dependencies validation found the following issues:");
            foreach (var sdk in problematicSdks)
            {
                Log.Warning($"'{sdk.SdkName}': {sdk.Message}");
            }
        }

        private static void ShowValidationResultDialog(List<ProblematicSdk> problematicSdks)
        {
            string sdkNames = String.Join("\n", problematicSdks.Select(s => s.SdkName));
            string message = $"The following dependencies have issues:\n{sdkNames}\n\nSee Console for detailed information";
            EditorUtility.DisplayDialog(DmConstants.UI.DialogTitle, message, "OK");
        }
    }
}
