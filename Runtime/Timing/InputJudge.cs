using System;

namespace RhythmShowcase.Timing
{
    public enum JudgementTier
    {
        Flawless,
        Accurate,
        Miss
    }

    /// <summary>
    /// Timing windows expressed in seconds. Smaller values always take priority.
    /// </summary>
    public readonly struct TimingWindows
    {
        public TimingWindows(double flawlessSeconds, double accurateSeconds)
        {
            if (flawlessSeconds < 0.0)
            {
                throw new ArgumentOutOfRangeException(nameof(flawlessSeconds));
            }

            if (accurateSeconds < flawlessSeconds)
            {
                throw new ArgumentOutOfRangeException(nameof(accurateSeconds), "Accurate must be at least as wide as Flawless.");
            }

            FlawlessSeconds = flawlessSeconds;
            AccurateSeconds = accurateSeconds;
        }

        public double FlawlessSeconds { get; }
        public double AccurateSeconds { get; }
    }

    public readonly struct JudgementResult
    {
        public JudgementResult(JudgementTier tier, double signedErrorSeconds)
        {
            Tier = tier;
            SignedErrorSeconds = signedErrorSeconds;
        }

        public JudgementTier Tier { get; }
        public double SignedErrorSeconds { get; }
        public double AbsoluteErrorMilliseconds => Math.Abs(SignedErrorSeconds) * 1000.0;
    }

    /// <summary>
    /// Converts beat positions to seconds and evaluates timing without relying on Unity frame time.
    /// </summary>
    public static class InputJudge
    {
        public static JudgementResult EvaluateAtBeat(
            double inputBeat,
            double targetBeat,
            double bpm,
            TimingWindows windows)
        {
            double signedErrorSeconds = BeatMath.BeatToSeconds(inputBeat - targetBeat, bpm);
            double absoluteError = Math.Abs(signedErrorSeconds);

            if (absoluteError <= windows.FlawlessSeconds)
            {
                return new JudgementResult(JudgementTier.Flawless, signedErrorSeconds);
            }

            if (absoluteError <= windows.AccurateSeconds)
            {
                return new JudgementResult(JudgementTier.Accurate, signedErrorSeconds);
            }

            return new JudgementResult(JudgementTier.Miss, signedErrorSeconds);
        }
    }
}
