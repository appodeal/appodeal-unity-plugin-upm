// ReSharper disable CheckNamespace

using System;
using System.Diagnostics.CodeAnalysis;

namespace AppodealStack.Monetization.Common
{
    /// <summary>
    /// Class containing monetization events.
    /// </summary>
    [SuppressMessage("ReSharper", "ArrangeAccessorOwnerBody")]
    [SuppressMessage("ReSharper", "EventNeverSubscribedTo.Global")]
    public static class AppodealCallbacks
    {
        public sealed class Sdk
        {
            #region Sdk Singleton

            private Sdk() { }

            private static Sdk _instance;

            private static readonly object Lock = new();

            /// <summary>
            /// Returns an instance of the <see cref="AppodealCallbacks.Sdk"/> class.
            /// </summary>
            public static Sdk Instance
            {
                get
                {
                    if (_instance == null)
                    {
                        lock (Lock)
                        {
                            if (_instance == null)
                            {
                                _instance = new Sdk();
                                _instance.InitializeCallbacks();
                            }
                        }
                    }

                    return _instance;
                }
            }

            #endregion

            private ISdkProxyListener _sdkEventsImpl;

            /// <summary>
            /// Returns an instance of the <see cref="SdkProxyListener"/> class.
            /// </summary>
            internal ISdkProxyListener SdkEventsImpl => _sdkEventsImpl ??= new SdkProxyListener();

            /// <summary>
            /// <para>
            /// Raised when Appodeal SDK initialization is finished.
            /// </para>
            /// Arguments are of a type <see cref="SdkInitializedEventArgs"/>.
            /// </summary>
            public static event EventHandler<SdkInitializedEventArgs> OnInitialized;

            private void InitializeCallbacks()
            {
                SdkEventsImpl.OnInitialized += (_, args) => OnInitialized?.Invoke(this, args);
            }
        }

        /// <summary>
        /// Class containing AdRevenue events.
        /// </summary>
        public sealed class AdRevenue
        {
            #region AdRevenue Singleton

            private AdRevenue() { }

            private static AdRevenue _instance;

            private static readonly object Lock = new();

            /// <summary>
            /// Returns an instance of the <see cref="AppodealCallbacks.AdRevenue"/> class.
            /// </summary>
            public static AdRevenue Instance
            {
                get
                {
                    if (_instance == null)
                    {
                        lock (Lock)
                        {
                            if (_instance == null)
                            {
                                _instance = new AdRevenue();
                                _instance.InitializeCallbacks();
                            }
                        }
                    }
                    return _instance;
                }
            }

            #endregion

            private IAdRevenueProxyListener _adRevenueEventsImpl;

            /// <summary>
            /// Returns an instance of the <see cref="AdRevenueProxyListener"/> class.
            /// </summary>
            internal IAdRevenueProxyListener AdRevenueEventsImpl => _adRevenueEventsImpl ??= new AdRevenueProxyListener();

            /// <summary>
            /// <para>
            /// Raised when Appodeal SDK tracks ad impression.
            /// </para>
            /// Arguments are of a type <see cref="AdRevenueEventArgs"/>.
            /// </summary>
            public static event EventHandler<AdRevenueEventArgs> OnReceived;

            private void InitializeCallbacks()
            {
                AdRevenueEventsImpl.OnReceived += (_, args) => OnReceived?.Invoke(this, args);
            }
        }

        /// <summary>
        /// Class containing purchase-validation events for AF ROI360 feature.
        /// </summary>
        public sealed class Purchase
        {
            #region Purchase Singleton

            private Purchase() { }

            private static Purchase _instance;

            private static readonly object Lock = new();

            /// <summary>
            /// Returns an instance of the <see cref="AppodealCallbacks.Purchase"/> class.
            /// </summary>
            public static Purchase Instance
            {
                get
                {
                    if (_instance == null)
                    {
                        lock (Lock)
                        {
                            if (_instance == null)
                            {
                                _instance = new Purchase();
                                _instance.InitializeCallbacks();
                            }
                        }
                    }
                    return _instance;
                }
            }

            #endregion

            private PurchaseProxyListener _purchaseEventsImpl;

            /// <summary>
            /// Returns an instance of the <see cref="PurchaseProxyListener"/> class.
            /// </summary>
            [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
            internal PurchaseProxyListener PurchaseEventsImpl => _purchaseEventsImpl ??= new PurchaseProxyListener();

            /// <summary>
            /// <para>
            /// Raised when purchases have been successfully validated and tracked.
            /// </para>
            /// Arguments are of a type <see cref="PurchaseValidatedEventArgs"/>.
            /// </summary>
            public static event EventHandler<PurchaseValidatedEventArgs> OnValidationSucceeded;

            /// <summary>
            /// <para>
            /// Raised when purchase validation fails.
            /// </para>
            /// Arguments are of a type <see cref="PurchaseValidationFailedEventArgs"/>.
            /// </summary>
            public static event EventHandler<PurchaseValidationFailedEventArgs> OnValidationFailed;

            private void InitializeCallbacks()
            {
                PurchaseEventsImpl.OnValidationSucceeded += (_, args) => OnValidationSucceeded?.Invoke(this, args);
                PurchaseEventsImpl.OnValidationFailed += (_, args) => OnValidationFailed?.Invoke(this, args);
            }
        }

        /// <summary>
        /// Class containing InAppPurchase validation events.
        /// </summary>
        public sealed class InAppPurchase
        {
            #region InAppPurchase Singleton

            private InAppPurchase() { }

            private static InAppPurchase _instance;

            private static readonly object Lock = new();

            /// <summary>
            /// Returns an instance of the <see cref="AppodealCallbacks.InAppPurchase"/> class.
            /// </summary>
            public static InAppPurchase Instance
            {
                get
                {
                    if (_instance == null)
                    {
                        lock (Lock)
                        {
                            if (_instance == null)
                            {
                                _instance = new InAppPurchase();
                                _instance.InitializeCallbacks();
                            }
                        }
                    }
                    return _instance;
                }
            }

            #endregion

            private IInAppPurchaseValidationProxyListener _purchaseEventsImpl;

            /// <summary>
            /// Returns an instance of the <see cref="InAppPurchaseValidationProxyListener"/> class.
            /// </summary>
            [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
            internal IInAppPurchaseValidationProxyListener PurchaseEventsImpl => _purchaseEventsImpl ??= new InAppPurchaseValidationProxyListener();

            /// <summary>
            /// <para>
            /// Raised when In-App purchase is successfully validated.
            /// </para>
            /// Arguments are of a type <see cref="InAppPurchaseEventArgs"/>.
            /// </summary>
            public static event EventHandler<InAppPurchaseEventArgs> OnValidationSucceeded;

            /// <summary>
            /// <para>
            /// Raised when In-App purchase validation fails.
            /// </para>
            /// Arguments are of a type <see cref="InAppPurchaseEventArgs"/>.
            /// </summary>
            public static event EventHandler<InAppPurchaseEventArgs> OnValidationFailed;

            private void InitializeCallbacks()
            {
                PurchaseEventsImpl.OnValidationSucceeded += (_, args) => OnValidationSucceeded?.Invoke(this, args);
                PurchaseEventsImpl.OnValidationFailed += (_, args) => OnValidationFailed?.Invoke(this, args);
            }
        }

        /// <summary>
        /// Class containing Mrec ad lifecycle events.
        /// </summary>
        public sealed class Mrec
        {
            #region Mrec Singleton

            private Mrec() { }

            private static Mrec _instance;

            private static readonly object Lock = new();

            /// <summary>
            /// Returns an instance of the <see cref="AppodealCallbacks.Mrec"/> class.
            /// </summary>
            public static Mrec Instance
            {
                get
                {
                    if (_instance == null)
                    {
                        lock (Lock)
                        {
                            if (_instance == null)
                            {
                                _instance = new Mrec();
                                _instance.InitializeCallbacks();
                            }
                        }
                    }
                    return _instance;
                }
            }

            #endregion

            private IMrecAdProxyListener _mrecAdEventsImpl;

            /// <summary>
            /// Returns an instance of the <see cref="MrecAdProxyListener"/> class.
            /// </summary>
            internal IMrecAdProxyListener MrecAdEventsImpl => _mrecAdEventsImpl ??= new MrecAdProxyListener();

            /// <summary>
            /// <para>
            /// Raised when Mrec is loaded.
            /// </para>
            /// Arguments are of a type <see cref="AdLoadedEventArgs"/>.
            /// </summary>
            public static event EventHandler<AdLoadedEventArgs> OnLoaded;

            /// <summary>
            /// Raised when Mrec fails to load after passing the waterfall.
            /// </summary>
            public static event EventHandler OnFailedToLoad;

            /// <summary>
            /// Raised a few seconds after Mrec is displayed on the screen.
            /// </summary>
            public static event EventHandler OnShown;

            /// <summary>
            /// Raised when attempt to show Mrec fails for some reason.
            /// </summary>
            public static event EventHandler OnShowFailed;

            /// <summary>
            /// Raised when user clicks on Mrec ad.
            /// </summary>
            public static event EventHandler OnClicked;

            /// <summary>
            /// Raised when Mrec expires and should not be used.
            /// </summary>
            public static event EventHandler OnExpired;

            private void InitializeCallbacks()
            {
                MrecAdEventsImpl.OnLoaded += (_, args) => OnLoaded?.Invoke(this, args);
                MrecAdEventsImpl.OnFailedToLoad += (_, args) => OnFailedToLoad?.Invoke(this, args);
                MrecAdEventsImpl.OnShown += (_, args) => OnShown?.Invoke(this, args);
                MrecAdEventsImpl.OnShowFailed += (_, args) => OnShowFailed?.Invoke(this, args);
                MrecAdEventsImpl.OnClicked += (_, args) => OnClicked?.Invoke(this, args);
                MrecAdEventsImpl.OnExpired += (_, args) => OnExpired?.Invoke(this, args);
            }
        }

        /// <summary>
        /// Class containing Banner ad lifecycle events.
        /// </summary>
        public sealed class Banner
        {
            #region Banner Singleton

            private Banner() { }

            private static Banner _instance;

            private static readonly object Lock = new();

            /// <summary>
            /// Returns an instance of the <see cref="AppodealCallbacks.Banner"/> class.
            /// </summary>
            public static Banner Instance
            {
                get
                {
                    if (_instance == null)
                    {
                        lock (Lock)
                        {
                            if (_instance == null)
                            {
                                _instance = new Banner();
                                _instance.InitializeCallbacks();
                            }
                        }
                    }
                    return _instance;
                }
            }

            #endregion

            private IBannerAdProxyListener _bannerAdEventsImpl;

            /// <summary>
            /// Returns an instance of the <see cref="BannerAdProxyListener"/> class.
            /// </summary>
            internal IBannerAdProxyListener BannerAdEventsImpl => _bannerAdEventsImpl ??= new BannerAdProxyListener();

            /// <summary>
            /// <para>
            /// Raised when Banner is loaded.
            /// </para>
            /// Arguments are of a type <see cref="BannerLoadedEventArgs"/>.
            /// </summary>
            public static event EventHandler<BannerLoadedEventArgs> OnLoaded;

            /// <summary>
            /// Raised when Banner fails to load after passing the waterfall.
            /// </summary>
            public static event EventHandler OnFailedToLoad;

            /// <summary>
            /// Raised a few seconds after Banner is displayed on the screen.
            /// </summary>
            public static event EventHandler OnShown;

            /// <summary>
            /// Raised when attempt to show Banner fails for some reason.
            /// </summary>
            public static event EventHandler OnShowFailed;

            /// <summary>
            /// Raised when user clicks on the Banner ad.
            /// </summary>
            public static event EventHandler OnClicked;

            /// <summary>
            /// Raised when Banner expires and should not be used.
            /// </summary>
            public static event EventHandler OnExpired;

            private void InitializeCallbacks()
            {
                BannerAdEventsImpl.OnLoaded += (_, args) => OnLoaded?.Invoke(this, args);
                BannerAdEventsImpl.OnFailedToLoad += (_, args) => OnFailedToLoad?.Invoke(this, args);
                BannerAdEventsImpl.OnShown += (_, args) => OnShown?.Invoke(this, args);
                BannerAdEventsImpl.OnShowFailed += (_, args) => OnShowFailed?.Invoke(this, args);
                BannerAdEventsImpl.OnClicked += (_, args) => OnClicked?.Invoke(this, args);
                BannerAdEventsImpl.OnExpired += (_, args) => OnExpired?.Invoke(this, args);
            }
        }

        /// <summary>
        /// Class containing Interstitial ad lifecycle events.
        /// </summary>
        public sealed class Interstitial
        {
            #region Interstitial Singleton

            private Interstitial() { }

            private static Interstitial _instance;

            private static readonly object Lock = new();

            /// <summary>
            /// Returns an instance of the <see cref="AppodealCallbacks.Interstitial"/> class.
            /// </summary>
            public static Interstitial Instance
            {
                get
                {
                    if (_instance == null)
                    {
                        lock (Lock)
                        {
                            if (_instance == null)
                            {
                                _instance = new Interstitial();
                                _instance.InitializeCallbacks();
                            }
                        }
                    }
                    return _instance;
                }
            }

            #endregion

            private IInterstitialAdProxyListener _interstitialAdEventsImpl;

            /// <summary>
            /// Returns an instance of the <see cref="InterstitialAdProxyListener"/> class.
            /// </summary>
            internal IInterstitialAdProxyListener InterstitialAdEventsImpl => _interstitialAdEventsImpl ??= new InterstitialAdProxyListener();

            /// <summary>
            /// <para>
            /// Raised when Interstitial is loaded.
            /// </para>
            /// Arguments are of a type <see cref="AdLoadedEventArgs"/>.
            /// </summary>
            public static event EventHandler<AdLoadedEventArgs> OnLoaded;

            /// <summary>
            /// Raised when Interstitial fails to load after passing the waterfall.
            /// </summary>
            public static event EventHandler OnFailedToLoad;

            /// <summary>
            /// Raised a few seconds after Interstitial is displayed on the screen.
            /// </summary>
            public static event EventHandler OnShown;

            /// <summary>
            /// Raised when attempt to show Interstitial fails for some reason.
            /// </summary>
            public static event EventHandler OnShowFailed;

            /// <summary>
            /// Raised when user closes Interstitial.
            /// </summary>
            public static event EventHandler OnClosed;

            /// <summary>
            /// Raised when user clicks on the Interstitial ad.
            /// </summary>
            public static event EventHandler OnClicked;

            /// <summary>
            /// Raised when Interstitial expires and should not be used.
            /// </summary>
            public static event EventHandler OnExpired;

            private void InitializeCallbacks()
            {
                InterstitialAdEventsImpl.OnLoaded += (_, args) => OnLoaded?.Invoke(this, args);
                InterstitialAdEventsImpl.OnFailedToLoad += (_, args) => OnFailedToLoad?.Invoke(this, args);
                InterstitialAdEventsImpl.OnShown += (_, args) => OnShown?.Invoke(this, args);
                InterstitialAdEventsImpl.OnShowFailed += (_, args) => OnShowFailed?.Invoke(this, args);
                InterstitialAdEventsImpl.OnClosed += (_, args) => OnClosed?.Invoke(this, args);
                InterstitialAdEventsImpl.OnClicked += (_, args) => OnClicked?.Invoke(this, args);
                InterstitialAdEventsImpl.OnExpired += (_, args) => OnExpired?.Invoke(this, args);
            }
        }

        /// <summary>
        /// Class containing RewardedVideo ad lifecycle events.
        /// </summary>
        public sealed class RewardedVideo
        {
            #region RewardedVideo Singleton

            private RewardedVideo() { }

            private static RewardedVideo _instance;

            private static readonly object Lock = new();

            /// <summary>
            /// Returns an instance of the <see cref="AppodealCallbacks.RewardedVideo"/> class.
            /// </summary>
            public static RewardedVideo Instance
            {
                get
                {
                    if (_instance == null)
                    {
                        lock (Lock)
                        {
                            if (_instance == null)
                            {
                                _instance = new RewardedVideo();
                                _instance.InitializeCallbacks();
                            }
                        }
                    }
                    return _instance;
                }
            }

            #endregion

            private IRewardedVideoAdProxyListener _rewardedVideoAdEventsImpl;

            /// <summary>
            /// Returns an instance of the <see cref="RewardedVideoAdProxyListener"/> class.
            /// </summary>
            internal IRewardedVideoAdProxyListener RewardedVideoAdEventsImpl => _rewardedVideoAdEventsImpl ??= new RewardedVideoAdProxyListener();

            /// <summary>
            /// <para>
            /// Raised when Rewarded Video is loaded.
            /// </para>
            /// Arguments are of a type <see cref="AdLoadedEventArgs"/>.
            /// </summary>
            public static event EventHandler<AdLoadedEventArgs> OnLoaded;

            /// <summary>
            /// Raised when Rewarded Video fails to load after passing the waterfall.
            /// </summary>
            public static event EventHandler OnFailedToLoad;

            /// <summary>
            /// Raised a few seconds after Rewarded Video is displayed on the screen.
            /// </summary>
            public static event EventHandler OnShown;

            /// <summary>
            /// Raised when attempt to show Rewarded Video fails for some reason.
            /// </summary>
            public static event EventHandler OnShowFailed;

            /// <summary>
            /// <para>
            /// Raised when user closes Rewarded Video.
            /// </para>
            /// Arguments are of a type <see cref="RewardedVideoClosedEventArgs"/>.
            /// </summary>
            public static event EventHandler<RewardedVideoClosedEventArgs> OnClosed;

            /// <summary>
            /// <para>
            /// Raised when Rewarded Video has been watched to the end.
            /// </para>
            /// Arguments are of a type <see cref="RewardedVideoFinishedEventArgs"/>.
            /// </summary>
            public static event EventHandler<RewardedVideoFinishedEventArgs> OnFinished;

            /// <summary>
            /// Raised when user clicks on Rewarded Video ad.
            /// </summary>
            public static event EventHandler OnClicked;

            /// <summary>
            /// Raised when Rewarded Video expires and should not be used.
            /// </summary>
            public static event EventHandler OnExpired;

            private void InitializeCallbacks()
            {
                RewardedVideoAdEventsImpl.OnLoaded += (_, args) => OnLoaded?.Invoke(this, args);
                RewardedVideoAdEventsImpl.OnFailedToLoad += (_, args) => OnFailedToLoad?.Invoke(this, args);
                RewardedVideoAdEventsImpl.OnShown += (_, args) => OnShown?.Invoke(this, args);
                RewardedVideoAdEventsImpl.OnShowFailed += (_, args) => OnShowFailed?.Invoke(this, args);
                RewardedVideoAdEventsImpl.OnClosed += (_, args) => OnClosed?.Invoke(this, args);
                RewardedVideoAdEventsImpl.OnFinished += (_, args) => OnFinished?.Invoke(this, args);
                RewardedVideoAdEventsImpl.OnClicked += (_, args) => OnClicked?.Invoke(this, args);
                RewardedVideoAdEventsImpl.OnExpired += (_, args) => OnExpired?.Invoke(this, args);
            }
        }
    }
}
