using System.Diagnostics.CodeAnalysis;
using AppodealCM.Unity.Common;

namespace AppodealCM.Unity.Platforms
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class ConsentManagerException : IConsentManagerException
    {
        private readonly IConsentManagerException consentManagerException;

        public ConsentManagerException()
        {
            consentManagerException = ConsentManagerClientFactory.GetConsentManagerException();
        }

        public ConsentManagerException(IConsentManagerException androidConsentManagerException)
        {
            consentManagerException = androidConsentManagerException;
        }

        public string getReason()
        {
            return consentManagerException.getReason();
        }

        public int getCode()
        {
            return consentManagerException.getCode();
        }
    }
}
