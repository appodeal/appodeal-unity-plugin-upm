// ReSharper disable CheckNamespace

using System;
using System.Text.RegularExpressions;

namespace AppodealInc.Mediation.Analytics.Editor
{
    internal abstract class BaseSanitizer
    {
        protected const RegexOptions StandardOptions = RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant;

        private const int RegexTimeoutMs = 2_000;
        protected static readonly TimeSpan RegexTimeout = TimeSpan.FromMilliseconds(RegexTimeoutMs);

        private static readonly Regex JwtPattern = new(
            @"\b(jwt|bearer)[\s=:]*(?:(?<quotes>['""])(?<token>([A-Za-z0-9_-]{4,}\.){2}[A-Za-z0-9_-]{4,})\k<quotes>|(?<token>([A-Za-z0-9_-]{4,}\.){2}[A-Za-z0-9_-]{4,}))",
            StandardOptions, RegexTimeout);

        private static readonly Regex OAuthTokenPattern = new(
            @"(oauth|bearer)[\s=:]*(?:(?<quotes>['""])(?<token>[A-Za-z0-9\-_]{20,})\k<quotes>|(?<token>[A-Za-z0-9\-_]{20,}))",
            StandardOptions, RegexTimeout);

        private static readonly Regex Base64Pattern = new(
            @"(secret|key|token|auth|credential)[\s=:]*(?:(?<quotes>['""])(?<value>[A-Za-z0-9+/]{16,}={0,2})\k<quotes>|(?<value>[A-Za-z0-9+/]{16,}={0,2}))",
            StandardOptions, RegexTimeout);

        private static readonly Regex PrivateKeyPattern = new(
            @"-----BEGIN\s+(RSA\s+)?PRIVATE\s+KEY-----[\s\S]*?-----END\s+(RSA\s+)?PRIVATE\s+KEY-----",
            StandardOptions, RegexTimeout);

        private static readonly Regex CertificatePattern = new(
            @"-----BEGIN\s+CERTIFICATE-----[\s\S]*?-----END\s+CERTIFICATE-----",
            StandardOptions, RegexTimeout);

        private static readonly Regex StandaloneUrlPattern = new(
            @"\b(?<protocol>https?://)(?<username>[^:@\s'""\n]+):(?<password>[^@\s'""\n]+)@(?<host>[^/\s'""\n]+)(?<path>[^'"";\s\n]*)",
            StandardOptions, RegexTimeout);

        private static readonly Regex SshUrlCredentialPattern = new(
            @"\b(?<protocol>ssh://)(?<username>[^:@\s'""\n]+):(?<password>[^@\s'""\n]+)@(?<host>[^:/\s'""\n]+)(?::(?<port>\d+))?(?<path>/[^'"";\s\n]*)?",
            StandardOptions, RegexTimeout);

        private static readonly Regex SshUrlTokenPattern = new(
            @"\b(?<protocol>ssh://)(?<token>[^:@\s'""\n]+)@(?<host>[^:/\s'""\n]+)(?::(?<port>\d+))?(?<path>/[^'"";\s\n]*)?",
            StandardOptions, RegexTimeout);

        private static readonly Regex EnvVarPattern = new(
            @"ENV\s*\[\s*(?<quotes>['""])(?<varname>[^'""]*(?:password|secret|key|token|auth|credential|api)[^'""]*)\k<quotes>\s*\]",
            StandardOptions, RegexTimeout);

        private static readonly Regex EnvVarInterpolationPattern = new(
            @"#\{\s*ENV\s*\[\s*(?<quotes>['""])(?<varname>[^'""]*(?:password|secret|key|token|auth|credential|api)[^'""]*)\k<quotes>\s*\]\s*\}",
            StandardOptions, RegexTimeout);

        private static readonly Regex EnvVarAssignmentPattern = new(
            @"ENV\s*\[\s*(?<quotes>['""])(?<varname>[^'""]*(?:password|secret|key|token|auth|credential|api)[^'""]*)\k<quotes>\s*\]\s*=\s*(?:(?<valuequotes>['""])(?<value>[^'""]+)\k<valuequotes>|(?<value>[^'"";\s\n]+))",
            StandardOptions, RegexTimeout);

        private static readonly Regex ApiKeyPattern = new(
            @"\b(api[\-_]?key|apikey|client[\-_]?secret|access[\-_]?token)\s*[=:]\s*(?:(?<quotes>['""])(?<value>[^'"";\s\n]+)\k<quotes>|(?<value>[^'"";\s\n]+))",
            StandardOptions, RegexTimeout);

        private static readonly Regex SecretPattern = new(
            @"\b([\w]*secret[\w]*)\s*[=:]\s*(?:(?<quotes>['""])(?<value>[^'"";\s\n]+)\k<quotes>|(?<value>[^'"";\s\n]+))",
            StandardOptions, RegexTimeout);

        private static readonly Regex GenericCredentialPattern = new(
            @"\b(username|password|user|pass)\s*[=:]\s*(?:(?<quotes>['""])(?<value>[^'"";\s\n]+)\k<quotes>|(?<value>[^'"";\s\n]+))",
            StandardOptions, RegexTimeout);

        private static readonly Regex RepositoryCredentialPattern = new(
            @"(repo|repository|artifactory|nexus)[\-_]?(user|username|pass|password|token)\s*[=:]\s*(?:(?<quotes>['""])(?<value>[^'"";\s\n]+)\k<quotes>|(?<value>[^'"";\s\n]+))",
            StandardOptions, RegexTimeout);

        public string Sanitize(string content)
        {
            if (String.IsNullOrEmpty(content)) return content;

            try
            {
                string sanitizedContent = content;

                sanitizedContent = SanitizeUrlCredentials(sanitizedContent);
                sanitizedContent = SanitizePemContent(sanitizedContent);
                sanitizedContent = SanitizeEnvironmentVariables(sanitizedContent);
                sanitizedContent = SanitizeCredentialPatterns(sanitizedContent);
                sanitizedContent = SanitizePlatformSpecific(sanitizedContent);

                return sanitizedContent;
            }
            catch (Exception e)
            {
                Logger.Log($"Error during sanitization: {e.Message}");
                return content;
            }
        }

        protected abstract string SanitizeUrlCredentials(string content);
        protected abstract string SanitizePlatformSpecific(string content);

        protected static string ReplaceValueInMatch(Match match, string replacement, string groupName = "value")
        {
            var valueGroup = match.Groups[groupName];
            if (!valueGroup.Success) return match.Value;

            return match.Value[..(valueGroup.Index - match.Index)] +
                   replacement +
                   match.Value[(valueGroup.Index - match.Index + valueGroup.Length)..];
        }

        private static string SanitizePemContent(string content)
        {
            if (String.IsNullOrEmpty(content)) return content;

            try
            {
                content = PrivateKeyPattern.Replace(content, "[PRIVATE_KEY_REDACTED]");
                content = CertificatePattern.Replace(content, "[CERTIFICATE_REDACTED]");

                return content;
            }
            catch (Exception e)
            {
                Logger.Log($"Error sanitizing PEM content: {e.Message}");
                return content;
            }
        }

        private static string SanitizeEnvironmentVariables(string content)
        {
            if (String.IsNullOrEmpty(content)) return content;

            try
            {
                content = EnvVarPattern.Replace(content, match => ReplaceValueInMatch(match, "[ENV_VAR_REDACTED]", "varname"));
                content = EnvVarInterpolationPattern.Replace(content, match => ReplaceValueInMatch(match, "[ENV_VAR_REDACTED]", "varname"));
                content = EnvVarAssignmentPattern.Replace(content, match => ReplaceValueInMatch(match, "[ENV_VAR_ASSIGNMENT_REDACTED]"));

                return content;
            }
            catch (Exception e)
            {
                Logger.Log($"Error sanitizing environment variables: {e.Message}");
                return content;
            }
        }

        private static string SanitizeCredentialPatterns(string content)
        {
            if (String.IsNullOrEmpty(content)) return content;

            try
            {
                content = JwtPattern.Replace(content, match => ReplaceValueInMatch(match, "[JWT_TOKEN_REDACTED]", "token"));
                content = OAuthTokenPattern.Replace(content, match => ReplaceValueInMatch(match, "[OAUTH_TOKEN_REDACTED]", "token"));
                content = Base64Pattern.Replace(content, match => ReplaceValueInMatch(match, "[ENCODED_SECRET_REDACTED]"));

                content = ApiKeyPattern.Replace(content, match => ReplaceValueInMatch(match, "[API_KEY_REDACTED]"));
                content = SecretPattern.Replace(content, match => ReplaceValueInMatch(match, "[SECRET_REDACTED]"));
                content = GenericCredentialPattern.Replace(content, match => ReplaceValueInMatch(match, "[CREDENTIAL_REDACTED]"));
                content = RepositoryCredentialPattern.Replace(content, match => ReplaceValueInMatch(match, "[REPO_CREDENTIAL_REDACTED]"));

                return content;
            }
            catch (Exception e)
            {
                Logger.Log($"Error sanitizing credential patterns: {e.Message}");
                return content;
            }
        }

        protected static string SanitizeStandaloneUrls(string content)
        {
            if (String.IsNullOrEmpty(content)) return content;

            try
            {
                content = StandaloneUrlPattern.Replace(content, match =>
                {
                    string protocol = match.Groups["protocol"].Value;
                    string host = match.Groups["host"].Value;
                    string path = match.Groups["path"].Value;

                    return $"{protocol}[USERNAME_REDACTED]:[PASSWORD_REDACTED]@{host}{path}";
                });

                content = SshUrlCredentialPattern.Replace(content, match =>
                {
                    string protocol = match.Groups["protocol"].Value;
                    string host = match.Groups["host"].Value;
                    string port = match.Groups["port"].Value;
                    string path = match.Groups["path"].Value;

                    string portPart = !String.IsNullOrEmpty(port) ? $":{port}" : "";
                    return $"{protocol}[USERNAME_REDACTED]:[PASSWORD_REDACTED]@{host}{portPart}{path}";
                });

                content = SshUrlTokenPattern.Replace(content, match =>
                {
                    string protocol = match.Groups["protocol"].Value;
                    string host = match.Groups["host"].Value;
                    string port = match.Groups["port"].Value;
                    string path = match.Groups["path"].Value;

                    string portPart = !String.IsNullOrEmpty(port) ? $":{port}" : "";
                    return $"{protocol}[TOKEN_REDACTED]@{host}{portPart}{path}";
                });

                return content;
            }
            catch (Exception e)
            {
                Logger.Log($"Error sanitizing standalone URLs: {e.Message}");
                return content;
            }
        }
    }
}
