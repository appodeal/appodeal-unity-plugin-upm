// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Common
{
    /// <summary>
    /// <para>
    /// Base interface containing common method signatures of the <see langword="InAppPurchase"/> classes.
    /// </para>
    /// See <see href="https://docs.appodeal.com/unity/advanced/in-app-purchases?distribution=upm"/> for more details.
    /// </summary>
    public interface IInAppPurchaseBase
    {
        string GetPrice();
        string GetCurrency();
        string GetAdditionalParameters();
    }
}
