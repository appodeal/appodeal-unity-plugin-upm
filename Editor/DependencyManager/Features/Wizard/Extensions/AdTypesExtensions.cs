namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal static class AdTypesExtensions
    {
        public static AdTypes? Combine(this AdTypes? a, AdTypes? b)
        {
            if (!a.HasValue) return b;
            if (!b.HasValue) return a;
            return a.Value | b.Value;
        }
    }
}
