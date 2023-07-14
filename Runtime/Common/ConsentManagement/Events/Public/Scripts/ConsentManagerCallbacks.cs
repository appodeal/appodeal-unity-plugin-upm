using System;

// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Common
{
    /// <summary>
    /// Class containing Consent-Flow-Related events.
    /// </summary>
    public static class ConsentManagerCallbacks
    {
        /// <summary>
        /// Class containing Consent Form events.
        /// </summary>
        public sealed class ConsentForm
        {
            #region ConsentForm Singleton

            private ConsentForm() { }

            private static ConsentForm _instance;

            private static readonly object Lock = new object();

            /// <summary>
            /// Returns an instance of the <see cref="ConsentManagerCallbacks.ConsentForm"/> class.
            /// </summary>
            public static ConsentForm Instance
            {
                get
                {
                    if (_instance == null)
                    {
                        lock (Lock)
                        {
                            if (_instance == null)
                            {
                                _instance = new ConsentForm();
                                _instance.InitializeCallbacks();
                            }
                        }
                    }
                    return _instance;
                }
            }

            #endregion

            private IConsentFormProxyListener _consentFormEventsImpl;

            /// <summary>
            /// Returns an instance of the <see cref="ConsentFormProxyListener"/> class.
            /// </summary>
            public IConsentFormProxyListener ConsentFormEventsImpl => _consentFormEventsImpl ??= new ConsentFormProxyListener();

            /// <summary>
            /// Raised when the Consent Form is successfully loaded.
            /// </summary>
            public static event EventHandler OnLoaded;

            /// <summary>
            /// <para>
            /// Raised when loading or showing of the Consent Form fails.
            /// </para>
            /// Arguments are of a type <see cref="ConsentManagerExceptionEventArgs"/>.
            /// </summary>
            public static event EventHandler<ConsentManagerExceptionEventArgs> OnExceptionOccurred;

            /// <summary>
            /// Raised when the Consent Form window appears on the screen.
            /// </summary>
            public static event EventHandler OnOpened;

            /// <summary>
            /// <para>
            /// Raised when the Consent Form window is closed.
            /// </para>
            /// Arguments are of a type <see cref="ConsentEventArgs"/>.
            /// </summary>
            public static event EventHandler<ConsentEventArgs> OnClosed;

            private void InitializeCallbacks()
            {
                ConsentFormEventsImpl.OnLoaded += (sender, args) => OnLoaded?.Invoke(this, args);
                ConsentFormEventsImpl.OnExceptionOccurred += (sender, args) => OnExceptionOccurred?.Invoke(this, args);
                ConsentFormEventsImpl.OnOpened += (sender, args) => OnOpened?.Invoke(this, args);
                ConsentFormEventsImpl.OnClosed += (sender, args) => OnClosed?.Invoke(this, args);
            }
        }

        /// <summary>
        /// Class containing Consent Info events.
        /// </summary>
        public sealed class ConsentInfo
        {
            #region ConsentInfo Singleton

            private ConsentInfo() { }

            private static ConsentInfo _instance;

            private static readonly object Lock = new object();

            /// <summary>
            /// Returns an instance of the <see cref="ConsentManagerCallbacks.ConsentInfo"/> class.
            /// </summary>
            public static ConsentInfo Instance
            {
                get
                {
                    if (_instance == null)
                    {
                        lock (Lock)
                        {
                            if (_instance == null)
                            {
                                _instance = new ConsentInfo();
                                _instance.InitializeCallbacks();
                            }
                        }
                    }
                    return _instance;
                }
            }

            #endregion

            private IConsentInfoProxyListener _consentInfoEventsImpl;

            /// <summary>
            /// Returns an instance of the <see cref="ConsentInfoProxyListener"/> class.
            /// </summary>
            public IConsentInfoProxyListener ConsentInfoEventsImpl => _consentInfoEventsImpl ??= new ConsentInfoProxyListener();

            /// <summary>
            /// <para>
            /// Raised when status of the Consent is obtained from Appodeal server.
            /// </para>
            /// Arguments are of a type <see cref="ConsentEventArgs"/>.
            /// </summary>
            public static event EventHandler<ConsentEventArgs> OnUpdated;

            /// <summary>
            /// <para>
            /// Raised when obtaining status of the Consent from Appodeal server fails.
            /// </para>
            /// Arguments are of a type <see cref="ConsentManagerExceptionEventArgs"/>.
            /// </summary>
            public static event EventHandler<ConsentManagerExceptionEventArgs> OnFailedToUpdate;

            private void InitializeCallbacks()
            {
                ConsentInfoEventsImpl.OnUpdated += (sender, args) => OnUpdated?.Invoke(this, args);
                ConsentInfoEventsImpl.OnFailedToUpdate += (sender, args) => OnFailedToUpdate?.Invoke(this, args);
            }
        }
    }
}
