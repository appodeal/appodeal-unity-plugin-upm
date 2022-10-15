using System.Collections.Generic;

// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Common
{
    /// <summary>
    /// Interface containing signatures of Appodeal Initialization callback methods.
    /// </summary>
    public interface IAppodealInitializationListener
    {
        /// <summary>
        /// <para>
        /// Raised when Appodeal SDK initialization is finished.
        /// </para>
        /// See <see href=""/> for more details.
        /// </summary>
        /// <param name="errors">list of errors, if any. Otherwise - null.</param>
        void OnInitializationFinished(List<string> errors);
    }
}
