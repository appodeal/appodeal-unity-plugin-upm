// ReSharper disable CheckNamespace

using System.Diagnostics.CodeAnalysis;

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
