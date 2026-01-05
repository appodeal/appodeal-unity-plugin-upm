using System;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal class DropdownOverlayEventArgs : EventArgs
    {
        public DropdownPopupController Controller { get; }

        public DropdownOverlayEventArgs(DropdownPopupController controller)
        {
            Controller = controller;
        }
    }
}
