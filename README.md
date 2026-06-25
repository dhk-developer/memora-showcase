# PageBeat
 **PageBeat is an original, story-driven rhythm game in active development.**
>
> It combines precision rhythm gameplay, a camera-recording visual language, a bespoke Unity authoring workflow and a character-led presentation layer. The public repository is a technical and creative case study, not a source release.

<!-- Optional repository banner. Add only after you have an approved public asset.
![PageBeat banner](media/branding/pagebeat-banner.png)
-->

**Suggested hero visual:** `media/branding/pagebeat-banner.png`  
A wide, clean key visual or cropped gameplay composition. Keep this free of spoilers, unreleased song names and UI that is still likely to change.

---

## At a glance

| Area | What PageBeat is exploring |
| --- | --- |
| **Genre** | A rhythm game built around timing, moving judgement lines and expressive chart presentation. |
| **Creative identity** | A light, camera-recording interface frames the performance through a rim, aperture and recording language rather than treating the playfield as a static lane. |
| **Gameplay** | Tap, flick, hold and slide interactions are timed against a beat-synchronised gamefield. Notes can be presented through varied trajectories and visual approaches while preserving a clear timing contract. |
| **Authoring** | A custom Unity Editor beatmap workspace supports internal chart creation, audio preview, timeline-driven editing, visual motion and event authoring. |
| **Technical focus** | Beat-domain timing, data-driven charts, responsive custom UI, deterministic visual motion, editor performance and careful separation of gameplay rules from presentation. |
| **Project status** | In active development. Public materials are curated to show the work without exposing production code, commercial assets, unreleased content or proprietary chart data. |

---

## What is PageBeat?

PageBeat is built around a simple idea: a rhythm game should feel like a performance being recorded, not merely a sequence of objects falling through a fixed lane.

The player interacts with notes as they reach active judgement lines, but the field itself is expressive. Lines can move, rotate, appear in different configurations and participate in the musical choreography. Notes can approach through carefully directed visual paths, while the underlying judgement remains anchored to beat timing and the intended hit position. The goal is to create charts that are readable at speed but still have the sense of motion, framing and visual punctuation associated with a music performance.

Outside the gamefield, PageBeat uses a character-led interface and a narrative presentation layer. The game is designed to give the rhythm gameplay an identity beyond a menu and song list, while keeping the core interaction immediate: hear the music, read the field, act on the beat.

**Suggested gameplay visual:** `media/gameplay/gameplay-hero.gif`  
Place a 5 to 8 second silent GIF or short MP4 preview here. Choose a section that shows note motion, the camera-style rim and at least one moving judgement-line moment without revealing a full chart.

<!-- Replace when ready:
![Short PageBeat gameplay clip](media/gameplay/gameplay-hero.gif)
-->

---

## Core gameplay

### Timing first

PageBeat is designed around beat-based timing. Each chart is authored in musical beats rather than as a loose sequence of screen timestamps. This gives the authoring tools and runtime a shared musical coordinate system: notes, line events, visual effects and scene-level choreography can all be described against the same timeline.

At runtime, audio scheduling and gameplay timing are treated as related but distinct concerns. The game uses a stable timing reference for judging player input, while player calibration is handled separately so that an individual device or audio setup can be adjusted without changing the chart itself. This matters for rhythm play because the game should preserve the authored musical relationship even when a player needs to account for their own display, controller or audio latency.

The scoring model supports graded timing outcomes, combo progression and result construction. The visible result is intended to tell the player more than whether a chart was completed: it reflects timing accuracy, consistency and performance across the song.

### Notes and interactions

The playable vocabulary currently centres on four familiar rhythm interactions, presented in a PageBeat-specific field:

- **Tap notes** reward a timed press as the note reaches the judgement line.
- **Flick notes** introduce directional or gesture-based input at the moment of judgement.
- **Hold notes** require a timed start followed by sustained control through their active duration.
- **Slide notes** use anchors and continuous progression to create a path-based interaction rather than a single isolated hit.

The note system is deliberately not limited to one static lane direction. The game can present notes through different approach directions and visual trajectories, allowing a chart to shape attention and momentum in response to the music.

**Suggested explanatory still:** `media/gameplay/note-types-and-feedback.png`  
Place this immediately after the note list. Capture a clean, staged view showing tap, flick, hold and slide examples at once, with no debug labels or internal chart IDs.

<!-- Replace when ready:
![A staged note-type overview](media/gameplay/note-types-and-feedback.png)
-->

### A responsive gamefield rather than a fixed lane

Judgement lines are active gameplay objects. They can form the visual backbone of a chart, but they can also change their pose and presence over time. This creates room for charts where the visual field moves with the rhythm rather than simply displaying notes above a permanently fixed target.

The important design constraint is clarity. PageBeat treats readability as a gameplay requirement, not an afterthought. Motion, visual effects and note approaches are intended to support the player’s understanding of when and where to act. The charting workflow therefore separates the **gameplay pose** of a note or line from visual-only movement where appropriate. A note can have a more expressive visual entrance without making the timing target ambiguous.

This distinction is one of the project’s central engineering ideas. It makes ambitious presentation possible without allowing decorative animation to silently change the gameplay rule underneath the player.

### Visual approach tracks

Each note can be given a keyframed visual approach. Rather than relying only on a single linear spawn-to-hit path, the author can define a controlled sequence of visual states over the note’s visible lifetime. These states can affect presentation properties such as position, scale, rotation and alpha, with easing used to shape how the motion travels between points.

The system supports non-linear movement that is useful for musical emphasis: a note can drift, arc, overshoot, settle, fade, or carry a short visual tail after its gameplay moment. PageBeat also supports persistence beyond the nominal hit moment where the visual design calls for it. That is useful for creating controlled afterimages, resolving motion and effects that complete naturally rather than abruptly disappearing at the exact point of judgement.

The gameplay result remains authoritative. Visual persistence is presentation, not an extra judgement window.

**Suggested technical visual:** `media/editor/visual-approach-keyframes.png`  
Place this after the visual-approach section. Use the editor panel and preview side by side. Crop out file paths, song titles, full chart data and any production-only preset names.

<!-- Replace when ready:
![Visual approach keyframes and preview](media/editor/visual-approach-keyframes.png)
-->

---

## The camera-recording visual language

PageBeat frames gameplay through a camera-inspired interface. A rim and corner treatment establish the screen as a recording space, while aperture and recording elements create a sense of focus, capture and musical punctuation.

This visual language is not intended to obscure the field. It provides a consistent identity across gameplay, menus and transitions while leaving the notes and judgement lines readable. The camera motif also gives the team a useful design vocabulary for choreography: framing can tighten, rotate, breathe or respond to musical structure without making the game feel like a generic overlay.

The playfield is complemented by subtle feedback effects. Input moments can produce concise visual confirmation, and line-level effects can reinforce impact without turning every action into a large screen-wide interruption. The emphasis is on fast feedback that helps the player feel connected to the beat.

**Suggested detail still:** `media/gameplay/camera-rim-and-aperture.png`  
Place this here. Capture a section where the camera rim, aperture and note feedback are visible together, preferably during a visually calm passage so the framing reads clearly.

<!-- Replace when ready:
![Camera rim, aperture and gameplay feedback](media/gameplay/camera-rim-and-aperture.png)
-->

---

## Chart choreography and visual systems

### Data-driven charts

PageBeat uses a data-driven beatmap model. A chart can define notes, judgement lines, line events, visual line effects, camera/aperture events, global choreography, visual layers, judgement points and scroll-speed changes through structured chart data.

This approach gives the project a practical authoring benefit: music-facing decisions can be captured as data and previewed without recompiling gameplay code for every chart change. It also supports the separation of reusable runtime systems from song-specific creative content.

The public repository does not include production beatmaps, the full data schema, custom chart libraries or authoring presets. Those are part of the unreleased game’s content pipeline. What is shared here is the design approach: PageBeat is structured so that musical intent can be represented, validated and rendered as a coherent system.

### Choreography as a first-class charting concern

In PageBeat, choreography is not limited to note placement. The chart can use the surrounding field as part of the music’s visual performance. Judgement lines can be animated, visual layers can be introduced or adjusted, and camera/aperture events can mark a musical transition, accent or section change.

This makes charting closer to staging a short performance than arranging a fixed lane sequence. The technical challenge is maintaining a clear priority order: input and judgement must stay reliable, while visual systems remain synchronised and do not create runaway cost on larger maps.

### Repeatable musical structures

The chart model supports repeat-oriented structures for events where a musical pattern recurs. This helps avoid manually recreating the same timing logic throughout a chart and allows authors to work at the level of musical phrases where appropriate. Repetition is still treated as authored intent rather than as uncontrolled randomisation, so the output remains predictable during editing and gameplay.

**Suggested diagram:** `media/diagrams/chart-to-runtime-flow.png`  
Place this after the choreography section. A simple diagram should show: Chart data → validation → runtime timing → gameplay judgement / visual presentation / feedback. Keep it high level; do not expose class diagrams or private JSON fields.

<!-- Replace when ready:
![High-level chart-to-runtime flow](media/diagrams/chart-to-runtime-flow.png)
-->

---

## Custom Unity beatmap editor

PageBeat includes a bespoke internal editor workspace built in Unity. It exists to make chart authoring practical for the developer, not to expose user-generated content tooling in the released game.

The editor is designed around the real authoring loop:

1. Select or load a chart and audio source.
2. Navigate through the musical timeline using beat-domain controls and audio preview.
3. Place or adjust notes, lines and related events.
4. Inspect a local preview of how the gamefield will present the chosen section.
5. Tune visual approach keyframes, line motion, layers, aperture events and other choreography.
6. Validate the resulting data before runtime testing.

The editor has received particular attention because it is where creative iteration and technical constraints meet. Authoring rhythm content is inherently visual, and a charting tool needs to make timing, density, motion and readability inspectable without requiring the developer to repeatedly enter a full play session for every small change.

### Audio preview and beat navigation

The workspace supports audio-focused authoring, including waveform-oriented tooling, timeline navigation and preview controls. Slower, pitch-conscious preview workflows are part of the broader design so that dense passages can be inspected without losing their musical relationship.

### Layered inspection

Large charts can contain many categories of timed data. The editor therefore supports targeted inspection of layers and event types rather than forcing every authoring decision into one overloaded view. This helps keep the working surface legible while still allowing the developer to reason about how note timing, judgement-line behaviour and presentation systems interact.

### Visual preview and performance work

The gamefield preview is an important authoring feature but can become expensive when a chart contains many notes, visual events or active lines. PageBeat includes dedicated work on preview caching, scalable rendering and selective evaluation so the editor can remain usable as chart size increases.

This is an active engineering area rather than a claim that all performance problems are solved. The project treats editor responsiveness as product-quality work because the speed of the authoring loop directly affects the quality and quantity of future chart content.

**Suggested editor overview:** `media/editor/beatmap-editor-overview.png`  
Place this at the start of the editor section. Capture the timeline, central preview and inspector at a readable scale. Redact or crop source folders, personal paths, unreleased track names and data values that would expose the production schema.

<!-- Replace when ready:
![PageBeat internal beatmap editor](media/editor/beatmap-editor-overview.png)
-->

**Suggested editor workflow GIF:** `media/gifs/editor-to-preview-workflow.gif`  
Place this at the end of the editor section. A 6 to 10 second clip can show selecting an event, changing a value and observing the preview update. Avoid opening code windows or displaying full JSON files.

<!-- Replace when ready:
![Editor-to-preview workflow](media/gifs/editor-to-preview-workflow.gif)
-->

---

## Engineering approach

### Clear responsibilities between systems

PageBeat is organised around separable responsibilities. The chart describes what should happen in musical time. Runtime systems resolve that data into notes, lines, state and effects. Presentation systems render the result. Scoring and results systems record the player’s performance. Editor systems help author and inspect the data without becoming part of the shipped player experience.

This separation is useful for both development and debugging. It makes it easier to ask whether an issue is caused by the chart data, timing conversion, gameplay rule, visual resolver, editor preview or display layer, instead of treating the entire gamefield as one indivisible behaviour.

### Deterministic presentation where it matters

Some visual motion is procedural, but it is designed to be reproducible. A charted moment should look consistent between editing and playback when the same authored inputs are used. This matters for rhythm content because unpredictable presentation makes it harder to validate whether a chart is fair, readable and musically intentional.

### Runtime performance and object reuse

Rhythm gameplay can involve a large number of short-lived visual objects. PageBeat uses reusable runtime patterns to reduce avoidable allocation and instantiation cost during play. The aim is not only raw frame rate. It is consistency: timing-focused games benefit from stable behaviour when the screen becomes busy.

### Responsive UI built for the game’s visual identity

The menus and gameplay interface use custom UI components and theme-aware presentation rather than relying solely on generic default controls. The current direction combines a light, modern, high-contrast presentation with a camera-inspired frame and deliberately restrained accent colours.

The main menu, song selection, settings, gameplay, loading and results experiences are treated as connected parts of the product. Scene transitions and feedback are designed to make moving between them feel intentional rather than like a series of disconnected Unity screens.

### Player settings and calibration

Player-facing settings include controls intended to support timing calibration and personal comfort. The project keeps player-specific offset handling distinct from the authored music timing so that adjustment for one player does not alter the intended chart for everyone else.

**Suggested systems diagram:** `media/diagrams/pagebeat-systems-overview.png`  
Place this after the engineering approach section. Show four boxes: Authoring, Chart Data, Runtime Gameplay, Player Experience. Connect them with simple arrows. Do not show private script names or repository topology.

<!-- Replace when ready:
![PageBeat systems overview](media/diagrams/pagebeat-systems-overview.png)
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
