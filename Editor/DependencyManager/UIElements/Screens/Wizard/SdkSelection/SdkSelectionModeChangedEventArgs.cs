using System;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal class SdkSelectionModeChangedEventArgs : EventArgs
    {
        public SdkSelectionMode NewMode { get; }

        public SdkSelectionModeChangedEventArgs(SdkSelectionMode newMode)
        {
            NewMode = newMode;
        }
    }
}
