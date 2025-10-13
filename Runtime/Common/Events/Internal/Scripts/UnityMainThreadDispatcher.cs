// ReSharper disable CheckNamespace

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using UnityEngine;
using UnityEngine.Scripting;

[assembly: AlwaysLinkAssembly]

namespace AppodealStack.Monetization.Common
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    internal static class UnityMainThreadDispatcher
    {
        private static SynchronizationContext _unityContext;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void SetContext() => _unityContext = SynchronizationContext.Current;

        public static void Post(SendOrPostCallback d, object state = null) => _unityContext?.Post(d, state);

        public static void Send(SendOrPostCallback d, object state = null) => _unityContext?.Send(d, state);
    }
}
