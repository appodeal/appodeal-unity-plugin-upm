using UnityEngine;
using UnityEngine.UIElements;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal class SdkTooltipPopupView
    {
        private const string TemplatePath = "Appodeal/DependencyManager/SdkTooltip/SdkTooltipPopup";

        private readonly string _sdkName;
        private readonly string _requirement;
        private readonly string _description;

        public VisualElement Root { get; private set; }

        public SdkTooltipPopupView(string sdkName, string requirement, string description)
        {
            _sdkName = sdkName;
            _requirement = requirement;
            _description = description;
        }

        public bool TryLoadFromTemplate()
        {
            var template = Resources.Load<VisualTreeAsset>(TemplatePath);
            if (template == null)
            {
                LogHelper.LogError($"[SdkTooltipView] Failed to load template from Resources/{TemplatePath}");
                return false;
            }

            Root = template.Instantiate();

            var tooltip = Root.Q<VisualElement>(DmConstants.Uxml.SdkTooltip.Popup.ContentContainer);
            var sdkNameLabel = tooltip?.Q<Label>(DmConstants.Uxml.SdkTooltip.Popup.SdkName);
            var requirementLabel = tooltip?.Q<Label>(DmConstants.Uxml.SdkTooltip.Popup.SdkRequirementStatus);
            var descriptionLabel = tooltip?.Q<Label>(DmConstants.Uxml.SdkTooltip.Popup.SdkDescription);

            if (tooltip == null || sdkNameLabel == null || requirementLabel == null || descriptionLabel == null)
            {
                LogHelper.LogError("[SdkTooltipView] Failed to find required elements in template");
                return false;
            }

            sdkNameLabel.text = _sdkName;
            requirementLabel.text = _requirement;
            descriptionLabel.text = _description;

            Root.pickingMode = PickingMode.Ignore;
            tooltip.pickingMode = PickingMode.Ignore;

            return true;
        }

        public void SetPosition(float x, float y)
        {
            Root.style.position = Position.Absolute;
            Root.style.left = x;
            Root.style.top = y;
        }
    }
}
