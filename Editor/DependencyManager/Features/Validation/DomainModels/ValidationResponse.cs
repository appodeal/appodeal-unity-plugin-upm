using System.Collections.Generic;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal class ValidationResponse
    {
        public List<ProblematicSdk> ProblematicSdks { get; set; }

        public bool HasProblematicSdks => ProblematicSdks != null && ProblematicSdks.Count > 0;
    }
}
