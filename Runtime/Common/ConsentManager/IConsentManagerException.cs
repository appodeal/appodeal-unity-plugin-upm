using System.Diagnostics.CodeAnalysis;

namespace AppodealCM.Unity.Common
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public interface IConsentManagerException
    {
        string getReason();
        int getCode();
    }
}
