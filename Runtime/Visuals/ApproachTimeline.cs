using System;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmShowcase.Visuals
{
    public enum TimelineEase
    {
        Linear,
        Hold,
        InQuad,
        OutQuad,
        InOutCubic,
        OutExpo
    }

    public enum TimelineBoundaryMode
    {
        Clamp,
        ContinueLinearly,
        Hide
    }

    /// <summary>
    /// A note-presentation keyframe. BeatOffset is measured relative to the note's target beat:
    /// negative values occur before judgement, zero occurs at judgement and positive values are
    /// deliberately supported for presentation that persists after the note is hit.
    /// </summary>
    [Serializable]
    public struct ApproachKeyframe
    {
        public float beatOffset;
        public float progress;
        public float lateralOffset;
        public float normalOffset;
        [Range(0f, 1f)] public float alpha;
        [Min(0f)] public float scale;
        public float rotationDegrees;
        public TimelineEase easeToNext;
    }

    public readonly struct ApproachSample
    {
        public ApproachSample(float progress, float lateralOffset, float normalOffset, float alpha, float scale, float rotationDegrees)
        {
            Progress = progress;
            LateralOffset = lateralOffset;
            NormalOffset = normalOffset;
            Alpha = alpha;
            Scale = scale;
            RotationDegrees = rotationDegrees;
        }

        public float Progress { get; }
        public float LateralOffset { get; }
        public float NormalOffset { get; }
        public float Alpha { get; }
        public float Scale { get; }
        public float RotationDegrees { get; }

        public static ApproachSample Default => new ApproachSample(1f, 0f, 0f, 1f, 1f, 0f);
        public ApproachSample WithAlpha(float alpha) => new ApproachSample(Progress, LateralOffset, NormalOffset, alpha, Scale, RotationDegrees);
    }

    /// <summary>
    /// Evaluates an authored presentation track without allocating. Input keyframes must be in
    /// ascending beat-offset order. The evaluator permits non-monotonic progress to support
    /// deliberate fake-outs and reverse movement.
    /// </summary>
    public static class ApproachTimeline
    {
        public static ApproachSample Evaluate(
            IReadOnlyList<ApproachKeyframe> keyframes,
            float beatOffset,
            TimelineBoundaryMode beforeFirst = TimelineBoundaryMode.Clamp,
            TimelineBoundaryMode afterLast = TimelineBoundaryMode.Clamp)
        {
            if (keyframes == null || keyframes.Count == 0)
            {
                return ApproachSample.Default;
            }

            if (keyframes.Count == 1)
            {
                return ToSample(keyframes[0]);
            }

            if (beatOffset <= keyframes[0].beatOffset)
            {
                return EvaluateBoundary(keyframes[0], keyframes[1], beatOffset, beforeFirst, true);
            }

            int finalIndex = keyframes.Count - 1;
            if (beatOffset >= keyframes[finalIndex].beatOffset)
            {
                return EvaluateBoundary(keyframes[finalIndex - 1], keyframes[finalIndex], beatOffset, afterLast, false);
            }

            for (int index = 0; index < finalIndex; index++)
            {
                ApproachKeyframe from = keyframes[index];
                ApproachKeyframe to = keyframes[index + 1];
                if (beatOffset >= from.beatOffset && beatOffset <= to.beatOffset)
                {
                    return Interpolate(from, to, beatOffset);
                }
            }

            // A malformed list cannot silently return an arbitrary pose.
            return ToSample(keyframes[finalIndex]);
        }

        public static float EvaluateEase(TimelineEase ease, float t)
        {
            t = Mathf.Clamp01(t);
            return ease switch
            {
                TimelineEase.Hold => 0f,
                TimelineEase.InQuad => t * t,
                TimelineEase.OutQuad => 1f - ((1f - t) * (1f - t)),
                TimelineEase.InOutCubic => t < 0.5f ? 4f * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 3f) * 0.5f,
                TimelineEase.OutExpo => t >= 1f ? 1f : 1f - Mathf.Pow(2f, -10f * t),
                _ => t
            };
        }

        private static ApproachSample EvaluateBoundary(
            ApproachKeyframe first,
            ApproachKeyframe second,
            float beatOffset,
            TimelineBoundaryMode mode,
            bool beforeFirst)
        {
            ApproachKeyframe boundary = beforeFirst ? first : second;
            if (mode == TimelineBoundaryMode.Clamp)
            {
                return ToSample(boundary);
            }

            if (mode == TimelineBoundaryMode.Hide)
            {
                return ToSample(boundary).WithAlpha(0f);
            }

            return Extrapolate(first, second, beatOffset);
        }

        private static ApproachSample Interpolate(ApproachKeyframe from, ApproachKeyframe to, float beatOffset)
        {
            float span = Mathf.Max(0.00001f, to.beatOffset - from.beatOffset);
            float t = (beatOffset - from.beatOffset) / span;
            float eased = EvaluateEase(from.easeToNext, t);

            return new ApproachSample(
                Mathf.LerpUnclamped(from.progress, to.progress, eased),
                Mathf.LerpUnclamped(from.lateralOffset, to.lateralOffset, eased),
                Mathf.LerpUnclamped(from.normalOffset, to.normalOffset, eased),
                Mathf.LerpUnclamped(from.alpha, to.alpha, eased),
                Mathf.LerpUnclamped(from.scale, to.scale, eased),
                Mathf.LerpUnclamped(from.rotationDegrees, to.rotationDegrees, eased));
        }

        private static ApproachSample Extrapolate(ApproachKeyframe from, ApproachKeyframe to, float beatOffset)
        {
            float span = Mathf.Max(0.00001f, to.beatOffset - from.beatOffset);
            float t = (beatOffset - from.beatOffset) / span;

            return new ApproachSample(
                Mathf.LerpUnclamped(from.progress, to.progress, t),
                Mathf.LerpUnclamped(from.lateralOffset, to.lateralOffset, t),
                Mathf.LerpUnclamped(from.normalOffset, to.normalOffset, t),
                Mathf.LerpUnclamped(from.alpha, to.alpha, t),
                Mathf.LerpUnclamped(from.scale, to.scale, t),
                Mathf.LerpUnclamped(from.rotationDegrees, to.rotationDegrees, t));
        }

        private static ApproachSample ToSample(ApproachKeyframe frame)
        {
            return new ApproachSample(frame.progress, frame.lateralOffset, frame.normalOffset, frame.alpha, frame.scale, frame.rotationDegrees);
        }
    }
}
