// ReSharper disable CheckNamespace

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
            using var inAppPurchaseJavaClass = new AndroidJavaClass(AndroidConstants.JavaClassName.AppodealInAppPurchase);
            _inAppPurchaseBuilder = purchaseType switch
            {
                PlayStorePurchaseType.Subs => inAppPurchaseJavaClass.CallStatic<AndroidJavaObject>(AndroidConstants.JavaMethodName.InAppPurchase.NewSubscriptionBuilder),
                PlayStorePurchaseType.InApp => inAppPurchaseJavaClass.CallStatic<AndroidJavaObject>(AndroidConstants.JavaMethodName.InAppPurchase.NewInAppBuilder),
                _ => throw new ArgumentOutOfRangeException(nameof(purchaseType), purchaseType, null)
            };
        }

        private AndroidJavaObject GetBuilder()
        {
            return _inAppPurchaseBuilder;
        }

        public IPlayStoreInAppPurchase Build()
        {
            _inAppPurchase = GetBuilder().Call<AndroidJavaObject>(AndroidConstants.JavaMethodName.InAppPurchaseBuilder.Build);
            return new AndroidPlayStoreInAppPurchase(_inAppPurchase);
        }

        public void WithPublicKey(string publicKey)
        {
            GetBuilder().Call<AndroidJavaObject>(AndroidConstants.JavaMethodName.InAppPurchaseBuilder.WithPublicKey, publicKey);
        }

        public void WithSignature(string signature)
        {
            GetBuilder().Call<AndroidJavaObject>(AndroidConstants.JavaMethodName.InAppPurchaseBuilder.WithSignature, signature);
        }

        public void WithPurchaseData(string purchaseData)
        {
            GetBuilder().Call<AndroidJavaObject>(AndroidConstants.JavaMethodName.InAppPurchaseBuilder.WithPurchaseData, purchaseData);
        }

        public void WithPrice(string price)
        {
            GetBuilder().Call<AndroidJavaObject>(AndroidConstants.JavaMethodName.InAppPurchaseBuilder.WithPrice, price);
        }

        public void WithCurrency(string currency)
        {
            GetBuilder().Call<AndroidJavaObject>(AndroidConstants.JavaMethodName.InAppPurchaseBuilder.WithCurrency, currency);
        }

        public void WithSku(string sku)
        {
            GetBuilder().Call<AndroidJavaObject>(AndroidConstants.JavaMethodName.InAppPurchaseBuilder.WithSku, sku);
        }

        public void WithOrderId(string orderId)
        {
            GetBuilder().Call<AndroidJavaObject>(AndroidConstants.JavaMethodName.InAppPurchaseBuilder.WithOrderId, orderId);
        }

        public void WithDeveloperPayload(string developerPayload)
        {
            GetBuilder().Call<AndroidJavaObject>(AndroidConstants.JavaMethodName.InAppPurchaseBuilder.WithDeveloperPayload, developerPayload);
        }

        public void WithPurchaseToken(string purchaseToken)
        {
            GetBuilder().Call<AndroidJavaObject>(AndroidConstants.JavaMethodName.InAppPurchaseBuilder.WithPurchaseToken, purchaseToken);
        }

        public void WithPurchaseTimestamp(long purchaseTimestamp)
        {
            GetBuilder().Call<AndroidJavaObject>(AndroidConstants.JavaMethodName.InAppPurchaseBuilder.WithPurchaseTimestamp, purchaseTimestamp);
        }

        public void WithAdditionalParameters(Dictionary<string, string> additionalParameters)
        {
            try
            {
                using var map = new AndroidJavaObject(AndroidConstants.JavaClassName.HashMap);
                foreach (var entry in additionalParameters)
                {
                    map.Call<AndroidJavaObject>("put", entry.Key, entry.Value);
                }

                GetBuilder().Call<AndroidJavaObject>(AndroidConstants.JavaMethodName.InAppPurchaseBuilder.WithAdditionalParams, map);
            }
            catch (Exception e)
            {
                AndroidAppodealHelper.LogIntegrationError(e.Message);
            }
        }
    }
}
