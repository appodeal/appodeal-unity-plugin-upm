using System;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    [Flags]
    internal enum AdTypes
    {
        Banner = 1,
        Interstitial = 2,
        Rewarded = 4,
        Mrec = 8,
        Native = 16,
        All = Banner | Interstitial | Rewarded | Mrec | Native
    }
}
