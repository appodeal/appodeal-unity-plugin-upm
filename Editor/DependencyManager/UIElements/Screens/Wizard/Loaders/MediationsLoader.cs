using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal static class MediationsLoader
    {
        public static async Task<Outcome<List<GenericSdkCardController>>> TryLoadAsync(string pluginVersion, CancellationToken cancellationToken)
        {
            var fetchOutcome = await DataLoader.FetchMediationsAsync(pluginVersion, cancellationToken);
            if (cancellationToken.IsCancellationRequested) return Failure.Create("Cancelled", "Operation was cancelled");

            if (!fetchOutcome.IsSuccess)
            {
                LogHelper.LogError($"Failed to load mediations: {fetchOutcome.Failure.Message}");
                return fetchOutcome.Failure;
            }

            var controllers = CreateControllers(fetchOutcome.Value);
            return controllers;
        }

        private static List<GenericSdkCardController> CreateControllers(WizardResponse data)
        {
            var controllers = new List<GenericSdkCardController>();

            foreach (var (_, androidSdk, iosSdk) in data.GroupSdksByName())
            {
                var controller = new GenericSdkCardController(androidSdk, iosSdk);
                if (controller.TryInitialize())
                {
                    controllers.Add(controller);
                }
                else
                {
                    controller.Dispose();
                }
            }

            return controllers;
        }
    }
}
