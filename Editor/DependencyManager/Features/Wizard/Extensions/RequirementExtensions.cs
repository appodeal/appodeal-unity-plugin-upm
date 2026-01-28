namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal static class RequirementExtensions
    {
        public static Requirement GetStricter(this Requirement a, Requirement b)
        {
            if (a == Requirement.Required || b == Requirement.Required) return Requirement.Required;
            if (a == Requirement.Default || b == Requirement.Default) return Requirement.Default;
            return Requirement.Optional;
        }
    }
}
