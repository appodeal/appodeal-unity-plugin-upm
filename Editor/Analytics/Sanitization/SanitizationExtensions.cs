// ReSharper disable CheckNamespace

namespace AppodealInc.Mediation.Analytics.Editor
{
    internal static class SanitizationExtensions
    {
        private static readonly GradleFileSanitizer GradleSanitizer = new();
        private static readonly PodfileSanitizer PodfileSanitizer = new();

        internal static string SanitizeGradleContent(this string input) => GradleSanitizer.Sanitize(input);

        internal static string SanitizePodfileContent(this string input) => PodfileSanitizer.Sanitize(input);
    }
}
