namespace AppodealAds.Unity.Common {
    public static class AppodealAdType {
        public const int NONE = 0;
        public const int INTERSTITIAL = 3;
        public const int BANNER = 4;
        public const int BANNER_BOTTOM = 8;
        public const int BANNER_TOP = 16;
        public const int BANNER_VIEW = 64;
        public const int MREC = 512;
        public const int REWARDED_VIDEO = 128;
        public const int BANNER_LEFT = 1024;
        public const int BANNER_RIGHT = 2048;
#if UNITY_ANDROID || UNITY_EDITOR
        public const int NON_SKIPPABLE_VIDEO = 128;
#elif UNITY_IPHONE
		public const int NON_SKIPPABLE_VIDEO = 256;
#endif

        public const int BANNER_HORIZONTAL_SMART = -1;
        public const int BANNER_HORIZONTAL_CENTER = -2;
        public const int BANNER_HORIZONTAL_RIGHT = -3;
        public const int BANNER_HORIZONTAL_LEFT = -4;
        public const int BANNER_VERTICAL_BOTTOM = -5;
        public const int BANNER_VERTICAL_TOP = -6;
    }
}
