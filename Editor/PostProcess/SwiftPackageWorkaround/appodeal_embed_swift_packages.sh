#!/bin/sh
# Copies Swift Package artifacts into the application bundle, as '[CP] Embed Pods Frameworks' does
# for CocoaPods. EDM4U links the packages into UnityFramework only, so dynamic frameworks and
# resource bundles never reach the '.app': the application fails to launch with 'Library not
# loaded: @rpath/...' or crashes with 'unable to find bundle named ...'.
# https://github.com/googlesamples/unity-jar-resolver/issues/779
#
# The Appodeal Unity plugin copies this file into the Xcode project on every build. Edit the
# original in the package.

set -eu

FRAMEWORKS="${TARGET_BUILD_DIR}/${FRAMEWORKS_FOLDER_PATH}"
RESOURCES="${TARGET_BUILD_DIR}/${UNLOCALIZED_RESOURCES_FOLDER_PATH}"

# The rsync call of CocoaPods. Headers and modules are only needed to compile; '-L' resolves the
# symlinks an archive build leaves in BUILT_PRODUCTS_DIR, which the installer rejects; '--delete'
# keeps the copy a mirror of the source across SDK updates.
copy() {
    rsync -aL --delete --filter "- Headers" --filter "- PrivateHeaders" --filter "- Modules" "$@"
}

# Binary targets land in BUILT_PRODUCTS_DIR, dynamic products built from source in PackageFrameworks.
# Static frameworks are already inside the binaries; whether one is dynamic is known only from its
# Mach-O. UnityFramework is embedded by Xcode.
for framework in "${BUILT_PRODUCTS_DIR}"/*.framework "${BUILT_PRODUCTS_DIR}"/PackageFrameworks/*.framework; do
    name="${framework##*/}"
    binary="${framework}/${name%.framework}"
    [ "${name}" != "UnityFramework.framework" ] && [ -f "${binary}" ] || continue
    case "$(file -b "${binary}")" in *"dynamically linked shared library"*) ;; *) continue ;; esac

    copy "${framework}" "${FRAMEWORKS}/"

    # Xcode signs only what its own phases embed.
    if [ -n "${EXPANDED_CODE_SIGN_IDENTITY:-}" ] && [ "${CODE_SIGNING_REQUIRED:-}" != "NO" ] && [ "${CODE_SIGNING_ALLOWED:-}" != "NO" ]; then
        codesign --force --sign "${EXPANDED_CODE_SIGN_IDENTITY}" ${OTHER_CODE_SIGN_FLAGS:-} \
            --preserve-metadata=identifier,entitlements "${FRAMEWORKS}/${name}"
    fi
done

# Bundle.module looks in Bundle.main.resourceURL, the application bundle. They hold no code, so the
# signature of the application covers them.
for bundle in "${BUILT_PRODUCTS_DIR}"/*.bundle; do
    [ -d "${bundle}" ] || continue
    copy "${bundle}" "${RESOURCES}/"
done
