namespace AppodealInc.Mediation.Shared.Editor
{
    internal static class DevMode
    {
        public static bool IsEnabled
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
