using System;
using UnityEngine;
using System.Collections;

/// <summary>
/// This class should be attached to the audio source for which synchronization should occur, and is 
/// responsible for synching up the beginning of the audio clip with all active beat counters and pattern counters.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class BeatSynchronizer : MonoBehaviour {

	public float bpm = 120f;		// Tempo in beats per minute of the audio clip.
	public float startDelay = 1f;	// Number of seconds to delay the start of audio playback.
	public delegate void AudioStartAction(double syncTime);
	public static event AudioStartAction OnAudioStart;

	public static Action OnInitialize;

	public void Initialize(AudioClip audioClip, bool isLooping, float beatsPerMinute, float audioSourceDelayTime)
	{
		AudioSource audioSource = GetComponent<AudioSource>();

		audioSource.clip = audioClip;
		audioSource.loop = isLooping;
		bpm = beatsPerMinute;

		OnInitialize?.Invoke();

		double initTime = AudioSettings.dspTime;
		audioSource.PlayScheduled(initTime + startDelay);
		OnAudioStart?.Invoke(initTime + startDelay);
	}

	/*
	void Start ()
	{
		double initTime = AudioSettings.dspTime;
		GetComponent<AudioSource>().PlayScheduled(initTime + startDelay);
		if (OnAudioStart != null) {
			OnAudioStart(initTime + startDelay);
		}
	}
	*/
}
