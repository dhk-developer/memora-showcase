# Judging a tap

> Simplified conceptual representation. Not production source code.

```text
function hitPoint(chart, note):
    # Judgement geometry only; presentation is never consulted.
    t = note.judgement.beat
    target = note.judgement.target
    switch target.kind:
        case "line":   pose = linePoseAt(chart, target.lineId, t)            # sampled at the hit beat
                       return pose.origin + pose.tangent * target.along + pose.normal * target.offset
        case "free":   return playfieldToWorld(target.position)
        case "note":   other = chart.note(target.noteId)
                       return headPositionAt(chart, other, t)                # the other note, at THIS note's beat
        case "point":  return pointPathAt(chart, target.pointId, t)          # authored, possibly moving

function judgeTap(note, tapSongTime, windows):
    delta = tapSongTime - beatToSeconds(note.judgement.beat)
    if abs(delta) <= windows.perfect: return PERFECT
    if abs(delta) <= windows.great:   return GREAT
    return NONE     # outside the window: the tap belongs to no note; misses are assigned when a window closes
```

Key properties:

- Every function above is a pure function of *(chart, beat)*. Nothing reads frame time, live transforms or accumulated state.
- Visual-only notes never reach `judgeTap`.
- Decorative layers, camera motion and effects are applied after `hitPoint`, so they can't move it.
