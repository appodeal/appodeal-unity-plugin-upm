using UnityEditor;
using AppodealAds.Unity.Editor.AppodealManager;
using AppodealAds.Unity.Editor.InternalResources;

namespace AppodealAds.Unity.Editor.NetworkConfigs
{
    static class NetworkConfigsInstaller
    {
        [InitializeOnLoadMethod]
        static void InstallNetworkConfigs()
        {
            if (AppodealSettings.Instance.WereConfigsImported)
                return;

            AppodealDependencyUtils.ImportConfigsFromPackage();
        }
    
    }
}
