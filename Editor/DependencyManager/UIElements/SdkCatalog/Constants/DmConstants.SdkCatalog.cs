using AppodealInc.Mediation.Utils.Editor;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    public static partial class DmConstants
    {
        internal static class SdkCatalog
        {
            private const string DmRootDir = AppodealEditorConstants.PackageDir + "/Editor/DependencyManager";
            private const string ResourcesDir = DmRootDir + "/Resources/Appodeal/DependencyManager";
            public const string SdksInfoPath = ResourcesDir + "/SdkCatalog/SdksInfo.asset";

            public const string UnlistedSdkId = "unlisted_sdk";
        }
    }
}
