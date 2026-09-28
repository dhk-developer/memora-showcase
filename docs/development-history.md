# Development history

Memora began in late 2025 under the working title **PageBeat**. It is developed by one person, alongside full-time work.

| Area of work (roughly in order) | Focus |
|---|---|
| Prototype | Moving judgement lines, basic tap/hold/slide/flick, DSP-scheduled audio, JSON charts |
| Expressive notes | Floating notes, keyframed approach presentation in beats, visual-only notes, choreography effects |
| Targets | Notes aimed at other notes and at moving authored points; predictive and adaptive approach |
| Shared evaluation | Extracting runtime and editor evaluators after the preview drifted from gameplay; parity checks |
| Authoring workspace | Notes / lines / visuals workspaces, layers dock, typed inspector edits, scoped undo |
| Phrase library | Parameterised, versioned presets with ID remapping, ports and pinned instances |
| Presentation | A shared UI design system, vector UI pipeline, scene transitions, latency calibration |
| Story | Story editor, compiled episodes, reader, characters and progression, crash-safe saves |
| Consolidation (Sep 2026) | Removal of obsolete tooling, the measured editor performance pass, architecture documentation |

## How it evolved

Most features arrived as **optional data**, so earlier charts kept working at each step. The biggest architectural shift was the move to shared evaluators. Once the editor preview and gameplay called the same functions, a whole class of "looks different in the editor" bugs disappeared, and seeking became reliable.

## Status

In active development. Not released. Media will be added to this repository as it becomes safe to publish.
