using System;
using System.Diagnostics.CodeAnalysis;
using AppodealStack.ConsentManagement.Common;

// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Platforms.Ios
{
    /// <summary>
    /// iOS implementation of <see langword="IConsentForm"/> interface.
    /// </summary>
    [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
    public class IosConsentForm : IConsentForm
    {
        public IosConsentForm(IntPtr intPtr)
        {
            new ConsentFormObjCBridge(intPtr);
        }

        public void Load()
        {
            ConsentFormObjCBridge.Load();
        }

        public void Show()
        {
            ConsentFormObjCBridge.Show();
        }

        public bool IsLoaded()
        {
            return ConsentFormObjCBridge.IsLoaded();
        }

        public bool IsShowing()
        {
            return ConsentFormObjCBridge.IsShowing();
        }
    }
}
