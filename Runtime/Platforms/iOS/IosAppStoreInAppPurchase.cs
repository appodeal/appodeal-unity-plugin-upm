// ReSharper disable CheckNamespace

using System;
using System.Collections.Generic;
using System.Linq;
using AppodealStack.Monetization.Common;

namespace AppodealStack.Monetization.Platforms.Ios
{
    internal class IosAppStoreInAppPurchase : IAppStoreInAppPurchase
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
