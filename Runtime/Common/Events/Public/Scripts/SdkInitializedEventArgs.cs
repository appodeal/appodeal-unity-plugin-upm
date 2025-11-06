// ReSharper disable CheckNamespace

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AppodealStack.Monetization.Common
{
    /// <summary>
    /// This class is designed to store arguments of
    /// <see cref="AppodealCallbacks.Sdk.OnInitialized">OnSdkInitialized</see> event.
    /// </summary>
    public class SdkInitializedEventArgs : EventArgs
    {
        /// <summary>
        /// Returns list of initialization errors, if any. Otherwise - null.
        /// </summary>
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
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
