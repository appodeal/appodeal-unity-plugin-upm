using UnityEngine;
using AppodealStack.ConsentManagement.Common;

// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Platforms.Dummy
{
    /// <summary>
    /// Unity Editor implementation of <see langword="IConsentForm"/> interface.
    /// </summary>
    public class DummyConsentForm : IConsentForm
    {
        public void Load()
        {
            Debug.Log(Utils.GetDummyMessage());
        }

        public void Show()
        {
            Debug.Log(Utils.GetDummyMessage());
        }

        public bool IsLoaded()
        {
            Debug.Log(Utils.GetDummyMessage());
            return false;
        }

        public bool IsShowing()
        {
            Debug.Log(Utils.GetDummyMessage());
            return false;
        }
    }
}
