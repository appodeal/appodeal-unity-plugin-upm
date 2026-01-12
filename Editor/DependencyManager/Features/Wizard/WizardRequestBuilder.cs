using System.Collections.Generic;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal static class WizardRequestBuilder
    {
        public static WizardRequestDto BuildComponentsRequest(IEnumerable<ISdkCardController> mediationControllers)
        {
            var request = new WizardRequestDto
            {
                android = new PlatformSelectionDto { mediations = new List<int>() },
                ios = new PlatformSelectionDto { mediations = new List<int>() }
            };

            foreach (var ctrl in mediationControllers)
            {
                if (!ctrl.IsSelected) continue;
                (int? androidId, int? iosId) = GetSelectedVersionIds(ctrl);
                if (androidId.HasValue) request.android.mediations.Add(androidId.Value);
                if (iosId.HasValue) request.ios.mediations.Add(iosId.Value);
            }

            return request;
        }

        public static WizardRequestDto BuildDependenciesRequest(IEnumerable<ISdkCardController> mediationControllers,
                                                                IEnumerable<ISdkCardController> adNetworkControllers,
                                                                IEnumerable<ISdkCardController> serviceControllers)
        {
            var request = new WizardRequestDto
            {
                android = new PlatformSelectionDto
                {
                    mediations = new List<int>(),
                    networks = new List<int>(),
                    services = new List<int>()
                },
                ios = new PlatformSelectionDto
                {
                    mediations = new List<int>(),
                    networks = new List<int>(),
                    services = new List<int>()
                }
            };

            foreach (var ctrl in mediationControllers)
            {
                if (!ctrl.IsSelected) continue;
                (int? androidId, int? iosId) = GetSelectedVersionIds(ctrl);
                if (androidId.HasValue) request.android.mediations.Add(androidId.Value);
                if (iosId.HasValue) request.ios.mediations.Add(iosId.Value);
            }

            foreach (var ctrl in adNetworkControllers)
            {
                if (!ctrl.IsSelected) continue;
                (int? androidId, int? iosId) = GetSelectedVersionIds(ctrl);
                if (androidId.HasValue) request.android.networks.Add(androidId.Value);
                if (iosId.HasValue) request.ios.networks.Add(iosId.Value);
            }

            foreach (var ctrl in serviceControllers)
            {
                if (!ctrl.IsSelected) continue;
                (int? androidId, int? iosId) = GetSelectedVersionIds(ctrl);
                if (androidId.HasValue) request.android.services.Add(androidId.Value);
                if (iosId.HasValue) request.ios.services.Add(iosId.Value);
            }

            return request;
        }

        public static List<SdkSelectionState> BuildSelectionStates(IEnumerable<ISdkCardController> mediationControllers,
                                                                   IEnumerable<ISdkCardController> adNetworkControllers,
                                                                   IEnumerable<ISdkCardController> serviceControllers)
        {
            var states = new List<SdkSelectionState>();

            AddSelectionStates(states, mediationControllers);
            AddSelectionStates(states, adNetworkControllers);
            AddSelectionStates(states, serviceControllers);

            return states;
        }

        public static bool HasMediationSelections(WizardRequestDto request)
        {
            return request?.android?.mediations?.Count > 0 || request?.ios?.mediations?.Count > 0;
        }

        private static (int? androidId, int? iosId) GetSelectedVersionIds(ISdkCardController controller)
        {
            return (controller.SelectedAndroidVersion?.Id, controller.SelectedIosVersion?.Id);
        }

        private static void AddSelectionStates(List<SdkSelectionState> states, IEnumerable<ISdkCardController> controllers)
        {
            foreach (var ctrl in controllers)
            {
                if (!ctrl.HasAnyVersions) continue;

                states.Add(new SdkSelectionState
                {
                    sdkId = ctrl.SdkId,
                    isSelected = ctrl.IsSelected
                });
            }
        }
    }
}
