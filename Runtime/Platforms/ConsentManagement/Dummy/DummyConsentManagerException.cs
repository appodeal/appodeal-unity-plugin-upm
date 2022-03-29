using UnityEngine;
using AppodealStack.ConsentManagement.Common;

// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Platforms.Dummy
{
    /// <summary>
    /// Unity Editor implementation of <see langword="IConsentManagerException"/> interface.
    /// </summary>
    public class DummyConsentManagerException : IConsentManagerException
    {
        public string GetReason()
        {
            Debug.Log(Utils.GetDummyMessage());
            return Utils.GetDummyMessage();
        }

        public int GetCode()
        {
            Debug.Log(Utils.GetDummyMessage());
            return 0;
        }
    }
}
