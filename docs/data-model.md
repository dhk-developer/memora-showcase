# Data model

> Simplified conceptual representation. Not the production schema. Field names are illustrative.

See [examples/conceptual-chart.json](../examples/conceptual-chart.json) for a complete small example.

## Principles

1. **Human-readable JSON.** Charts are diffable and reviewable.
2. **Stable IDs** on every authored object, because notes, cues, layers and presets refer to each other by ID.
3. **Units are explicit.** Time is in *beats* (seconds only at the audio boundary). Positions are in declared spaces: along-line 0–1, line-normal world units, or normalised playfield coordinates.
4. **Authoring data never holds runtime state.** Hit, missed and held states live in a separate runtime record.
5. **Derived data is never serialised.** Indexes, caches and prepared audio windows are rebuilt from revisions.
6. **Unknown fields survive a round trip.** Compatibility is a contract, not a best effort.

## Main record types (conceptual)

| Record | Key fields | Notes |
|---|---|---|
| Chart | `schemaVersion`, `bpm`, `audioOffset`, `songId` | Single BPM by design (see timing doc) |
| Note | `id`, `judgement{beat, action, target, endBeat?, anchors?}`, `presentation{approachKeys[], leadBeats?}`, `visualOnly` | Judgement and presentation are separate objects |
| Line | `id`, `lifetime{start, duration}`, `hittable`, `visible` | Lines are actors with lifetimes, not a fixed track |
| Line event | `lineId`, `channel` (position / rotation / appearance), `from`, `to`, `beat`, `duration`, `easing`, `repeat?` | Channels stay independent |
| Target point | `id`, `path[]`, `lifetime` | Authored, possibly moving hit location |
| Scroll-speed event | `beat`, `duration`, `from`, `to`, `easing` | Presentation only, never timing |
| Visual source | `kind`, `lifetime`, `parameters`, `automation[]` | Rings, sweeps, tunnels, emitters, etc. |
| Layer | `sourceId`, `property`, `operation`, `timing`, `order` | Transform adds, scale and alpha multiply, tint blends |
| Preset instance | `presetId`, `version`, `idMap`, `portBindings`, `baseline` | Pins applied phrases to a version |

## Repeat semantics (documented contract)

- Repeat count includes the original occurrence. Spacing is start-to-start.
- Overlapping occurrences of one source: the newest applicable occurrence wins; the source is not duplicated.
- Persistent channels (position, rotation, appearance) hold their final value. Temporary presentation expires and never "latches" into permanent style.

## Saves

Story progress uses a checksummed envelope and an atomic replace:

```
payload   = serialise(profile)
envelope  = { payload, checksum: sha256(payload) }
write envelope → "save.pending"
replace "save" with "save.pending", keeping "save.bak"
```

On load it tries current, then pending, then backup. A save from a newer schema is never overwritten by an older build.
