using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AppodealStack.Monetization.Common;

// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Platforms.Android
{
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    public class AndroidPlayStoreInAppPurchaseBuilder : IPlayStoreInAppPurchaseBuilder
    {
        private readonly AndroidJavaObject _inAppPurchaseBuilder;
        private AndroidJavaObject _inAppPurchase;

        public AndroidPlayStoreInAppPurchaseBuilder(PlayStorePurchaseType purchaseType)
        {
            switch (purchaseType)
            {
                case PlayStorePurchaseType.Subs:
                    _inAppPurchaseBuilder = new AndroidJavaClass("com.appodeal.ads.inapp.InAppPurchase").CallStatic<AndroidJavaObject>("newSubscriptionBuilder");
                    break;
                case PlayStorePurchaseType.InApp:
                    _inAppPurchaseBuilder = new AndroidJavaClass("com.appodeal.ads.inapp.InAppPurchase").CallStatic<AndroidJavaObject>("newInAppBuilder");
                    break;
            }
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
            var map = new AndroidJavaObject("java.util.HashMap");
            foreach (var entry in additionalParameters)
            {
                map.Call<AndroidJavaObject>("put", entry.Key, Helper.GetJavaObject(entry.Value));
            }

            GetBuilder().Call<AndroidJavaObject>("withAdditionalParams", map);
        }
    }
}
