using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AppodealStack.Monetization.Common;
using AppodealStack.Monetization.Platforms;

// ReSharper disable CheckNamespace
namespace AppodealStack.Monetization.Api
{
    /// <summary>
    /// <para>AppStoreInAppPurchase Unity API for developers, including documentation.</para>
    /// See <see href="https://docs.appodeal.com/unity/advanced/in-app-purchases?distribution=upm"/> for more details.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class AppStoreInAppPurchase : IAppStoreInAppPurchase
    {
        /// <summary>
        /// Provides access to a native object that implements IAppStoreInAppPurchase interface.
        /// </summary>
        public IAppStoreInAppPurchase NativeInAppPurchase { get; }

        /// <summary>
        /// Public constructor of the <see langword="AppStoreInAppPurchase"/> class.
        /// </summary>
        /// <param name="purchase">class which implements AppodealStack.Monetization.Common.IAppStoreInAppPurchase interface.</param>
        public AppStoreInAppPurchase(IAppStoreInAppPurchase purchase)
        {
            NativeInAppPurchase = purchase.NativeInAppPurchase;
        }

        /// <summary>
        /// <para>Gets the purchase type.</para>
        /// See <see href="https://docs.appodeal.com/unity/advanced/in-app-purchases?distribution=upm"/> for more details.
        /// </summary>
        /// <returns>Type of the purchase as AppStorePurchaseType object.</returns>
        public AppStorePurchaseType GetPurchaseType()
        {
            return NativeInAppPurchase.GetPurchaseType();
        }

        /// <summary>
        /// <para>Gets an id of the purchased product.</para>
        /// See <see href="https://docs.appodeal.com/unity/advanced/in-app-purchases?distribution=upm"/> for more details.
        /// </summary>
        /// <returns>Product Id as string.</returns>
        public string GetProductId()
        {
            return NativeInAppPurchase.GetProductId();
        }

        /// <summary>
        /// <para>Gets the transaction id of the purchase.</para>
        /// See <see href="https://docs.appodeal.com/unity/advanced/in-app-purchases?distribution=upm"/> for more details.
        /// </summary>
        /// <returns>Id of the transaction as string.</returns>
        public string GetTransactionId()
        {
            return NativeInAppPurchase.GetTransactionId();
        }

        /// <summary>
        /// <para>Gets the price of the purchase.</para>
        /// See <see href="https://docs.appodeal.com/unity/advanced/in-app-purchases?distribution=upm"/> for more details.
        /// </summary>
        /// <returns>Price as string.</returns>
        public string GetPrice()
        {
            return NativeInAppPurchase.GetPrice();
        }

        /// <summary>
        /// <para>Gets the currency of the purchase.</para>
        /// See <see href="https://docs.appodeal.com/unity/advanced/in-app-purchases?distribution=upm"/> for more details.
        /// </summary>
        /// <returns>Currency as string.</returns>
        public string GetCurrency()
        {
            return NativeInAppPurchase.GetCurrency();
        }

        /// <summary>
        /// <para>Gets the additional parameters of the purchase.</para>
        /// See <see href="https://docs.appodeal.com/unity/advanced/in-app-purchases?distribution=upm"/> for more details.
        /// </summary>
        /// <returns>Additional parameters as string.</returns>
        public string GetAdditionalParameters()
        {
            return NativeInAppPurchase.GetAdditionalParameters();
        }

        /// <summary>
        /// Builder class is responsible for creating an object of the <see langword="AppStoreInAppPurchase"/> class.
        /// </summary>
        public class Builder
        {
            private readonly IAppStoreInAppPurchaseBuilder _appStoreInAppPurchaseBuilder;

            private IAppStoreInAppPurchaseBuilder GetBuilderInstance()
            {
                return _appStoreInAppPurchaseBuilder;
            }

            /// <summary>
            /// Public constructor of the <see langword="Builder"/> class.
            /// </summary>
            /// <param name="purchaseType">type of the purchase.</param>
            public Builder(AppStorePurchaseType purchaseType)
            {
                 _appStoreInAppPurchaseBuilder = AppodealAdsClientFactory.GetAppStoreInAppPurchaseBuilder(purchaseType);
            }

            /// <summary>
            /// Builds the AppStoreInAppPurchase object using all data you have set via the other Builder's methods.
            /// </summary>
            /// <returns>Object of type <see langword="AppStoreInAppPurchase"/>.</returns>
            public AppStoreInAppPurchase Build()
            {
                return new AppStoreInAppPurchase(GetBuilderInstance().Build());
            }

            /// <summary>
            /// Sets an id of the purchased product.
            /// </summary>
            /// <param name="productId">product id as string.</param>
            /// <returns>An instance of the builder class.</returns>
            public Builder WithProductId(string productId)
            {
                GetBuilderInstance().WithProductId(productId);
                return this;
            }

            /// <summary>
            /// Sets the transaction id of the purchase.
            /// </summary>
            /// <param name="transactionId">id of the transaction as string.</param>
            /// <returns>An instance of the builder class.</returns>
            public Builder WithTransactionId(string transactionId)
            {
                GetBuilderInstance().WithTransactionId(transactionId);
                return this;
            }

            /// <summary>
            /// Sets the price of the purchase.
            /// </summary>
            /// <param name="price">purchase price as string.</param>
            /// <returns>An instance of the builder class.</returns>
            public Builder WithPrice(string price)
            {
                GetBuilderInstance().WithPrice(price);
                return this;
            }

            /// <summary>
            /// Sets the currency of the purchase.
            /// </summary>
            /// <param name="currency">purchase currency as string.</param>
            /// <returns>An instance of the builder class.</returns>
            public Builder WithCurrency(string currency)
            {
                GetBuilderInstance().WithCurrency(currency);
                return this;
            }

            /// <summary>
            /// Sets the additional parameters of the purchase.
            /// </summary>
            /// <param name="additionalParameters">additional parameters as string.</param>
            /// <returns>An instance of the builder class.</returns>
            public Builder WithAdditionalParameters(Dictionary<string, string> additionalParameters)
            {
                GetBuilderInstance().WithAdditionalParameters(additionalParameters);
                return this;
            }
        }
    }
}
