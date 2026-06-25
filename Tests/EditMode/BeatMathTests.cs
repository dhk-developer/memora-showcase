using NUnit.Framework;
using RhythmShowcase.Timing;

namespace RhythmShowcase.Tests.EditMode
{
    public sealed class BeatMathTests
    {
        [Test]
        public void BeatToSeconds_At120Bpm_MapsOneBeatToHalfASecond()
        {
            Assert.That(BeatMath.BeatToSeconds(1.0, 120.0), Is.EqualTo(0.5).Within(0.000001));
        }

        [Test]
        public void InputJudge_UsesTheNarrowerWindowFirst()
        {
            TimingWindows windows = new TimingWindows(0.04, 0.10);
            JudgementResult result = InputJudge.EvaluateAtBeat(1.05, 1.0, 120.0, windows);
            Assert.That(result.Tier, Is.EqualTo(JudgementTier.Flawless));
        }

        [Test]
        public void InputJudge_MarksLateInputOutsideAllWindowsAsMiss()
        {
            TimingWindows windows = new TimingWindows(0.04, 0.10);
            JudgementResult result = InputJudge.EvaluateAtBeat(1.30, 1.0, 120.0, windows);
            Assert.That(result.Tier, Is.EqualTo(JudgementTier.Miss));
        }
    }
}
