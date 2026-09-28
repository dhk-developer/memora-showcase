# Compiling a phrase preset

> Simplified conceptual representation. Not production source code.

```text
function applyPreset(chart, preset, params, portBindings):
    validate params against preset.parameterSchema       # whitelisted, typed, bounded; no code execution
    for port in preset.ports: require portBindings[port] exists in chart

    idMap = {}
    for record in preset.records:
        idMap[record.id] = chart.uniqueId(prefix = record.id)

    newRecords = []
    for record in preset.records:
        r = deepCopy(record)
        r.id = idMap[record.id]
        rewrite every internal reference in r via idMap
        rewrite every port reference in r via portBindings
        substitute parameter expressions in r with params
        newRecords.append(r)

    check dependencies (lifetimes, target cycles) on the result
    instance = { presetId, version: preset.version, idMap, portBindings, params, baseline: hash(newRecords) }

    chart.transaction("Apply phrase"):                   # one undo step
        chart.add(newRecords)
        chart.addInstance(instance)

function reapply(chart, instance, newerPreset):
    if hash(current records of instance) != instance.baseline:
        refuse("Local edits would be overwritten")       # never silently replace an author's changes
    ... otherwise remove old records and apply newerPreset with the same bindings ...
```
