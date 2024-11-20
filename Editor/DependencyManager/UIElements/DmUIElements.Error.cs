// ReSharper disable CheckNamespace

using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal static partial class DmUIElements
    {
        private static VisualTreeAsset _errorVta;

        public static VisualElement CreateErrorUI()
        {
            VisualElement error = GetErrorAsset().Instantiate();

            error.Q<Button>(DmConstants.Uxml.Error.ReloadButton).clicked += () =>
            {
                LogHelper.Log($"{DmConstants.Uxml.Error.ReloadButton}() is clicked");

                SettingsService.OpenProjectSettings($"Project/{DmConstants.SettingsProviderWindowName}");
            };
            error.Q<Button>(DmConstants.Uxml.Error.ContactSupportButton).clicked += () =>
            {
                LogHelper.Log($"{DmConstants.Uxml.Error.ContactSupportButton}() is clicked");

                Application.OpenURL(DmConstants.ContactSupportUrl);
            };

            return error;
        }

        private static VisualTreeAsset GetErrorAsset()
        {
            return _errorVta ??= AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(DmConstants.ErrorUxmlPath);
        }
    }
}
