# Gameplay system

> Simplified conceptual representation. Not production source code.

## The problem

In a moving-line rhythm game, *where and when is this note judged?* stops being trivial. A note may belong to a rotating line, float in open space, aim at another note, or aim at a point that is still moving. At the same time, chart authors want its entrance to be expressive: curves, spirals, snaps, overshoots.

If one transform drives both what the player sees and what they must hit, every visual idea becomes a gameplay risk.

## The judgement contract

![Judgement contract versus presentation](../diagrams/judgement-contract.svg)

Every playable note has two layers that never write to each other:

| Layer | Contains | Consumed by |
|---|---|---|
| **Judgement contract** | beat, target, action (tap / hold / slide / flick), duration or anchors | input judgement, scoring, autoplay, miss detection |
| **Presentation** | approach keyframes in beats: offset, scale, opacity, rotation, easing; visual lead time; post-hit tail | renderers only |

Three resolution paths (*where is it judged*, *where is its head drawn*, *where does its cue line start*) are computed separately and meet at the same hit point.

**Visual-only notes** use the presentation layer alone. They are drawn like notes but are never judged, scored, missed or played by autoplay.

## Targets

A note's target can be:

- a position along a judgement line (and an offset from it);
- a free position in the playfield;
- **another note**, sampled at *this* note's hit beat, not the other note's own beat;
- an **authored point** that may be moving.

Targets are sampled from chart time, never from a live object. That is what keeps forward play and backward seeking identical.

## Predictive and adaptive approach

- **Predictive** (default): aim at the target's pose *at the hit beat*. It is readable when a line is about to move.
- **Adaptive**: follow the target's *current* pose. When a line teleports (a zero-duration jump), apply a correction so the note doesn't jump with it. Corrections are rebuilt from a compiled teleport schedule each time, so the result doesn't depend on the previous frame or on seek order.

See [examples/pseudocode/adaptive-correction.md](../examples/pseudocode/adaptive-correction.md).

## Input

- Multi-touch with per-finger tracking (mouse in the editor and on desktop).
- Flicks need a minimum travel distance and must reset between flicks.
- Holds and slides are judged while held. Scoring units are one for a tap, two for a hold, and one per anchor for a slide.

## Scoring

Maximum 1,000,000: 900,000 for accuracy (Perfect = 100%, Great = 70%) plus 100,000 for combo. A full Perfect run always totals exactly 1,000,000. The [reference implementation](../Runtime/Scoring/ScoreAccumulator.cs) in this repository models the same idea.

## Choreography and effects

A separate choreography layer adds rings, scanner sweeps, tunnels, decorative note rain, edge visualisers, screen pulses and hit-linked flashes. It also drives an animated camera "aperture" that frames the playfield. Each effect declares what it outputs, and a **layers** system composes transforms, rotation, scale, thickness, alpha and tint in a documented order. None of it can alter the scoring layer.
