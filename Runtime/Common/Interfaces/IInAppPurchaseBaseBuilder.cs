// ReSharper disable CheckNamespace

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AppodealStack.Monetization.Common
{
    /// <summary>
    /// <para>
    /// Base interface containing common method signatures of the <see langword="InAppPurchaseBuilder"/> classes.
    /// </para>
    /// See <see href="https://docs.appodeal.com/unity/advanced/in-app-purchases?distribution=upm"/> for more details.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedParameter.Global")]
    public interface IInAppPurchaseBaseBuilder
    {
        void WithPrice(string price);
        void WithCurrency(string currency);
        void WithAdditionalParameters(Dictionary<string, string> additionalParameters);
    }
}
