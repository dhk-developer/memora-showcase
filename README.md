# Memora: technical showcase

**Memora** (working title *PageBeat*) is an independent rhythm game with its own authoring environment, built in **Unity 6 and C#** for Android.
Judgement lines move, notes float free, follow other notes or converge on moving points, and the whole field is choreographed to the music. Every note is still judged at a single, deterministic time and place.

This repository explains **how it's engineered**, without publishing the game. It contains architecture notes, diagrams, a conceptual data model, clean-room pseudocode, and a small, tested Unity code sample.

[![Memora gameplay: notes and bars sweep across a black playfield](media/gameplay.webp)](https://dhk-developer.github.io/memora.html)

> Gameplay footage, a playable timing demo and the full write-up: **[dhk-developer.github.io/memora.html](https://dhk-developer.github.io/memora.html)**

---

## The core idea: a judgement contract

![Judgement contract versus presentation](diagrams/judgement-contract.svg)

Every playable note has two layers that never write to each other:

- a **judgement contract**: beat, target and action, which input is compared against;
- **presentation**: how it enters, moves, scales, fades and leaves, authored as keyframes in beats.

Decorative systems (layers, camera, effects) are applied *after* judgement geometry is resolved, so they can't move a scoring target. That one rule is what makes ambitious choreography safe.

## Engineering highlights

| Problem | Approach | Read more |
|---|---|---|
| Gameplay must stay locked to audio | Song time from the audio **DSP clock**, all state **sampled at the beat**, per-device **latency calibration** with an outlier-resistant estimate | [timing-and-judgement.md](docs/timing-and-judgement.md) |
| Notes that aim at moving things | Targets can be lines, free points, **other notes** (sampled at *this* note's beat) or moving authored points; **predictive** and **adaptive** approach with stateless teleport correction | [gameplay-system.md](docs/gameplay-system.md) |
| An editor that can stand anywhere in the song | **Shared evaluators** for runtime and preview, interval-tree lifetime indexes, compact repeat schedules, latest-request-wins seeking | [editor-system.md](docs/editor-system.md) |
| Reusable phrases without fragile references | Presets **compile** to independent records: ID remapping, typed parameters, ports, one undo step, **version pinning** | [editor-system.md](docs/editor-system.md#phrase-presets) |
| Editor slowed on large charts | Profiled: whole-chart serialisation on every repaint. Fixed with edit transactions and per-pass caches. **Mean repaint 119.8 → 14.9 ms** on the densest chart, verified by 18,125 parity checks | [performance.md](docs/performance.md) |
| Old charts and saves must keep working | Compatibility contracts, dormant legacy fields, save preflight, **checksummed atomic saves** | [data-model.md](docs/data-model.md) |

![Authoring to runtime pipeline](diagrams/pipeline.svg)

## Repository map

```text
docs/
  architecture.md          System overview and boundaries
  gameplay-system.md       Judgement contract, targets, approach modes, input, scoring
  timing-and-judgement.md  DSP clock, windows, latency calibration
  editor-system.md         Custom editor, seeking, phrase presets
  data-model.md            Conceptual chart and save model
  performance.md           A measured optimisation, with before/after data
  technical-decisions.md   Problem → options → decision → trade-off
  development-history.md   How the system evolved
diagrams/                  SVG diagrams used above (light and dark aware)
examples/
  conceptual-chart.json    A small chart in the conceptual model
  pseudocode/              Clean-room pseudocode for the key algorithms
media/                     Gameplay and editor stills (footage on the portfolio page)

Runtime/  Editor/  Tests/  Samples/  Resources/
                           Small Unity/C# reference implementation (see below)
```

## Reference implementation (Unity / C#)

A compact, framework-light sample written for this repository that demonstrates some of the principles above in runnable form. It is **not** Memora's production code.

- `Runtime/Audio/ScheduledBeatClock.cs`: DSP-scheduled playback and beat reporting
- `Runtime/Timing/BeatMath.cs`, `InputJudge.cs`: beat/second conversion and configurable judgement windows
- `Runtime/Scoring/ScoreAccumulator.cs`: a one-million-point accuracy + combo model
- `Runtime/Visuals/ApproachTimeline.cs`, `ApproachPoseResolver.cs`, `DeterministicTrajectory.cs`: keyframed, beat-domain approach presentation with post-hit persistence and deterministic motion
- `Runtime/Validation/ChartValidator.cs`: validation of a compact chart model
- `Runtime/Performance/ComponentPool.cs`: a reusable component pool
- `Tests/EditMode/`: NUnit edit-mode tests for the maths, timeline, scoring and validation

**Try it:** create a Unity project (2022.3 LTS or newer) and copy `Runtime/`, `Editor/`, `Tests/`, `Samples/` and `Resources/` into a folder under `Assets/`, for example `Assets/RhythmShowcase/`. Run the Edit Mode tests from the Test Runner. **Window → Rhythm Showcase → Validate Sample Chart** validates the included JSON.

## What is deliberately not here

Production source, charts, editor tooling, story content, character art, music and release configuration. Illustrative material is labelled *"Simplified conceptual representation. Not production source code."*

## Author

**Dae Kang**, sole designer and developer of Memora. Business Analyst, moving into software development.
[Portfolio](https://dhk-developer.github.io) · [LinkedIn](https://www.linkedin.com/in/daehurn-kang-003650209) · [GitHub](https://github.com/dhk-developer)

© 2026 Daehurn Kang. All rights reserved. See [LICENSE](LICENSE).
