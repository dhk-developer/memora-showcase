# Adaptive approach with teleports

> Simplified conceptual representation. Not production source code.

An adaptive note follows its target's *current* pose. If the target line jumps instantly (a zero-duration "teleport"), a naive follower would jump too. Instead, the note keeps its visual continuity and eases the difference out before the hit.

```text
function adaptiveHead(note, currentBeat, teleports, sample):
    # teleports: sorted beats of zero-duration target jumps, compiled once from the chart
    base = sample(currentBeat)                    # where a plain follower would be now
    corrections = []
    for tb in teleports between note.approachStart and min(currentBeat, note.hitBeat):
        before = sample(tb - EPSILON)
        after  = sample(tb)
        jump = before - after
        if length(jump) > MIN_JUMP: corrections.append((tb, jump))
    for (tb, jump) in corrections:
        # full offset at the teleport, fading to zero by the hit beat
        base += jump * fade(from = tb, to = note.hitBeat, at = currentBeat)
    return base
```

Why it's built this way:

- **Stateless.** The result depends only on *(chart, beat)*, not on the previous frame. Seeking backwards gives the same answer as playing forwards.
- **Compact.** Repeated teleports are stored once and expanded only for the queried window.
- **Judgement-safe.** The correction reaches zero by the hit beat, so the note always arrives at the contract's hit point.
