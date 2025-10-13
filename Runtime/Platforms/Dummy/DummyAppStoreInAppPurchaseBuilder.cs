// ReSharper disable CheckNamespace

using System.Collections.Generic;
using UnityEngine;
using AppodealStack.Monetization.Common;

namespace AppodealStack.Monetization.Platforms.Dummy
{
    /// <summary>
    /// Unity Editor implementation of the <see langword="IAppStoreInAppPurchaseBuilder"/> interface.
    /// </summary>
    internal class DummyAppStoreInAppPurchaseBuilder : IAppStoreInAppPurchaseBuilder
    {
        private const string DummyMessage = " To test in-app purchases, install your application on the Android/iOS device.";

        public void WithPrice(string price)
        {
            Debug.Log("Call to WithPrice on not supported platform." + DummyMessage);
        }

        public void WithCurrency(string currency)
        {
            Debug.Log("Call to WithCurrency on not supported platform." + DummyMessage);
        }

        public void WithAdditionalParameters(Dictionary<string, string> additionalParameters)
        {
            Debug.Log("Call to WithAdditionalParameters on not supported platform." + DummyMessage);
        }

        public void WithProductId(string productId)
        {
            Debug.Log("Call to WithProductId on not supported platform." + DummyMessage);
        }

        public void WithTransactionId(string transactionId)
        {
            Debug.Log("Call to WithTransactionId on not supported platform." + DummyMessage);
        }

        public IAppStoreInAppPurchase Build()
        {
            Debug.Log("Call to Build on not supported platform." + DummyMessage);
            return null;
        }
    }
}
