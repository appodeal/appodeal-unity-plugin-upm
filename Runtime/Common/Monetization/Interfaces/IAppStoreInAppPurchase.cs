// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Common
{
    /// <summary>
    /// <para>
    /// Interface containing method signatures of the <see langword="AppStoreInAppPurchase"/> class.
    /// </para>
    /// See <see href="https://docs.appodeal.com/unity/advanced/in-app-purchases?distribution=upm"/> for more details.
    /// </summary>
    public interface IAppStoreInAppPurchase : IInAppPurchaseBase
    {
        AppStorePurchaseType GetPurchaseType();
        string GetProductId();
        string GetTransactionId();
        IAppStoreInAppPurchase NativeInAppPurchase { get; }
    }
}
