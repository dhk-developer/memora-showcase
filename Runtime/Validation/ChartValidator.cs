using System;
using System.Collections.Generic;
using RhythmShowcase.Data;
using RhythmShowcase.Visuals;

namespace RhythmShowcase.Validation
{
    public enum ValidationSeverity
    {
        Warning,
        Error
    }

    public readonly struct ValidationIssue
    {
        public ValidationIssue(ValidationSeverity severity, string location, string message)
        {
            Severity = severity;
            Location = location;
            Message = message;
        }

        public ValidationSeverity Severity { get; }
        public string Location { get; }
        public string Message { get; }
        public override string ToString() => $"[{Severity}] {Location}: {Message}";
    }

    /// <summary>
    /// Validates only the public showcase chart contract. It has no AssetDatabase dependency,
    /// which keeps it usable in editor tools, tests and future automated build checks.
    /// </summary>
    public static class ChartValidator
    {
        public static IReadOnlyList<ValidationIssue> Validate(ShowcaseChart chart)
        {
            List<ValidationIssue> issues = new List<ValidationIssue>();
            if (chart == null)
            {
                issues.Add(new ValidationIssue(ValidationSeverity.Error, "Chart", "Chart data is missing."));
                return issues;
            }

            if (chart.bpm <= 0f)
            {
                issues.Add(new ValidationIssue(ValidationSeverity.Error, "Chart.bpm", "BPM must be greater than zero."));
            }

            if (chart.laneCount < 1)
            {
                issues.Add(new ValidationIssue(ValidationSeverity.Error, "Chart.laneCount", "At least one lane is required."));
            }

            if (chart.notes == null)
            {
                issues.Add(new ValidationIssue(ValidationSeverity.Error, "Chart.notes", "Notes collection is missing."));
                return issues;
            }

            HashSet<string> ids = new HashSet<string>(StringComparer.Ordinal);
            for (int index = 0; index < chart.notes.Count; index++)
            {
                ValidateNote(chart.notes[index], index, chart.laneCount, ids, issues);
            }

            return issues;
        }

        private static void ValidateNote(
            ShowcaseNote note,
            int index,
            int laneCount,
            HashSet<string> ids,
            List<ValidationIssue> issues)
        {
            string location = $"notes[{index}]";
            if (note == null)
            {
                issues.Add(new ValidationIssue(ValidationSeverity.Error, location, "Note entry is null."));
                return;
            }

            if (string.IsNullOrWhiteSpace(note.id))
            {
                issues.Add(new ValidationIssue(ValidationSeverity.Warning, location, "Note has no stable id."));
            }
            else if (!ids.Add(note.id))
            {
                issues.Add(new ValidationIssue(ValidationSeverity.Error, location + ".id", $"Duplicate note id '{note.id}'."));
            }

            if (note.beat < 0f)
            {
                issues.Add(new ValidationIssue(ValidationSeverity.Error, location + ".beat", "Beat cannot be negative."));
            }

            if (note.lane < 0 || note.lane >= laneCount)
            {
                issues.Add(new ValidationIssue(ValidationSeverity.Error, location + ".lane", "Lane is outside the chart lane count."));
            }

            bool hasDuration = note.type == ShowcaseNoteType.Hold || note.type == ShowcaseNoteType.Slide;
            if (hasDuration && note.endBeat <= note.beat)
            {
                issues.Add(new ValidationIssue(ValidationSeverity.Error, location + ".endBeat", "Sustained notes must end after they begin."));
            }

            ValidateApproach(location + ".approach", note.approach, issues);
        }

        private static void ValidateApproach(string location, List<ApproachKeyframe> frames, List<ValidationIssue> issues)
        {
            if (frames == null || frames.Count == 0)
            {
                issues.Add(new ValidationIssue(ValidationSeverity.Warning, location, "No visual approach track is authored."));
                return;
            }

            float previousOffset = float.NegativeInfinity;
            for (int index = 0; index < frames.Count; index++)
            {
                ApproachKeyframe frame = frames[index];
                string frameLocation = $"{location}[{index}]";
                if (frame.beatOffset < previousOffset)
                {
                    issues.Add(new ValidationIssue(ValidationSeverity.Error, frameLocation, "Keyframes must be in ascending beat-offset order."));
                }

                if (frame.alpha < 0f || frame.alpha > 1f)
                {
                    issues.Add(new ValidationIssue(ValidationSeverity.Warning, frameLocation + ".alpha", "Alpha is expected to be between zero and one."));
                }

                if (frame.scale < 0f)
                {
                    issues.Add(new ValidationIssue(ValidationSeverity.Warning, frameLocation + ".scale", "Negative scale is probably unintended."));
                }

                previousOffset = frame.beatOffset;
            }
        }
    }
}
