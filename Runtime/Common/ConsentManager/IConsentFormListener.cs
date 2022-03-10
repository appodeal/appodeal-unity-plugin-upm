using System.Diagnostics.CodeAnalysis;

namespace AppodealStack.ConsentManager.Common
{
    /// <summary>
    /// <para>
    /// Interface containing signatures of ConsentForm callback methods.
    /// </para>
    /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "InvalidXmlDocComment")]
    [SuppressMessage("ReSharper", "UnusedParameter.Global")]
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    public interface IConsentFormListener
    {
         void onConsentFormLoaded();
         void onConsentFormError(IConsentManagerException consentManagerException);
         void onConsentFormOpened();
         void onConsentFormClosed(IConsent consent);
    }
}
