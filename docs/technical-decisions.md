# Technical decisions

Each entry follows *problem → options → decision → trade-off*.

### 1. Separate judgement from presentation
- **Problem:** expressive note motion kept threatening fair, explainable judgement.
- **Options:** one transform for both; or two layers with judgement resolved first.
- **Decision:** two layers. Decorative systems apply after judgement geometry.
- **Trade-off:** more data per note and two paths to keep consistent. In return, "why was that a Miss?" always has one inspectable answer.

### 2. Time from the audio DSP clock, state sampled at the beat
- **Problem:** frame time drifts and jitters. Integrating motion per frame accumulates error.
- **Decision:** schedule audio, derive song time from DSP time, and sample every pose at the beat.
- **Trade-off:** every system has to be expressible as a function of time, which rules out some simulation-style effects. That constraint is also what makes seeking possible.

### 3. Shared evaluators for editor and runtime
- **Problem:** the preview's own approximations drifted from gameplay in effects, projection and audio response.
- **Decision:** extract *(chart, beat) → state* evaluators used by both, and add parity checks.
- **Trade-off:** extraction after the fact cost more than designing it in from the start. That's the main lesson.

### 4. Presets compile to pinned copies
- **Problem:** reusable passages, but finished songs must never change by accident.
- **Options:** live references; or compiled copies.
- **Decision:** compile, remap IDs, bind ports, pin the version, and make re-apply explicit.
- **Trade-off:** library fixes don't propagate automatically.

### 5. Keep IMGUI for the editor
- **Problem:** the editor felt slow, and a framework rewrite was tempting.
- **Decision:** profile first. The costs were serialisation and invalidation, so fix those. See [performance.md](performance.md).
- **Trade-off:** IMGUI is older technology, and a future migration would need its own focus, undo and DPI testing.

### 6. Treat existing data as a stakeholder
- **Problem:** features evolve, and old charts and saves must keep working.
- **Decision:** unknown fields and IDs are contracts. Superseded fields stay readable but dormant, and a save preflight protects unsupported data.
- **Trade-off:** some legacy fields remain in the model indefinitely.

### 7. Defer variable BPM rather than approximate it
- **Problem:** tempo changes are common in music.
- **Decision:** don't fake them with scroll-speed changes. They need one invertible clock across playback, grid, waveform and judgement.
- **Trade-off:** single-BPM charts for now.

### 8. Crash-safe saves
- **Decision:** checksummed envelope, write to pending, atomic replace with backup, and never downgrade a newer schema.
- **Trade-off:** slightly more I/O per save, and none of the "my progress disappeared" class of bug.

## What I'd do differently

- Put assembly boundaries around evaluators, editor and story from the start.
- Split the two largest orchestration classes by responsibility, not just by file.
- Measure on target devices earlier.
