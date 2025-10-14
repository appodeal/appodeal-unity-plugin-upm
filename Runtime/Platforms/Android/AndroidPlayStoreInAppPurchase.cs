// ReSharper disable CheckNamespace

using System;
using UnityEngine;
using AppodealStack.Monetization.Common;

namespace AppodealStack.Monetization.Platforms.Android
{
    internal class AndroidPlayStoreInAppPurchase : IPlayStoreInAppPurchase
    {
        public IPlayStoreInAppPurchase NativeInAppPurchase { get; }

        private readonly AndroidJavaObject _inAppPurchase;

        internal AndroidPlayStoreInAppPurchase(AndroidJavaObject inAppPurchase)
        {
            _inAppPurchase = inAppPurchase;
            NativeInAppPurchase = this;
        }

        public AndroidJavaObject GetInAppPurchase()
        {
            return _inAppPurchase;
        }

        public PlayStorePurchaseType GetPurchaseType()
        {
            string type = GetInAppPurchase().Call<AndroidJavaObject>(AndroidConstants.JavaMethodName.InAppPurchase.GetPurchaseType).Call<string>("toString");

            return type switch
            {
                AndroidConstants.JavaFieldName.AppodealPlayStorePurchaseTypeSubs => PlayStorePurchaseType.Subs,
                AndroidConstants.JavaFieldName.AppodealPlayStorePurchaseTypeInApp => PlayStorePurchaseType.InApp,
                _ => throw new ArgumentOutOfRangeException(nameof(PlayStorePurchaseType), type, null)
            };
        }

        public string GetPublicKey()
        {
            return GetInAppPurchase().Call<string>(AndroidConstants.JavaMethodName.InAppPurchase.GetPublicKey);
        }

        public string GetSignature()
        {
            return GetInAppPurchase().Call<string>(AndroidConstants.JavaMethodName.InAppPurchase.GetSignature);
        }

        public string GetPurchaseData()
        {
            return GetInAppPurchase().Call<string>(AndroidConstants.JavaMethodName.InAppPurchase.GetPurchaseData);
        }

        public string GetPrice()
        {
            return GetInAppPurchase().Call<string>(AndroidConstants.JavaMethodName.InAppPurchase.GetPrice);
        }

        public string GetCurrency()
        {
            return GetInAppPurchase().Call<string>(AndroidConstants.JavaMethodName.InAppPurchase.GetCurrency);
        }

        public string GetSku()
        {
            return GetInAppPurchase().Call<string>(AndroidConstants.JavaMethodName.InAppPurchase.GetSku);
        }

        public string GetOrderId()
        {
            return GetInAppPurchase().Call<string>(AndroidConstants.JavaMethodName.InAppPurchase.GetOrderId);
        }

        public string GetPurchaseToken()
        {
            return GetInAppPurchase().Call<string>(AndroidConstants.JavaMethodName.InAppPurchase.GetPurchaseToken);
        }

        public long GetPurchaseTimestamp()
        {
            return GetInAppPurchase().Call<long>(AndroidConstants.JavaMethodName.InAppPurchase.GetPurchaseTimestamp);
        }

        public string GetAdditionalParameters()
        {
            return GetInAppPurchase().Call<AndroidJavaObject>(AndroidConstants.JavaMethodName.InAppPurchase.GetAdditionalParameters).Call<string>("toString");
        }

        public string GetDeveloperPayload()
        {
            return GetInAppPurchase().Call<string>(AndroidConstants.JavaMethodName.InAppPurchase.GetDeveloperPayload);
        }
    }
}
