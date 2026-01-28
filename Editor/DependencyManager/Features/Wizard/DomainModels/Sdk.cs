using System;
using System.Collections.Generic;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal class Sdk
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public Category Category { get; set; }
        public Requirement Requirement { get; set; }
        public AdTypes? AdTypes { get; set; }
        public List<VersionInfo> Versions { get; set; }
        public string Warning { get; set; }

        public bool HasVersions => Versions != null && Versions.Count > 0;
        public bool HasWarning => !String.IsNullOrEmpty(Warning);
        public bool IsAdNetwork => Category == Category.AdNetwork;
        public bool IsService => Category == Category.Service;
    }
}
