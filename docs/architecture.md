# Architecture

> Conceptual description written for this repository. Names are illustrative. They describe responsibilities, not production class names.

## Three products, one data model

Memora is best understood as three things that share a single chart model:

| Part | Responsibility |
|---|---|
| **Rhythm runtime** | Plays a chart against the audio clock, moves lines and notes, judges multi-touch input, scores, and renders effects. |
| **Authoring environment** | A custom Unity editor window for building charts: timeline and object tree, gamefield preview, audio and waveform, layers, phrase library, undo. A separate story editor compiles narrative episodes. |
| **Narrative and meta layer** | Story reader, characters and outfits, progression and rewards, settings and calibration, crash-safe saves. |

Scale, for context: about 320 C# scripts (roughly two-thirds runtime, one-third editor tooling) and nine scenes.

## Data path

![Authoring to runtime pipeline](../diagrams/pipeline.svg)

1. **Chart file.** Readable JSON with stable IDs for every authored object.
2. **Load.** Compatibility handling and normalisation. Unknown fields are preserved, and legacy fields are read but may be dormant.
3. **Chart model.** The authoring contract. Runtime state (for example, whether a note has been hit) lives separately and is never written back into it.
4. **Indexes.** Derived, *revisioned* structures such as interval trees of object lifetimes, compact repeat schedules and a cached scroll-distance integral. They are rebuilt or invalidated by revision and never serialised.
5. **Evaluators.** Pure functions of *(chart, beat)* that return line poses, target positions, note approach presentation, effect geometry and layer composition.
6. **Two consumers.** Gameplay and the editor preview call the same evaluators and differ only in how they draw.

## Boundaries that matter

- **Judgement before presentation.** Scoring geometry is resolved first. Decorative systems (layers, camera, effects) are applied afterwards and cannot move a scoring target. See [gameplay-system.md](gameplay-system.md).
- **Editor-only data stays in the editor.** Timing-analysis profiles and generated preview audio never ship in a player build.
- **Explicit extension points.** Custom target objects register a provider under a stable kind and ID, and must answer as a pure function of *(chart, object ID, beat, bounds)*. Reading a live transform or the frame clock is forbidden by contract.

## Scene-level structure

Title → Main menu → Song select / Story select / Character select → Loading → Gameplay → Results, plus Settings (including latency calibration). Scene transitions share a cover, load and reveal sequence, and scene-owned music stops with its scene.

## Known structural debt

Two orchestration classes (the gameplay controller and the editor window) are large and split across partial files. That organises them but does not reduce coupling. The planned next step is assembly boundaries around the evaluators, the editor and the story system. See [technical-decisions.md](technical-decisions.md).
