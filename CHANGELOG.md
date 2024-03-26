# Appodeal Unity Plugin (UPM distribution)

## Changelog

*The full changelog can always be obtained at [Appodeal website](https://docs.appodeal.com/unity/changelog).*

### 3.3.0-beta.3 (March 26, 2024)

+ Updated Appodeal Android SDK to v3.3.0-beta.3
+ Updated Appodeal iOS SDK to v3.3.0-beta.3

### 3.3.0-beta.2 (February 22, 2024)

+ Updated Appodeal Android SDK to v3.3.0-beta.2
+ Updated Appodeal iOS SDK to v3.3.0-beta.2
+ Disabled Appodeal Dependency Manager tool

### 3.3.0-beta.1 (January 31, 2024)

+ Updated Appodeal Android SDK to v3.3.0-beta.1
+ Updated Appodeal iOS SDK to v3.3.0-beta.1
+ Removed `UpdateGdprConsent()` method
+ Removed `UpdateCcpaConsent()` method
+ Removed `GdprUserConsent` enum
+ Removed `CcpaUserConsent` enum

### 3.2.1 (January 24, 2024)

+ Updated Appodeal Android SDK to v3.2.1
+ Updated Appodeal iOS SDK to v3.2.1
+ Added default Appodeal Settings values

### 3.3.0-alpha.1 (January 11, 2024)

+ Updated Appodeal Android SDK to v3.3.0-alpha.1
+ Updated Appodeal iOS SDK to v3.3.0-alpha.1
+ Updated ad networks in `AppodealNetworks` class

### 3.2.1-beta.1 (January 03, 2024)

+ Updated Appodeal Android SDK to v3.2.1-beta.1
+ Updated Appodeal iOS SDK to v3.2.1-beta.1
+ Removed `ConsentManager` API
+ Removed `UpdateConsent()` method
+ Deprecated `UpdateGdprConsent()` method
+ Deprecated `UpdateCcpaConsent()` method

### 3.2.0 (December 14, 2023)

+ Updated Appodeal Android SDK to v3.2.0
+ Updated Appodeal iOS SDK to v3.2.0

### 3.2.0-beta.2 (November 23, 2023)

+ Updated Appodeal Android SDK to v3.2.0-beta.2
+ Updated Appodeal iOS SDK to v3.2.0-beta.2
+ Updated appodeal.androidlib dir content
+ Fixed iOS bridge dismiss callbacks
+ Fixed android `onRewardedVideoFinished` callback
+ Fixed conversion to java types
+ Fixed Firebase json validation

### 3.2.0-beta.1 (October 04, 2023)

+ Updated Appodeal Android SDK to v3.2.0-beta.1
+ Updated Appodeal iOS SDK to v3.2.0-beta.1
+ Updated ad networks in `AppodealNetworks` class
+ Replaced deprecated iOS methods
+ Minor fixes

### 3.1.3 (September 07, 2023)

+ Updated Appodeal Android SDK to v3.1.3
+ Updated Appodeal iOS SDK to v3.1.3

### 3.2.0-alpha.2 (September 5, 2023)

+ Updated Appodeal Android SDK to v3.2.0-alpha.6
+ Updated Appodeal iOS SDK to v3.2.0-alpha.5

### 3.2.0-alpha.1 (August 1, 2023)

+ Updated Appodeal Android SDK to v3.2.0-alpha.2
+ Updated Appodeal iOS SDK to v3.2.0-alpha.1
+ Changed Android dependencies distribution type
+ Updated ad networks in `AppodealNetworks` class
+ Updated minimal supported Unity version to v2020.3.16
+ Changed package name to `com.appodeal.mediation`

### 3.1.3-beta.2 (June 28, 2023)

+ Updated Appodeal Android SDK to v3.1.3-beta.2
+ Updated Appodeal iOS SDK to v3.1.3-beta.2

### 3.1.3-beta.1 (May 26, 2023)

+ Updated Appodeal Android SDK to v3.1.3-beta.1
+ Updated Appodeal iOS SDK to v3.1.3-beta.1
+ Synced context for callbacks on Android
+ Switched EDM distribution to UPM one
+ Bumped minimal EDM version to v1.2.175
+ Made editor ads improvements & fixes
+ Removed deprecated methods & classes
+ Added `Appodeal.GetPredictedEcpmForPlacement` method
+ Minor improvements

### 3.0.2 (January 17, 2023)

+ Updated Appodeal Android SDK to 3.0.2
+ Updated Appodeal iOS SDK to 3.0.2
+ Added Meta Client Token (iOS) to Appodeal Settings
+ Fixed wrong editor version scripting symbol
+ Minor improvements

### 3.0.1 (November 08, 2022)

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

### 3.0.0 (June 21, 2022)

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
