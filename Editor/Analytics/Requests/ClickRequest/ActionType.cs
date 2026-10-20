using System;

namespace AppodealInc.Mediation.Analytics.Editor
{
    [Serializable]
    internal enum ActionType
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
