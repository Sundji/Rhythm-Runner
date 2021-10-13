using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace Divit.RhythmRunner
{
    public class GameView : MonoBehaviour
    {
        [SerializeField] private GameObject _gameViewUserInterface = null;
        [SerializeField] private GameController _gameController = null;
        [SerializeField] private XRController _leftController = null;

        [Header("UI Canvases")]
        [SerializeField] private GameObject _endCanvas = null;
        [SerializeField] private GameObject _optionsCanvas = null;
        [SerializeField] private GameObject _scoreCanvas = null;

        [Header("End Canvas UI Elements")]
        [SerializeField] private Button _endRestartButton = null;
        [SerializeField] private Button _endMenuButton = null;
        [SerializeField] private TextMeshProUGUI _endMessageText = null;
        [SerializeField] private TextMeshProUGUI _endScoreText = null;
        [SerializeField] private TextMeshProUGUI _endHighscoreText = null;
        [SerializeField] private GameObject _winParticleEffect = null;

        [Header("Health UI Elements")]
        [SerializeField] private Slider _healthSlider = null;

        [Header("Options UI Elements")]
        [SerializeField] private Button _optionsContinueButton = null;
        [SerializeField] private Button _optionsMenuButton = null;

        [Header("Score UI Elements")]
        [SerializeField] private TextMeshProUGUI _highscoreText = null;
        [SerializeField] private TextMeshProUGUI _scoreText = null;

        [Header("Durations")]
        [SerializeField] private float _sliderAnimationDurationFullValue = 2.5f;

        private bool _isActive;
        private bool _isGameOver;

        private void Awake()
        {
            _gameController.OnInitialized += OnInitialize;
            _gameController.OnExitGame += OnExitGame;
            _gameController.OnLevelFinished += OnLevelFinished;

            _gameController.OnPlayerDeath += OnPlayerDeath;
            _gameController.OnPlayerHealthChanged += OnPlayerHealthChanged;
            _gameController.OnPlayerNewHighscore += OnPlayerNewHighscore;
            _gameController.OnPlayerScoreChanged += OnPlayerScoreChanged;

            _endMenuButton.onClick.AddListener(ReturnToMenu);
            _endRestartButton.onClick.AddListener(RestartLevel);

            _optionsContinueButton.onClick.AddListener(ContinueGame);
            _optionsMenuButton.onClick.AddListener(ReturnToMenu);
        }

        private void OnDestroy()
        {
            _gameController.OnInitialized -= OnInitialize;
            _gameController.OnExitGame -= OnExitGame;
            _gameController.OnLevelFinished -= OnLevelFinished;

            _gameController.OnPlayerDeath -= OnPlayerDeath;
            _gameController.OnPlayerHealthChanged -= OnPlayerHealthChanged;
            _gameController.OnPlayerNewHighscore -= OnPlayerNewHighscore;
            _gameController.OnPlayerScoreChanged -= OnPlayerScoreChanged;

            _endMenuButton.onClick.RemoveListener(ReturnToMenu);
            _endRestartButton.onClick.RemoveListener(RestartLevel);

            _optionsContinueButton.onClick.RemoveListener(ContinueGame);
            _optionsMenuButton.onClick.RemoveListener(ReturnToMenu);
        }

        private void Update()
        {
            if (_isActive && !_isGameOver)
            {
                if (_leftController.inputDevice.TryGetFeatureValue(CommonUsages.menuButton, out bool isPressed) && isPressed)
                {
                    PauseGame();
                }
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    PauseGame();
                }
            }
        }

        private void OnInitialize()
        {
            DisplayGameView();
        }

        private void OnExitGame()
        {
            HideGameView();
        }

        private void OnPlayerHealthChanged()
        {
            float healthValue = (float)_gameController.GetPlayerHealth() / (float)_gameController.GetPlayerMaximumHealth();
            float animationDurationFactor = _healthSlider.value - healthValue;
            _healthSlider.DOKill();
            _healthSlider.DOValue(healthValue, _sliderAnimationDurationFullValue * animationDurationFactor);
        }

        private void OnPlayerNewHighscore()
        {
            _highscoreText.text = _gameController.GetLevelHighscore().ToString();
        }

        private void OnPlayerScoreChanged()
        {
            _scoreText.text = _gameController.GetPlayerScore().ToString();
        }

        private void DisplayGameView()
        {
            _gameViewUserInterface.SetActive(true);
            _isActive = true;
            _isGameOver = false;

            _endCanvas.SetActive(false);
            _optionsCanvas.SetActive(false);
            _scoreCanvas.SetActive(true);

            OnPlayerHealthChanged();
            OnPlayerNewHighscore();
            OnPlayerScoreChanged();
        }

        private void HideGameView()
        {
            _gameViewUserInterface.SetActive(false);
            _isActive = false;
        }

        private void DisplayEndInformation(string message)
        {
            _endCanvas.SetActive(true);
            _endMessageText.text = message;
            _endScoreText.text = _gameController.GetPlayerScore().ToString();
            _endHighscoreText.text = _gameController.GetLevelHighscore().ToString();
            _scoreCanvas.SetActive(false);
        }

        private void ContinueGame()
        {
            _gameController.Continue();
            _optionsCanvas.SetActive(false);
            _scoreCanvas.SetActive(true);
        }

        private void PauseGame()
        {
            _gameController.Pause();
            _optionsCanvas.SetActive(true);
            _scoreCanvas.SetActive(false);
        }

        private void RestartLevel()
        {
            _gameController.RestartLevel();
        }

        private void ReturnToMenu()
        {
            _gameController.ReturnToMenu();
        }

        private void OnLevelFinished()
        {
            _isGameOver = true;
            DisplayEndInformation("LEVEL CLEARED");

            if (_winParticleEffect)
            {
                GameObject particleEffect = Instantiate(_winParticleEffect);
                particleEffect.transform.position = _endCanvas.transform.position;
            }
        }

        private void OnPlayerDeath()
        {
            _isGameOver = true;
            DisplayEndInformation("GAME OVER");
        }
    }
}