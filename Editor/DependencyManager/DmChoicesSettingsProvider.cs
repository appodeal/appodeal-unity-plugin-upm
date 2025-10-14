// ReSharper disable CheckNamespace

using UnityEditor;
using UnityEngine.UIElements;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal class DmChoicesSettingsProvider : SettingsProvider
    {
        private DmChoicesSettingsProvider(string path, SettingsScope scope = SettingsScope.User) : base(path, scope) {}

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            LogHelper.Log($"{nameof(OnActivate)}() method is called");

            if (DmChoicesScriptableObject.Instance == null)
            {
                LogHelper.LogError("DM Settings window cannot be displayed as its data asset failed to load.");
                return;
            }

            if (!DmUIElements.IsSettingsAssetLoadable())
            {
                LogHelper.LogError("DM Settings window cannot be displayed as its visual asset failed to load.");
                return;
            }

            var settings = DmUIElements.CreateSettingsUI();
            rootElement.Add(settings);
        }

        public override void OnDeactivate()
        {
            LogHelper.Log($"{nameof(OnDeactivate)}() method is called");

            DmChoicesScriptableObject.SaveAsync();
        }

        [SettingsProvider]
        public static SettingsProvider CreateDmChoicesSettingsProvider()
        {
            return new DmChoicesSettingsProvider($"Project/{DmConstants.SettingsProviderWindowName}/Settings", SettingsScope.Project) { label = "Settings" };
        }
    }
}
