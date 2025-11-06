// ReSharper disable CheckNamespace

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.Scripting;
using AppodealStack.Monetization.Common;

namespace AppodealStack.Monetization.Platforms.Android
{
    /// <summary>
    /// Android implementation of the <see cref="AppodealStack.Monetization.Common.IInAppPurchaseValidationListener"/> interface.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal class InAppPurchaseValidationCallbacks : AndroidJavaProxy
    {
        private readonly IInAppPurchaseValidationListener _listener;

        internal InAppPurchaseValidationCallbacks(IInAppPurchaseValidationListener listener) : base(AndroidConstants.JavaInterfaceName.InAppPurchaseCallbacks)
        {
            _listener = listener;
        }

        [Preserve]
        private void onInAppPurchaseValidateSuccess(AndroidJavaObject purchase, AndroidJavaObject errors)
        {
            UnityMainThreadDispatcher.Post(_ => _listener?.OnInAppPurchaseValidationSucceeded(CreateResponse(purchase, errors)));
        }

        [Preserve]
        private void onInAppPurchaseValidateFail(AndroidJavaObject purchase, AndroidJavaObject errors)
        {
            UnityMainThreadDispatcher.Post(_ => _listener?.OnInAppPurchaseValidationFailed(CreateResponse(purchase, errors)));
        }

        private static string CreateResponse(AndroidJavaObject purchase, AndroidJavaObject errors)
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
                try
                {
                    var errorsList = new List<string>();
                    int countOfErrors = errors.Call<int>("size");
                    for (int i = 0; i < countOfErrors; i++)
                    {
                        errorsList.Add($"\"{errors.Call<AndroidJavaObject>("get", i).Call<string>("toString")}\"");
                    }
                    responseError += String.Join(",", errorsList);
                }
                catch (Exception e)
                {
                    AndroidAppodealHelper.LogIntegrationError(e.Message);
                }
            }
            responseError += ']';

            return $"{{{responsePurchase},{responseError}}}";
        }
    }
}
