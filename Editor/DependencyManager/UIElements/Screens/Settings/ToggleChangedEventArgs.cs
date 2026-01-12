using System;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal class ToggleChangedEventArgs : EventArgs
    {
        public bool NewValue { get; }

        public ToggleChangedEventArgs(bool newValue)
        {
            NewValue = newValue;
        }
    }
}
