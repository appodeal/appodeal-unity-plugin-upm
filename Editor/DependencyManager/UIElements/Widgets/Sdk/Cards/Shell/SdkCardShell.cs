using UnityEngine;
using UnityEngine.UIElements;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
#if UNITY_6000_0_OR_NEWER
    [UxmlElement]
#else
    // ReSharper disable once PartialTypeWithSinglePart
#endif
    internal partial class SdkCardShell : VisualElement
    {
        private const string CommonStyleSheetPath = "Appodeal/DependencyManager/SdkCards/SdkCardsCommon";

#if !UNITY_6000_0_OR_NEWER
        public new class UxmlFactory : UxmlFactory<SdkCardShell> { }
#endif

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
