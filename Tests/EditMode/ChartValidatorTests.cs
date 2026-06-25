using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using RhythmShowcase.Data;
using RhythmShowcase.Validation;

namespace RhythmShowcase.Tests.EditMode
{
    public sealed class ChartValidatorTests
    {
        [Test]
        public void Validate_FlagsDuplicateIdsAndInvalidLane()
        {
            ShowcaseChart chart = new ShowcaseChart
            {
                bpm = 120f,
                laneCount = 2,
                notes = new List<ShowcaseNote>
                {
                    new ShowcaseNote { id = "a", beat = 1f, lane = 0 },
                    new ShowcaseNote { id = "a", beat = 2f, lane = 5 }
                }
            };

            IReadOnlyList<ValidationIssue> issues = ChartValidator.Validate(chart);
            Assert.That(issues.Any(issue => issue.Message.Contains("Duplicate")), Is.True);
            Assert.That(issues.Any(issue => issue.Message.Contains("outside")), Is.True);
        }
    }
}
