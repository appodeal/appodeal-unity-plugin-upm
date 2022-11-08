using System;
using System.Collections.Generic;

// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Common
{
    /// <summary>
    /// This class is designed to store arguments of
    /// <see cref="AppodealEvents.OnSdkInitialized">OnSdkInitialized</see> event.
    /// </summary>
    public class SdkInitializedEventArgs : EventArgs
    {
        /// <summary>
        /// Returns list of initialization errors, if any. Otherwise - null.
        /// </summary>
        public List<string> Errors { get; }

        /// <summary>
        /// Constructor for Sdk Initialized Event Args
        /// </summary>
        /// <param name="errors">Initialization errors, if any.</param>
        public SdkInitializedEventArgs(List<string> errors)
        {
            Errors = errors;
        }
    }
}
