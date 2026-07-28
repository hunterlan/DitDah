# DITDAH-5 — Segment symbols into letters and words

**Type:** Story
**Story points:** 3
**Priority:** High
**Depends on:** DITDAH-4
**Status:** To Do

## User story

As a user, I want a pause in my keying to close the current letter, and a longer pause to
close the current word, so that continuous tapping becomes structured Morse.

## Description

Take `IObservable<char>` of dots/dashes and produce:

- `IObservable<string>` of letters — e.g. `".-"`, `"-..."` (a pause of **~600ms** closes a letter)
- a word-boundary signal (a pause of **~1.5s** ends a word → emit a space)

The core idea: the symbol stream going quiet closes the current letter. Word boundaries
are the same idea again at a longer timescale, merged into the pipeline as a sentinel
value. This is the heart of the project — all of DitDah's time-based Rx lives here.

## Acceptance criteria

- Keying `.-` then pausing prints the complete sequence `".-"` as one unit
- A ~1.5s silence produces exactly one word separator (not one per additional second)
- No empty letters emitted (guard the `Buffer` output)
- Manual test: key `.-` (pause) `-...` (long pause) `-.-.` and observe
  `".-" "-..." " " "-.-."`-style output

## Rx concepts

`Buffer` with observable closing selector, layered `Throttle`s at different timescales,
`Merge`, marble-diagram reasoning (sketch this one on paper first — it helps)

## Notes

Getting "exactly one word separator per silence" right is the subtle part —
`Throttle` only fires after an event, so it naturally won't repeat during ongoing silence.
Convince yourself why with a marble diagram.
