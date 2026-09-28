# Device-latency calibration

> Simplified conceptual representation. Not production source code.

```text
REQUIRED = 4
samples = []
lastBar = -infinity

on tap(dspTime):
    bar = nearestAccentIndex(dspTime)            # accents are scheduled on the audio clock
    if bar <= lastBar: ignore                    # second finger / double tap on the same accent
    delta = dspTime - accentTime(bar)
    if abs(delta) > ACCEPTANCE: ignore           # not aimed at an accent
    samples.append(delta); lastBar = bar
    if len(samples) < REQUIRED: return

    s = sorted(samples)
    offset = (s[1] + s[2]) / 2                   # mean of the middle two: one stray tap can't dominate
    mad = median(abs(x - offset) for x in samples)
    reliable = mad <= max(ABS_TOLERANCE, beatLength * REL_TOLERANCE)
               and (s[2] - s[1]) <= max(PAIR_TOLERANCE, beatLength * PAIR_REL)
    if reliable: save(offset)                    # only a reliable set replaces the stored offset
    else:        ask for a retry                 # two conflicting clusters never overwrite a saved value
    samples = []
```
