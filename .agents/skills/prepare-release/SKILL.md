---
name: prepare-release
description:
  Bump the plugin version and generate CHANGELOG.md for a release branch with a
  local release-it run. Use when a release/X.Y.Z branch is ready for its version
  bump.
---

# Prepare a release

The release commit is made locally: the ruleset on `release/**` requires signed
commits, and a commit from a workflow is not signed.

1. Make sure the plugin version is already bumped in
   `Runtime/Common/AppodealVersions.cs` and in `pluginVersion` of
   `Editor/DependencyManager/DefaultDependencies/AppodealDependencies.txt`.
   release-it bumps only `package.json`.
2. Branch off the release branch:
   `git switch -c chore/vX.Y.Z-prepare-release release/X.Y.Z`.
3. Bump `package.json` and write the changelog, without commit, tag or push:

   ```sh
   npx -y -p release-it@19.2.4 -p @release-it/conventional-changelog@10.0.6 \
     release-it X.Y.Z --ci --no-git.requireBranch --no-git.commit \
     --no-git.tag --no-git.push --no-github.release
   ```

   To preview the changelog without writing files, run the same command with
   `--changelog` in place of the `--no-*` flags.

4. Show the new CHANGELOG.md section to the maintainer. It comes from commit
   messages, so an odd entry points to a commit message worth a look.
5. Hand over to the maintainer: they commit, open the PR into `release/X.Y.Z`,
   and create the tag and the GitHub release.

Keep the release-it versions in step with
`.github/workflows/prepare-package-release.yml`.
