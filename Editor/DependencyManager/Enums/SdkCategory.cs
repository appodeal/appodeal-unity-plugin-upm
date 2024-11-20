// ReSharper disable CheckNamespace

using System.Diagnostics.CodeAnalysis;

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
