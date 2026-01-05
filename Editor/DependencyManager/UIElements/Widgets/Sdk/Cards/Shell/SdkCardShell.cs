using UnityEngine;
using UnityEngine.UIElements;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal class SdkCardShell : VisualElement
    {
        private const string CommonStyleSheetPath = "Appodeal/DependencyManager/SdkCards/SdkCardsCommon";

        public new class UxmlFactory : UxmlFactory<SdkCardShell> { }

        public SdkCardShellView View { get; }

        public override VisualElement contentContainer => View?.Body ?? this;

        public SdkCardShell()
        {
            View = new SdkCardShellView();

            if (!View.TryLoadFromTemplate())
            {
                LogHelper.LogError("[SdkCardShell] Failed to initialize View");
                return;
            }

            var styleSheet = Resources.Load<StyleSheet>(CommonStyleSheetPath);
            if (styleSheet != null)
            {
                styleSheets.Add(styleSheet);
            }

            hierarchy.Add(View.Root);
        }

        public void SetPlatformDisabled(Platform platform, bool disabled)
        {
            string className = platform == Platform.Android
                ? DmConstants.Uss.SdkCards.Common.AndroidDisabledModifier
                : DmConstants.Uss.SdkCards.Common.IosDisabledModifier;

            EnableInClassList(className, disabled);
        }
    }
}
