# "What is active at this beat?"

> Simplified conceptual representation. Not production source code.

Charts contain thousands of objects with lifetimes `[start, end]`. Scanning all of them every frame, or on every editor seek, doesn't scale. An **augmented, self-balancing interval tree** answers overlap queries in *O(log n + k)*.

```text
node = { start, end, maxEnd, value, left, right, height }
# maxEnd = the largest end anywhere in this subtree

insert(node, item):            # ordinary balanced-BST insert keyed on start (ties: insertion order)
    ... rotate to keep heights balanced ...
    node.maxEnd = max(node.end, left.maxEnd, right.maxEnd)

query(node, beat, out):
    if node is null or node.maxEnd < beat: return      # nothing in this subtree is still alive
    query(node.left, beat, out)
    if node.start <= beat <= node.end: out.append(node.value)
    if node.start <= beat: query(node.right, beat, out)  # right subtree starts later; prune when too late
```

Design notes:

- Results are **never truncated**. A dense passage can't silently lose objects.
- Indexes are **derived and revisioned**. An edit bumps a revision and only affected indexes rebuild. They are never saved in the chart.
