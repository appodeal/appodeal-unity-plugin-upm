// ReSharper disable CheckNamespace

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;
using AppodealInc.Mediation.Analytics.Editor;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal class DmSettingsProvider : SettingsProvider
    {
        private Guid _sessionId;
        private DmConfigDto _dmConfig;

        private DmSettingsProvider(string path, SettingsScope scope = SettingsScope.User) : base(path, scope) {}

        public override async void OnActivate(string searchContext, VisualElement rootElement)
        {
            AnalyticsService.TrackClickEvent(ActionType.OpenDependencyManager);
            LogHelper.Log($"{nameof(OnActivate)}() method is called");

            if (DmChoicesScriptableObject.Instance == null)
            {
                LogHelper.LogError("DM window cannot be displayed as its data asset failed to load.");
                return;
            }

            if (!DmUIElements.AreAllDmAssetsLoadable())
            {
                LogHelper.LogError("DM window cannot be displayed as some of the visual assets failed to load.");
                return;
            }

            _sessionId = Guid.NewGuid();

            var loading = DmUIElements.CreateLoadingUI();
            rootElement.Add(loading);

            var (sessionId, configRequest) = await DataLoader.GetConfigAsync(_sessionId);
            if (sessionId != _sessionId) return;

            rootElement.Remove(loading);

            if (configRequest.IsSuccess) _dmConfig = configRequest.Value;
            else
            {
                var error = DmUIElements.CreateErrorUI();
                rootElement.Add(error);
                LogHelper.LogError($"Data collection failed. Reason - '{configRequest.Error.Message}'.");
                return;
            }

            var tooltip = DmUIElements.CreateTooltipUI(rootElement);
            var dm = DmUIElements.CreateDependencyManagerUI(rootElement, tooltip, _dmConfig);

            rootElement.Add(dm);
            rootElement.Add(tooltip);

            tooltip.style.display = DisplayStyle.None;
            tooltip.style.position = Position.Absolute;
        }

        public override void OnDeactivate()
        {
            AnalyticsService.TrackClickEvent(ActionType.CloseDependencyManager);
            LogHelper.Log($"{nameof(OnDeactivate)}() method is called");

            _sessionId = Guid.Empty;
            _dmConfig = null;
        }

        [SettingsProvider]
        public static SettingsProvider CreateDmSettingsProvider()
        {
            var provider = new DmSettingsProvider($"Project/{DmConstants.SettingsProviderWindowName}", SettingsScope.Project)
            {
                label = "Appodeal DM",
                keywords = new HashSet<string>(new[] { "Appodeal", "Dependency", "Manager", "DM" })
            };
            return provider;
        }
    }
}
