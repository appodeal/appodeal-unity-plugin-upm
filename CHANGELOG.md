# Changelog

## [4.4.0](https://github.com/appodeal/appodeal-unity-plugin-upm/compare/v4.3.0...v4.4.0) (2026-09-23)

### ⚠ BREAKING CHANGES

* remove SetLocationTracking API (#178)

### Features

* **analytics:** report swift package data in ios build request ([#177](https://github.com/appodeal/appodeal-unity-plugin-upm/issues/177)) ([3b98b0a](https://github.com/appodeal/appodeal-unity-plugin-upm/commit/3b98b0a803efbcc84b4db9f4783603727c3b50fd))
* **editor:** add embed script for Swift Package artifacts ([3e3cb5e](https://github.com/appodeal/appodeal-unity-plugin-upm/commit/3e3cb5e86f8efe3c50c3ebe31022eb743913f59c))
* **editor:** add XML models for remote swift packages ([ddc33c8](https://github.com/appodeal/appodeal-unity-plugin-upm/commit/ddc33c8314893d6ae1fc7e240286c04a5c1d7d47))
* **editor:** embed Swift Package artifacts in iOS builds ([cb37848](https://github.com/appodeal/appodeal-unity-plugin-upm/commit/cb378487e5e0e3dca9c096a0580dadd77fbfdd2b))
* **editor:** report swift package changes in dependencies diff ([a601445](https://github.com/appodeal/appodeal-unity-plugin-upm/commit/a60144553a177e3e0270d47d57e4c088b9a075a4))
* **editor:** run Swift Package embedder in iOS post-process ([6b43431](https://github.com/appodeal/appodeal-unity-plugin-upm/commit/6b43431750a7b10dc39ed21ace40196f02c710ff))
* update Appodeal Android SDK to v4.4.0 ([af95487](https://github.com/appodeal/appodeal-unity-plugin-upm/commit/af954877049818195c19c862c64f51135d29c591))
* update Appodeal iOS SDK to v4.4.0 ([31caca6](https://github.com/appodeal/appodeal-unity-plugin-upm/commit/31caca637f72030d59087f1353b60721b3c8d0f5))

### Bug Fixes

* defer DM page activation to avoid blank window ([#171](https://github.com/appodeal/appodeal-unity-plugin-upm/issues/171)) ([f5fb9e1](https://github.com/appodeal/appodeal-unity-plugin-upm/commit/f5fb9e1af3f0169cce9351a11f243db4b3024192))
* **editor:** check framework name with suffix in ContainsFramework ([c64354c](https://github.com/appodeal/appodeal-unity-plugin-upm/commit/c64354c04e0a52200ad0b7608ffe2066649ae59b))
* **editor:** clarify Firebase plist bundle id mismatch warning ([6b5fea0](https://github.com/appodeal/appodeal-unity-plugin-upm/commit/6b5fea03650f44f5f0608a4548a773edfeb1fb25))
* **editor:** dedupe Facebook URL scheme on append builds ([496f688](https://github.com/appodeal/appodeal-unity-plugin-upm/commit/496f688e987506009503574b759696b64b2fadb0))
* **editor:** detect AdMob adapter via SPM swift packages ([#175](https://github.com/appodeal/appodeal-unity-plugin-upm/issues/175)) ([52ccd80](https://github.com/appodeal/appodeal-unity-plugin-upm/commit/52ccd8076b5e2b7b21910d0c2f01fa37e9018ea8))
* **editor:** detect AppLovin MAX dependency beyond the Podfile ([#174](https://github.com/appodeal/appodeal-unity-plugin-upm/issues/174)) ([6250f68](https://github.com/appodeal/appodeal-unity-plugin-upm/commit/6250f68ef36659b86da9dffe54e454971ce6c92a))
* **editor:** keep remoteSwiftPackage nodes on xml round-trip ([6c13d83](https://github.com/appodeal/appodeal-unity-plugin-upm/commit/6c13d83657813558b487d382d2994af172bf0ef2))
* **editor:** overwrite Firebase plist on append builds ([61627dd](https://github.com/appodeal/appodeal-unity-plugin-upm/commit/61627ddad95361d32b96ba068c7ec570b52fe5dc))

### Miscellaneous Chores

* remove SetLocationTracking API ([#178](https://github.com/appodeal/appodeal-unity-plugin-upm/issues/178)) ([f7a991a](https://github.com/appodeal/appodeal-unity-plugin-upm/commit/f7a991ab2a70e10d11e6af16c5bf36612dc6c488))

## 4.3.0 (July 23, 2026)

+ Updated Appodeal Android SDK to v4.3.0
+ Updated Appodeal iOS SDK to v4.3.0
+ Changed Appodeal asset GUID to resolve Unity IAP conflict
+ Routed MREC readiness and lifecycle to the standalone MREC view on iOS
+ Changed plugin version lookup to use PackageInfo.FindForAssembly
+ Implemented `SetNonPersonalized` public API method

## 4.2.0 (June 16, 2026)

+ Updated Appodeal Android SDK to v4.2.0
+ Updated Appodeal iOS SDK to v4.2.0

## 4.1.0 (April 09, 2026)

+ Updated Appodeal Android SDK to v4.1.0
+ Updated Appodeal iOS SDK to v4.1.0
+ Implemented custom Appodeal SDK endpoint configuration
+ Added `AdAttributionKit` identifier support on iOS
+ Added `UxmlElement` attribute support for Unity 6.0+ UI Toolkit
+ Added release preparation workflow and configuration
+ Renamed AppLovin MAX CocoaPods prefix from `APD` to `Appodeal`
+ Changed ad revenue callbacks to run on a background thread
+ Disabled iOS native plugin files for `tvOS` builds
+ Configured iOS pods validation to run only when dependency definitions change

## 4.0.0 (February 02, 2026)

+ Updated Appodeal Android SDK to v4.0.0
+ Updated Appodeal iOS SDK to v4.0.0
+ Designed Dependency Manager v3

## 3.12.0 (December 08, 2025)

+ Updated Appodeal Android SDK to v3.12.0
+ Updated Appodeal iOS SDK to v3.12.0
+ Worked around Unity AssetDatabase issue on iOS CI builds

## 3.11.0 (November 06, 2025)

+ Updated Appodeal Android SDK to v3.11.0
+ Updated Appodeal iOS SDK to v3.11.0
+ Changed loading logic of plugin SO assets
+ Added minor development and distribution improvements

## 3.10.0 (September 12, 2025)

+ Updated Appodeal Android SDK to v3.10.0
+ Updated Appodeal iOS SDK to v3.10.0

## 3.9.0 (August 29, 2025)

+ Updated Appodeal Android SDK to v3.9.0
+ Updated Appodeal iOS SDK to v3.9.0
+ Developed Analytics module
+ Updated usage sample

## 3.8.1 (July 25, 2025)

+ Updated Appodeal Android SDK to v3.8.1
+ Updated Appodeal iOS SDK to v3.8.1
+ Added iOS pod minTargetSdk validator

## 3.8.0 (July 04, 2025)

+ Updated Appodeal Android SDK to v3.8.0
+ Updated Appodeal iOS SDK to v3.8.0
+ Minor fixes

## 3.7.0 (June 12, 2025)

+ Updated Appodeal Android SDK to v3.7.0
+ Updated Appodeal iOS SDK to v3.7.0
+ Reworked Android implementation of `showMediationDebugger` method

## 3.6.0 (May 30, 2025)

+ Updated Appodeal Android SDK to v3.6.0
+ Updated Appodeal iOS SDK to v3.6.0
+ Implemented AF ROI360 feature support

## 3.6.0-alpha.1 (May 14, 2025)

+ Updated Appodeal Android SDK to v3.6.0-alpha.1
+ Updated Appodeal iOS SDK to v3.6.0-alpha.1
+ Implemented AppLovin MAX Ad Review feature support

## 3.5.2 (April 15, 2025)

+ Updated Appodeal Android SDK to v3.5.2
+ Updated Appodeal iOS SDK to v3.5.2
+ Implemented selective dispatch for `LogEvent` public API method

## 3.5.1 (April 01, 2025)

+ Updated Appodeal Android SDK to v3.5.1
+ Updated Appodeal iOS SDK to v3.5.1
+ Implemented `ShowMediationDebugger` public API method

## 3.5.0 (March 11, 2025)

+ Updated Appodeal Android SDK to v3.5.0
+ Updated Appodeal iOS SDK to v3.5.0
+ Implemented `SetBidonEndpoint` and `GetBidonEndpoint` public API methods
+ Replaced certain SDK logos in Plugin's Dependency Manager
+ Added mediation engines ordering in Plugin's Dependency Manager
+ Added descriptions for new adapters in Plugin's Dependency Manager

## 3.4.2 (February 21, 2025)

+ Updated Appodeal Android SDK to v3.4.2
+ Updated Appodeal iOS SDK to v3.4.2

## 3.4.1 (December 23, 2024)

+ Updated Appodeal Android SDK to v3.4.1
+ Updated Appodeal iOS SDK to v3.4.1
+ Fixed Unity Editor warnings caused by plugin

## 3.4.0 (November 20, 2024)

+ Updated Appodeal Android SDK to v3.4.0
+ Updated Appodeal iOS SDK to v3.4.0

## 3.3.4 (October 18, 2024)

+ Updated Appodeal iOS SDK to v3.3.3

## 3.4.0-beta.2 (October 11, 2024)

+ Updated Appodeal iOS SDK to v3.4.0-beta.2

## 3.4.0-beta.1 (October 04, 2024)

+ Updated Appodeal Android SDK to v3.4.0-beta.1
+ Updated Appodeal iOS SDK to v3.4.0-beta.1

## 3.4.0-alpha.3 (September 20, 2024)

+ Updated Appodeal Android SDK to v3.4.0-alpha.3
+ Updated Appodeal iOS SDK to v3.4.0-alpha.2

## 3.3.3 (September 17, 2024)

+ Updated Appodeal Android SDK to v3.3.3

## 3.4.0-alpha.2 (September 13, 2024)

+ Updated Appodeal Android SDK to v3.4.0-alpha.2
+ Improved Android bridge

## 3.4.0-alpha.1 (September 06, 2024)

+ Updated Appodeal Android SDK to v3.4.0-alpha.1
+ Updated Appodeal iOS SDK to v3.4.0-alpha.1
+ Improved Android bridge
+ Changed internal scripts accessibility
+ Renamed some assemblies
+ Updated assemblies settings
+ Tuned up suppressions
+ Reloaded some .meta files
+ Changed deprecated entities

## 3.3.2 (July 30, 2024)

+ Updated Appodeal Android SDK to v3.3.2
+ Updated Appodeal iOS SDK to v3.3.2

## 3.3.1 (June 04, 2024)

+ Updated Appodeal Android SDK to v3.3.1
+ Updated Appodeal iOS SDK to v3.3.1
+ Implemented Dependency Manager tool
+ Changed skadnetwork ids parsing logic
+ Bumped minimal supported Unity version to v2021.3

## 3.3.0 (April 30, 2024)

+ Updated Appodeal Android SDK to v3.3.0
+ Updated Appodeal iOS SDK to v3.3.0

## 3.3.0-beta.5 (April 25, 2024)

+ Updated Appodeal Android SDK to v3.3.0-beta.5

## 3.3.0-beta.4 (April 17, 2024)

+ Updated Appodeal Android SDK to v3.3.0-beta.4
+ Updated Appodeal iOS SDK to v3.3.0-beta.4
+ Added Apple's privacy manifest for Appodeal SDK
+ Fixed iOS bridge RV didFinish callback crash

## 3.3.0-beta.3 (March 26, 2024)

+ Updated Appodeal Android SDK to v3.3.0-beta.3
+ Updated Appodeal iOS SDK to v3.3.0-beta.3

## 3.3.0-beta.2 (February 22, 2024)

+ Updated Appodeal Android SDK to v3.3.0-beta.2
+ Updated Appodeal iOS SDK to v3.3.0-beta.2
+ Disabled Appodeal Dependency Manager tool

## 3.3.0-beta.1 (January 31, 2024)

+ Updated Appodeal Android SDK to v3.3.0-beta.1
+ Updated Appodeal iOS SDK to v3.3.0-beta.1
+ Removed `UpdateGdprConsent()` method
+ Removed `UpdateCcpaConsent()` method
+ Removed `GdprUserConsent` enum
+ Removed `CcpaUserConsent` enum

## 3.2.1 (January 24, 2024)

+ Updated Appodeal Android SDK to v3.2.1
+ Updated Appodeal iOS SDK to v3.2.1
+ Added default Appodeal Settings values

## 3.3.0-alpha.1 (January 11, 2024)

+ Updated Appodeal Android SDK to v3.3.0-alpha.1
+ Updated Appodeal iOS SDK to v3.3.0-alpha.1
+ Updated ad networks in `AppodealNetworks` class

## 3.2.1-beta.1 (January 03, 2024)

+ Updated Appodeal Android SDK to v3.2.1-beta.1
+ Updated Appodeal iOS SDK to v3.2.1-beta.1
+ Removed `ConsentManager` API
+ Removed `UpdateConsent()` method
+ Deprecated `UpdateGdprConsent()` method
+ Deprecated `UpdateCcpaConsent()` method

## 3.2.0 (December 14, 2023)

+ Updated Appodeal Android SDK to v3.2.0
+ Updated Appodeal iOS SDK to v3.2.0

## 3.2.0-beta.2 (November 23, 2023)

+ Updated Appodeal Android SDK to v3.2.0-beta.2
+ Updated Appodeal iOS SDK to v3.2.0-beta.2
+ Updated appodeal.androidlib dir content
+ Fixed iOS bridge dismiss callbacks
+ Fixed android `onRewardedVideoFinished` callback
+ Fixed conversion to java types
+ Fixed Firebase json validation

## 3.2.0-beta.1 (October 04, 2023)

+ Updated Appodeal Android SDK to v3.2.0-beta.1
+ Updated Appodeal iOS SDK to v3.2.0-beta.1
+ Updated ad networks in `AppodealNetworks` class
+ Replaced deprecated iOS methods
+ Minor fixes

## 3.1.3 (September 07, 2023)

+ Updated Appodeal Android SDK to v3.1.3
+ Updated Appodeal iOS SDK to v3.1.3

## 3.2.0-alpha.2 (September 5, 2023)

+ Updated Appodeal Android SDK to v3.2.0-alpha.6
+ Updated Appodeal iOS SDK to v3.2.0-alpha.5

## 3.2.0-alpha.1 (August 1, 2023)

+ Updated Appodeal Android SDK to v3.2.0-alpha.2
+ Updated Appodeal iOS SDK to v3.2.0-alpha.1
+ Changed Android dependencies distribution type
+ Updated ad networks in `AppodealNetworks` class
+ Updated minimal supported Unity version to v2020.3.16
+ Changed package name to `com.appodeal.mediation`

## 3.1.3-beta.2 (June 28, 2023)

+ Updated Appodeal Android SDK to v3.1.3-beta.2
+ Updated Appodeal iOS SDK to v3.1.3-beta.2

## 3.1.3-beta.1 (May 26, 2023)

+ Updated Appodeal Android SDK to v3.1.3-beta.1
+ Updated Appodeal iOS SDK to v3.1.3-beta.1
+ Synced context for callbacks on Android
+ Switched EDM distribution to UPM one
+ Bumped minimal EDM version to v1.2.175
+ Made editor ads improvements & fixes
+ Removed deprecated methods & classes
+ Added `Appodeal.GetPredictedEcpmForPlacement` method
+ Minor improvements

## 3.0.2 (January 17, 2023)

+ Updated Appodeal Android SDK to 3.0.2
+ Updated Appodeal iOS SDK to 3.0.2
+ Added Meta Client Token (iOS) to Appodeal Settings
+ Fixed wrong editor version scripting symbol
+ Minor improvements

## 3.0.1 (November 08, 2022)

+ Updated Appodeal Android SDK to 3.0.1
+ Updated Appodeal iOS SDK to 3.0.1
+ Updated EDM Unity Plugin to 1.2.174
+ Removed `Dummy.swift` file as the new version of EDM plugin creates it by default
+ Changed Xcode project settings: bitcode is now off by default as Apple deprecated it
+ Implemented Events
+ Implemented `GetReward` method
+ Implemented `OnAdRevenueReceived` callback
+ Fixed a bug with Dependency Manager on Windows
+ Fixed a bug with re-importing plugin data when rebuilding Library dir
+ Minor improvements

## 3.0.0 (June 21, 2022)

+ Updated Appodeal Android SDK to 3.0.0
+ Updated Appodeal iOS SDK to 3.0.0
+ Updated Editor tools with new features
+ Implemented test ads in Unity Editor
+ Reworked API for better usability
+ Added XML comments for API
+ Fixed a few bugs
+ Removed `NonSkippable` ad type
+ Removed `UserSettings`, `Consent`, `ConsentFormBuilder`, `ConsentManageException` classes
+ Removed setBannerBackground method
+ Replaced `showAsActivity` & `showAsDialog` methods of `ConsentForm` class with a single `Show()` method
