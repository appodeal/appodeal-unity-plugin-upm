using UnityEngine;
using System.Collections.Generic;
using AppodealStack.Monetization.Common;

// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Platforms.Dummy
{
    /// <summary>
    /// Unity Editor implementation of <see langword="IPlayStoreInAppPurchaseBuilder"/> interface.
    /// </summary>
    public class DummyPlayStoreInAppPurchaseBuilder : IPlayStoreInAppPurchaseBuilder
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

        public void WithPublicKey(string publicKey)
        {
            Debug.Log("Call to WithPublicKey on not supported platform." + DummyMessage);
        }

        public void WithSignature(string signature)
        {
            Debug.Log("Call to WithSignature on not supported platform." + DummyMessage);
        }

        public void WithPurchaseData(string purchaseData)
        {
            Debug.Log("Call to WithPurchaseData on not supported platform." + DummyMessage);
        }

        public void WithSku(string sku)
        {
            Debug.Log("Call to WithSku on not supported platform." + DummyMessage);
        }

        public void WithOrderId(string orderId)
        {
            Debug.Log("Call to WithOrderId on not supported platform." + DummyMessage);
        }

        public void WithPurchaseToken(string purchaseToken)
        {
            Debug.Log("Call to WithPurchaseToken on not supported platform." + DummyMessage);
        }

        public void WithPurchaseTimestamp(long purchaseTimestamp)
        {
            Debug.Log("Call to WithPurchaseTimestamp on not supported platform." + DummyMessage);
        }

        public void WithDeveloperPayload(string developerPayload)
        {
            Debug.Log("Call to WithDeveloperPayload on not supported platform." + DummyMessage);
        }

        public IPlayStoreInAppPurchase Build()
        {
            Debug.Log("Call to Build on not supported platform." + DummyMessage);
            return null;
        }
    }
}
