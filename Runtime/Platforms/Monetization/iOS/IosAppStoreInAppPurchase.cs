using System;
using System.Linq;
using System.Collections.Generic;
using AppodealStack.Monetization.Common;

// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Platforms.Ios
{
    public class IosAppStoreInAppPurchase : IAppStoreInAppPurchase
    {
        public IAppStoreInAppPurchase NativeInAppPurchase { get; }
        private AppStorePurchaseType PurchaseType { get; }

        public string Price { get; set; } = String.Empty;
        public string Currency { get; set; } = String.Empty;
        public string ProductId { get; set; } = String.Empty;
        public string TransactionId { get; set; } = String.Empty;
        public Dictionary<string, string> AdditionalParameters { get; set; }

        public IosAppStoreInAppPurchase(AppStorePurchaseType purchaseType)
        {
            PurchaseType = purchaseType;
            NativeInAppPurchase = this;
        }

        public AppStorePurchaseType GetPurchaseType()
        {
            return PurchaseType;
        }

        public string GetProductId()
        {
            return ProductId;
        }

        public string GetTransactionId()
        {
            return TransactionId;
        }

        public string GetPrice()
        {
            return Price;
        }

        public string GetCurrency()
        {
            return Currency;
        }

        public string GetAdditionalParameters()
        {
            return AdditionalParameters?.Aggregate("", (current, keyValues) => current + (keyValues.Key + "=" + keyValues.Value + ",")).TrimEnd(',') ?? String.Empty;
        }
    }
}
