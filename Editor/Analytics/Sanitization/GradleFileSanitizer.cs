// ReSharper disable CheckNamespace

using System;
using System.Text.RegularExpressions;

namespace AppodealInc.Mediation.Analytics.Editor
{
    internal class GradleFileSanitizer : BaseSanitizer
    {
        private static readonly Regex UrlCredentialPattern = new(
            @"(?<prefix>url\s*[=:]\s*['""]?)(?<protocol>https?://)(?<username>[^:@\s'""\n]+):(?<password>[^@\s'""\n]+)@(?<host>[^/\s'""\n]+)(?<path>[^'""]*?)(?<suffix>['""]?)",
            StandardOptions, RegexTimeout);

        private static readonly Regex UrlTokenPattern = new(
            @"(?<prefix>url\s*[=:]\s*['""]?)(?<protocol>https?://)(?<token>[^:@\s'""\n]+)@(?<host>[^/\s'""\n]+)(?<path>[^'""]*?)(?<suffix>['""]?)",
            StandardOptions, RegexTimeout);

        private static readonly Regex EncryptedPrivateKeyPattern = new(
            @"-----BEGIN\s+ENCRYPTED\s+PRIVATE\s+KEY-----[\s\S]*?-----END\s+ENCRYPTED\s+PRIVATE\s+KEY-----",
            StandardOptions, RegexTimeout);

        private static readonly Regex PublicKeyPattern = new(
            @"-----BEGIN\s+(RSA\s+)?PUBLIC\s+KEY-----[\s\S]*?-----END\s+(RSA\s+)?PUBLIC\s+KEY-----",
            StandardOptions, RegexTimeout);

        private static readonly Regex KeystorePasswordPattern = new(
            @"(store|key)Password\s*[=:]\s*(?:(?<quotes>['""])(?<value>[^'"";\s\n]+)\k<quotes>|(?<value>[^'"";\s\n]+))",
            StandardOptions, RegexTimeout);

        private static readonly Regex KeystoreFilePattern = new(
            @"storeFile\s*[=:]\s*file\s*\(\s*(?<quotes>['""])(?<value>[^'""]+)\k<quotes>\s*\)",
            StandardOptions, RegexTimeout);

        private static readonly Regex KeystoreFileLiteralPattern = new(
            @"storeFile\s*[=:]\s*(?:(?<quotes>['""])(?<value>[^'"";\s\n]+)\k<quotes>|(?<value>[^'"";\s\n]+))",
            StandardOptions, RegexTimeout);

        private static readonly Regex KeyAliasPattern = new(
            @"(keyAlias)\s*[=:]\s*(?:(?<quotes>['""])(?<value>[^'"";\s\n]+)\k<quotes>|(?<value>[^'"";\s\n]+))",
            StandardOptions, RegexTimeout);

        private static readonly Regex ReleaseKeystorePattern = new(
            @"(RELEASE_|DEBUG_)?(STORE|KEY)_(PASSWORD|FILE|ALIAS)\s*[=:]\s*(?:(?<quotes>['""])(?<value>[^'"";\s\n]+)\k<quotes>|(?<value>[^'"";\s\n]+))",
            StandardOptions, RegexTimeout);

        private static readonly Regex RepoCredentialPattern = new(
            @"(maven|nexus|artifactory|repo)?(User|Username|Pass|Password)\s*[=:]\s*(?:(?<quotes>['""])(?<value>[^'"";\s\n]+)\k<quotes>|(?<value>[^'"";\s\n]+))",
            StandardOptions, RegexTimeout);

        private static readonly Regex GenericKeyPattern = new(
            @"\b([a-zA-Z]+[Kk]ey)\s*[=:]\s*(?:(?<quotes>['""])(?<value>[^'"";\s\n]+)\k<quotes>|(?<value>[^'"";\s\n]+))",
            StandardOptions, RegexTimeout);

        private static readonly Regex CloudCredentialPattern = new(
            @"\b(access[\-_]?key|secret[\-_]?key|aws[\-_]?key)\s*[=:]\s*(?:(?<quotes>['""])(?<value>[^'"";\s\n]+)\k<quotes>|(?<value>[^'"";\s\n]+))",
            StandardOptions, RegexTimeout);

        private static readonly Regex DbPattern = new(
            @"(?<prefix>jdbc:|mongodb://|mysql://|postgresql://|redis://|[^/\s]+://[^/\s]*?)(?<username>[^:@\s/]+):(?<password>[^@\s/]+)@",
            StandardOptions, RegexTimeout);

        private static readonly Regex SystemGetPattern = new(
            @"System\.(?<method>getenv|getProperty)\s*\(\s*(?<quote>['""])(?<varname>[^'""]*(?:password|secret|key|token|auth|credential)[^'""]*)\k<quote>\s*\)",
            StandardOptions, RegexTimeout);

        private static readonly Regex SetterMethodPattern = new(
            @"\bset(Username|Password|Token|ApiKey|Secret)\s*\(\s*(?<quotes>['""])(?<value>(?:\\.|[^'""])*)\k<quotes>\s*\)",
            StandardOptions, RegexTimeout);

        private static readonly Regex BuildConfigFieldPattern = new(
            @"buildConfigField\s*\(\s*['""][^'""]*['""],\s*['""](?<name>[^'""]*(?:key|token|secret|password|credential)[^'""]*)['""],\s*(?<quotes>['""])(?<value>(?:\\.|[^'""])*)\k<quotes>\s*\)",
            StandardOptions, RegexTimeout);

        private static readonly Regex ResValuePattern = new(
            @"resValue\s*\(\s*['""][^'""]*['""],\s*['""](?<name>[^'""]*(?:key|token|secret|password|credential)[^'""]*)['""],\s*(?<quotes>['""])(?<value>(?:\\.|[^'""])*)\k<quotes>\s*\)",
            StandardOptions, RegexTimeout);

        private static readonly Regex ManifestPlaceholderPattern = new(
            @"manifestPlaceholders\s*[\[=].*?(?<key>\w*(?:key|token|secret|password|credential)\w*)\s*[:=]\s*(?:(?<quotes>['""])(?<value>[^'"",\s\]]+)\k<quotes>|(?<value>[^'"",\s\]]+))",
            StandardOptions | RegexOptions.Singleline, RegexTimeout);

        private static readonly Regex GradlePropertyPattern = new(
            @"(?:project\.findProperty|providers\.gradleProperty|hasProperty)\s*\(\s*(?<quote>['""])(?<varname>[^'""]*(?:password|secret|key|token|auth|credential)[^'""]*)\k<quote>\s*\)",
            StandardOptions, RegexTimeout);

        private static readonly Regex FirebaseKeyPattern = new(
            @"(firebase|google)[\s=:]*(?:(?<quotes>['""])(?<key>[A-Za-z0-9\-_]{30,})\k<quotes>|(?<key>[A-Za-z0-9\-_]{30,}))",
            StandardOptions, RegexTimeout);

        private static readonly Regex EnvVarPattern = new(
            @"\$\{?(?<varname>[A-Za-z_]*(?:password|secret|key|token|auth|credential)[A-Za-z_]*)\}?",
            StandardOptions, RegexTimeout);

        protected override string SanitizeUrlCredentials(string content)
        {
            if (String.IsNullOrEmpty(content)) return content;

            try
            {
                content = UrlCredentialPattern.Replace(content, match =>
                {
                    string prefix = match.Groups["prefix"].Value;
                    string protocol = match.Groups["protocol"].Value;
                    string host = match.Groups["host"].Value;
                    string path = match.Groups["path"].Value;
                    string suffix = match.Groups["suffix"].Value;

                    return $"{prefix}{protocol}[USERNAME_REDACTED]:[PASSWORD_REDACTED]@{host}{path}{suffix}";
                });

                content = UrlTokenPattern.Replace(content, match =>
                {
                    string prefix = match.Groups["prefix"].Value;
                    string protocol = match.Groups["protocol"].Value;
                    string host = match.Groups["host"].Value;
                    string path = match.Groups["path"].Value;
                    string suffix = match.Groups["suffix"].Value;

                    return $"{prefix}{protocol}[TOKEN_REDACTED]@{host}{path}{suffix}";
                });

                content = SanitizeStandaloneUrls(content);

                return content;
            }
            catch (Exception e)
            {
                Logger.Log($"Error sanitizing URL credentials: {e.Message}");
                return content;
            }
        }

        protected override string SanitizePlatformSpecific(string content)
        {
            if (String.IsNullOrEmpty(content)) return content;

            try
            {
                content = EncryptedPrivateKeyPattern.Replace(content, "[ENCRYPTED_PRIVATE_KEY_REDACTED]");
                content = PublicKeyPattern.Replace(content, "[PUBLIC_KEY_REDACTED]");

                content = SanitizePropertyCredentials(content);
                content = SanitizeSpecialPatterns(content);

                return content;
            }
            catch (Exception e)
            {
                Logger.Log($"Error sanitizing Gradle platform-specific patterns: {e.Message}");
                return content;
            }
        }

        private static string SanitizePropertyCredentials(string content)
        {
            if (String.IsNullOrEmpty(content)) return content;

            try
            {
                content = KeystoreFilePattern.Replace(content, match => ReplaceValueInMatch(match, "[KEYSTORE_FILE_REDACTED]"));
                content = KeystoreFileLiteralPattern.Replace(content, match => ReplaceValueInMatch(match, "[KEYSTORE_FILE_REDACTED]"));
                content = KeystorePasswordPattern.Replace(content, match => ReplaceValueInMatch(match, "[KEYSTORE_REDACTED]"));
                content = KeyAliasPattern.Replace(content, match => ReplaceValueInMatch(match, "[KEY_ALIAS_REDACTED]"));
                content = ReleaseKeystorePattern.Replace(content, match => ReplaceValueInMatch(match, "[KEYSTORE_REDACTED]"));

                content = RepoCredentialPattern.Replace(content, match => ReplaceValueInMatch(match, "[REPO_CREDENTIAL_REDACTED]"));

                content = GenericKeyPattern.Replace(content, match => ReplaceValueInMatch(match, "[API_KEY_REDACTED]"));
                content = CloudCredentialPattern.Replace(content, match => ReplaceValueInMatch(match, "[CLOUD_CREDENTIAL_REDACTED]"));
                content = SetterMethodPattern.Replace(content, match => ReplaceValueInMatch(match, "[SETTER_REDACTED]"));

                return content;
            }
            catch (Exception e)
            {
                Logger.Log($"Error sanitizing property credentials: {e.Message}");
                return content;
            }
        }

        private static string SanitizeSpecialPatterns(string content)
        {
            if (String.IsNullOrEmpty(content)) return content;

            try
            {
                content = DbPattern.Replace(content, match =>
                {
                    string prefix = match.Groups["prefix"].Value;
                    return $"{prefix}[USERNAME_REDACTED]:[PASSWORD_REDACTED]@";
                });

                content = SystemGetPattern.Replace(content, match => ReplaceValueInMatch(match, "[ENV_VAR_REDACTED]", "varname"));

                content = BuildConfigFieldPattern.Replace(content, match => ReplaceValueInMatch(match, "[BUILD_CONFIG_REDACTED]"));
                content = ResValuePattern.Replace(content, match => ReplaceValueInMatch(match, "[RES_VALUE_REDACTED]"));
                content = ManifestPlaceholderPattern.Replace(content, match => ReplaceValueInMatch(match, "[MANIFEST_REDACTED]"));

                content = GradlePropertyPattern.Replace(content, match => ReplaceValueInMatch(match, "[GRADLE_PROP_REDACTED]", "varname"));

                content = FirebaseKeyPattern.Replace(content, match => ReplaceValueInMatch(match, "[FIREBASE_KEY_REDACTED]", "key"));
                content = EnvVarPattern.Replace(content, match => ReplaceValueInMatch(match, "[ENV_VAR_REDACTED]", "varname"));

                return content;
            }
            catch (Exception e)
            {
                Logger.Log($"Error sanitizing special patterns: {e.Message}");
                return content;
            }
        }
    }
}
