using System.Diagnostics.CodeAnalysis;

// ReSharper disable once CheckNamespace
namespace AppodealInc.Mediation.DependencyManager.Editor
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    internal enum Requirement
    {
        Unknown = -1,
        Default,
        Required,
        Optional,
        Deprecated
    }
}
