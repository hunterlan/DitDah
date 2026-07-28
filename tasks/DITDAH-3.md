# DITDAH-3 — Spike: choose dot/dash detection strategy

**Type:** Spike
**Story points:** 1
**Priority:** High
**Depends on:** DITDAH-2
**Status:** Done (resolved during planning)

## Problem

A console app cannot see key-up events or hold duration — the terminal delivers
characters, not key transitions, on every OS. So dots and dashes can't be keyed by
press-and-hold without OS-specific tricks. A strategy was needed for telling the two apart.

## Options considered

- **A — infer holds from key auto-repeat bursts.** Rejected: depends on OS/terminal
  repeat delay and rate; confusing failure modes; open-ended timing fiddling.
- **B — two keys, one per symbol.** Chosen.
- **C — poll physical key state via P/Invoke (`GetAsyncKeyState`).** Rejected:
  Windows-only.
- **D — tap-count keying (one tap = dot, quick double-tap = dash).** Viable and
  portable, but adds debounce latency to every symbol and complexity ahead of the
  segmentation work in DITDAH-5, which teaches the same operators anyway.

## Decision

**Option B.** One key emits a dot, another emits a dash (suggested: `j` = dot, `k` = dash,
or `.` and `-` literally). Classification becomes a trivial, fully portable mapping with
zero timing dependence. All time-based Rx work — the actual learning target — lives in
the letter/word segmentation (DITDAH-5), which is unaffected by this choice.

## Consequences

- DITDAH-4 shrinks to a filter-and-map story (3 SP → 1 SP)
- DITDAH-5 becomes the first (and central) encounter with time-based operators
- No thresholds to calibrate for symbol entry; the only tunable timings are the
  letter/word pause durations in DITDAH-5
