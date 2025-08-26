// ReSharper disable CheckNamespace

using System;
using System.Text.RegularExpressions;

namespace AppodealInc.Mediation.Analytics.Editor
{
    internal class PodfileSanitizer : BaseSanitizer
    {
        private static readonly Regex SourceUrlCredentialPattern = new(
            @"source\s+(?<quotes>['""])(?<protocol>https?://)(?<username>[^:@\s'""\n]+):(?<password>[^@\s'""\n]+)@(?<host>[^/\s'""\n]+)(?<path>[^'""]*?)\k<quotes>",
            StandardOptions, RegexTimeout);

        private static readonly Regex PodGitUrlCredentialPattern = new(
            @"(?<prefix>pod\s+[^,]+,\s*):git\s*=>\s*(?<quotes>['""])(?<protocol>https?://)(?<username>[^:@\s'""\n]+):(?<password>[^@\s'""\n]+)@(?<host>[^/\s'""\n]+)(?<path>[^'""]*?)\k<quotes>",
            StandardOptions, RegexTimeout);

        private static readonly Regex PodHttpUrlTokenPattern = new(
            @"(?<prefix>pod\s+[^,]+,\s*):http\s*=>\s*(?<quotes>['""])(?<protocol>https?://)(?<token>[^:@\s'""\n]+)@(?<host>[^/\s'""\n]+)(?<path>[^'""]*?)\k<quotes>",
            StandardOptions, RegexTimeout);

        private static readonly Regex PodSourcePattern = new(
            @"source\s+(?<quotes>['""])(?<url>https?://[^'""]+)\k<quotes>",
            StandardOptions, RegexTimeout);

        private static readonly Regex CertificatePathPattern = new(
            @"(certificate|cert|provisioning|profile)[\-_]?path\s*[=:]\s*(?:(?<quotes>['""])(?<value>[^'""]+)\k<quotes>|(?<value>[^'"";\s\n]+))",
            StandardOptions, RegexTimeout);

        private static readonly Regex KeychainPattern = new(
            @"(keychain|security)\s+[^'""]*(?:(?<quotes>['""])(?<value>[^'""]*(?:password|secret|key)[^'""]*)\k<quotes>|(?<value>[^'"";\s\n]*(?:password|secret|key)[^'"";\s\n]*))",
            StandardOptions, RegexTimeout);

        private static readonly Regex ConfigurationValuePattern = new(
            @"(config|configuration)\s*\[\s*(?<quotes>['""])(?<key>[^'""]*(?:key|token|secret|password|credential)[^'""]*)\k<quotes>\s*\]\s*=\s*(?:(?<valuequotes>['""])(?<value>[^'""]+)\k<valuequotes>|(?<value>[^'"";\s\n]+))",
            StandardOptions, RegexTimeout);

        private static readonly Regex ScriptPhaseCredentialPattern = new(
            @"script_phase[^{]*\{[^}]*(?:(?<quotes>['""])(?<value>[^'""]*(?:password|secret|key|token|auth|credential)[^'""]*)\k<quotes>|(?<value>[^'"";\s\n]*(?:password|secret|key|token|auth|credential)[^'"";\s\n]*))",
            StandardOptions | RegexOptions.Singleline, RegexTimeout);

        private static readonly Regex RubyStringInterpolationPattern = new(
            @"#\{\s*(?<varname>[^}]*(?:password|secret|key|token|auth|credential)[^}]*)\s*\}",
            StandardOptions, RegexTimeout);

        private static readonly Regex RubyVariablePattern = new(
            @"@(?<varname>[a-zA-Z_][a-zA-Z0-9_]*(?:password|secret|key|token|auth|credential)[a-zA-Z0-9_]*)",
            StandardOptions, RegexTimeout);

        private static readonly Regex SshKeyPattern = new(
            @"ssh-(?:rsa|dss|ed25519|ecdsa)\s+[A-Za-z0-9+/=]+",
            StandardOptions, RegexTimeout);

        private static readonly Regex GitSshUrlPattern = new(
            @"git@(?<host>[^:]+):(?<path>[^'"";\s\n]+)",
            StandardOptions, RegexTimeout);

        private static readonly Regex GitHubTokenPattern = new(
            @"(github|gh)[\s=:]*(?:(?<quotes>['""])(?<token>gh[a-z]_[A-Za-z0-9]{36})\k<quotes>|(?<token>gh[a-z]_[A-Za-z0-9]{36}))",
            StandardOptions, RegexTimeout);

        private static readonly Regex GitLabTokenPattern = new(
            @"(gitlab|gl)[\s=:]*(?:(?<quotes>['""])(?<token>glpat-[A-Za-z0-9\-_]{20,})\k<quotes>|(?<token>glpat-[A-Za-z0-9\-_]{20,}))",
            StandardOptions, RegexTimeout);

        private static readonly Regex HexSecretPattern = new(
            @"(secret|key|token)[\s=:]*(?:(?<quotes>['""])(?<value>[a-fA-F0-9]{32,})\k<quotes>|(?<value>[a-fA-F0-9]{32,}))",
            StandardOptions, RegexTimeout);

        private static readonly Regex AppleDeveloperTeamPattern = new(
            @"(team|developer)[\-_]?id\s*[=:]\s*(?:(?<quotes>['""])(?<value>[A-Z0-9]{10})\k<quotes>|(?<value>[A-Z0-9]{10}))",
            StandardOptions, RegexTimeout);

        private static readonly Regex ProvisioningProfilePattern = new(
            @"(provisioning|profile)[\-_]?uuid\s*[=:]\s*(?:(?<quotes>['""])(?<value>[A-Fa-f0-9]{8}-[A-Fa-f0-9]{4}-[A-Fa-f0-9]{4}-[A-Fa-f0-9]{4}-[A-Fa-f0-9]{12})\k<quotes>|(?<value>[A-Fa-f0-9]{8}-[A-Fa-f0-9]{4}-[A-Fa-f0-9]{4}-[A-Fa-f0-9]{4}-[A-Fa-f0-9]{12}))",
            StandardOptions, RegexTimeout);

        protected override string SanitizeUrlCredentials(string content)
        {
            if (String.IsNullOrEmpty(content)) return content;

            try
            {
                content = SourceUrlCredentialPattern.Replace(content, match =>
                {
                    string quotes = match.Groups["quotes"].Value;
                    string protocol = match.Groups["protocol"].Value;
                    string host = match.Groups["host"].Value;
                    string path = match.Groups["path"].Value;

                    return $"source {quotes}{protocol}[USERNAME_REDACTED]:[PASSWORD_REDACTED]@{host}{path}{quotes}";
                });

                content = PodGitUrlCredentialPattern.Replace(content, match =>
                {
                    string prefix = match.Groups["prefix"].Value;
                    string quotes = match.Groups["quotes"].Value;
                    string protocol = match.Groups["protocol"].Value;
                    string host = match.Groups["host"].Value;
                    string path = match.Groups["path"].Value;

                    return $"{prefix}:git => {quotes}{protocol}[USERNAME_REDACTED]:[PASSWORD_REDACTED]@{host}{path}{quotes}";
                });

                content = PodHttpUrlTokenPattern.Replace(content, match =>
                {
                    string prefix = match.Groups["prefix"].Value;
                    string quotes = match.Groups["quotes"].Value;
                    string protocol = match.Groups["protocol"].Value;
                    string host = match.Groups["host"].Value;
                    string path = match.Groups["path"].Value;

                    return $"{prefix}:http => {quotes}{protocol}[TOKEN_REDACTED]@{host}{path}{quotes}";
                });

                content = SanitizeStandaloneUrls(content);

                content = GitSshUrlPattern.Replace(content, match => ReplaceValueInMatch(match, "[SSH_REPO_REDACTED]", "path"));

                return content;
            }
            catch (Exception e)
            {
                Logger.Log($"Error sanitizing Podfile URL credentials: {e.Message}");
                return content;
            }
        }

        protected override string SanitizePlatformSpecific(string content)
        {
            if (String.IsNullOrEmpty(content)) return content;

            try
            {
                content = SshKeyPattern.Replace(content, "[SSH_KEY_REDACTED]");

                content = RubyStringInterpolationPattern.Replace(content, match => ReplaceValueInMatch(match, "[RUBY_VAR_REDACTED]", "varname"));
                content = RubyVariablePattern.Replace(content, match => ReplaceValueInMatch(match, "[RUBY_VAR_REDACTED]", "varname"));

                content = GitHubTokenPattern.Replace(content, match => ReplaceValueInMatch(match, "[GITHUB_TOKEN_REDACTED]", "token"));
                content = GitLabTokenPattern.Replace(content, match => ReplaceValueInMatch(match, "[GITLAB_TOKEN_REDACTED]", "token"));
                content = HexSecretPattern.Replace(content, match => ReplaceValueInMatch(match, "[HEX_SECRET_REDACTED]"));

                content = SanitizeSpecialPatterns(content);

                return content;
            }
            catch (Exception e)
            {
                Logger.Log($"Error sanitizing Podfile platform-specific patterns: {e.Message}");
                return content;
            }
        }

        private static string SanitizeSpecialPatterns(string content)
        {
            if (String.IsNullOrEmpty(content)) return content;

            try
            {
                content = CertificatePathPattern.Replace(content, match => ReplaceValueInMatch(match, "[CERTIFICATE_PATH_REDACTED]"));
                content = KeychainPattern.Replace(content, match => ReplaceValueInMatch(match, "[KEYCHAIN_REDACTED]"));
                content = ConfigurationValuePattern.Replace(content, match => ReplaceValueInMatch(match, "[CONFIG_VALUE_REDACTED]"));
                content = ScriptPhaseCredentialPattern.Replace(content, match => ReplaceValueInMatch(match, "[SCRIPT_CREDENTIAL_REDACTED]"));
                content = AppleDeveloperTeamPattern.Replace(content, match => ReplaceValueInMatch(match, "[APPLE_TEAM_ID_REDACTED]"));
                content = ProvisioningProfilePattern.Replace(content, match => ReplaceValueInMatch(match, "[PROVISIONING_UUID_REDACTED]"));

                content = PodSourcePattern.Replace(content, match =>
                {
                    string url = match.Groups["url"].Value;
                    return url.Contains("@") ? match.Value.Replace(url, "[PRIVATE_SOURCE_REDACTED]") : match.Value;
                });

                return content;
            }
            catch (Exception e)
            {
                Logger.Log($"Error sanitizing Podfile special patterns: {e.Message}");
                return content;
            }
        }
    }
}
