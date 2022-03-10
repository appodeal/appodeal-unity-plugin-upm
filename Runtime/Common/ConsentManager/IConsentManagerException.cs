using System.Diagnostics.CodeAnalysis;

namespace AppodealStack.ConsentManager.Common
{
    /// <summary>
    /// <para>
    /// Interface containing method signatures of the <see langword="ConsentManagerException"/> class.
    /// </para>
    /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "InvalidXmlDocComment")]
    [SuppressMessage("ReSharper", "UnusedParameter.Global")]
    public interface IConsentManagerException
    {
        string getReason();
        int getCode();
    }
}
