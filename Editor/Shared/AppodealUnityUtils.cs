namespace AppodealInc.Mediation.Shared.Editor
{
    public static class AppodealUnityUtils
    {
        public const string KeySkAdNetworkItems = "SKAdNetworkItems";
        public const string KeySkAdNetworkID = "SKAdNetworkIdentifier";
        public const string KeyAdNetworkIdentifiers = "AdNetworkIdentifiers";
        public const string NsUserTrackingUsageDescription = "This identifier will be used to deliver personalized ads to you";

        #region Optional Android Permissions

        public const string CoarseLocation = "android.permission.ACCESS_COARSE_LOCATION";
        public const string FineLocation = "android.permission.ACCESS_FINE_LOCATION";
        public const string ExternalStorageWrite = "android.permission.WRITE_EXTERNAL_STORAGE";
        public const string AccessWifiState = "android.permission.ACCESS_WIFI_STATE";
        public const string Vibrate = "android.permission.VIBRATE";

        #endregion

        public static bool IsDevModeEnabled
        {
            get
            {
#if APPODEAL_DEV
                return true;
#else
                return false;
#endif
            }
        }
    }
}
