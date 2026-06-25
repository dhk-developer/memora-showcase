using System;
using RhythmShowcase.Timing;

namespace RhythmShowcase.Scoring
{
    /// <summary>
    /// A compact score model with a fixed maximum. Accuracy and combo are intentionally
    /// separate pools so a player can see both judgement quality and sustained consistency.
    /// </summary>
    public sealed class ScoreAccumulator
    {
        public const int MaximumScore = 1_000_000;

        private const double AccuracyPool = 850_000.0;
        private const double ComboPool = 150_000.0;

        private readonly int totalUnits;
        private int processedUnits;
        private int currentCombo;
        private int bestCombo;
        private int flawlessCount;
        private int accurateCount;
        private int missCount;
        private double accuracyPoints;
        private double comboPoints;

        public ScoreAccumulator(int totalUnits)
        {
            if (totalUnits <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(totalUnits));
            }

            this.totalUnits = totalUnits;
        }

        public int CurrentScore
        {
            get
            {
                int rounded = (int)Math.Round(accuracyPoints + comboPoints);
                return Math.Max(0, Math.Min(MaximumScore, rounded));
            }
        }
        public int CurrentCombo => currentCombo;
        public int BestCombo => bestCombo;
        public int FlawlessCount => flawlessCount;
        public int AccurateCount => accurateCount;
        public int MissCount => missCount;
        public bool IsComplete => processedUnits >= totalUnits;
        public bool FullCombo => IsComplete && missCount == 0;

        public double ProjectedAccuracyPercent
        {
            get
            {
                int remaining = Math.Max(0, totalUnits - processedUnits);
                double weighted = flawlessCount + (accurateCount * 0.75) + remaining;
                return (weighted / totalUnits) * 100.0;
            }
        }

        public void Apply(JudgementTier judgement)
        {
            if (IsComplete)
            {
                return;
            }

            processedUnits++;
            accuracyPoints += (AccuracyPool / totalUnits) * GetWeight(judgement);

            if (judgement == JudgementTier.Miss)
            {
                missCount++;
                currentCombo = 0;
                return;
            }

            if (judgement == JudgementTier.Flawless)
            {
                flawlessCount++;
            }
            else
            {
                accurateCount++;
            }

            currentCombo++;
            bestCombo = Math.Max(bestCombo, currentCombo);

            // 1 + 2 + ... + N distributes the complete combo pool exactly across a full run.
            double denominator = totalUnits * (totalUnits + 1) / 2.0;
            comboPoints += ComboPool * currentCombo / denominator;
        }

        private static double GetWeight(JudgementTier judgement)
        {
            return judgement switch
            {
                JudgementTier.Flawless => 1.0,
                JudgementTier.Accurate => 0.75,
                _ => 0.0
            };
        }
    }
}
