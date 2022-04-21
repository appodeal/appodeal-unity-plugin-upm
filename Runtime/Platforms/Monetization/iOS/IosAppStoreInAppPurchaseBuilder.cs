using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using AppodealStack.Monetization.Common;

// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Platforms.Ios
{
    public class IosAppStoreInAppPurchaseBuilder : IAppStoreInAppPurchaseBuilder
    {
        private IosAppStoreInAppPurchase _purchase;

        public IosAppStoreInAppPurchaseBuilder(AppStorePurchaseType purchaseType)
        {
            _purchase = new IosAppStoreInAppPurchase(purchaseType);
        }

        public IAppStoreInAppPurchase Build()
        {
            return _purchase;
        }

        public void WithProductId(string productId)
        {
            _purchase.ProductId = productId;
        }

        public void WithTransactionId(string transactionId)
        {
            _purchase.TransactionId = transactionId;
        }

        public void WithPrice(string price)
        {
            _purchase.Price = price;
        }

        public void WithCurrency(string currency)
        {
            _purchase.Currency = currency;
        }

        public void WithAdditionalParameters(Dictionary<string, string> additionalParameters)
        {
            _purchase.AdditionalParameters = additionalParameters;
        }
    }
}
