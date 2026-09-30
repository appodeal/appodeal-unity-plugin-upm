# Development flow

## Branches

`main` holds released code only. Each release gets a `release/X.Y.Z` branch cut
from `main`. Work branches start from the release branch and are named after the
ticket: `feature/SDK-123-short-name`, `fix/SDK-123-...`, `chore/SDK-123-...`.

## From ticket to merge

1. Take a ticket in the SDK project in Jira and create the work branch.
2. Commit with Conventional Commits messages; they become the changelog.
3. Open a PR into the release branch and fill in the template.
4. CI runs lint, test, build, review and the secret scan. When
   `AppodealDependencies.txt` changes, the pods job also checks that every pod
   is published with the iOS minimum target from the XML and posts a report. To
   merge into a release branch, everything except review must pass.
5. The author merges once the required checks pass. A review is welcome but not
   required here: the code owner review is mandatory on the release PR into
   `main`.

## Release

1. Bump the plugin version and prepare the changelog with the
   [prepare-release](../.agents/skills/prepare-release/SKILL.md) skill.
2. Open a PR from `release/X.Y.Z` into `main`. Here every CI job, including
   pods, is required, and a code owner has to approve.
3. The maintainer tags `vX.Y.Z` and publishes the GitHub release draft. Games
   install the plugin by git URL with that tag, so the tag is the release.

## Rollback

Do not move a published tag: `packages-lock.json` in games already points to its
commit. To roll back, users pin the previous tag in their
`Packages/manifest.json`; the fix ships as a new patch release.
