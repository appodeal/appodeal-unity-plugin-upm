using AppodealInc.Mediation.Shared.Editor;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    public static partial class DmConstants
    {
        internal static class Choices
        {
            private const string ResourceName = "AppodealDmChoices";
            public const string FileName = ResourceName + ".asset";
            public const string FilePath = PluginPaths.EditorResourcesDir + "/" + FileName;
            public const string ResourcePath = PluginPaths.ResourcesSubDir + "/" + ResourceName;
        }
    }
}
