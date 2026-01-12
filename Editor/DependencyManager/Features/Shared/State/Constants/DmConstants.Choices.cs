using AppodealInc.Mediation.Utils.Editor;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    public static partial class DmConstants
    {
        internal static class Choices
        {
            private const string ResourceName = "AppodealDmChoices";
            public const string FileName = ResourceName + ".asset";
            public const string EditorResourcesDir = AppodealEditorConstants.EditorResourcesDir;
            public const string FilePath = EditorResourcesDir + "/" + FileName;
            public const string ResourcePath = AppodealEditorConstants.ResourcesSubPath + "/" + ResourceName;
        }
    }
}
