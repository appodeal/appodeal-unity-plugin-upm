// ReSharper disable CheckNamespace

using System;

namespace AppodealInc.Mediation.Analytics.Editor
{
    [Serializable]
    public enum ActionType
    {
        OpenDocumentation = 0,
        OpenDependencyManager,
        UpdateDependencies,
        CloseDependencyManager,
        OpenAppodealSettings,
        CloseAppodealSettings,
        UpdatePlugin,
        RemovePlugin
    }
}
