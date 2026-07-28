# DITDAH-6 — Decode Morse sequences to text

**Type:** Story
**Story points:** 2
**Priority:** High
**Depends on:** DITDAH-5
**Status:** To Do

## User story

As a user, I want my keyed Morse translated into readable text so that the app actually
decodes messages.

## Description

Map each letter sequence from DITDAH-5 through a Morse table into characters, and
accumulate the decoded message.

- `Dictionary<string, char>` for A–Z and 0–9 (international Morse; ~36 entries, grab a
  table from Wikipedia)
- Unknown sequences decode to `'?'` rather than throwing — `OnError` would kill the
  whole pipeline
- Accumulate the message with `Scan` so the *stream itself* carries the full decoded
  text (`IObservable<string>` of progressively longer messages) — no mutable state
  outside the pipeline

## Acceptance criteria

- Keying `... --- ...` produces `SOS`
- Word gaps appear as spaces in the decoded text
- An invalid sequence (e.g. `".-.-.-.-"`) shows `?` and the pipeline keeps running
- End-to-end manual test passes: this is the epic's MVP line — everything after
  this task is polish

## Rx concepts

`Select`, `Scan` (stateful accumulation inside the stream), error isolation
