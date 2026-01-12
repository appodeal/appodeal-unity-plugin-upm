using UnityEngine.UIElements;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal interface ISdkCardView
    {
        VisualElement Root { get; }
        SdkCardShell Shell { get; }
        bool TryLoadFromTemplate();
    }
}
