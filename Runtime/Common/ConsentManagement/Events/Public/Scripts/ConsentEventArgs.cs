using System;

// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Common
{
    /// <summary>
    /// This class is designed to store arguments of
    /// <see cref="ConsentManagerCallbacks.ConsentInfo.OnUpdated">ConsentInfo.OnUpdated</see> and
    /// <see cref="ConsentManagerCallbacks.ConsentForm.OnClosed">ConsentForm.OnClosed</see> events.
    /// </summary>
    public class ConsentEventArgs : EventArgs
    {
        /// <summary>
        /// Returns object of type <see cref="IConsent"/> which can then be passed to Appodeal SDK.
        /// </summary>
        public IConsent Consent { get; }

        /// <summary>
        /// Constructor for Consent Event Args
        /// </summary>
        /// <param name="consent">IConsent object.</param>
        public ConsentEventArgs(IConsent consent)
        {
            Consent = consent;
        }
    }
}
