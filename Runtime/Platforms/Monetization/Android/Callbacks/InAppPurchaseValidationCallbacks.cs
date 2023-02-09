using System;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using AppodealStack.Monetization.Common;

// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Platforms.Android
{
    /// <summary>
    /// Android implementation of <see langword="IInAppPurchaseValidationListener"/> interface.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public class InAppPurchaseValidationCallbacks : AndroidJavaProxy
    {
        private readonly IInAppPurchaseValidationListener _listener;
        private static SynchronizationContext _unityContext;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void GetContext() => _unityContext = SynchronizationContext.Current;

        internal InAppPurchaseValidationCallbacks(IInAppPurchaseValidationListener listener) : base("com.appodeal.ads.inapp.InAppPurchaseValidateCallback")
        {
            _listener = listener;
        }

        private void onInAppPurchaseValidateSuccess(AndroidJavaObject purchase, AndroidJavaObject errors)
        {
            _unityContext?.Post(obj => _listener?.OnInAppPurchaseValidationSucceeded(CreateResponse(purchase, errors)), null);
        }

        private void onInAppPurchaseValidateFail(AndroidJavaObject purchase, AndroidJavaObject errors)
        {
            _unityContext?.Post(obj => _listener?.OnInAppPurchaseValidationFailed(CreateResponse(purchase, errors)), null);
        }

        private string CreateResponse(AndroidJavaObject purchase, AndroidJavaObject errors)
        {
            var androidPurchase = new AndroidPlayStoreInAppPurchase(purchase);

            string responsePurchase = "\"InAppPurchase\":{";
            responsePurchase += $"\"PublicKey\":\"{androidPurchase.GetPublicKey() ?? String.Empty}\",";
            responsePurchase += $"\"Signature\":\"{androidPurchase.GetSignature() ?? String.Empty}\",";
            responsePurchase += $"\"PurchaseData\":\"{androidPurchase.GetPurchaseData() ?? String.Empty}\",";
            responsePurchase += $"\"Price\":\"{androidPurchase.GetPrice() ?? String.Empty}\",";
            responsePurchase += $"\"Currency\":\"{androidPurchase.GetCurrency() ?? String.Empty}\",";
            responsePurchase += $"\"Sku\":\"{androidPurchase.GetSku() ?? String.Empty}\",";
            responsePurchase += $"\"OrderId\":\"{androidPurchase.GetOrderId() ?? String.Empty}\",";
            responsePurchase += $"\"PurchaseToken\":\"{androidPurchase.GetPurchaseToken() ?? String.Empty}\",";
            responsePurchase += $"\"PurchaseTimestamp\":\"{androidPurchase.GetPurchaseTimestamp()}\",";
            responsePurchase += $"\"AdditionalParameters\":\"{androidPurchase.GetAdditionalParameters() ?? String.Empty}\",";
            responsePurchase += $"\"DeveloperPayload\":\"{androidPurchase.GetDeveloperPayload() ?? String.Empty}\"}}";

            string responseError = "\"Errors\":[";
            if (errors != null)
            {
                var errorsList = new List<string>();
                int countOfErrors = errors.Call<int>("size");
                for (int i = 0; i < countOfErrors; i++)
                {
                    errorsList.Add($"\"{errors.Call<AndroidJavaObject>("get", i).Call<string>("toString")}\"");
                }
                responseError += String.Join(",", errorsList);
            }
            responseError += ']';

            return $"{{{responsePurchase},{responseError}}}";
        }
    }
}
