using System.Collections.Generic;
using NUnit.Framework;
using RhythmShowcase.Visuals;

namespace RhythmShowcase.Tests.EditMode
{
    public sealed class ApproachTimelineTests
    {
        [Test]
        public void Evaluate_InterpolatesWithinTheAuthoredRange()
        {
            List<ApproachKeyframe> frames = new List<ApproachKeyframe>
            {
                new ApproachKeyframe { beatOffset = -2f, progress = 0f, alpha = 0f, scale = 1f, easeToNext = TimelineEase.Linear },
                new ApproachKeyframe { beatOffset = 0f, progress = 1f, alpha = 1f, scale = 1f, easeToNext = TimelineEase.Linear }
            };

            ApproachSample sample = ApproachTimeline.Evaluate(frames, -1f);
            Assert.That(sample.Progress, Is.EqualTo(0.5f).Within(0.0001f));
            Assert.That(sample.Alpha, Is.EqualTo(0.5f).Within(0.0001f));
        }

        [Test]
        public void Evaluate_CanPersistAfterTheHitByClampingToTheLastKeyframe()
        {
            List<ApproachKeyframe> frames = new List<ApproachKeyframe>
            {
                new ApproachKeyframe { beatOffset = -1f, progress = 0f, alpha = 1f, scale = 1f, easeToNext = TimelineEase.Linear },
                new ApproachKeyframe { beatOffset = 0.5f, progress = 1f, alpha = 0.4f, scale = 1.2f, easeToNext = TimelineEase.Linear }
            };

            ApproachSample sample = ApproachTimeline.Evaluate(frames, 1.5f, afterLast: TimelineBoundaryMode.Clamp);
            Assert.That(sample.Progress, Is.EqualTo(1f));
            Assert.That(sample.Alpha, Is.EqualTo(0.4f));
        }

        [Test]
        public void Evaluate_CanContinueTheLastSegmentAfterTheHit()
        {
            List<ApproachKeyframe> frames = new List<ApproachKeyframe>
            {
                new ApproachKeyframe { beatOffset = -1f, progress = 0f, alpha = 0f, scale = 1f, easeToNext = TimelineEase.Linear },
                new ApproachKeyframe { beatOffset = 0f, progress = 1f, alpha = 1f, scale = 1f, easeToNext = TimelineEase.Linear }
            };

            ApproachSample sample = ApproachTimeline.Evaluate(frames, 0.5f, afterLast: TimelineBoundaryMode.ContinueLinearly);
            Assert.That(sample.Progress, Is.EqualTo(1.5f).Within(0.0001f));
        }
    }
}
