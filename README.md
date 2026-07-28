# DitDah

> A console Morse code decoder built with Rx.NET. Tap a key — DitDah classifies your taps
> into dots and dashes, segments them into letters and words, and decodes them to text live.

**Epic:** DITDAH-0 — Evening Rx.NET learning project
**Timebox:** one evening, ~2–3h of pure coding
**Goal:** operator fluency with Rx.NET (`System.Reactive`), not architecture. Everything lives in a single `Program.cs`.

## Rx concepts covered

| Concept | Where |
|---|---|
| Wrapping an imperative source (`Console.ReadKey`) | DITDAH-2 |
| `Where` / `Select` — small composable stages | DITDAH-4 |
| `Throttle` as a buffer-closing boundary | DITDAH-5 |
| `Buffer` / `Window` | DITDAH-5 |
| `Scan` / `Select` transformation pipelines | DITDAH-6 |
| Schedulers, `ObserveOn` | DITDAH-7 |
| Resource cleanup, `IDisposable` | DITDAH-8 |

## Board

| ID | Title | Type | SP | Priority | Depends on |
|---|---|---|---|---|---|
| [DITDAH-1](tasks/DITDAH-1.md) | Bootstrap console project with System.Reactive | Task | 1 | Highest | — |
| [DITDAH-2](tasks/DITDAH-2.md) | Expose keyboard input as an IObservable | Story | 2 | Highest | DITDAH-1 |
| [DITDAH-3](tasks/DITDAH-3.md) | Spike: dot/dash strategy — ✅ resolved: two-key input | Spike | 1 | High | DITDAH-2 |
| [DITDAH-4](tasks/DITDAH-4.md) | Classify key presses into dots and dashes | Story | 1 | High | DITDAH-3 |
| [DITDAH-5](tasks/DITDAH-5.md) | Segment symbols into letters and words | Story | 3 | High | DITDAH-4 |
| [DITDAH-6](tasks/DITDAH-6.md) | Decode Morse sequences to text | Story | 2 | High | DITDAH-5 |
| [DITDAH-7](tasks/DITDAH-7.md) | Live console display | Story | 2 | Medium | DITDAH-6 |
| [DITDAH-8](tasks/DITDAH-8.md) | Polish: cheat sheet, clean shutdown | Task | 1 | Low (stretch) | DITDAH-7 |

**Total:** 13 SP core + stretch (DITDAH-3 already resolved during planning). If the evening
runs short, DITDAH-7 can be reduced to plain `Console.WriteLine` output and DITDAH-8 dropped
entirely — the epic is still a success once DITDAH-6 works end to end.

## Suggested time budget

- DITDAH-1..2 — ~30 min (plumbing)
- DITDAH-4 — ~15 min (trivial by design; see the DITDAH-3 decision)
- DITDAH-5 — ~1h (the heart of the project; the actual Rx learning happens here)
- DITDAH-6 — ~30 min
- DITDAH-7..8 — whatever is left
