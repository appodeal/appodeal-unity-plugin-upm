// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Common
{
    /// <summary>
    /// <para>
    /// Interface containing method signatures of the <see langword="PlayStoreInAppPurchase"/> class.
    /// </para>
    /// See <see href="https://docs.appodeal.com/unity/advanced/in-app-purchases?distribution=upm"/> for more details.
    /// </summary>
    public interface IPlayStoreInAppPurchase : IInAppPurchaseBase
    {
        PlayStorePurchaseType GetPurchaseType();
        string GetSku();
        string GetOrderId();
        string GetSignature();
        string GetPublicKey();
        string GetPurchaseData();
        string GetPurchaseToken();
        string GetDeveloperPayload();
        long GetPurchaseTimestamp();
        IPlayStoreInAppPurchase NativeInAppPurchase { get; }
    }
}
