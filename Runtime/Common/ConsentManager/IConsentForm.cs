using System.Diagnostics.CodeAnalysis;

namespace AppodealCM.Unity.Common
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public interface IConsentForm
    {
        void load();
        void showAsActivity();
        void showAsDialog();
        bool isLoaded();
        bool isShowing();
    }
}