// ReSharper Disable CheckNamespace
namespace AppodealStack.Monetization.Common
{
    /// <summary>
    /// <para>
    /// This class is designed to store reward parameters.
    /// </para>
    /// See <see href="https://faq.appodeal.com/en/articles/1133435-reward-setting"/> for more details.
    /// </summary>
    /// <remarks>Amount and Currency are set via app settings on Appodeal website and can be obtained at runtime via <see cref="IRewardedVideoAdListener.OnRewardedVideoFinished"/> callback.</remarks>
    public class AppodealReward
    {
        /// <summary>
        /// <para>
        /// The amount of reward user will get for watching an ad.
        /// </para>
        /// See <see href="https://faq.appodeal.com/en/articles/1133435-reward-setting"/> for more details.
        /// </summary>
        public double Amount;

        /// <summary>
        /// <para>
        /// The currency name that can be used to differentiate rewards.
        /// </para>
        /// See <see href="https://faq.appodeal.com/en/articles/1133435-reward-setting"/> for more details.
        /// </summary>
        /// <remarks>It can be null if currency name was not specified in settings.</remarks>
        public string Currency;

        /// <summary>
        /// Returns ad revenue information as a json-formatted string.
        /// </summary>
        /// <param name="isPretty">If true, format the output for readability. If false, format the output for minimum size. Default is false.</param>
        public string ToJsonString(bool isPretty = false) => UnityEngine.JsonUtility.ToJson(this, isPretty);
    }
}
