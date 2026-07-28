# DITDAH-2 — Expose keyboard input as an IObservable

**Type:** Story
**Story points:** 2
**Priority:** Highest
**Depends on:** DITDAH-1
**Status:** To Do

## User story

As a developer learning Rx, I want console keypresses exposed as `IObservable<ConsoleKeyInfo>`
so that every downstream feature is a pure stream transformation.

## Description

`Console.ReadKey` is blocking and imperative — the opposite of a stream. Wrap it so the rest
of the app never touches `Console` input directly. This is the canonical "bridge the
imperative world into Rx" exercise: a background reader loop pushing into the pipeline,
with disposal wired up so the loop stops when the subscription ends.

## Acceptance criteria

- A single `IObservable<ConsoleKeyInfo>` (or `IObservable<DateTimeOffset>` of tap timestamps)
  exists and is the only place `Console.ReadKey` is called
- `intercept: true` so taps don't echo garbage to the screen
- Stream is shared (`Publish().RefCount()` or `Subject`) — two subscribers must both see
  every keypress, not steal them from each other
- Manual test: a debug subscription prints a marker per tap

## Rx concepts

`Observable.Create`, `Disposable.Create`, hot vs cold observables, `Publish`/`RefCount`
