using System;
using UnityEngine;

namespace Divit.RhythmRunner
{
    public class CustomBeatSynchronizer : MonoBehaviour
    {
        [SerializeField] private AudioSource _privateAudioSource = null;
        [SerializeField] private AudioSource _publicAudioSource = null;

        public static Action<double> OnAudioStart;
        public static Action<CustomBeatSynchronizer> OnInitialize;

        private float _audioDuration;

        public float Bpm
        {
            get;
            private set;
        }

        public bool IsPaused
        {
            get;
            private set;
        }

        public bool IsOver
        {
            get
            {
                return _audioDuration - _publicAudioSource.time < 0.015f;
            }
        }

        public AudioSource BeatSynchronizationAudioSource
        {
            get
            {
                return _privateAudioSource;
            }
        }

        public AudioSource GameAudioSource
        {
            get
            {
                return _publicAudioSource;
            }
        }

        public void InitializeBeatSynchronizer(AudioClip audioClip, float bpm, float delayTime)
        {
            _audioDuration = audioClip.length;

            _privateAudioSource.clip = audioClip;
            _privateAudioSource.volume = 0;

            Bpm = bpm;
            OnInitialize?.Invoke(this);

            _publicAudioSource.clip = audioClip;
            _publicAudioSource.volume = 1;

            double initTime = AudioSettings.dspTime;
            _privateAudioSource.PlayScheduled(initTime);
            _publicAudioSource.PlayScheduled(initTime + delayTime);
            OnAudioStart?.Invoke(initTime);
        }

        public void Continue()
        {
            AudioListener.pause = false;
            IsPaused = false;
        }

        public void Pause()
        {
            AudioListener.pause = true;
            IsPaused = true;
        }
    }
}