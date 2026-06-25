using NUnit.Framework;
using RhythmShowcase.Scoring;
using RhythmShowcase.Timing;

namespace RhythmShowcase.Tests.EditMode
{
    public sealed class ScoreAccumulatorTests
    {
        [Test]
        public void PerfectRun_ReachesTheFixedMaximum()
        {
            ScoreAccumulator score = new ScoreAccumulator(4);
            for (int index = 0; index < 4; index++)
            {
                score.Apply(JudgementTier.Flawless);
            }

            Assert.That(score.CurrentScore, Is.EqualTo(ScoreAccumulator.MaximumScore));
            Assert.That(score.FullCombo, Is.True);
        }

        [Test]
        public void Miss_ResetsTheCurrentCombo()
        {
            ScoreAccumulator score = new ScoreAccumulator(3);
            score.Apply(JudgementTier.Flawless);
            score.Apply(JudgementTier.Miss);
            Assert.That(score.CurrentCombo, Is.EqualTo(0));
            Assert.That(score.BestCombo, Is.EqualTo(1));
        }
    }
}
