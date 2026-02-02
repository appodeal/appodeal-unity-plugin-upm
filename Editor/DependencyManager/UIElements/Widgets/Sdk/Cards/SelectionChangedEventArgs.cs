using System;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal class SelectionChangedEventArgs : EventArgs
    {
        public bool IsSelected { get; }

        public SelectionChangedEventArgs(bool isSelected)
        {
            IsSelected = isSelected;
        }
    }
}
