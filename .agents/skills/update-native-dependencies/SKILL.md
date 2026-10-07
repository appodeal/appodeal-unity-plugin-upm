---
name: update-native-dependencies
description:
  Update Appodeal SDK, adapter, pod, Maven or Swift package versions in
  AppodealDependencies.txt. Use for any change to native dependency versions.
---

# Update native dependencies

All native versions live in
`Editor/DependencyManager/DefaultDependencies/AppodealDependencies.txt`, in
three lists: `androidPackage` (Maven specs), `iosPod` and `remoteSwiftPackage`.

1. Change the versions. One adapter has the same version as a pod and as a Swift
   package; decode Swift package tags as described in AGENTS.md.
2. Every `iosPod` has a `minTargetSdk` that must equal the iOS deployment target
   of its podspec. Check it:

   ```sh
   pip install requests
   python3 .github/scripts/validate_min_sdk.py Editor/DependencyManager/DefaultDependencies/AppodealDependencies.txt
   ```

   The script writes `pod_sdk_report.md` and exits with 1 on a mismatch or a
   missing podspec. CI runs it on every PR that touches the file and blocks the
   merge into `main` and into release branches.

3. When both an Appodeal wrapper and an AppLovin MAX wrapper exist for the same
   network, open `Package.swift` of both tags and compare the upstream SDK in
   `.package(url:, exact:)`. Different pins make SPM fail to resolve the whole
   graph. If no pair of tags matches, the MAX adapter for that network stays on
   pods.
4. Build the iOS test project twice, with Swift packages on and off in EDM,
   and the Android test project once.
5. Commit with a message that names the platform and the version, for example
   `feat: update Appodeal iOS SDK to v4.4.0`.
