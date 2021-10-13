using System;
using System.Collections.Generic;
using UnityEngine;

namespace Divit.RhythmRunner
{
    public class TutorialController : MonoBehaviour
    {
        [SerializeField] private MenuController _menuController = null;
        [SerializeField] private TutorialSpawner _tutorialSpawnerPrefab = null;

        [SerializeField] private Player _player = null;

        public Action OnInitialize;
        public Action OnExitTutorial;

        public Action OnPlayerHealthChanged;
        public Action OnPlayerScoreChanged;

        private int _index;
        private List<TutorialPartType> _tutorialParts = new List<TutorialPartType>
        {
            TutorialPartType.PAUSE,
            TutorialPartType.OBSTACLE,
            TutorialPartType.OBSTACLE_COLLISION,
            TutorialPartType.DESTRUCTIBLE_OBSTACLE,
            TutorialPartType.COIN
        };
        private TutorialSpawner _tutorialSpawner;

        public bool IsOver
        {
            get
            {
                return _index >= _tutorialParts.Count;
            }
        }

        public void NextTutorialPart()
        {
            _index++;
        }

        public string StartTutorialPart()
        {
            if (_tutorialParts[_index] != TutorialPartType.PAUSE)
            {
                _tutorialSpawner.SpawnNext(_tutorialParts[_index]);
            }
            return Constants.TUTORIAL_DATA[_tutorialParts[_index]];
        }

        public void ResetTutorial()
        {
            _index = 0;
        }

        public int GetPlayerHealth()
        {
            return _player.GetHealth();
        }

        public int GetPlayerMaximumHealth()
        {
            return _player.GetMaximumHealth();
        }

        public int GetPlayerScore()
        {
            return _player.GetScore();
        }

        public void InitializeTutorial()
        {
            _index = 0;
            _player.ResetPlayer();

            if (_tutorialSpawner != null)
            {
                Destroy(_tutorialSpawner);
            }

            _tutorialSpawner = Instantiate(_tutorialSpawnerPrefab);
            OnInitialize?.Invoke();
        }

        public void ReturnToMenu()
        {
            ExitTutorial();
            _menuController.InitializeMenu();
        }

        private void Awake()
        {
            PlayerHealth.OnHealthChanged += OnHealthChanged;
            PlayerScore.OnScoreChanged += OnScoreChanged;
        }

        private void OnDestroy()
        {
            PlayerHealth.OnHealthChanged -= OnHealthChanged;
            PlayerScore.OnScoreChanged -= OnScoreChanged;
        }

        private void Start()
        {
            if (PlayerPrefs.HasKey(Constants.USE_CHECK_KEY))
            {
                ExitTutorial();
            }
            else
            {
                InitializeTutorial();
                PlayerPrefs.SetInt(Constants.USE_CHECK_KEY, 1);
                PlayerPrefs.Save();
            }
        }

        private void OnHealthChanged()
        {
            OnPlayerHealthChanged?.Invoke();
        }

        private void OnScoreChanged()
        {
            OnPlayerScoreChanged?.Invoke();
        }

        private void ExitTutorial()
        {
            if (_tutorialSpawner != null)
            {
                Destroy(_tutorialSpawner);
            }
            OnExitTutorial?.Invoke();
        }
    }
}