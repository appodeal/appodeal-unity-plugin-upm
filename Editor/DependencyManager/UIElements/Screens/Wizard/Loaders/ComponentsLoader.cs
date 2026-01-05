using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal static class ComponentsLoader
    {
        public static async Task<Outcome<(List<AdNetworkSdkCardController> adNetworks, List<GenericSdkCardController> services)>> TryLoadAsync(
            WizardRequestDto request,
            string pluginVersion,
            CancellationToken cancellationToken)
        {
            if (!WizardRequestBuilder.HasMediationSelections(request))
            {
                LogHelper.LogError("No mediations selected. Cannot load SDKs");
                return Failure.Create("ValidationError", "No mediations selected");
            }

            var fetchOutcome = await DataLoader.FetchComponentsAsync(request, pluginVersion, cancellationToken);
            if (cancellationToken.IsCancellationRequested) return Failure.Create("Cancelled", "Operation was cancelled");

            if (!fetchOutcome.IsSuccess)
            {
                LogHelper.LogError($"Failed to load SDKs: {fetchOutcome.Failure.Message}");
                return fetchOutcome.Failure;
            }

            var (adNetworks, services) = CreateControllers(fetchOutcome.Value);
            return (adNetworks, services);
        }

        private static (List<AdNetworkSdkCardController> adNetworks, List<GenericSdkCardController> services) CreateControllers(WizardResponse data)
        {
            var adNetworkControllers = new List<AdNetworkSdkCardController>();
            var serviceControllers = new List<GenericSdkCardController>();

            foreach (var (_, androidSdk, iosSdk) in data.GroupSdksByName())
            {
                var sdk = androidSdk ?? iosSdk;
                if (sdk.IsAdNetwork)
                {
                    var controller = CreateAdNetworkController(androidSdk, iosSdk);
                    if (controller != null) adNetworkControllers.Add(controller);
                }
                else if (sdk.IsService)
                {
                    var controller = CreateServiceController(androidSdk, iosSdk);
                    if (controller != null) serviceControllers.Add(controller);
                }
            }

            return (adNetworkControllers, serviceControllers);
        }

        private static AdNetworkSdkCardController CreateAdNetworkController(Sdk androidSdk, Sdk iosSdk)
        {
            var controller = new AdNetworkSdkCardController(androidSdk, iosSdk);
            if (controller.TryInitialize()) return controller;
            controller.Dispose();
            return null;
        }

        private static GenericSdkCardController CreateServiceController(Sdk androidSdk, Sdk iosSdk)
        {
            var controller = new GenericSdkCardController(androidSdk, iosSdk);
            if (controller.TryInitialize()) return controller;
            controller.Dispose();
            return null;
        }
    }
}
