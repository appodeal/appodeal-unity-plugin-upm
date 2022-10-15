﻿using System;

// ReSharper Disable CheckNamespace
namespace AppodealStack.ConsentManagement.Common
{
    public interface IConsentFormProxyListener : IConsentFormListener
    {
        IConsentFormListener Listener { get; set; }

        event EventHandler OnLoaded;
        event EventHandler<ConsentManagerExceptionEventArgs> OnExceptionOccured;
        event EventHandler OnOpened;
        event EventHandler<ConsentEventArgs> OnClosed;
    }
}
