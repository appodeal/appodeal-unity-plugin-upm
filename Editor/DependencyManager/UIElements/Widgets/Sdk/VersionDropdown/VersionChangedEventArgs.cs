using System;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal class VersionChangedEventArgs : EventArgs
    {
        public VersionInfo Version { get; }

        public VersionChangedEventArgs(VersionInfo version)
        {
            Version = version;
        }
    }
}
