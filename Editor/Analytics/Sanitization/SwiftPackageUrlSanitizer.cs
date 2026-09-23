// ReSharper disable CheckNamespace

using System;
using System.Text.RegularExpressions;

namespace AppodealInc.Mediation.Analytics.Editor
{
    internal class SwiftPackageUrlSanitizer : BaseSanitizer
    {
        private static readonly Regex HttpUrlTokenPattern = new(
            @"\b(?<protocol>https?://)(?<token>[^:@/\s'""\n]+)@(?<host>[^/\s'""\n]+)(?<path>[^'"";\s\n]*)",
            StandardOptions, RegexTimeout);

        private static readonly Regex GitSshUrlPattern = new(
            @"git@(?<host>[^:]+):(?<path>[^'"";\s\n]+)",
            StandardOptions, RegexTimeout);

        protected override string SanitizeUrlCredentials(string content)
        {
            if (String.IsNullOrEmpty(content)) return content;

            try
            {
                content = SanitizeStandaloneUrls(content);
                content = HttpUrlTokenPattern.Replace(content, match => ReplaceValueInMatch(match, "[TOKEN_REDACTED]", "token"));
                content = GitSshUrlPattern.Replace(content, match => ReplaceValueInMatch(match, "[SSH_REPO_REDACTED]", "path"));

                return content;
            }
            catch (Exception e)
            {
                Logger.Log($"Error sanitizing Swift package URL credentials: {e.Message}");
                return content;
            }
        }

        protected override string SanitizePlatformSpecific(string content) => content;
    }
}
