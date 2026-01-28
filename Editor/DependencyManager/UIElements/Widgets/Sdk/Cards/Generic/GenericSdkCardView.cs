using UnityEngine;
using UnityEngine.UIElements;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal class GenericSdkCardView : ISdkCardView
    {
        private const string TemplatePath = "Appodeal/DependencyManager/SdkCards/GenericSdkCardTemplate";

        private VisualElement _sdkIcon;

        public VisualElement Root { get; private set; }

        public SdkCardShell Shell { get; private set; }

        public bool TryLoadFromTemplate()
        {
            var template = Resources.Load<VisualTreeAsset>(TemplatePath);
            if (template == null)
            {
                LogHelper.LogError($"[GenericSdkCardView] Failed to load template from Resources/{TemplatePath}");
                return false;
            }

            Root = template.Instantiate();

            Shell = Root.Q<SdkCardShell>(DmConstants.Uxml.SdkCards.Generic.ShellName);
            if (Shell == null)
            {
                LogHelper.LogError("[GenericSdkCardView] SdkCardShell not found in template");
                return false;
            }

            _sdkIcon = Root.Q<VisualElement>(DmConstants.Uxml.SdkCards.Generic.SdkIcon);

            return true;
        }

        public void SetSdkIconTexture(Texture2D texture)
        {
            if (_sdkIcon != null && texture != null)
            {
                _sdkIcon.style.backgroundImage = new StyleBackground(texture);
            }
        }
    }
}
