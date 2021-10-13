using SynchronizerData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Divit.RhythmRunner
{
	public class CustomBeatCounter : MonoBehaviour
	{
		[SerializeField] BeatType _beatType = BeatType.OnBeat;
		[SerializeField] BeatValue _beatValue = BeatValue.WholeBeat;
		[SerializeField] BeatValue _beatOffset = BeatValue.None;
		[SerializeField] bool _beatOffsetNegative = false;
		[SerializeField] int _beatScalar = 1;
		[SerializeField] float _loopTime = 0;

		[SerializeField] private List<BeatObserver> _beatObservers = new List<BeatObserver>();

		private CustomBeatSynchronizer _beatSynchronizer;

		private AudioSource _audioSource;
		private float _audioFrequency;

		private float _currentSample;
		private float _nextSample;
		private float _samplePeriod;
		private float _sampleOffset;

		private void Awake()
		{
			CustomBeatSynchronizer.OnInitialize += OnInitializeBeatSynchronizer;
			CustomBeatSynchronizer.OnAudioStart += StartBeatCheck;
		}

		private void OnDestroy()
		{
			CustomBeatSynchronizer.OnInitialize -= OnInitializeBeatSynchronizer;
			CustomBeatSynchronizer.OnAudioStart -= StartBeatCheck;
		}

		private void OnInitializeBeatSynchronizer(CustomBeatSynchronizer beatSynchronizer)
		{
			_beatSynchronizer = beatSynchronizer;

			_audioSource = beatSynchronizer.BeatSynchronizationAudioSource;
			_audioFrequency = _audioSource.clip.frequency;

			float bpm = beatSynchronizer.Bpm;
			_samplePeriod = (60f / (bpm * BeatDecimalValues.values[(int)_beatValue])) * _audioFrequency;

			if (_beatOffset != BeatValue.None)
			{
				_sampleOffset = (60f / (bpm * BeatDecimalValues.values[(int)_beatOffset])) * _audioFrequency;
				_sampleOffset = _beatOffsetNegative ? _samplePeriod - _sampleOffset : _sampleOffset;
			}

			_samplePeriod *= _beatScalar;
			_sampleOffset *= _beatScalar;
			_nextSample = 0;
		}

		private void StartBeatCheck(double syncTime)
		{
			_nextSample = (float)syncTime * _audioFrequency;
			StartCoroutine(BeatCheck());
		}

		private IEnumerator BeatCheck()
		{
			while (!_beatSynchronizer.IsOver)
			{
				if (!_beatSynchronizer.IsPaused)
				{
					_currentSample = (float)AudioSettings.dspTime * _audioFrequency;

					if (_currentSample >= (_nextSample + _sampleOffset))
					{
						foreach (BeatObserver beatObserver in _beatObservers)
						{
							beatObserver.BeatNotify(_beatType);
						}
						_nextSample += _samplePeriod;
					}

					yield return new WaitForSeconds(_loopTime / 1000f);
				}
				yield return null;
			}
		}
	}
}