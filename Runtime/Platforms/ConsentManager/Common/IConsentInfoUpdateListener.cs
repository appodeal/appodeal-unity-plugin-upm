using System.Diagnostics.CodeAnalysis;

namespace AppodealCM.Unity.Platforms
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    public interface IConsentInfoUpdateListener
    {
        void onConsentInfoUpdated(Consent consent);
        void onFailedToUpdateConsentInfo(ConsentManagerException error);
    }
}