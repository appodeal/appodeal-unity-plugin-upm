using System.Diagnostics.CodeAnalysis;
using AppodealStack.ConsentManagement.Common;
using AppodealStack.ConsentManagement.Platforms;

// ReSharper disable CheckNamespace
namespace AppodealStack.ConsentManagement.Api
{
    /// <summary>
    /// <para>Consent Manager Unity API for developers, including documentation.</para>
    /// See <see href="https://docs.appodeal.com/unity/data-protection/gdpr-and-ccpa"/> for more details.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class ConsentManager
    {
        private readonly IConsentManager _consentManager;

        private IConsentManager GetNativeConsentManager()
        {
            return _consentManager;
        }

        private ConsentManager()
        {
            _consentManager = ConsentManagerClientFactory.GetConsentManager();
        }

        /// <summary>
        /// <para>Gets an instance of <see langword="ConsentManager"/> class.</para>
        /// See <see href="https://docs.appodeal.com/unity/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <returns>Object of type <see langword="ConsentManager"/>.</returns>
        public static ConsentManager GetInstance()
        {
            return new ConsentManager();
        }

        /// <summary>
        /// <para>Determines the status of user's consent by requesting information from server.</para>
        /// See <see href="https://docs.appodeal.com/unity/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <remarks>Once result is obtained, either <see langword="OnConsentInfoUpdated"/> or <see langword="OnFailedToUpdateConsentInfo"/> callback method will be triggered.</remarks>
        /// <param name="appodealAppKey">appodeal app key that was assigned to your app when it was created.</param>
        /// <param name="listener">class which implements AppodealStack.ConsentManager.Common.IConsentInfoUpdateListener interface.</param>
        public void RequestConsentInfoUpdate(string appodealAppKey, IConsentInfoUpdateListener listener = null)
        {
            GetNativeConsentManager().RequestConsentInfoUpdate(appodealAppKey, listener);
        }

        /// <summary>
        /// <para>Disables auto displaying of Apple's ATT request window by Consent Manager. (Supported only for <see langword="iOS"/> platform)</para>
        /// See <see href="https://docs.appodeal.com/unity/data-protection/app-tracking-transparency?distribution=upm"/> for more details.
        /// </summary>
        /// <remarks>Should be used before showing the ConsentForm window.</remarks>
        public void DisableAppTrackingTransparencyRequest()
        {
            GetNativeConsentManager().DisableAppTrackingTransparencyRequest();
        }

        /// <summary>
        /// <para>Sets custom vendor to be displayed in the consent form. Allows, for example, to add yourself as a vendor.</para>
        /// See <see href="https://docs.appodeal.com/unity/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <remarks>Should be called before using the <see cref="RequestConsentInfoUpdate"/> method.</remarks>
        /// <param name="customVendor">an instance of <see langword="Vendor"/> class.</param>
        public void SetCustomVendor(Vendor customVendor)
        {
            GetNativeConsentManager().SetCustomVendor(customVendor);
        }

        /// <summary>
        /// <para>Gets the vendor object that was previously set via the <see langword="SetCustomVendor"/>method.</para>
        /// See <see href="https://docs.appodeal.com/unity/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <param name="bundle">2nd parameter of your custom vendor constructor.</param>
        /// <returns>Object of type <see langword="Vendor"/> if exists, otherwise null.</returns>
        public Vendor GetCustomVendor(string bundle)
        {
            var vendor = GetNativeConsentManager().GetCustomVendor(bundle);
            return vendor == null ? null : new Vendor(vendor);
        }

        /// <summary>
        /// <para>Checks whether or not ConsentManager uses SharedPreference / NSUserDefaults to overwrite iAB keys.</para>
        /// See <see href="https://docs.appodeal.com/unity/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <remarks>ConsentManagerStorage.SharedPreference value will be returned only if the <see langword="ConsentManager.SetStorage(ConsentManagerStorage.SharedPreference)"/> method was called before using the <see langword="RequestConsentInfoUpdate"/> method.</remarks>
        /// <returns>Object of type <see langword="ConsentManagerStorage"/>.</returns>
        public ConsentManagerStorage GetStorage()
        {
            return GetNativeConsentManager().GetStorage();
        }

        /// <summary>
        /// <para>Checks whether or not user is subject to the GDPR and CCPA acts and therefore if the consent window should be shown prior collection of personal data.</para>
        /// See <see href="https://docs.appodeal.com/unity/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <remarks>Before getting response on <see cref="RequestConsentInfoUpdate"/> method this parameter is undefined. (Unknown status)</remarks>
        /// <returns>Object of type <see langword="ConsentShouldShow"/>.</returns>
        public ConsentShouldShow ShouldShowConsentDialog()
        {
            return GetNativeConsentManager().ShouldShowConsentDialog();
        }

        /// <summary>
        /// <para>Defines whether or not ConsentManager can use SharedPreference / NSUserDefaults to overwrite iAB keys.</para>
        /// See <see href="https://docs.appodeal.com/unity/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <remarks>It is <see langword="ConsentManagerStorage.None"/> by default.</remarks>
        /// <param name="iabStorage">ConsentManagerStorage.SharedPreference - allows data to be stored in preferences, ConsentManagerStorage.None - disallows data to be stored in preferences.</param>
        public void SetStorage(ConsentManagerStorage iabStorage)
        {
            GetNativeConsentManager().SetStorage(iabStorage);
        }

        /// <summary>
        /// <para>Gets the ConsentZone object that contains information about whether on not the user is subject to either GDPR or CCPA.</para>
        /// See <see href="https://docs.appodeal.com/unity/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <remarks>Before getting response on <see cref="RequestConsentInfoUpdate"/> method this parameter is undefined. (Unknown status)</remarks>
        /// <returns>Object of type <see langword="ConsentZone"/>.</returns>
        public ConsentZone GetConsentZone()
        {
            return GetNativeConsentManager().GetConsentZone();
        }

        /// <summary>
        /// <para>Gets the ConsentStatus object that contains information about whether on not the user has granted consent on collecting personal data.</para>
        /// See <see href="https://docs.appodeal.com/unity/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <remarks>Before getting response on <see cref="RequestConsentInfoUpdate"/> method this parameter is undefined. (Unknown status)</remarks>
        /// <returns>Object of type <see langword="ConsentStatus"/>.</returns>
        public ConsentStatus GetConsentStatus()
        {
            return GetNativeConsentManager().GetConsentStatus();
        }

        /// <summary>
        /// <para>Gets the IConsent object that can be further used in <see langword="Appodeal.UpdateConsent"/> method to let Appodeal SDK know whether or not collection of personal data is allowed.</para>
        /// See <see href="https://docs.appodeal.com/unity/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <remarks>It can only be used after the <see langword="OnConsentInfoUpdated"/> callback method was called.</remarks>
        /// <returns>Object of type <see langword="IConsent"/>.</returns>
        public IConsent GetConsent()
        {
            return GetNativeConsentManager().GetConsent();
        }
    }
}
