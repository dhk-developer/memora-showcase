using UnityEngine;

namespace RhythmShowcase.Visuals
{
    public enum TrajectoryShape
    {
        None,
        Sine,
        Arc,
        Spiral,
        Jitter
    }

    [System.Serializable]
    public struct TrajectorySettings
    {
        public TrajectoryShape shape;
        [Min(0f)] public float amplitude;
        [Min(0.01f)] public float cycles;
        public float phaseDegrees;
        [Min(0.01f)] public float leadBeats;
    }

    /// <summary>
    /// Adds reproducible presentational motion to an existing approach position.
    /// It never changes the target timing or input windows.
    /// </summary>
    public static class DeterministicTrajectory
    {
        public static Vector2 Apply(
            Vector2 basePosition,
            Vector2 approachDirection,
            string stableId,
            float currentBeat,
            float targetBeat,
            TrajectorySettings settings)
        {
            if (settings.shape == TrajectoryShape.None || settings.amplitude <= 0f)
            {
                return basePosition;
            }

            Vector2 direction = approachDirection.sqrMagnitude > 0.000001f ? approachDirection.normalized : Vector2.up;
            Vector2 lateral = new Vector2(-direction.y, direction.x);
            float lead = Mathf.Max(0.01f, settings.leadBeats);
            float elapsed = 1f - Mathf.Clamp01((targetBeat - currentBeat) / lead);
            float envelope = 1f - elapsed;
            float cycles = Mathf.Max(0.01f, settings.cycles);
            float phase = settings.phaseDegrees * Mathf.Deg2Rad;
            float angle = (elapsed * cycles * Mathf.PI * 2f) + phase;

            return settings.shape switch
            {
                TrajectoryShape.Sine => basePosition + lateral * (Mathf.Sin(angle) * settings.amplitude * envelope),
                TrajectoryShape.Arc => basePosition + lateral * (Mathf.Sin(elapsed * Mathf.PI) * settings.amplitude),
                TrajectoryShape.Spiral => basePosition
                    + lateral * (Mathf.Cos(angle) * settings.amplitude * envelope)
                    + direction * (Mathf.Sin(angle) * settings.amplitude * envelope),
                TrajectoryShape.Jitter => ApplyJitter(basePosition, direction, lateral, stableId, elapsed, settings.amplitude * envelope, cycles),
                _ => basePosition
            };
        }

        private static Vector2 ApplyJitter(
            Vector2 position,
            Vector2 direction,
            Vector2 lateral,
            string stableId,
            float elapsed,
            float amplitude,
            float cycles)
        {
            int step = Mathf.FloorToInt(elapsed * cycles * 20f);
            float lateralNoise = StableSignedNoise(stableId, step, 17);
            float longitudinalNoise = StableSignedNoise(stableId, step, 53);
            return position + lateral * (lateralNoise * amplitude) + direction * (longitudinalNoise * amplitude * 0.3f);
        }

        private static float StableSignedNoise(string text, int step, int salt)
        {
            unchecked
            {
                uint hash = 2166136261;
                if (!string.IsNullOrEmpty(text))
                {
                    for (int index = 0; index < text.Length; index++)
                    {
                        hash ^= text[index];
                        hash *= 16777619;
                    }
                }

                hash ^= (uint)step;
                hash *= 16777619;
                hash ^= (uint)salt;
                hash *= 16777619;
                return (hash / (float)uint.MaxValue) * 2f - 1f;
            }
        }
    }
}
