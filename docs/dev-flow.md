# Development flow

## Branches

`main` holds released code only. Each release gets a `release/X.Y.Z` branch cut
from `main`. Work branches start from the release branch and are named after the
ticket: `feature/SDK-123-short-name`, `fix/SDK-123-...`, `chore/SDK-123-...`.

## From ticket to merge

1. Take a ticket in the SDK project in Jira and create the work branch.
2. Commit with Conventional Commits messages; they become the changelog.
3. Open a PR into the release branch and fill in the template.
4. CI runs lint, test, build and review. When `AppodealDependencies.txt`
   changes, the pods job also checks iOS minimum targets and posts a report. On
   release branches nothing is a required check, so a red pods job is a warning;
   small fixes can also go into the release branch directly.
5. A code owner approves and the author merges.

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
