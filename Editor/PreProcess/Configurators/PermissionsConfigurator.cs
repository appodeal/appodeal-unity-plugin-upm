using AppodealInc.Mediation.PluginSettings.Editor;

namespace AppodealInc.Mediation.PreProcess.Editor
{
    internal static class PermissionsConfigurator
    {
        private const string CoarseLocation = "android.permission.ACCESS_COARSE_LOCATION";
        private const string FineLocation = "android.permission.ACCESS_FINE_LOCATION";
        private const string ExternalStorageWrite = "android.permission.WRITE_EXTERNAL_STORAGE";
        private const string AccessWifiState = "android.permission.ACCESS_WIFI_STATE";
        private const string Vibrate = "android.permission.VIBRATE";

        public static void Configure(AndroidManifest manifest, AppodealSettings settings)
        {
            if (settings.AccessCoarseLocationPermission) manifest.AddPermission(CoarseLocation);
            if (settings.AccessFineLocationPermission) manifest.AddPermission(FineLocation);
            if (settings.WriteExternalStoragePermission) manifest.AddPermission(ExternalStorageWrite);
            if (settings.AccessWifiStatePermission) manifest.AddPermission(AccessWifiState);
            if (settings.VibratePermission) manifest.AddPermission(Vibrate);
        }
    }
}
