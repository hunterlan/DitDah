# DITDAH-8 — Polish: cheat sheet, clean shutdown

**Type:** Task
**Story points:** 1
**Priority:** Low (stretch — only if time remains)
**Depends on:** DITDAH-7
**Status:** To Do

## Description

Small quality-of-life items, each independent — cherry-pick in any order:

- [ ] **Morse cheat sheet** — print the A–Z/0–9 table on startup so you don't need
      Wikipedia open while playing
- [ ] **Clean shutdown** — `Esc` completes the key observable (`TakeUntil` or
      `TakeWhile`), all subscriptions dispose via a `CompositeDisposable`, app exits
      without `Ctrl+C`
- [ ] **Backspace** — deleting the last decoded character; interesting because it means
      the `Scan` accumulator in DITDAH-6 must handle a "remove" event, not just appends
- [ ] **Timing calibration** — a startup mode that measures *your* tap/hold timings and
      sets the DITDAH-4/5 thresholds from them instead of the hardcoded constants

## Acceptance criteria

- Whatever is picked works; whatever isn't is left unchecked — this task is
  explicitly allowed to be partially done or skipped

## Rx concepts

`TakeUntil`, `CompositeDisposable`, lifecycle and disposal
