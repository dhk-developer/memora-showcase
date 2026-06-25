using System;
using RhythmShowcase.Timing;
using UnityEngine;

namespace RhythmShowcase.Audio
{
    /// <summary>
    /// Starts an AudioSource against Unity's DSP clock and exposes the corresponding musical beat.
    /// It avoids frame-time accumulation, which would gradually drift from audio playback.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class ScheduledBeatClock : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField, Min(1f)] private double bpm = 120.0;
        [SerializeField, Min(0.05f)] private double scheduleLeadSeconds = 0.15;

        private double scheduledStartDspTime;
        private bool isScheduled;

        public bool IsScheduled => isScheduled;
        public double Bpm => bpm;
        public double ScheduledStartDspTime => scheduledStartDspTime;
        public double CurrentBeat => isScheduled
            ? BeatMath.BeatAtDspTime(AudioSettings.dspTime, scheduledStartDspTime, bpm)
            : 0.0;

        private void Reset()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public bool ScheduleFromStart()
        {
            if (audioSource == null || audioSource.clip == null)
            {
                Debug.LogWarning("ScheduledBeatClock requires an AudioSource with an AudioClip.", this);
                return false;
            }

            audioSource.Stop();
            scheduledStartDspTime = AudioSettings.dspTime + scheduleLeadSeconds;
            audioSource.PlayScheduled(scheduledStartDspTime);
            isScheduled = true;
            return true;
        }

        public void StopPlayback()
        {
            if (audioSource != null)
            {
                audioSource.Stop();
            }

            isScheduled = false;
        }

        public void SetBpm(double newBpm)
        {
            if (isScheduled)
            {
                throw new InvalidOperationException("Stop playback before changing the clock BPM.");
            }

            // Exercise the validation path before mutating the clock.
            BeatMath.SecondsPerBeat(newBpm);
            bpm = newBpm;
        }
    }
}
