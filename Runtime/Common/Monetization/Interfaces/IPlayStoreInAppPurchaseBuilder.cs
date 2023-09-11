// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Common
{
    /// <summary>
    /// <para>
    /// Interface containing method signatures of the <see langword="PlayStoreInAppPurchaseBuilder"/> class.
    /// </para>
    /// See <see href="https://docs.appodeal.com/unity/advanced/in-app-purchases?distribution=upm"/> for more details.
    /// </summary>
    public interface IPlayStoreInAppPurchaseBuilder : IInAppPurchaseBaseBuilder
    {
        void WithSku(string sku);
        void WithOrderId(string orderId);
        void WithPublicKey(string publicKey);
        void WithSignature(string signature);
        void WithPurchaseData(string purchaseData);
        void WithPurchaseToken(string purchaseToken);
        void WithDeveloperPayload(string developerPayload);
        void WithPurchaseTimestamp(long purchaseTimestamp);
        IPlayStoreInAppPurchase Build();
    }
}
