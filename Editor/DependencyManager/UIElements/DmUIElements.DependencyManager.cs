using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

// ReSharper disable once CheckNamespace
namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal static partial class DmUIElements
    {
        private static VisualTreeAsset _dependencyManagerVta;

        public static VisualElement CreateDependencyManagerUI(VisualElement rootElement, VisualElement tooltip, DmConfigDto config)
        {
            VisualElement dm = GetDependencyManagerAsset().Instantiate();
            PopulateDmWithSdkCards(dm, tooltip, config);
            SubscribeToGenerateButton(rootElement, dm, config);
            SubscribeToSelectionButtons(dm, config.RemotePluginConfig);

            return dm;
        }

        private static VisualTreeAsset GetDependencyManagerAsset()
        {
            return _dependencyManagerVta ??= AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(DmConstants.DependencyManagerUxmlPath);
        }

        private static void PopulateDmWithSdkCards(VisualElement dm, VisualElement tooltip, DmConfigDto config)
        {
            var mediationsRoot = dm.Q<VisualElement>(DmConstants.Uxml.Dm.MediationsContainer);
            var mediationSdksGrouped = config.RemotePluginConfig.GetGroupedSdks(SdkCategory.Mediation);
            mediationSdksGrouped.ForEach(sdkGroup => mediationsRoot.Add(CreateSdkCardElement(sdkGroup, tooltip)));

            var adNetworksRoot = dm.Q<VisualElement>(DmConstants.Uxml.Dm.AdNetworksContainer);
            var adNetworkSdksGrouped = config.RemotePluginConfig.GetGroupedSdks(SdkCategory.AdNetwork);

            adNetworkSdksGrouped.ForEach(sdkGroup =>
            {
                var adTypes = config.RemotePluginConfig.GetSupportedAdTypes(sdkGroup[0].status);
                var mediations = config.RemotePluginConfig.GetSupportedMediations(sdkGroup[0].status);
                adNetworksRoot.Add(CreateAdNetworkSdkCardElement(sdkGroup, adTypes, mediations, tooltip));
            });

            var servicesRoot = dm.Q<VisualElement>(DmConstants.Uxml.Dm.ServicesContainer);
            var serviceSdksGrouped = config.RemotePluginConfig.GetGroupedSdks(SdkCategory.Service);
            serviceSdksGrouped.ForEach(sdkGroup => servicesRoot.Add(CreateSdkCardElement(sdkGroup, tooltip)));
        }

        private static void SubscribeToGenerateButton(VisualElement rootElement, VisualElement dm, DmConfigDto config)
        {
            dm.Q<Button>(DmConstants.Uxml.Dm.GenerateButton).clicked += async () =>
            {
                LogHelper.Log($"{DmConstants.Uxml.Dm.GenerateButton}() is clicked");

                SaveSdkSelectionStates(rootElement);

                var urlRequest = await UrlBuilder.GetDependenciesUrlAsync(config.Plugins, config.RemotePluginConfig);
                if (!urlRequest.IsSuccess)
                {
                    ShowFailPopup(dm);
                    LogHelper.LogError($"Cannot update dependencies. Reason - '{urlRequest.Error.Message}'.");
                    return;
                }

                var remoteDepsRequest = await DataLoader.GetRemoteDependenciesAsync(urlRequest.Value);
                if (!remoteDepsRequest.IsSuccess)
                {
                    ShowFailPopup(dm);
                    LogHelper.LogError($"Cannot update dependencies. Reason - '{remoteDepsRequest.Error.Message}'.");
                    return;
                }

                var xmlRequest = XmlHandler.DeserializeDependencies(remoteDepsRequest.Value);
                if (xmlRequest.IsSuccess)
                {
                    if (XmlHandler.UpdateDependencies(xmlRequest.Value))
                    {
                        var diff = DependenciesDiff.Get(config.LocalDeps, xmlRequest.Value);
                        string message = $"{LogHelper.Colorize(Color.green, "Successfully")} updated dependencies.";
                        LogHelper.LogDepsUpdate(message, diff);

                        config.LocalDeps = xmlRequest.Value;

                        ShowSuccessPopup(dm);
                    }
                    else
                    {
                        ShowFailPopup(dm);
                    }
                }
                else
                {
                    LogHelper.LogError($"Cannot update dependencies. Reason - '{xmlRequest.Error.Message}'.");
                }
            };
        }

        private static void SubscribeToSelectionButtons(VisualElement dm, PluginConfigResponseModel config)
        {
            var sdks = config.GetMergedSdks();

            dm.Q<Button>(DmConstants.Uxml.Dm.SelectAllButton).clicked += () =>
            {
                LogHelper.Log($"{DmConstants.Uxml.Dm.SelectAllButton}() is clicked");

                dm.Query(className: DmConstants.Uss.SdkClassName).ForEach(ve =>
                {
                    var toggle = ve.Q<Toggle>(DmConstants.Uxml.SdkCard.IsSdkCardSelected);
                    if (toggle.enabledSelf) toggle.value = true;
                });
            };

            dm.Q<Button>(DmConstants.Uxml.Dm.SelectNoneButton).clicked += () =>
            {
                LogHelper.Log($"{DmConstants.Uxml.Dm.SelectNoneButton}() is clicked");

                dm.Query(className: DmConstants.Uss.SdkClassName).ForEach(ve =>
                {
                    var toggle = ve.Q<Toggle>(DmConstants.Uxml.SdkCard.IsSdkCardSelected);
                    if (toggle.enabledSelf) toggle.value = false;
                });
            };

            dm.Q<Button>(DmConstants.Uxml.Dm.SelectDefaultButton).clicked += () =>
            {
                LogHelper.Log($"{DmConstants.Uxml.Dm.SelectDefaultButton}() is clicked");

                dm.Query(className: DmConstants.Uss.SdkClassName).ForEach(ve =>
                {
                    var toggle = ve.Q<Toggle>(DmConstants.Uxml.SdkCard.IsSdkCardSelected);
                    if (!toggle.enabledSelf) return;

                    string sdkId = ve.Q<Label>(DmConstants.Uxml.SdkCard.SdkId).text;
                    bool isDefault = (sdks.FirstOrDefault(el => el.status == sdkId)?.requirement ?? Requirement.Unknown) == Requirement.Default;
                    toggle.value = isDefault;
                });
            };

            dm.Q<Button>(DmConstants.Uxml.Dm.SelectCustomButton).clicked += () =>
            {
                LogHelper.Log($"{DmConstants.Uxml.Dm.SelectCustomButton}() is clicked");

                dm.Query(className: DmConstants.Uss.SdkClassName).ForEach(ve =>
                {
                    var toggle = ve.Q<Toggle>(DmConstants.Uxml.SdkCard.IsSdkCardSelected);
                    if (!toggle.enabledSelf) return;

                    string sdkId = ve.Q<Label>(DmConstants.Uxml.SdkCard.SdkId).text;
                    if (DmChoicesScriptableObject.IsSdkSelectionStateKnown(sdkId, out var status))
                    {
                        toggle.value = status.isSelected;
                    }
                    else
                    {
                        bool isDefault = (sdks.FirstOrDefault(el => el.status == sdkId)?.requirement ?? Requirement.Unknown) == Requirement.Default;
                        toggle.value = isDefault;
                    }
                });
            };
        }

        private static void SaveSdkSelectionStates(VisualElement rootElement)
        {
            var output = new List<SdkSelectionStateModel>();

            rootElement.Query(className: DmConstants.Uss.SdkClassName).ForEach(ve =>
            {
                output.Add(new SdkSelectionStateModel
                {
                    sdkId = ve.Q<Label>(DmConstants.Uxml.SdkCard.SdkId).text,
                    isSelected = ve.Q<Toggle>(DmConstants.Uxml.SdkCard.IsSdkCardSelected).value
                });
            });

            DmChoicesScriptableObject.Instance.LastSyncSdkStates = output.OrderBy(el => el.sdkId).ToList();
            DmChoicesScriptableObject.SaveAsync();
        }

        private static void ShowSuccessPopup(VisualElement dm)
        {
            ShowPopup(dm, DmConstants.Uxml.Dm.SuccessStatusPopup);
        }

        private static void ShowFailPopup(VisualElement dm)
        {
            ShowPopup(dm, DmConstants.Uxml.Dm.FailStatusPopup);
        }

        private static async void ShowPopup(VisualElement dm, string popupName)
        {
            var popup = dm.Q<VisualElement>(popupName);
            popup.style.display = DisplayStyle.Flex;
            await Task.Delay(2000);
            popup.style.display = DisplayStyle.None;
        }
    }
}
