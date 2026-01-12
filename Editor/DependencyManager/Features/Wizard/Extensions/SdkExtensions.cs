using System;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal static class SdkExtensions
    {
        public static Requirement GetStrictestRequirement(this Sdk firstSdk, Sdk secondSdk)
        {
            if (firstSdk != null && secondSdk != null) return firstSdk.Requirement.GetStricter(secondSdk.Requirement);
            if (firstSdk != null) return firstSdk.Requirement;
            if (secondSdk != null) return secondSdk.Requirement;

            throw new InvalidOperationException("Both Android and iOS models are null. This should never happen");
        }
    }
}
