# DITDAH-4 — Classify key presses into dots and dashes

**Type:** Story
**Story points:** 1
**Priority:** High
**Depends on:** DITDAH-3
**Status:** To Do

## User story

As a user, I want one key to emit a dot (`.`) and another to emit a dash (`-`) so that my
keying is turned into Morse symbols.

## Description

Transform the raw key stream (DITDAH-2) into `IObservable<char>` emitting `'.'` and `'-'`,
per the DITDAH-3 decision: two dedicated keys, one per symbol. Keys that aren't part of
the scheme are filtered out here so downstream stages only ever see valid symbols.

## Acceptance criteria

- `IObservable<char>` emits `'.'` for the dot key and `'-'` for the dash key,
  immediately on press
- All other keys are ignored (no downstream effect), except control keys reserved
  for app lifecycle (e.g. `Esc` in DITDAH-8)
- The two key bindings are named constants at the top of `Program.cs`
- Manual test: pressing dot, dot, dash prints `. . -`

## Rx concepts

`Where`, `Select` — keeping a pipeline stage small and composable
