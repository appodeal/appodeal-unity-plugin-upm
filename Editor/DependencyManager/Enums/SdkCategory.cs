using System.Diagnostics.CodeAnalysis;

// ReSharper disable once CheckNamespace
namespace AppodealInc.Mediation.DependencyManager.Editor
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    internal enum SdkCategory
    {
        Unknown = -1,
        Other,
        Mediation,
        AdNetwork,
        Service
    }
}
