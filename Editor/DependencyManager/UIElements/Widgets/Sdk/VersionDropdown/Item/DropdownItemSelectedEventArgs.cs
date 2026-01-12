using System;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal class DropdownItemSelectedEventArgs : EventArgs
    {
        public VersionInfo Model { get; }

        public DropdownItemSelectedEventArgs(VersionInfo model)
        {
            Model = model;
        }
    }
}
