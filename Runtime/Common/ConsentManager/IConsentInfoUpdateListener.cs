using System.Diagnostics.CodeAnalysis;

namespace AppodealStack.ConsentManager.Common
{
    /// <summary>
    /// <para>
    /// Interface containing signatures of ConsentInfoUpdate callback methods.
    /// </para>
    /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "InvalidXmlDocComment")]
    [SuppressMessage("ReSharper", "UnusedParameter.Global")]
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    public interface IConsentInfoUpdateListener
    {
        void onConsentInfoUpdated(IConsent consent);
        void onFailedToUpdateConsentInfo(IConsentManagerException error);
    }
}
