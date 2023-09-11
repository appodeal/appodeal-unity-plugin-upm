using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AppodealStack.Monetization.Common;
using AppodealStack.Monetization.Platforms;

// ReSharper disable CheckNamespace
namespace AppodealStack.Monetization.Api
{
    /// <summary>
    /// <para>PlayStoreInAppPurchase Unity API for developers, including documentation.</para>
    /// See <see href="https://docs.appodeal.com/unity/advanced/in-app-purchases?distribution=upm"/> for more details.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class PlayStoreInAppPurchase : IPlayStoreInAppPurchase
    {
        /// <summary>
        /// Provides access to a native object that implements IPlayStoreInAppPurchase interface.
        /// </summary>
        public IPlayStoreInAppPurchase NativeInAppPurchase { get; }

        /// <summary>
        /// Public constructor of the <see langword="PlayStoreInAppPurchase"/> class.
        /// </summary>
        /// <param name="purchase">class which implements AppodealStack.Monetization.Common.IPlayStoreInAppPurchase interface.</param>
        public PlayStoreInAppPurchase(IPlayStoreInAppPurchase purchase)
        {
            NativeInAppPurchase = purchase.NativeInAppPurchase;
        }

        /// <summary>
        /// <para>Gets the purchase type.</para>
        /// See <see href="https://docs.appodeal.com/unity/advanced/in-app-purchases?distribution=upm"/> for more details.
        /// </summary>
        /// <returns>Type of the purchase as PlayStorePurchaseType object.</returns>
        public PlayStorePurchaseType GetPurchaseType()
        {
            return NativeInAppPurchase.GetPurchaseType();
        }

        /// <summary>
        /// <para>Gets the public key of the purchase.</para>
        /// See <see href="https://docs.appodeal.com/unity/advanced/in-app-purchases?distribution=upm"/> for more details.
        /// </summary>
        /// <returns>Public key as string.</returns>
        public string GetPublicKey()
        {
            return NativeInAppPurchase.GetPublicKey();
        }

        /// <summary>
        /// <para>Gets the signature of the purchase.</para>
        /// See <see href="https://docs.appodeal.com/unity/advanced/in-app-purchases?distribution=upm"/> for more details.
        /// </summary>
        /// <returns>Signature as string.</returns>
        public string GetSignature()
        {
            return NativeInAppPurchase.GetSignature();
        }

        /// <summary>
        /// <para>Gets the purchase data of the purchase.</para>
        /// See <see href="https://docs.appodeal.com/unity/advanced/in-app-purchases?distribution=upm"/> for more details.
        /// </summary>
        /// <returns>Purchase data as string.</returns>
        public string GetPurchaseData()
        {
            return NativeInAppPurchase.GetPurchaseData();
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
        /// <para>Gets the SKU of the purchase.</para>
        /// See <see href="https://docs.appodeal.com/unity/advanced/in-app-purchases?distribution=upm"/> for more details.
        /// </summary>
        /// <returns>SKU as string.</returns>
        public string GetSku()
        {
            return NativeInAppPurchase.GetSku();
        }

        /// <summary>
        /// <para>Gets the order id of the purchase.</para>
        /// See <see href="https://docs.appodeal.com/unity/advanced/in-app-purchases?distribution=upm"/> for more details.
        /// </summary>
        /// <returns>Order id as string.</returns>
        public string GetOrderId()
        {
            return NativeInAppPurchase.GetOrderId();
        }

        /// <summary>
        /// <para>Gets the token of the purchase.</para>
        /// See <see href="https://docs.appodeal.com/unity/advanced/in-app-purchases?distribution=upm"/> for more details.
        /// </summary>
        /// <returns>Purchase token as string.</returns>
        public string GetPurchaseToken()
        {
            return NativeInAppPurchase.GetPurchaseToken();
        }

        /// <summary>
        /// <para>Gets the timestamp of the purchase.</para>
        /// See <see href="https://docs.appodeal.com/unity/advanced/in-app-purchases?distribution=upm"/> for more details.
        /// </summary>
        /// <returns>Purchase timestamp as string.</returns>
        public long GetPurchaseTimestamp()
        {
            return NativeInAppPurchase.GetPurchaseTimestamp();
        }

        /// <summary>
        /// <para>Gets the developer payload of the purchase.</para>
        /// See <see href="https://docs.appodeal.com/unity/advanced/in-app-purchases?distribution=upm"/> for more details.
        /// </summary>
        /// <returns>Developer payload as string.</returns>
        public string GetDeveloperPayload()
        {
            return NativeInAppPurchase.GetDeveloperPayload();
        }

        /// <summary>
        /// Builder class is responsible for creating an object of the <see langword="PlayStoreInAppPurchase"/> class.
        /// </summary>
        public class Builder
        {
            private readonly IPlayStoreInAppPurchaseBuilder _playStoreInAppPurchaseBuilder;

            private IPlayStoreInAppPurchaseBuilder GetBuilderInstance()
            {
                return _playStoreInAppPurchaseBuilder;
            }

            /// <summary>
            /// Public constructor of the <see langword="Builder"/> class.
            /// </summary>
            /// <param name="purchaseType">type of the purchase.</param>
            public Builder(PlayStorePurchaseType purchaseType)
            {
                 _playStoreInAppPurchaseBuilder = AppodealAdsClientFactory.GetPlayStoreInAppPurchaseBuilder(purchaseType);
            }

            /// <summary>
            /// Builds the PlayStoreInAppPurchase object using all data you have set via the other Builder's methods.
            /// </summary>
            /// <returns>Object of type <see langword="PlayStoreInAppPurchase"/>.</returns>
            public PlayStoreInAppPurchase Build()
            {
                return new PlayStoreInAppPurchase(GetBuilderInstance().Build());
            }

            /// <summary>
            /// Sets the public key of the purchase.
            /// </summary>
            /// <param name="publicKey">public key as string.</param>
            /// <returns>An instance of the builder class.</returns>
            public Builder WithPublicKey(string publicKey)
            {
                GetBuilderInstance().WithPublicKey(publicKey);
                return this;
            }

            /// <summary>
            /// Sets the signature of the purchase.
            /// </summary>
            /// <param name="signature">purchase signature as string.</param>
            /// <returns>An instance of the builder class.</returns>
            public Builder WithSignature(string signature)
            {
                GetBuilderInstance().WithSignature(signature);
                return this;
            }

            /// <summary>
            /// Sets the purchase data.
            /// </summary>
            /// <param name="purchaseData">purchase data as string.</param>
            /// <returns>An instance of the builder class.</returns>
            public Builder WithPurchaseData(string purchaseData)
            {
                GetBuilderInstance().WithPurchaseData(purchaseData);
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
            /// Sets the SKU of the purchase.
            /// </summary>
            /// <param name="sku">purchase SKU as string.</param>
            /// <returns>An instance of the builder class.</returns>
            public Builder WithSku(string sku)
            {
                GetBuilderInstance().WithSku(sku);
                return this;
            }

            /// <summary>
            /// Sets the order id of the purchase.
            /// </summary>
            /// <param name="orderId">order id as string.</param>
            /// <returns>An instance of the builder class.</returns>
            public Builder WithOrderId(string orderId)
            {
                GetBuilderInstance().WithOrderId(orderId);
                return this;
            }

            /// <summary>
            /// Sets the token of the purchase.
            /// </summary>
            /// <param name="purchaseToken">Purchase token as string.</param>
            /// <returns>An instance of the builder class.</returns>
            public Builder WithPurchaseToken(string purchaseToken)
            {
                GetBuilderInstance().WithPurchaseToken(purchaseToken);
                return this;
            }

            /// <summary>
            /// Sets the timestamp of the purchase.
            /// </summary>
            /// <param name="purchaseTimestamp">purchase timestamp as long.</param>
            /// <returns>An instance of the builder class.</returns>
            public Builder WithPurchaseTimestamp(long purchaseTimestamp)
            {
                GetBuilderInstance().WithPurchaseTimestamp(purchaseTimestamp);
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

            /// <summary>
            /// Sets the developer payload of the purchase.
            /// </summary>
            /// <param name="developerPayload">developer payload as string.</param>
            /// <returns>An instance of the builder class.</returns>
            public Builder WithDeveloperPayload(string developerPayload)
            {
                GetBuilderInstance().WithDeveloperPayload(developerPayload);
                return this;
            }
        }
    }
}
