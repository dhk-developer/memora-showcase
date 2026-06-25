# PageBeat

> An original, choreography-led rhythm game in development, built in Unity with C#.

 PageBeat takes direct inspiration from preceding rhythm games, such as *Phigros*, *Arcaea*, *Tone Sphere*, etc.
 
PageBeat is an independent original project with its own systems, visual direction, tools and narrative framing. The game is designed so that a chart has several distinct layers, each with a clear job:

- **Gameplay objects** define what the player can actually hit, where the target is and when it is judged.
- **Visual approach tracks** determine how a playable note enters, moves through and exits the field.
- **Floating notes and moving target points** allow patterns to exist away from a judgement line without turning the entire gamefield into guesswork.
- **Choreography and camera events** animate the space around the chart without becoming accidental gameplay.
- **Visual-only notes and effects** can sell a musical idea while remaining explicitly non-interactive.

The production project, source code, beatmaps, music, story material and unreleased assets are all credited to myself, as the sole author of this project. There are current no plans to open this project up for open source / collaborative work. In this public repository, I have exposed some features of the game as a public case study: a record of the design direction, engineering problems and approved media that can be shared before launch.

<!--
Suggested hero media: a 10-15 second muted gameplay GIF showing a moving line, floating notes,
visual approach animation and a short hit-feedback sequence.

<p align="center">
  <img src="media/gameplay/pagebeat-hero.gif" alt="PageBeat gameplay showing moving judgement lines, floating notes and visual choreography" width="100%" />
</p>
-->

## What the player is reading

In PageBeat, the field is authored as a moving space.

A playable note still has an unambiguous judgement contract: a scheduled beat, a target location, a note action and, where relevant, a duration or anchor path. That contract is deliberately separate from how the note is drawn. A note may drift in from the edge, spiral around its target, snap between beats, fade in late, overshoot the judgement point or leave a short visual tail after it has been hit. None of that changes the moment at which the player is judged.

### Judgement lines as chart actors

Judgement lines are authored objects rather than a fixed backdrop as expected in typical VSRGs. They can move, rotate, enter, leave and change their role over the course of a chart. This is very similar to their function in another rhythm game, Phigros, which is, to this author's knowledge, the most modern innovator of this design. A dense phrase can bring attention into a tight central area; a musical release can open the field back out; a rotation can redirect the eye before a new pattern begins.

The line is still a functional target. Its movement is evaluated from chart data, not improvised at runtime, so the same section can be rehearsed, previewed and played consistently.

<!--
Suggested media: a clean gameplay screenshot with two or more active lines at visibly different angles.
Avoid revealing an unreleased song title, full chart timeline or source-code panels.

<p align="center">
  <img src="media/gameplay/moving-judgement-lines.png" alt="PageBeat gameplay with independently moving judgement lines" width="100%" />
</p>
-->

## Playable notes, floating notes and visual-only notes

The playable chart supports the expected input vocabulary, but the important part is where those notes can live and how they can be presented.

### Judgement-line notes

A standard note belongs to a judgement line. Its hit position is resolved from that line’s authored state at the scheduled beat. This is the base layer for the chart, and it remains intentionally readable even when the surrounding presentation is ambitious.

### Floating notes

Floating notes are not attached to a line. They occupy authored positions in the visible playfield and can be used for patterns that would feel cramped or artificial if they had to remain line-relative.

A floating note can resolve in several ways:

- at its own authored position;
- at the scheduled position of another note; or
- at an authored judgement point, including one that is moving before the hit.

That makes it possible to build patterns where an object appears to travel into another target, converge on a moving point or arrive at a location that is only briefly relevant to the chart. Floating notes can also carry the same playable behaviours as the rest of the gamefield, including holds and slide paths.

The key constraint is that a floating pattern still needs a reliable final read. The movement may be elaborate, but the hit target at the judgement beat is deterministic.

### Visual-only notes

Not every note-shaped object in PageBeat is an input.

A visual-only note is rendered and animated through the same presentation language as a playable note, but it is never judged, scored, missed or handled by autoplay. It exists solely to support the choreography of a section: a fake pattern that passes behind the real chart, a burst of objects on a drop, a mirrored movement that makes a playable route easier to understand, or a deliberately misleading-looking flourish that remains clearly outside the active read.

<!--
Suggested media: a short labelled GIF. Keep the labels in the image itself rather than the README body:
"Playable note", "Floating note", "Visual-only note".

<p align="center">
  <img src="media/gameplay/playable-floating-visual-only.gif" alt="Comparison of PageBeat playable notes, floating notes and visual-only notes" width="100%" />
</p>
-->

## Visual approach tracks

A note’s approach is not limited to a fixed scroll direction.

Each note can carry a **visual approach track**, an authored set of keyframes evaluated in musical time. The track controls the presentation of the note relative to its actual judgement target. In practice, that means a chart author can shape position, rotation, scale, opacity and easing over the note’s visible life without changing its beat, input type or hit location.

This is not a generic animation system added on top of the chart. It is authored in the same beat domain as the note itself, which means its motion can be designed around phrases, subdivisions and accents rather than arbitrary seconds on a timeline.

### What this changes in practice

Visual approach tracks make several otherwise awkward chart ideas usable:

- A note can enter on a curve, then settle into a clean final approach.
- A sequence can step forward in musical increments rather than scrolling smoothly.
- A note can orbit or sway around a target before resolving into a clear hit.
- A hold or slide can retain a visual tail after its playable end point, allowing the image to finish a movement without extending the judgement window.
- A note can cross the nominal `t = 1` hit point visually, creating an overshoot or exit, while the gameplay event has already resolved.

The distinction between **visual completion** and **gameplay completion** is intentional. A player should never be asked to keep holding because an effect is still on screen, and an effect should not have to disappear abruptly just because the timing object has been judged.

### Readability controls

The authoring system also treats visibility as a deliberate part of the chart. A note’s visual lead window is defined separately from the keyframes themselves, so an elaborate animation does not accidentally make a note appear too early or too late. Lead timing can be managed from global scroll-speed behaviour or overridden by the chart author for a specific note.

<!--
Suggested media: an editor screenshot showing the keyframe timeline and gamefield preview together.
Crop out raw JSON, complete song metadata and any work-in-progress story material.

<p align="center">
  <img src="media/editor/visual-approach-keyframes.png" alt="PageBeat visual approach keyframes alongside the gamefield preview" width="100%" />
</p>
-->

## Motion without changing judgement

PageBeat has two related but separate motion systems.

### Gameplay motion

Gameplay motion changes a chart object’s actual authored target state. This includes judgement-line transforms, moving judgement points and the target paths that matter to playable notes. Because it affects what the player reads, it is part of the chart’s functional timing data.

### Presentation motion

Presentation motion changes how something looks on its way to, around or away from that target. This includes visual approach tracks, non-linear approach patterns, scale and opacity behaviour, animation tails, note-local effects and visual-only actors.

PageBeat’s interface is framed as a camera recording a performance. The gamefield includes a camera rim, corner markers, aperture treatment and recording cues. They give the chart a consistent visual language for framing attention, building pressure and releasing it again.

The aperture and global camera layers can be timed as part of a chart. A section can contract, open up, rotate, pulse or briefly reframe the entire field while the note logic continues to follow its own beat-based rules.

The aim is to give the game a recognisable vocabulary that connects the rhythm game, with the wider fiction of PageBeat - presenting a performance that is being recorded, replayed and shaped.

<!--
Suggested media: a high-resolution gameplay still where the camera rim and aperture are visible,
but the field remains readable.

<p align="center">
  <img src="media/gameplay/camera-rim-aperture.png" alt="PageBeat gamefield using the camera rim and aperture system" width="100%" />
</p>
-->

## Choreography and VFX

The chart can drive a separate choreography layer that is not tied to a particular note. This gives a song room to build visual rhythm across the whole field rather than relying only on note motion.

The current library includes families of effects such as:

- moving and rotating rings;
- scanner lines and barcode-style sweeps;
- pulse tunnels and travelling wave patterns;
- note rain, eruptions and bounce motifs;
- edge visualisers, tangent rings and spectrogram-like halos;
- hit-linked flashes, shakes, tilts, bursts and glitch treatments;
- line-local feedback that responds to a successful hit without changing the judgement target.

These effects are authored as timed chart events with their own durations, fades, easing and parameter automation. They are deliberately independent of the scoring layer. A chart can use an effect to underline a kick drum, create a transition or draw the eye toward a new region of the field without inventing a fake gameplay rule to do it.

### Hit feedback

The hit-feedback system is kept compact because the playfield already carries a lot of motion. Successful inputs produce a short, local response built from tapered arcs and a mixture of filled and hollow geometric particles. The visual language draws from the game’s soft white-and-blue base with note-specific accent colours, including pink for flicks and yellow for slide behaviour.

<!--
Suggested media: a close-up GIF at normal speed or 50% capture speed showing hit feedback.
A crop is more useful than a full-screen recording here.

<p align="center">
  <img src="media/vfx/hit-feedback-close-up.gif" alt="PageBeat local hit feedback with tapered arcs and geometric particles" width="100%" />
</p>
-->

<!--
Suggested media: a second GIF that shows several choreography effects during a single musical section.
Use only one polished example rather than a large gallery.

<p align="center">
  <img src="media/vfx/choreo-showcase.gif" alt="PageBeat chart choreography and visual effects" width="100%" />
</p>
-->

## Internal beatmap editor

The editor brings the chart’s major layers into one workspace:

- note placement and timing;
- judgement lines and their transform events;
- floating notes, target-note links and target points;
- visual approach keyframes and visible-lead behaviour;
- visual-only notes;
- choreography effects and parameter automation;
- aperture and global camera events;
- preset-driven starting points for recurring visual patterns;
- local gamefield preview, audio preview, waveform support and slower playback for inspection.


### Preview fidelity and performance

The gamefield preview is used to inspect a chart section before a full play-through, so it needs to evaluate the same kinds of note, line and presentation data that runtime gameplay uses. Work on the editor has  included caching and selective evaluation so that the preview remains useful when the chart stops being small.

<!--
Suggested media: one full editor overview. Use an example chart with neutral or public-safe metadata.
Do not show code, file paths, raw schema panels, unreleased song titles or spoilers.

<p align="center">
  <img src="media/editor/beatmap-editor-overview.png" alt="PageBeat internal beatmap editor with chart timeline and gamefield preview" width="100%" />
</p>
-->

<!--
Suggested media: a short before-and-after GIF. Edit one visual approach keyframe, then show the preview result.
This is likely the strongest engineering proof in the repository.

<p align="center">
  <img src="media/editor/keyframe-to-preview.gif" alt="Editing a PageBeat visual approach keyframe and reviewing the result in the preview" width="100%" />
</p>
-->


---

[ WORK IN PROGRESS ]

---

Included in this repository is a small Unity/C# code sample that demonstrates selected engineering ideas developed while building **PageBeat**. This does not include the publishing of PageBeat’s production source, beatmaps, tooling, game content, art, music or proprietary data formats. The contents in this repository serve only to demonstrate the core principles that drive the game mechanics.

This repository and its contents are provided for viewing as a portfolio and project case study only. No permission is granted to copy, modify, distribute, reverse engineer, create derivative works from, or commercially exploit any material in this repository without prior written permission from the copyright holder.

## What this demonstrates

- DSP-clocked beat timing for audio-driven applications
- Beat-to-seconds conversion and configurable input-judgement windows
- A one-million-point scoring model with combo progression
- Keyframed note-approach presentation with easing, non-monotonic motion and explicit post-hit persistence
- Deterministic procedural presentation motion, including a stable jitter mode
- Validation of a compact, serialisable rhythm-chart model
- Edit-mode NUnit tests for pure gameplay maths and validation behaviour

## Use in Unity

1. Create a blank Unity project using Unity 2022.3 LTS or newer.
2. Copy `Assets/RhythmShowcase` into the project’s `Assets` folder.
3. Open **Window → Rhythm Showcase → Validate Sample Chart** to run the validator against the included JSON sample.
4. Open the Test Runner and run the Edit Mode tests.
5. Add `ScheduledBeatClock` to a GameObject with an `AudioSource` to inspect scheduled DSP playback and beat reporting.

The scripts are intentionally framework-light: all runtime code depends only on UnityEngine, while the editor utility depends on UnityEditor and the tests use Unity’s bundled NUnit integration.

## Repository structure

```text
Assets/RhythmShowcase/
  Runtime/
    Audio/          DSP scheduling and beat clock
    Data/           Public chart data contract
    Timing/         Beat maths and input judgement
    Scoring/        Score and combo model
    Visuals/        Keyframe and trajectory evaluators
    Performance/    Reusable Unity component pool
    Validation/     Pure chart-validation rules
  Editor/           Minimal validation menu item
  Tests/EditMode/   NUnit tests
  Samples/          A compact JSON chart sample
Docs/               Design decisions and public-release boundaries
```

## Public-release boundary

This repository deliberately excludes PageBeat’s production code, gameplay scenes, production beatmaps, editor windows, presentation presets, story content, artwork, audio, proprietary schemas and release configuration.

© 2026 Daehurn Kang. All rights reserved.
