using System;

namespace RhythmShowcase.Timing
{
    /// <summary>
    /// Converts between musical beats and seconds for a constant-tempo section.
    /// The class is intentionally stateless so it can be used by runtime, tools and tests.
    /// </summary>
    public static class BeatMath
    {
        public const double MinimumBpm = 1.0;

        public static double SecondsPerBeat(double bpm)
        {
            if (bpm < MinimumBpm)
            {
                throw new ArgumentOutOfRangeException(nameof(bpm), bpm, "BPM must be greater than zero.");
            }

            return 60.0 / bpm;
        }

        public static double BeatToSeconds(double beat, double bpm)
        {
            return beat * SecondsPerBeat(bpm);
        }

        public static double SecondsToBeat(double seconds, double bpm)
        {
            return seconds / SecondsPerBeat(bpm);
        }

        public static double BeatAtDspTime(double dspTime, double playbackDspStart, double bpm)
        {
            return SecondsToBeat(dspTime - playbackDspStart, bpm);
        }
    }
}
