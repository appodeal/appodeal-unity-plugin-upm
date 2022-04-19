using UnityEngine;
using System.Collections.Generic;
using AppodealStack.Monetization.Common;

// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Platforms.Android
{
    public class AndroidInAppPurchaseBuilder : IInAppPurchaseBuilder
    {
        private readonly AndroidJavaObject _inAppPurchaseBuilder;
        private AndroidJavaObject _inAppPurchase;

        public AndroidInAppPurchaseBuilder(AndroidPurchaseType purchaseType)
        {
            switch (purchaseType)
            {
                case AndroidPurchaseType.Subs:
                    _inAppPurchaseBuilder = new AndroidJavaClass("com.appodeal.ads.modules.common.inapp.InAppPurchase").CallStatic<AndroidJavaObject>("newSubscriptionBuilder");
                    break;
                case AndroidPurchaseType.InApp:
                    _inAppPurchaseBuilder = new AndroidJavaClass("com.appodeal.ads.modules.common.inapp.InAppPurchase").CallStatic<AndroidJavaObject>("newInAppBuilder");
                    break;
            }
        }

        private AndroidJavaObject GetBuilder()
        {
            return _inAppPurchaseBuilder;
        }

        public IInAppPurchase Build()
        {
            _inAppPurchase = new AndroidJavaObject("com.appodeal.ads.modules.common.inapp.InAppPurchase");
            _inAppPurchase = GetBuilder().Call<AndroidJavaObject>("build");
            return new AndroidInAppPurchase(_inAppPurchase);
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

        public void WithAdditionalParams(Dictionary<string, string> additionalParameters)
        {
            var map = new AndroidJavaObject("java.util.HashMap");
            foreach (var entry in additionalParameters)
            {
                map.Call<AndroidJavaObject>("put", entry.Key, Helper.GetJavaObject(entry.Value));
            }

            GetBuilder().Call<AndroidJavaObject>("withAdditionalParams", map);
        }
    }
    
    public class AndroidInAppPurchase : IInAppPurchase
    {
        public IInAppPurchase NativeInAppPurchase { get; }

        private readonly AndroidJavaObject _inAppPurchase;

        public AndroidInAppPurchase(AndroidJavaObject inAppPurchase)
        {
            _inAppPurchase = inAppPurchase;
            NativeInAppPurchase = this;
        }

        public AndroidJavaObject GetInAppPurchase()
        {
            return _inAppPurchase;
        }

        public AndroidPurchaseType GetPurchaseType()
        {
            var purchaseType = AndroidPurchaseType.Subs;
            string type = GetInAppPurchase().Call<AndroidJavaObject>("getType").Call<string>("toString");

            switch (type)
            {
                case "Subs":
                    purchaseType = AndroidPurchaseType.Subs;
                    break;
                case "InApp":
                    purchaseType = AndroidPurchaseType.InApp;
                    break;
            }

            return purchaseType;
        }

        public string GetPublicKey()
        {
            return GetInAppPurchase().Call<string>("getPublicKey");
        }

        public string GetSignature()
        {
            return GetInAppPurchase().Call<string>("getSignature");
        }

        public string GetPurchaseData()
        {
            return GetInAppPurchase().Call<string>("getPurchaseData");
        }

        public string GetPrice()
        {
            return GetInAppPurchase().Call<string>("getPrice");
        }

        public string GetCurrency()
        {
            return GetInAppPurchase().Call<string>("getCurrency");
        }

        public string GetSku()
        {
            return GetInAppPurchase().Call<string>("getSku");
        }

        public string GetOrderId()
        {
            return GetInAppPurchase().Call<string>("getOrderId");
        }

        public string GetPurchaseToken()
        {
            return GetInAppPurchase().Call<string>("getPurchaseToken");
        }

        public long GetPurchaseTimestamp()
        {
            return GetInAppPurchase().Call<long>("getPurchaseTimestamp");
        }

        public string GetAdditionalParameters()
        {
            return GetInAppPurchase().Call<AndroidJavaObject>("getAdditionalParameters").Call<string>("toString");
        }

        public string GetDeveloperPayload()
        {
            return GetInAppPurchase().Call<string>("getDeveloperPayload");
        }
    }
}
