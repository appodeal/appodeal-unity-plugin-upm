using UnityEditor;
using UnityEngine.UIElements;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal class DmChoicesSettingsProvider : SettingsProvider
    {
        private SettingsScreenController _settingsController;

        private VisualElement _rootElement;

        private DmChoicesSettingsProvider(string path, SettingsScope scope = SettingsScope.User) : base(path, scope) { }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            if (DmChoicesScriptableObject.Instance == null)
            {
                LogHelper.LogError("DM Settings window cannot be displayed as its data asset failed to load");
                return;
            }

            _rootElement = rootElement;
            _rootElement?.Clear();

            _settingsController = new SettingsScreenController();
            if (!_settingsController.TryInitialize())
            {
                LogHelper.LogError("DM Settings window cannot be displayed as its visual asset failed to load");
                return;
            }

            _rootElement?.Add(_settingsController?.Root);
        }

        public override void OnDeactivate()
        {
            _settingsController?.Dispose();
            _settingsController = null;

            _rootElement?.Clear();
            _rootElement = null;

            DmChoicesScriptableObject.SaveToDisk();
        }

        [SettingsProvider]
        public static SettingsProvider CreateDmChoicesSettingsProvider()
        {
            return new DmChoicesSettingsProvider($"Project/{DmConstants.UI.WindowName}/Settings", SettingsScope.Project) { label = "Settings" };
        }
    }
}
