#!/usr/bin/env bash
# PreToolUse hook: asks for confirmation before commands that rewrite history,
# delete files or publish something.
# ponytail: greps the raw hook JSON instead of parsing it; switch to jq if a
# pattern ever matches text outside the command.
input=$(cat)

risky='git push|git reset --hard|git clean -[a-z]*f|git branch -D|git tag|git rebase|rm -rf|gh release|gh pr merge|npm publish|release-it'

if printf '%s' "$input" | grep -Eq "$risky"; then
  cat <<'JSON'
{"hookSpecificOutput":{"hookEventName":"PreToolUse","permissionDecision":"ask","permissionDecisionReason":"The command pushes, rewrites history, deletes files or publishes. Confirm it."}}
JSON
fi
exit 0
