using UnityEngine;

namespace RhythmShowcase.Visuals
{
    /// <summary>
    /// Converts a timeline sample into a world-space pose relative to an arrival point and a
    /// local tangent/normal basis. It does not know about charts, cameras or input judgement.
    /// </summary>
    public static class ApproachPoseResolver
    {
        public static Vector2 ResolvePosition(
            Vector2 arrivalPosition,
            Vector2 tangent,
            Vector2 normal,
            Vector2 preferredArrivalDirection,
            float fullApproachDistance,
            float localOffsetScale,
            ApproachSample sample)
        {
            Vector2 approachDirection = SafeNormal(preferredArrivalDirection, normal);
            Vector2 safeTangent = SafeNormal(tangent, new Vector2(-approachDirection.y, approachDirection.x));
            Vector2 safeNormal = SafeNormal(normal, approachDirection);

            float remainingDistance = fullApproachDistance * (1f - sample.Progress);
            return arrivalPosition
                + (approachDirection * remainingDistance)
                + (safeTangent * (sample.LateralOffset * localOffsetScale))
                + (safeNormal * (sample.NormalOffset * localOffsetScale));
        }

        private static Vector2 SafeNormal(Vector2 candidate, Vector2 fallback)
        {
            if (candidate.sqrMagnitude > 0.000001f)
            {
                return candidate.normalized;
            }

            return fallback.sqrMagnitude > 0.000001f ? fallback.normalized : Vector2.up;
        }
    }
}
