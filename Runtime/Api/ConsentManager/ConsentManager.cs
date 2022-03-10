using System.Diagnostics.CodeAnalysis;
using AppodealStack.ConsentManager.Common;
using AppodealStack.ConsentManager.Platforms;

namespace AppodealStack.ConsentManager.Api
{
    /// <summary>
    /// <para>Consent Manager Unity API for developers, including documentation.</para>
    /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "InvalidXmlDocComment")]
    [SuppressMessage("ReSharper", "UnusedMethodReturnValue.Global")]
    public class ConsentManager
    {
        private readonly IConsentManager nativeConsentManager;

        private IConsentManager GetNativeConsentManager()
        {
            return nativeConsentManager;
        }

        private ConsentManager()
        {
            nativeConsentManager = ConsentManagerClientFactory.GetConsentManager();
        }

        /// <summary>
        /// <para>Gets an instance of <see langword="ConsentManager"/> class.</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <returns>Object of type <see langword="ConsentManager"/>.</returns>
        public static ConsentManager getInstance()
        {
            return new ConsentManager();
        }

        /// <summary>
        /// <para>Determines the status of user's consent by requesting information from server.</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <remarks>Once result is obtained, either <see langword="onConsentInfoUpdated"/> or <see langword="onFailedToUpdateConsentInfo"/> callback method will be triggered.</remarks>
        /// <param name="appodealAppKey">appodeal app key that was assigned to your app when it was created.</param>
        /// <param name="listener">class which implements AppodealStack.ConsentManager.Common.IConsentInfoUpdateListener interface.</param>
        public void requestConsentInfoUpdate(string appodealAppKey, IConsentInfoUpdateListener listener)
        {
            GetNativeConsentManager().requestConsentInfoUpdate(appodealAppKey, listener);
        }

        /// <summary>
        /// <para>Disables auto displaying of Apple's ATT request window by Consent Manager. (Supported only for <see langword="iOS"/> platform)</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/app-tracking-transparency"/> for more details.
        /// </summary>
        /// <remarks>Should be used before showing the ConsentForm window.</remarks>
        public void disableAppTrackingTransparencyRequest()
        {
            GetNativeConsentManager().disableAppTrackingTransparencyRequest();
        }

        /// <summary>
        /// <para>Sets custom vendor to be displayed in the consent form. Allows, for example, to add yourself as a vendor.</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <remarks>Should be called before using the <see cref="requestConsentInfoUpdate"/> method.</remarks>
        /// <param name="customVendor">an instance of <see langword="Vendor"/> class.</param>
        public void setCustomVendor(Vendor customVendor)
        {
            nativeConsentManager.setCustomVendor(customVendor);
        }

        /// <summary>
        /// <para>Gets the vendor object that was previously set via the <see langword="setCustomVendor"/>method.</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <param name="bundle">2nd parameter of your custom vendor constructor.</param>
        /// <returns>Object of type <see langword="Vendor"/>.</returns>
        public Vendor getCustomVendor(string bundle)
        {
            return new Vendor(nativeConsentManager.getCustomVendor(bundle));
        }

        /// <summary>
        /// <para>Checks whether or not ConsentManager uses SharedPreference / NSUserDefaults to overwrite iAB keys.</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <remarks>ConsentManagerStorage.SHARED_PREFERENCE value will be returned only if the <see langword="ConsentManager.setStorage(ConsentManagerStorage.SHARED_PREFERENCE)"/> method was called before using the <see langword="requestConsentInfoUpdate"/> method.</remarks>
        /// <returns>Object of type <see langword="ConsentManagerStorage"/>.</returns>
        public ConsentManagerStorage getStorage()
        {
            return nativeConsentManager.getStorage();
        }

        /// <summary>
        /// <para>Checks whether or not user is subject to the GDPR and CCPA acts and therefore if the consent window should be shown prior collection of personal data.</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <remarks>Before getting responce on <see cref="requestConsentInfoUpdate"/> method this parameter is undefined. (UNKNOWN status)</remarks>
        /// <returns>Object of type <see langword="ConsentShouldShow"/>.</returns>
        public ConsentShouldShow shouldShowConsentDialog()
        {
            return nativeConsentManager.shouldShowConsentDialog();
        }

        /// <summary>
        /// <para>Defines whether or not ConsentManager can use SharedPreference / NSUserDefaults to overwrite iAB keys.</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <remarks>It is <see langword="ConsentManagerStorage.NONE"/> by default.</remarks>
        /// <param name="iabStorage">ConsentManagerStorage.SHARED_PREFERENCE - allows data to be stored in preferences, ConsentManagerStorage.NONE - disallows data to be stored in preferences.</param>
        public void setStorage(ConsentManagerStorage iabStorage)
        {
            nativeConsentManager.setStorage(iabStorage);
        }

        /// <summary>
        /// <para>Gets the ConsentZone object that contains information about whether on not the user is subject to either GDPR or CCPA.</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <remarks>Before getting responce on <see cref="requestConsentInfoUpdate"/> method this parameter is undefined. (UNKNOWN status)</remarks>
        /// <returns>Object of type <see langword="ConsentZone"/>.</returns>
        public ConsentZone getConsentZone()
        {
            return nativeConsentManager.getConsentZone();
        }

        /// <summary>
        /// <para>Gets the ConsentStatus object that contains information about whether on not the user has granted consent on collecting personal data.</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <remarks>Before getting responce on <see cref="requestConsentInfoUpdate"/> method this parameter is undefined. (UNKNOWN status)</remarks>
        /// <returns>Object of type <see langword="ConsentStatus"/>.</returns>
        public ConsentStatus getConsentStatus()
        {
            return nativeConsentManager.getConsentStatus();
        }

        /// <summary>
        /// <para>Gets the Consent object that can be further used in <see langword="Appodeal.updateConsent"/> method to let Appodeal SDK know whether or not collection of personal data is allowed.</para>
        /// See <see href="https://wiki.appodeal.com/en/unity/get-started/data-protection/gdpr-and-ccpa"/> for more details.
        /// </summary>
        /// <remarks>It can only be used after the <see langword="onConsentInfoUpdated"/> callback method was called.</remarks>
        /// <returns>Object of type <see langword="Consent"/>.</returns>
        public Consent getConsent()
        {
            return new Consent(nativeConsentManager.getConsent());
        }
    }
}
