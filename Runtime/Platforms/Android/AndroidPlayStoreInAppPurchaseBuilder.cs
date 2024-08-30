// ReSharper Disable CheckNamespace

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using AppodealStack.Monetization.Common;

namespace AppodealStack.Monetization.Platforms.Android
{
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    internal class AndroidPlayStoreInAppPurchaseBuilder : IPlayStoreInAppPurchaseBuilder
    {
        private readonly AndroidJavaObject _inAppPurchaseBuilder;
        private AndroidJavaObject _inAppPurchase;

        internal AndroidPlayStoreInAppPurchaseBuilder(PlayStorePurchaseType purchaseType)
        {
            using var inAppPurchaseJavaClass = new AndroidJavaClass("com.appodeal.ads.inapp.InAppPurchase");
            _inAppPurchaseBuilder = purchaseType switch
            {
                PlayStorePurchaseType.Subs => inAppPurchaseJavaClass.CallStatic<AndroidJavaObject>("newSubscriptionBuilder"),
                PlayStorePurchaseType.InApp => inAppPurchaseJavaClass.CallStatic<AndroidJavaObject>("newInAppBuilder"),
                _ => throw new ArgumentOutOfRangeException(nameof(purchaseType), purchaseType, null)
            };
        }

        private AndroidJavaObject GetBuilder()
        {
            return _inAppPurchaseBuilder;
        }

        public IPlayStoreInAppPurchase Build()
        {
            _inAppPurchase = GetBuilder().Call<AndroidJavaObject>("build");
            return new AndroidPlayStoreInAppPurchase(_inAppPurchase);
        }

        public void WithPublicKey(string publicKey)
        {
            GetBuilder().Call<AndroidJavaObject>("withPublicKey", publicKey);
        }

        public void WithSignature(string signature)
        {
            GetBuilder().Call<AndroidJavaObject>("withSignature", signature);
        }

        public void WithPurchaseData(string purchaseData)
        {
            GetBuilder().Call<AndroidJavaObject>("withPurchaseData", purchaseData);
        }

        public void WithPrice(string price)
        {
            GetBuilder().Call<AndroidJavaObject>("withPrice", price);
        }

        public void WithCurrency(string currency)
        {
            GetBuilder().Call<AndroidJavaObject>("withCurrency", currency);
        }

        public void WithSku(string sku)
        {
            GetBuilder().Call<AndroidJavaObject>("withSku", sku);
        }

        public void WithOrderId(string orderId)
        {
            GetBuilder().Call<AndroidJavaObject>("withOrderId", orderId);
        }

        public void WithDeveloperPayload(string developerPayload)
        {
            GetBuilder().Call<AndroidJavaObject>("withDeveloperPayload", developerPayload);
        }

        public void WithPurchaseToken(string purchaseToken)
        {
            GetBuilder().Call<AndroidJavaObject>("withPurchaseToken", purchaseToken);
        }

        public void WithPurchaseTimestamp(long purchaseTimestamp)
        {
            GetBuilder().Call<AndroidJavaObject>("withPurchaseTimestamp", purchaseTimestamp);
        }

        public void WithAdditionalParameters(Dictionary<string, string> additionalParameters)
        {
            using var map = new AndroidJavaObject("java.util.HashMap");
            foreach (var entry in additionalParameters)
            {
                map.Call<AndroidJavaObject>("put", entry.Key, entry.Value);
            }

            GetBuilder().Call<AndroidJavaObject>("withAdditionalParams", map);
        }
    }
}
