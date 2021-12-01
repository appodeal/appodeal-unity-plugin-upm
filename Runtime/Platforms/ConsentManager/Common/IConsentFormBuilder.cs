using System.Diagnostics.CodeAnalysis;
using AppodealCM.Unity.Common;

namespace AppodealCM.Unity.Platforms
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedParameter.Global")]
    public interface IConsentFormBuilder
    {
        IConsentForm build();
        void withListener(IConsentFormListener consentFormListener);
    }
}