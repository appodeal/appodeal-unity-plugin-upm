using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AppodealStack.Monetization.Common;

// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Platforms.Ios
{
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    public class IosAppStoreInAppPurchaseBuilder : IAppStoreInAppPurchaseBuilder
    {
        private readonly IosAppStoreInAppPurchase _purchase;

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
