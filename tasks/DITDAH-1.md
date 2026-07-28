# DITDAH-1 — Bootstrap console project with System.Reactive

**Type:** Task
**Story points:** 1
**Priority:** Highest
**Depends on:** —
**Status:** To Do

## Description

Create the .NET console project and verify the Rx pipeline runs at all before writing any
real logic. This task exists so that tooling problems (SDK version, NuGet, terminal quirks)
are flushed out in the first 15 minutes, not in the middle of the interesting work.

## Tasks

- [ ] `dotnet new console -n DitDah` (top-level statements, latest LTS SDK is fine)
- [ ] `dotnet add package System.Reactive`
- [ ] Smoke test: subscribe to `Observable.Interval(TimeSpan.FromSeconds(1))`, print ticks,
      exit on any keypress

## Acceptance criteria

- `dotnet run` prints a tick every second and exits cleanly on a keypress
- No projects, folders, or abstractions beyond a single `Program.cs`

## Notes

Resist the urge to add layers. The epic's definition of done is operator fluency,
not a maintainable codebase.
