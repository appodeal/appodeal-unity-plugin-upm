using AppodealInc.Mediation.Shared.Editor;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal static class DmLogging
    {
        public static readonly AppodealLogger Log = new("DM", () => DmChoicesScriptableObject.Instance?.EnableVerboseLogging ?? false);
    }
}
