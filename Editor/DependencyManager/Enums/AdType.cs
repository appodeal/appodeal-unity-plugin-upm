using System.Diagnostics.CodeAnalysis;

// ReSharper disable once CheckNamespace
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
