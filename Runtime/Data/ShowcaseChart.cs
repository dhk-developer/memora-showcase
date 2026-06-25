using System;
using System.Collections.Generic;
using RhythmShowcase.Visuals;
using UnityEngine;

namespace RhythmShowcase.Data
{
    [Serializable]
    public sealed class ShowcaseChart
    {
        [Min(1f)] public float bpm = 120f;
        [Range(1, 16)] public int laneCount = 4;
        public List<ShowcaseNote> notes = new List<ShowcaseNote>();
    }

    public enum ShowcaseNoteType
    {
        Tap,
        Hold,
        Flick,
        Slide
    }

    [Serializable]
    public sealed class ShowcaseNote
    {
        public string id;
        public ShowcaseNoteType type = ShowcaseNoteType.Tap;
        [Min(0f)] public float beat;
        [Min(0f)] public float endBeat;
        public int lane;
        public List<ApproachKeyframe> approach = new List<ApproachKeyframe>();
    }
}
