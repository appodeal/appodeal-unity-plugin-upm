using AppodealInc.Mediation.Shared.Editor;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    public static partial class DmConstants
    {
        internal static class Endpoints
        {
            public const string BaseUrl = ServiceHosts.Mds + "/v4/unity";
            public const string Validate = BaseUrl + "/validate";
            public const string Plugins = BaseUrl + "/versions";
        }
    }
}
