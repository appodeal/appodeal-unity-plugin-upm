using UnityEngine;
using UnityEngine.UIElements;
using AppodealInc.Mediation.Shared.Editor;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal class GenericSdkCardView : ISdkCardView
    {
        private static readonly AppodealLogger Log = DmLogging.Log;

        private const string TemplatePath = "Appodeal/DependencyManager/SdkCards/GenericSdkCardTemplate";

        private VisualElement _sdkIcon;

        public VisualElement Root { get; private set; }

        public SdkCardShell Shell { get; private set; }

        public bool TryLoadFromTemplate()
        {
            var template = Resources.Load<VisualTreeAsset>(TemplatePath);
            if (template == null)
            {
                Log.Error($"[GenericSdkCardView] Failed to load template from Resources/{TemplatePath}");
                return false;
            }

            Root = template.Instantiate();

            Shell = Root.Q<SdkCardShell>(DmConstants.Uxml.SdkCards.Generic.ShellName);
            if (Shell == null)
            {
                Log.Error("[GenericSdkCardView] SdkCardShell not found in template");
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
