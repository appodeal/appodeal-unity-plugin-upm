using System;
using UnityEngine;
using AppodealStack.Monetization.Common;

// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Platforms.Android
{
    public class AndroidPlayStoreInAppPurchase : IPlayStoreInAppPurchase
    {
        public IPlayStoreInAppPurchase NativeInAppPurchase { get; }

        private readonly AndroidJavaObject _inAppPurchase;

        public AndroidPlayStoreInAppPurchase (AndroidJavaObject inAppPurchase)
        {
            _inAppPurchase = inAppPurchase;
            NativeInAppPurchase = this;
        }

        public AndroidJavaObject GetInAppPurchase()
        {
            return _inAppPurchase;
        }

        public PlayStorePurchaseType GetPurchaseType()
        {
            string type = GetInAppPurchase().Call<AndroidJavaObject>("getType").Call<string>("toString");

            return type switch
            {
                "Subs" => PlayStorePurchaseType.Subs,
                "InApp" => PlayStorePurchaseType.InApp,
                _ => throw new ArgumentOutOfRangeException(nameof(PlayStorePurchaseType), type, null)
            };
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
