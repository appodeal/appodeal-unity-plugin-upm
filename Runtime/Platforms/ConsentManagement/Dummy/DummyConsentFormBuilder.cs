using UnityEngine;
using AppodealStack.ConsentManagement.Common;

// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Platforms.Dummy
{
    /// <summary>
    /// Unity Editor implementation of <see langword="IConsentFormBuilder"/> interface.
    /// </summary>
    public class DummyConsentFormBuilder : IConsentFormBuilder
    {
        public IConsentForm Build()
        {
            Debug.Log(Utils.GetDummyMessage());
            return null;
        }

        public void WithListener(IConsentFormListener consentFormListener)
        {
            Debug.Log(Utils.GetDummyMessage());
        }
    }
}
