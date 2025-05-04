#if UNITY_IOS
// ReSharper disable CheckNamespace

using System;
using System.Diagnostics;
using System.Text;

namespace AppodealInc.Mediation.MaxAdReview.Editor
{
    internal static class ShellUtil
    {
        public static (int exitCode, string output, string error) RunInBash(string program, string programArgs, string workingDirectory)
        {
            string bashArgs = $"-l -c \"{program} '{programArgs}'\"";

            var psi = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = bashArgs,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8,
                WorkingDirectory = workingDirectory,
            };

            using var process = Process.Start(psi);
            if (process == null)
            {
                UnityEngine.Debug.LogError($"[ShellUtil] Failed to start process `{psi.FileName} {psi.Arguments}`");
                return (-1, String.Empty, String.Empty);
            }

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            return (process.ExitCode, output, error);
        }
    }
}
#endif
