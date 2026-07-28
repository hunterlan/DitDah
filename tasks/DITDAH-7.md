# DITDAH-7 — Live console display

**Type:** Story
**Story points:** 2
**Priority:** Medium
**Depends on:** DITDAH-6
**Status:** To Do

## User story

As a user, I want to see the symbol I'm currently keying and the decoded message update
live, so that I get immediate feedback while tapping.

## Description

Render two lines that update in place (via `Console.SetCursorPosition` or `\r`):

```
keying:  -.-
message: SOS CA
```

- The "keying" line shows dots/dashes of the *in-progress* letter, clearing when the
  letter closes — `Merge` the symbol stream with a clear signal, or `Scan` over a small
  union type
- The "message" line renders the accumulated text from DITDAH-6
- All rendering marshalled to one place: `ObserveOn(Scheduler.Default)` or a dedicated
  render subscription — the point is to *see* which thread Rx callbacks arrive on
  (`Environment.CurrentManagedThreadId` in a debug print is instructive)

## Acceptance criteria

- Symbols appear on the keying line the moment each press ends
- Keying line resets when a letter completes; message line appends the decoded char
- No interleaved/torn console output (single rendering subscription)

## Rx concepts

`ObserveOn`, schedulers and threading, merging heterogeneous streams into one render stream

## Notes

De-scope freely if time runs out: plain `Console.WriteLine` per decoded letter still
completes the epic. The two-line live display is nice-to-have.
