using System;

// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Common
{
    /// <summary>
    /// This class is designed to store arguments of
    /// <see cref="ConsentManagerCallbacks.ConsentInfo.OnFailedToUpdate">ConsentInfo.OnFailedToUpdate</see> and
    /// <see cref="ConsentManagerCallbacks.ConsentForm.OnExceptionOccurred">ConsentForm.OnExceptionOccurred</see> events.
    /// </summary>
    public class ConsentManagerExceptionEventArgs : EventArgs
    {
        /// <summary>
        /// Returns object containing information about why the exception occurred.
        /// </summary>
        public IConsentManagerException Exception { get; }

        /// <summary>
        /// Constructor for Consent Manager Exception Event Args
        /// </summary>
        /// <param name="exception">IConsentManagerException object.</param>
        public ConsentManagerExceptionEventArgs(IConsentManagerException exception)
        {
            Exception = exception;
        }
    }
}
