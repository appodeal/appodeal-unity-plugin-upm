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
    /// Android implementation of the <see cref="AppodealStack.Monetization.Common.IPurchaseListener"/> interface.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal class AppodealPurchaseCallbacks : AndroidJavaProxy
    {
        private readonly IPurchaseListener _listener;

        internal AppodealPurchaseCallbacks(IPurchaseListener listener) : base(AndroidConstants.JavaInterfaceName.PurchaseCallbacks)
        {
            _listener = listener;
        }

        [Preserve]
        private void onPurchaseReceived(AndroidJavaObject purchases)
        {
            UnityMainThreadDispatcher.Post(_ => _listener?.OnPurchaseValidationSucceeded(GetPurchases(purchases)));
        }

        [Preserve]
        private void onPurchaseFailed(AndroidJavaObject reason, AndroidJavaObject purchases)
        {
            string message = reason?.Call<string>("getMessage");
            UnityMainThreadDispatcher.Post(_ => _listener?.OnPurchaseValidationFailed(message, GetPurchases(purchases)));
        }

        private static IEnumerable<string> GetPurchases(AndroidJavaObject jList)
        {
            if (jList == null) return Array.Empty<string>();

            try
            {
                int countOfEntries = jList.Call<int>("size");
                var purchases = new List<string>(countOfEntries);

                for (int i = 0; i < countOfEntries; i++)
                {
                    using var jEntry = jList.Call<AndroidJavaObject>("get", i);
                    if (jEntry == null) continue;

                    using var jsonObj = new AndroidJavaObject(AndroidConstants.JavaClassName.JsonObject, jEntry);
                    string purchase = jsonObj.Call<string>("toString");
                    if (purchase != null) purchases.Add(purchase);
                }
                return purchases;
            }
            catch (Exception e)
            {
                Debug.LogError($"[Appodeal Unity Plugin] Purchases parsing failed. Exception: {e.Message}");
                return Array.Empty<string>();
            }
        }
    }
}
