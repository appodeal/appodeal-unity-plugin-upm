// ReSharper disable CheckNamespace

using System.Diagnostics.CodeAnalysis;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    internal enum AdType
    {
        Unknown = 0,
        Banner,
        Interstitial,
        Native,
        Mrec,
        Rewarded
    }
}
