using UnityEditor.Android;
using AppodealInc.Mediation.PluginSettings.Editor;

namespace AppodealInc.Mediation.PreProcess.Editor
{
    internal class AndroidLibraryBuildProcessor : IPostGenerateGradleAndroidProject
    {
        public int callbackOrder => 10;

        public void OnPostGenerateGradleAndroidProject(string path)
        {
            var settings = AppodealSettings.Instance;
            if (settings == null) return;

            var library = AndroidLibrary.Open(path);

            PermissionsConfigurator.Configure(library.Manifest, settings);
            AdMobConfigurator.Configure(library.Manifest, settings);
            FacebookConfigurator.Configure(library.Manifest, settings);
            FirebaseConfigurator.Configure(library.GoogleServices, settings);

            library.Save();
        }
    }
}
