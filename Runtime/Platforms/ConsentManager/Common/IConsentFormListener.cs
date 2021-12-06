using System.Diagnostics.CodeAnalysis;

namespace AppodealCM.Unity.Platforms
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    public interface IConsentFormListener
    {
         void onConsentFormLoaded();
         void onConsentFormError(ConsentManagerException consentManagerException);
         void onConsentFormOpened();
         void onConsentFormClosed(Consent consent);
    }
}
