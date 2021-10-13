using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Divit.RhythmRunner
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private MenuController _menuController = null;
        [SerializeField] private Player _player = null;

        [Header("XR Ray Interactors")]
        [SerializeField] private XRRayInteractor _xrRayInteractorLeft = null;
        [SerializeField] private XRRayInteractor _xrRayInteractorRight = null;

        [Header("Spawners")]
        [SerializeField] private RandomRhythmSpawner _randomRhythmSpawnerPrefab = null;

        [Header("Lights")]
        [SerializeField] private Light _gameOverLight = null;
        [SerializeField] private float _gameOverLightIntensity = 5;

        [Header("Durations")]
        [SerializeField] private float _audioFadeDuration = 1.5f;
        [SerializeField] private float _lightFadeDuration = 1.5f;

        public Action OnInitialized;
        public Action OnExitGame;

        public Action OnLevelFinished;

        public Action OnPlayerDeath;
        public Action OnPlayerHealthChanged;
        public Action OnPlayerNewHighscore;
        public Action OnPlayerScoreChanged;

        private int _level;

        private RandomRhythmSpawner _spawner;

        public int GetLevelHighscore()
        {
            if (HighscoreDataManager.Manager.TryGetHighscoreEntry(_level, out HighscoreEntry entry))
            {
                return entry.Highscore;
            }
            return 0;
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

        public void InitializeGame(int level)
        {
            _level = level;
            _player.ResetPlayer();

            OnInitialized?.Invoke();
            CreateLevel();
        }

        public void ExitGame()
        {
            if (_spawner)
            {
                _spawner.DOKill();
                Destroy(_spawner.gameObject);
            }

            ChangeGameOverLightIntensity(0);
            EnableXrRayIndicators(true);
            OnExitGame?.Invoke();
        }

        public void Continue()
        {
            _spawner.Continue();
            EnableXrRayIndicators(false);
            Time.timeScale = 1;
        }

        public void Pause()
        {
            _spawner.Pause();
            EnableXrRayIndicators(true);
            Time.timeScale = 0;
        }

        public void RestartLevel()
        {
            Continue();
            ExitGame();
            InitializeGame(_level);
        }

        public void ReturnToMenu()
        {
            AudioListener.pause = false;
            Continue();
            ExitGame();
            _menuController.InitializeMenu();
        }

        private void Awake()
        {
            Player.OnPlayerDeath += OnDeath;
            PlayerHealth.OnHealthChanged += OnHealthChanged;
            PlayerScore.OnScoreChanged += OnScoreChanged;
            RandomRhythmSpawner.OnAudioEnd += OnAudioEnd;
        }

        private void Start()
        {
            ExitGame();
        }

        private void OnDestroy()
        {
            Player.OnPlayerDeath -= OnDeath;
            PlayerHealth.OnHealthChanged -= OnHealthChanged;
            PlayerScore.OnScoreChanged -= OnScoreChanged;
            RandomRhythmSpawner.OnAudioEnd -= OnAudioEnd;
        }

        private void OnDeath(int score)
        {
            _spawner.AudioSource.DOFade(0, _audioFadeDuration);
            _spawner.Deactivate();
            ChangeGameOverLightIntensity(_gameOverLightIntensity);
            EnableXrRayIndicators(true);

            OnPlayerDeath?.Invoke();
            if (HighscoreDataManager.Manager.CheckIfHighscore(score, _level))
            {
                OnPlayerNewHighscore.Invoke();
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

        private void OnAudioEnd()
        {
            _player.LevelCleared();
            EnableXrRayIndicators(true);
            OnLevelFinished?.Invoke();
        }

        private void CreateLevel()
        {
            if (Constants.LEVEL_AUDIO_DATA.TryGetValue(_level, out LevelAudioData levelAudioData))
            {
                _spawner = (RandomRhythmSpawner)Instantiate(_randomRhythmSpawnerPrefab);
                EnableXrRayIndicators(false);

                AudioClip audioClip = Resources.Load<AudioClip>(levelAudioData.AudioClipName);
                float bpm = levelAudioData.AudioClipBpm;
                float beatDelayTime = levelAudioData.BeatDelayTime;
                float delayTime = (_spawner.transform.position - _player.transform.position).magnitude / Constants.MOVEABLE_OBJECT_SPEED;

                _spawner.InitializeSpawnerAudio(audioClip, bpm, delayTime, beatDelayTime);
            }
        }

        private void ChangeGameOverLightIntensity(float intensity)
        {
            _gameOverLight.DOIntensity(intensity, _lightFadeDuration)
                .SetEase(Ease.Linear);
        }

        private void EnableXrRayIndicators(bool isEnabled)
        {
            _xrRayInteractorLeft.enabled = isEnabled;
            _xrRayInteractorRight.enabled = isEnabled;
        }
    }
}