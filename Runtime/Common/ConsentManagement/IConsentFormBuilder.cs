// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Common
{
    /// <summary>
    /// <para>
    /// Interface containing method signatures of the <see langword="ConsentFormBuilder"/> class.
    /// </para>
    /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
    /// </summary>
    public interface IConsentFormBuilder
    {
        IConsentForm Build();
        void WithListener(IConsentFormListener consentFormListener);
    }
}
