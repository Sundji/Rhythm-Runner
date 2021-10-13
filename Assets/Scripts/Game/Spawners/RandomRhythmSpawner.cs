using SynchronizerData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Divit.RhythmRunner
{
    [RequireComponent(typeof(BeatObserver))]
    public class RandomRhythmSpawner : BaseSpawner
    {
        [SerializeField] private CustomBeatSynchronizer _beatSynchronizer = null;
        [SerializeField] private List<MoveableObject> _objectsToSpawn = new List<MoveableObject>();

        public static Action OnAudioEnd;

        private BeatObserver _beatObserver;

        private bool _isPaused;

        public AudioSource AudioSource
        {
            get
            {
                return _beatSynchronizer.GameAudioSource;
            }
        }

        public void InitializeSpawnerAudio(AudioClip audioClip, float bpm, float audioSourceDelayTime, float audioBeatDelayTime)
        {
            _beatSynchronizer.InitializeBeatSynchronizer(audioClip, bpm, audioSourceDelayTime);
            float activationDelayTime = audioBeatDelayTime > audioSourceDelayTime ? audioBeatDelayTime : audioSourceDelayTime;
            StartCoroutine(WaitAndActivate(activationDelayTime));
        }

        public void Continue()
        {
            _isPaused = false;
            _beatSynchronizer.Continue();
        }

        public void Pause()
        {
            _isPaused = true;
            _beatSynchronizer.Pause();
        }

        protected override void Spawn()
        {
            int index = new Vector2Int(0, _objectsToSpawn.Count).GetRandomNumber();
            MoveableObject obstacle = (MoveableObject)Instantiate(_objectsToSpawn[index], _transform);
            obstacle.InitializeMoving(_transform.position);
        }

        private void Start()
        {
            _beatObserver = GetComponent<BeatObserver>();
            Deactivate();
        }

        private void Update()
        {
            if (_isSpawning && !_isPaused)
            {
                if (_beatSynchronizer.IsOver)
                {
                    OnAudioEnd?.Invoke();
                    Deactivate();
                }
                if ((_beatObserver.beatMask & BeatType.OnBeat) == BeatType.OnBeat)
                {
                    Spawn();
                }
            }
        }

        private IEnumerator WaitAndActivate(float activationDelayTime)
        {
            yield return new WaitForSeconds(activationDelayTime);
            Activate();
        }
    }
}