using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal static class DependencyValidationService
    {
        public static async Task<bool> ValidateAsync(bool isManual)
        {
            bool versionMatches = await VersionComparer.IsLocalDependenciesVersionMatchingPackageVersionAsync();
            if (!versionMatches)
            {
                LogHelper.LogWarning("Skipping validation due to version mismatch or error");
                return false;
            }

            var depsReadOutcome = DataLoader.TryReadLocalDependencies();
            if (!depsReadOutcome.IsSuccess)
            {
                LogHelper.LogError($"Failed to read dependencies file. Reason - '{depsReadOutcome.Failure.Message}'");
                return false;
            }

            var serializationOutcome = XmlHandler.TrySerializeDependencies(depsReadOutcome.Value);
            if (!serializationOutcome.IsSuccess)
            {
                LogHelper.LogError($"Failed to serialize dependencies to XML. Reason - '{serializationOutcome.Failure.Message}'");
                return false;
            }

            var validationOutcome = await DataLoader.ValidateDependenciesAsync(serializationOutcome.Value);
            if (!validationOutcome.IsSuccess)
            {
                LogHelper.LogError($"Dependencies validation failed. Reason - '{validationOutcome.Failure.Message}'");
                if (isManual)
                {
                    EditorUtility.DisplayDialog(DmConstants.UI.DialogTitle, $"Validation failed: {validationOutcome.Failure.Message}", "OK");
                }
                return true;
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
            LogHelper.LogWarning("Dependencies validation found the following issues:");
            foreach (var sdk in problematicSdks)
            {
                LogHelper.LogWarning($"'{sdk.SdkName}': {sdk.Message}");
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
