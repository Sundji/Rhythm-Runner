using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Divit.RhythmRunner
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private GameController _gameController = null;
        [SerializeField] private TutorialController _tutorialContoller = null;

        [Header("Background Audio")]
        [SerializeField] private AudioSource _backgroundAudioSource = null;
        [SerializeField] private float _backgroundAudioVolume = 0.15f;
        [SerializeField] private float _backgroundAudioFadeDuration = 1.5f;

        public Action OnInitialize;
        public Action OnExitMenu;

        public HashSet<int> GetLevels()
        {
            return Constants.LEVELS;
        }

        public void InitializeMenu()
        {
            TurnOnBackgroundAudio();
            OnInitialize?.Invoke();
        }

        public void StartGame(int level)
        {
            _gameController.InitializeGame(level);
            ExitMenu();
        }

        public void StartTutorial()
        {
            _tutorialContoller.InitializeTutorial();
            ExitMenu();
        }

        public void Quit()
        {
            Application.Quit();
        }

        private void Start()
        {
            if (PlayerPrefs.HasKey(Constants.USE_CHECK_KEY))
            {
                InitializeMenu();
            }
            else
            {
                ExitMenu();
            }
        }

        private void ExitMenu()
        {
            TurnOffBackgroundAudio();
            OnExitMenu?.Invoke();
        }
        private void TurnOffBackgroundAudio()
        {
            _backgroundAudioSource.DOFade(0, _backgroundAudioFadeDuration)
                .SetEase(Ease.Linear)
                .OnComplete(_backgroundAudioSource.Stop);
        }

        private void TurnOnBackgroundAudio()
        {
            _backgroundAudioSource.Play();
            _backgroundAudioSource.DOFade(_backgroundAudioVolume, _backgroundAudioFadeDuration)
                .SetEase(Ease.Linear);
        }
    }
}