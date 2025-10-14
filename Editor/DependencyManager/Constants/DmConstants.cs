// ReSharper disable CheckNamespace

using UnityEngine;
using AppodealInc.Mediation.Utils.Editor;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    public static partial class DmConstants
    {
        public const string BaseUrl = "https://mw-backend-new.appodeal.com/v3/release/manager/unity/appodeal";

        public const string DocumentationUrl = "https://docs.appodeal.com/unity/advanced/appodeal-plugin-manager";
        public const string ContactSupportUrl = "https://faq.appodeal.com/en/articles/96628-technical-support";

        public const string SettingsProviderWindowName = AppodealEditorConstants.DependencyManagerWindowName;

        public const string AutoCheckForUpdatesPerformedKey = "ApdDmAutoCheckForUpdatesPerformed";

        public const string AppodealPackageName = AppodealEditorConstants.PackageName;
        public const string AppodealPackageGitUrl = AppodealEditorConstants.GitRepoAddress;

        public const string DependenciesDir = AppodealEditorConstants.DependenciesDir;
        public const string DependenciesFilePath = AppodealEditorConstants.DependenciesFilePath;

        private const string DmChoicesResourceName = "AppodealDmChoices";
        public const string DmChoicesFileName = DmChoicesResourceName + ".asset";
        public const string EditorResourcesDir = AppodealEditorConstants.EditorResourcesDir;
        public const string DmChoicesSoFilePath = EditorResourcesDir + "/" + DmChoicesFileName;
        public const string DmChoicesResourcePath = AppodealEditorConstants.ResourcesSubPath + "/" + DmChoicesResourceName;

        public const string AndroidLibDir = AppodealEditorConstants.AppodealAndroidLibDir;

        private const string DmRootDir = AppodealEditorConstants.PackageDir + "/Editor/DependencyManager";

        private const string ResourcesDir = DmRootDir + "/Resources";
        private const string UITemplatesDir = ResourcesDir + "/UITemplates";

        public const string ErrorUxmlPath = UITemplatesDir + "/ApdDmError.uxml";
        public const string SettingsUxmlPath = UITemplatesDir + "/ApdDmSettings.uxml";
        public const string LoadingUxmlPath = UITemplatesDir + "/ApdDmLoading.uxml";
        public const string DependencyManagerUxmlPath = UITemplatesDir + "/ApdDmDependencyManager.uxml";
        public const string SdkCardUxmlPath = UITemplatesDir + "/ApdDmSdkCard.uxml";
        public const string AdNetworkSdkCardUxmlPath = UITemplatesDir + "/ApdDmAdNetworkSdkCard.uxml";
        public const string TooltipUxmlPath = UITemplatesDir + "/ApdDmTooltip.uxml";

        public const string SdksInfoSoPath = ResourcesDir + "/ScriptableObjects/ApdDmSdksInfo.asset";

        public const string UnlistedSdkInfoId = "unlisted_sdk";

        public static readonly Color DarkTooltipBgColor = new(0.22f, 0.22f, 0.22f, 0.9f);
        public static readonly Color LightTooltipBgColor = new(0.78f, 0.78f, 0.78f, 0.9f);

        // public static readonly string PluginConfigMockUrl = $"file://{Application.dataPath}/Config.txt";
    }
}
