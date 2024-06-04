using System.Diagnostics.CodeAnalysis;

// ReSharper disable once CheckNamespace
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
