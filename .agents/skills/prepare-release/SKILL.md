---
name: prepare-release
description:
  Bump the plugin version in all three places and generate CHANGELOG.md with a
  local release-it run. Use when a release/X.Y.Z branch is ready for its version
  bump.
---

# Prepare a release

Release branches accept changes only as signed commits through a PR, so the
release commit is made locally.

1. Branch off the release branch:
   `git switch -c chore/SDK-123-prepare-release release/X.Y.Z`.
2. Bump `package.json` and write the changelog. release-it needs a clean working
   tree, so run it before any other edit. `.release-it.json` turns off its
   commit, tag, push and GitHub release; the command only changes files.

   ```sh
   npx -y -p release-it@19.2.4 -p @release-it/conventional-changelog@10.0.6 \
     release-it X.Y.Z --ci
   ```

   To preview the changelog without writing files, add `--changelog`.

3. Set the same version in the other two places. The tests fail if the three
   values differ.
   - `AppodealPluginVersion` in `Runtime/Common/AppodealVersions.cs`
   - `pluginVersion` in
     `Editor/DependencyManager/DefaultDependencies/AppodealDependencies.txt`
4. Show the new CHANGELOG.md section to the maintainer. It comes from commit
   messages, so an odd entry points to a commit message worth a look.
5. Hand over to the maintainer: they make two signed commits and open the PR
   into `release/X.Y.Z`.
   - `chore: bump plugin version to X.Y.Z` with `package.json`,
     `AppodealVersions.cs` and `AppodealDependencies.txt`
   - `docs: update changelog for vX.Y.Z` with `CHANGELOG.md`
