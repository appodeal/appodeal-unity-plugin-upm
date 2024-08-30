// ReSharper disable CheckNamespace

using System.Diagnostics.CodeAnalysis;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    internal enum SdkStability
    {
        Unknown = -1,
        Stable,
        Beta,
        Alpha,
        Custom
    }
}
