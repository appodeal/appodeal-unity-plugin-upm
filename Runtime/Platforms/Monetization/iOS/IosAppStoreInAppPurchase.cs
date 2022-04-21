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

        public string Price { get; set; }
        public string Currency { get; set; }
        public string ProductId { get; set; }
        public string TransactionId { get; set; }
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
            var dictionaryString = AdditionalParameters.Aggregate("", (current, keyValues) => current + (keyValues.Key + "=" + keyValues.Value + ","));
            return dictionaryString.TrimEnd(',');
        }
    }
}
