# Appodeal Unity Plugin (UPM distribution)

## Changelog

*The full changelog can always be obtained at [Appodeal website](https://wiki.appodeal.com/en/unity/get-started/advanced/changelog).*

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
+ Removed Dummy.swift file as the new version of EDM plugin creates it by default
+ Changed Xcode project settings: bitcode is now off by default as Apple deprecated it
+ Implemented Events
+ Implemented GetReward method
+ Implemented OnAdRevenueReceived callback
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
+ Removed NonSkippable ad type
+ Removed UserSettings, Consent, ConsentFormBuilder, ConsentManageException classes
+ Removed setBannerBackground method
+ Replaced showAsActivity & showAsDialog methods of ConsentForm class with a single Show() method
