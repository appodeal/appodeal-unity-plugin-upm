using System.Collections.Generic;

// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Common
{
    /// <summary>
    /// <para>
    /// Interface containing method signatures of the <see langword="InAppPurchase"/> class.
    /// </para>
    /// See <see href=""/> for more details.
    /// </summary>
    public interface IInAppPurchase
    {
        AndroidPurchaseType GetPurchaseType();
        string GetPublicKey();
        string GetSignature();
        string GetPurchaseData();
        string GetPrice();
        string GetCurrency();
        string GetSku();
        string GetOrderId();
        string GetPurchaseToken();
        long GetPurchaseTimestamp();
        string GetAdditionalParameters();
        string GetDeveloperPayload();
        object NativeInAppPurchaseObject { get; }
    }

    /// <summary>
    /// <para>
    /// Interface containing method signatures of the <see langword="InAppPurchaseBuilder"/> class.
    /// </para>
    /// See <see href=""/> for more details.
    /// </summary>
    public interface IInAppPurchaseBuilder
    {
        void WithPublicKey(string publicKey);
        void WithSignature(string signature);
        void WithPurchaseData(string purchaseData);
        void WithPrice(string price);
        void WithCurrency(string currency);
        void WithSku(string sku);
        void WithOrderId(string orderId);
        void WithDeveloperPayload(string developerPayload);
        void WithPurchaseToken(string purchaseToken);
        void WithPurchaseTimestamp(long purchaseTimestamp);
        void WithAdditionalParams(Dictionary<string, string> additionalParameters);
        IInAppPurchase Build();
    }
}
