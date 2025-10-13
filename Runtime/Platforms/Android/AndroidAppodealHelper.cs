// ReSharper disable CheckNamespace

using System.Runtime.CompilerServices;
using UnityEngine;
using AppodealStack.Monetization.Common;

namespace AppodealStack.Monetization.Platforms.Android
{
    internal static class AndroidAppodealHelper
    {
        public static int GetJavaAdTypes(int adTypes)
        {
            int javaAdTypes = 0;

            if ((adTypes & AppodealAdType.Interstitial) > 0) javaAdTypes |= AndroidConstants.JavaAdType.Interstitial;
            if ((adTypes & AppodealAdType.Banner) > 0) javaAdTypes |= AndroidConstants.JavaAdType.Banner;
            if ((adTypes & AppodealAdType.RewardedVideo) > 0) javaAdTypes |= AndroidConstants.JavaAdType.RewardedVideo;
            if ((adTypes & AppodealAdType.Mrec) > 0) javaAdTypes |= AndroidConstants.JavaAdType.Mrec;

            return javaAdTypes;
        }

        public static int GetJavaShowStyle(int showStyle)
        {
            if ((showStyle & AppodealShowStyle.Interstitial) > 0) return AndroidConstants.JavaShowStyle.Interstitial;
            if ((showStyle & AppodealShowStyle.BannerBottom) > 0) return AndroidConstants.JavaShowStyle.BannerBottom;
            if ((showStyle & AppodealShowStyle.BannerTop) > 0) return AndroidConstants.JavaShowStyle.BannerTop;
            if ((showStyle & AppodealShowStyle.BannerLeft) > 0) return AndroidConstants.JavaShowStyle.BannerLeft;
            if ((showStyle & AppodealShowStyle.BannerRight) > 0) return AndroidConstants.JavaShowStyle.BannerRight;
            if ((showStyle & AppodealShowStyle.RewardedVideo) > 0) return AndroidConstants.JavaShowStyle.RewardedVideo;

            return 0;
        }

        public static int GetJavaYAxisPosition(int viewPosition)
        {
            return viewPosition switch
            {
                AppodealViewPosition.VerticalBottom => AndroidConstants.JavaShowStyle.BannerBottom,
                AppodealViewPosition.VerticalTop => AndroidConstants.JavaShowStyle.BannerTop,
                _ => viewPosition
            };
        }

        public static object GetJavaObject(object value)
        {
            if (value is not bool && value is not char && value is not int && value is not long && value is not float && value is not double && value is not string)
            {
                Debug.LogError($"[Appodeal Unity Plugin] Conversion of {value.GetType()} type to java is not implemented");
            }

            return value switch
            {
                bool => new AndroidJavaObject(AndroidConstants.JavaClassName.Boolean, value),
                char => new AndroidJavaObject(AndroidConstants.JavaClassName.Character, value),
                int => new AndroidJavaObject(AndroidConstants.JavaClassName.Integer, value),
                long => new AndroidJavaObject(AndroidConstants.JavaClassName.Long, value),
                float => new AndroidJavaObject(AndroidConstants.JavaClassName.Float, value),
                double => new AndroidJavaObject(AndroidConstants.JavaClassName.Double, value),
                string => value,
                _ => null
            };
        }

        public static void LogMethodNotSupported([CallerMemberName] string methodName = "")
        {
            Debug.LogWarning($"{AndroidConstants.MethodNotSupportedMessage} {methodName}()");
        }

        public static void LogIntegrationError(string message)
        {
            Debug.LogError($"{AndroidConstants.IntegrationErrorMessage} {message}");
        }
    }
}
