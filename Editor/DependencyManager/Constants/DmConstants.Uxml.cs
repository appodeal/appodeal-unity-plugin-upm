// ReSharper disable CheckNamespace

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    public static partial class DmConstants
    {
        internal static class Uxml
        {
            internal static class Dm
            {
                public const string MediationsContainer = "MediationsContainer";
                public const string AdNetworksContainer = "AdNetworksContainer";
                public const string ServicesContainer = "ServicesContainer";

                public const string GenerateButton = "GenerateButton";

                public const string SuccessStatusPopup = "SuccessStatusPopup";
                public const string FailStatusPopup = "FailStatusPopup";

                public const string SelectAllButton = "SelectAllButton";
                public const string SelectNoneButton = "SelectNoneButton";
                public const string SelectCustomButton = "SelectCustomButton";
                public const string SelectDefaultButton = "SelectDefaultButton";
            }

            internal static class SdkCard
            {
                public const string SdkId = "SdkId";
                public const string SdkName = "SdkName";

                public const string IsSdkCardSelected = "IsCardSelected";

                public const string AndroidSdk = "AndroidSdk";
                public const string AndroidSdkVersion = "AndroidSdkVersion";
                public const string IosSdk = "IosSdk";
                public const string IosSdkVersion = "IosSdkVersion";

                public const string SupportedMediationsContainer = "SupportedMediationsContainer";

                public const string SdkIcon = "SdkIcon";
                public const string InfoIcon = "InfoIcon";
                public const string MrecAdTypeIcon = "MrecIcon";
                public const string BannerAdTypeIcon = "BannerIcon";
                public const string RewardedAdTypeIcon = "RewardedIcon";
                public const string InterstitialAdTypeIcon = "InterstitialIcon";
            }

            internal static class Tooltip
            {
                public const string Root = "Tooltip";

                public const string SdkId = "SdkId";
                public const string SdkName = "SdkName";
                public const string SdkRequirementStatus = "SdkRequirementStatus";
                public const string SdkDescription = "SdkDescription";
            }

            internal static class Settings
            {
                public const string CheckForAdapterUpdatesToggle = "CheckForAdapterUpdates";
                public const string UpdateAdaptersAutomaticallyToggle = "UpdateAdaptersAutomatically";
                public const string CheckForPluginUpdatesToggle = "CheckForPluginUpdates";
                public const string IncludePluginBetaVersionsToggle = "IncludePluginBetVersions";
                public const string EnableLoggingToggle = "EnableLogging";
                public const string OpenDocumentationButton = "OpenDocumentation";
            }

            internal static class Error
            {
                public const string ReloadButton = "ReloadButton";
                public const string ContactSupportButton = "ContactSupportButton";
            }
        }
    }
}
