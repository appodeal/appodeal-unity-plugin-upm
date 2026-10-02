# Development flow

## Branches

`main` holds released code. Each release gets a `release/X.Y.Z` branch cut from
`main`. Work branches start from the release branch and are named after the
ticket: `feature/SDK-123-short-name`, `fix/SDK-123-short-name` or
`chore/SDK-123-short-name`. Nobody pushes to `main` or `release/*` directly;
every change goes through a pull request.

## What GitHub enforces

- Commits in `main` and `release/*` must be signed. GitHub merges a pull request
  with a merge commit only if all its commits are signed.
- Rebase merging is off. With a squash merge of several commits, the squash
  commit gets the PR title, which is not a Conventional Commits message and
  drops out of the changelog, so squash single-commit PRs only.
- Force pushes to `main` and `release/*` are blocked, and `main` can't be
  deleted.
- Tag names must match `^v\d+\.\d+\.\d+(-.+)?$`. A tag can't be moved or
  deleted; only a repository admin can bypass this.
- Merged head branches are deleted automatically.

## Feature or fix

1. Take a ticket in the SDK project in Jira and branch off the release branch.
2. Make signed commits with Conventional Commits messages; release-it builds
   CHANGELOG.md from them.
3. Open a PR into the release branch and fill in the template.
4. CI runs lint, the EditMode tests on Unity 2021.3 and on the newest 6000.3
   patch, the signed package build, the dependency review and the secret scan,
   and a separate workflow runs the Claude review. When
   `AppodealDependencies.txt` changes, the pods job checks that every pod is
   published with the iOS minimum target from the XML. Everything except the
   review must pass; the review only reports. Unity jobs of all PRs share one
   license and run one at a time, so a test job may wait in a queue.
5. No approval is required. The author merges once the checks pass.

## Release preparation

Follow the [prepare-release](../.agents/skills/prepare-release/SKILL.md) skill.
On a `chore/SDK-123-prepare-release` branch it bumps the plugin version in all
three places and updates CHANGELOG.md, and everything goes into one PR. The
tests compare the three values, so a PR that bumps only some of them fails.

## Release

1. Open a PR from `release/X.Y.Z` into `main`. Every CI check must pass,
   including the build and pods, and a member of `@appodeal/sdk-team` other than
   the author must approve. The Claude review is advisory: it fails only when
   the review service is down, never because of its findings, so read its
   comments and labels before you approve.
2. The release branch must be up to date with `main`. If `main` has moved, for
   example after a Dependabot PR, first open a PR from `main` into
   `release/X.Y.Z`. The Update branch button doesn't work here: it would push to
   the protected release branch directly.
3. Merge with a merge commit. A squash would turn the whole release into one
   commit in `main`, and its history would be lost for `git log`, blame and
   bisect.
4. After the merge, the Release workflow drafts the `vX.Y.Z` GitHub release on
   the merge commit, with notes from CHANGELOG.md and the signed tarball. It
   stops if the tag already exists or CHANGELOG.md has no section for the
   version.
5. The maintainer reads the draft and publishes it. Publishing creates the tag,
   and games install the plugin by git URL with that tag, so this click is the
   release. If the workflow failed, fix the cause and run it again from the
   Actions tab on `main`.

## Hotfix

Cut `release/X.Y.Z` for the next patch version from `main` and go through the
same steps.

## Dependabot

Once a month Dependabot opens one PR into `main` with GitHub Actions updates.
Its CI uses the Dependabot secrets and skips the review. Like any PR into
`main`, it needs an approval from `@appodeal/sdk-team`. Once it is merged, open
release PRs into `main` fall behind; see step 2 of Release.

## Pull requests from forks

The plugin is public, so anyone can open a PR from a fork, and the team needs a
way to take such changes in. Workflows for a fork PR start only after a
maintainer approves the run; read the diff first, especially anything under
`.github/`. A fork run gets no secrets, so the test, build and review jobs fail
and the PR can't be merged as it is. To accept the change, cherry-pick its
commits into an internal branch with your signature (`git cherry-pick -S`),
which keeps the contributor as the author, and open a new PR from there.

## Rollback

A published tag can't be moved, and `packages-lock.json` in games already points
to its commit. To roll back, users pin the previous tag in their
`Packages/manifest.json`; the fix ships as a new patch release.
