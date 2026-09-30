# Glossary

Terms used in the code, issues and PRs of this plugin.

## Ads

### Mediation

The Appodeal SDK asks many ad networks for an ad and shows the best one. The
plugin only wraps the native SDKs; mediation logic lives in them.

### Ad network

A demand source such as AdMob or Meta Audience Network. The ids are in
`AppodealNetworks`, and `Appodeal.DisableNetwork` switches one off.

### Adapter

The native library that connects one ad network to the Appodeal SDK. Each
adapter is a pod, a Swift package or a Maven artifact in
`AppodealDependencies.txt`.

### Ad type

Interstitial, RewardedVideo, Banner, Mrec. `AppodealAdType` stores them as bit
flags, so `Interstitial | RewardedVideo` is valid.

### Placement

A named spot in the game, set up in the Appodeal dashboard. `Show` and `CanShow`
take a placement name; the default one is "default".

### Cache, precache

Loading an ad before it is shown. With auto cache on, the SDK reloads ads
itself.

### Predicted eCPM

The expected revenue per thousand impressions for the loaded ad, from
`GetPredictedEcpm`.

### Ad revenue callback

Impression-level revenue data (`AppodealAdRevenue`), usually forwarded to an
attribution service.

### Segment

A user group from the dashboard with its own ad settings; see `GetSegmentId`.

### In-app purchase tracking

There are two modes. With ad ROI360, which Appodeal support turns on for the
app, the SDK finds and validates purchases itself, and `SetPurchaseCallbacks`
reports the results. Without it, the game calls `ValidateAppStoreInAppPurchase`
or `ValidatePlayStoreInAppPurchase` for each purchase.

## Build and dependencies

### EDM4U

External Dependency Manager for Unity. It reads `AppodealDependencies.txt` and
adds Gradle dependencies, pods and Swift packages to the game build.

### Dependency Manager (DM)

The plugin's Editor window for choosing ad networks and services. It edits the
game's copy of the dependencies XML.

### Swift package, `replacesPod`

The iOS SDK and adapters also ship as Swift packages. A `swiftPackage` entry
with `replacesPod` tells EDM4U to use SPM instead of that pod.

### Append and replace builds

Unity iOS build modes. Append keeps the existing Xcode project, so iOS
post-process steps must work when they run twice on the same project.

### Pre-process, post-process

Build hooks in `Editor/PreProcess/` and `Editor/PostProcess/` that patch the
Gradle files, Android manifest, Xcode project and Info.plist.

### `APPODEAL_DEV`

A scripting define for plugin development. It compiles the Android and iOS
platform assemblies in any Editor target.
