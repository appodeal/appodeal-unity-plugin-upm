namespace AppodealInc.Mediation.DependencyManager.Editor
{
    public static partial class DmConstants
    {
        internal static class Regex
        {
            public const string VersionGroupName = "version";
            public const string IdentifierGroupName = "identifier";
            public const string PluginVersionPattern = "^" + PluginVersionGroup + IdentifierGroup + "$";
            public const string AdapterVersionPattern = "^" + AdapterVersionGroup + IdentifierGroup + "$";

            private const string AllowedNumber = "(0|[1-9][0-9]*)";
            private const string PluginVersionGroup = "(?<" + VersionGroupName + ">" + AllowedNumber + @"\." + AllowedNumber + @"\." + AllowedNumber + ")";
            private const string AdapterVersionGroup = "(?<" + VersionGroupName + ">" + AllowedNumber + @"\." + AllowedNumber + @"\." + AllowedNumber + @"(\." + AllowedNumber + ")*)";
            private const string IdentifierGroup = "(?<" + IdentifierGroupName + @">(-beta\." + AllowedNumber + ")?)";
        }
    }
}
