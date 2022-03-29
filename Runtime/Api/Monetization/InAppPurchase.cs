using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AppodealStack.Monetization.Common;
using AppodealStack.Monetization.Platforms;

// ReSharper disable CheckNamespace
namespace AppodealStack.Monetization.Api
{
    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class InAppPurchase : IInAppPurchase
    {
        public object NativeInAppPurchaseObject { get; }

        private readonly IInAppPurchase _inAppPurchase;

        private IInAppPurchase GetInstance()
        {
            return _inAppPurchase;
        }

        /// <summary>
        /// Public constructor of the <see langword="InAppPurchase"/> class.
        /// </summary>
        /// <param name="purchase">class which implements AppodealStack.Monetization.Common.IInAppPurchase interface.</param>
        public InAppPurchase(IInAppPurchase purchase)
        {
            _inAppPurchase = purchase;
            NativeInAppPurchaseObject = purchase.NativeInAppPurchaseObject;
        }

        /// <summary>
        /// <para>Gets the purchase type.</para>
        /// See <see href=""/> for more details.
        /// </summary>
        /// <returns>Type of the purchase as AndroidPurchaseType object.</returns>
        public AndroidPurchaseType GetPurchaseType()
        {
            return GetInstance().GetPurchaseType();
        }

        /// <summary>
        /// <para>Gets the public key of the purchase.</para>
        /// See <see href=""/> for more details.
        /// </summary>
        /// <returns>Public key as string.</returns>
        public string GetPublicKey()
        {
            return GetInstance().GetPublicKey();
        }

        /// <summary>
        /// <para>Gets the signature of the purchase.</para>
        /// See <see href=""/> for more details.
        /// </summary>
        /// <returns>Signature as string.</returns>
        public string GetSignature()
        {
            return GetInstance().GetSignature();
        }

        /// <summary>
        /// <para>Gets the purchase data of the purchase.</para>
        /// See <see href=""/> for more details.
        /// </summary>
        /// <returns>Purchase data as string.</returns>
        public string GetPurchaseData()
        {
            return GetInstance().GetPurchaseData();
        }

        /// <summary>
        /// <para>Gets the price of the purchase.</para>
        /// See <see href=""/> for more details.
        /// </summary>
        /// <returns>Price as string.</returns>
        public string GetPrice()
        {
            return GetInstance().GetPrice();
        }

        /// <summary>
        /// <para>Gets the currency of the purchase.</para>
        /// See <see href=""/> for more details.
        /// </summary>
        /// <returns>Currency as string.</returns>
        public string GetCurrency()
        {
            return GetInstance().GetCurrency();
        }

        /// <summary>
        /// <para>Gets the additional parameters of the purchase.</para>
        /// See <see href=""/> for more details.
        /// </summary>
        /// <returns>Additional parameters as string.</returns>
        public string GetAdditionalParameters()
        {
            return GetInstance().GetAdditionalParameters();
        }

        /// <summary>
        /// <para>Gets the SKU of the purchase.</para>
        /// See <see href=""/> for more details.
        /// </summary>
        /// <returns>SKU as string.</returns>
        public string GetSku()
        {
            return GetInstance().GetSku();
        }

        /// <summary>
        /// <para>Gets the order id of the purchase.</para>
        /// See <see href=""/> for more details.
        /// </summary>
        /// <returns>Order id as string.</returns>
        public string GetOrderId()
        {
            return GetInstance().GetOrderId();
        }

        /// <summary>
        /// <para>Gets the token of the purchase.</para>
        /// See <see href=""/> for more details.
        /// </summary>
        /// <returns>Purchase token as string.</returns>
        public string GetPurchaseToken()
        {
            return GetInstance().GetPurchaseToken();
        }

        /// <summary>
        /// <para>Gets the timestamp of the purchase.</para>
        /// See <see href=""/> for more details.
        /// </summary>
        /// <returns>Purchase timestamp as string.</returns>
        public long GetPurchaseTimestamp()
        {
            return GetInstance().GetPurchaseTimestamp();
        }

        /// <summary>
        /// <para>Gets the developer payload of the purchase.</para>
        /// See <see href=""/> for more details.
        /// </summary>
        /// <returns>Developer payload as string.</returns>
        public string GetDeveloperPayload()
        {
            return GetInstance().GetDeveloperPayload();
        }

        #region Deprecated methods

        [Obsolete("It will be removed in the next release. Use the GetPurchaseType() method instead.", false)]
        public AndroidPurchaseType getType()
        {
            return GetInstance().GetPurchaseType();
        }

        [Obsolete("It will be removed in the next release. Use the capitalized version (GetPublicKey) of this method instead.", false)]
        public string getPublicKey()
        {
            return GetInstance().GetPublicKey();
        }

        [Obsolete("It will be removed in the next release. Use the capitalized version (GetSignature) of this method instead.", false)]
        public string getSignature()
        {
            return GetInstance().GetSignature();
        }

        [Obsolete("It will be removed in the next release. Use the capitalized version (GetPurchaseData) of this method instead.", false)]
        public string getPurchaseData()
        {
            return GetInstance().GetPurchaseData();
        }

        [Obsolete("It will be removed in the next release. Use the capitalized version (GetPrice) of this method instead.", false)]
        public string getPrice()
        {
            return GetInstance().GetPrice();
        }

        [Obsolete("It will be removed in the next release. Use the capitalized version (GetCurrency) of this method instead.", false)]
        public string getCurrency()
        {
            return GetInstance().GetCurrency();
        }

        [Obsolete("It will be removed in the next release. Use the capitalized version (GetAdditionalParameters) of this method instead.", false)]
        public string getAdditionalParameters()
        {
            return GetInstance().GetAdditionalParameters();
        }

        [Obsolete("It will be removed in the next release. Use the capitalized version (GetSku) of this method instead.", false)]
        public string getSku()
        {
            return GetInstance().GetSku();
        }

        [Obsolete("It will be removed in the next release. Use the capitalized version (GetOrderId) of this method instead.", false)]
        public string getOrderId()
        {
            return GetInstance().GetOrderId();
        }

        [Obsolete("It will be removed in the next release. Use the capitalized version (GetPurchaseToken) of this method instead.", false)]
        public string getPurchaseToken()
        {
            return GetInstance().GetPurchaseToken();
        }

        [Obsolete("It will be removed in the next release. Use the capitalized version (GetPurchaseTimestamp) of this method instead.", false)]
        public long getPurchaseTimestamp()
        {
            return GetInstance().GetPurchaseTimestamp();
        }

        [Obsolete("It will be removed in the next release. Use the capitalized version (GetDeveloperPayload) of this method instead.", false)]
        public string getDeveloperPayload()
        {
            return GetInstance().GetDeveloperPayload();
        }

        #endregion

        /// <summary>
        /// Builder class is responsible for creating an object of the <see langword="InAppPurchase"/> class.
        /// </summary>
        public class Builder
        {
            private readonly IInAppPurchaseBuilder _inAppPurchaseBuilder;

            private IInAppPurchaseBuilder GetBuilderInstance()
            {
                return _inAppPurchaseBuilder;
            }

            /// <summary>
            /// Public constructor of the <see langword="Builder"/> class.
            /// </summary>
            /// <param name="purchaseType">type of the purchase.</param>
            public Builder(AndroidPurchaseType purchaseType)
            {
                 _inAppPurchaseBuilder = AppodealAdsClientFactory.GetInAppPurchaseBuilder(purchaseType);
            }

            /// <summary>
            /// Builds the InAppPurchase object using all data you have set via the other Builder's methods.
            /// </summary>
            /// <returns>Object of type <see langword="InAppPurchase"/>.</returns>
            public InAppPurchase Build()
            {
                return new InAppPurchase(GetBuilderInstance().Build());
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
            public Builder WithAdditionalParams(Dictionary<string, string> additionalParameters)
            {
                GetBuilderInstance().WithAdditionalParams(additionalParameters);
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

            #region Deprecated Methods

            [Obsolete("It will be removed in the next release. Use the capitalized version (Build) of this method instead.", false)]
            public InAppPurchase build()
            {
                return new InAppPurchase(GetBuilderInstance().Build());
            }

            [Obsolete("It will be removed in the next release. Use the capitalized version (WithPublicKey) of this method instead.", false)]
            public Builder withPublicKey(string publicKey)
            {
                GetBuilderInstance().WithPublicKey(publicKey);
                return this;
            }

            [Obsolete("It will be removed in the next release. Use the capitalized version (WithSignature) of this method instead.", false)]
            public Builder withSignature(string signature)
            {
                GetBuilderInstance().WithSignature(signature);
                return this;
            }

            [Obsolete("It will be removed in the next release. Use the capitalized version (WithPurchaseData) of this method instead.", false)]
            public Builder withPurchaseData(string purchaseData)
            {
                GetBuilderInstance().WithPurchaseData(purchaseData);
                return this;
            }

            [Obsolete("It will be removed in the next release. Use the capitalized version (WithPrice) of this method instead.", false)]
            public Builder withPrice(string price)
            {
                GetBuilderInstance().WithPrice(price);
                return this;
            }

            [Obsolete("It will be removed in the next release. Use the capitalized version (WithCurrency) of this method instead.", false)]
            public Builder withCurrency(string currency)
            {
                GetBuilderInstance().WithCurrency(currency);
                return this;
            }

            [Obsolete("It will be removed in the next release. Use the capitalized version (WithSku) of this method instead.", false)]
            public Builder withSku(string sku)
            {
                GetBuilderInstance().WithSku(sku);
                return this;
            }

            [Obsolete("It will be removed in the next release. Use the capitalized version (WithOrderId) of this method instead.", false)]
            public Builder withOrderId(string orderId)
            {
                GetBuilderInstance().WithOrderId(orderId);
                return this;
            }

            [Obsolete("It will be removed in the next release. Use the capitalized version (WithPurchaseToken) of this method instead.", false)]
            public Builder withPurchaseToken(string purchaseToken)
            {
                GetBuilderInstance().WithPurchaseToken(purchaseToken);
                return this;
            }

            [Obsolete("It will be removed in the next release. Use the capitalized version (WithPurchaseTimestamp) of this method instead.", false)]
            public Builder withPurchaseTimestamp(long purchaseTimestamp)
            {
                GetBuilderInstance().WithPurchaseTimestamp(purchaseTimestamp);
                return this;
            }

            [Obsolete("It will be removed in the next release. Use the capitalized version (WithAdditionalParams) of this method instead.", false)]
            public Builder withAdditionalParams(Dictionary<string, string> additionalParameters)
            {
                GetBuilderInstance().WithAdditionalParams(additionalParameters);
                return this;
            }

            [Obsolete("It will be removed in the next release. Use the capitalized version (WithDeveloperPayload) of this method instead.", false)]
            public Builder withDeveloperPayload(string developerPayload)
            {
                GetBuilderInstance().WithDeveloperPayload(developerPayload);
                return this;
            }

            #endregion

        }
    }
}
