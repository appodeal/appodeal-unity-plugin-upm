// ReSharper disable CheckNamespace

using UnityEditor;
using UnityEngine.UIElements;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal static partial class DmUIElements
    {
        private static VisualTreeAsset _loadingVta;

        public static VisualElement CreateLoadingUI()
        {
            return GetLoadingAsset().Instantiate();
        }

        private static VisualTreeAsset GetLoadingAsset()
        {
            return _loadingVta ??= AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(DmConstants.LoadingUxmlPath);
        }
    }
}
