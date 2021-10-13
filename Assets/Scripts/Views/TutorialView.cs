using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using TMPro;

namespace Divit.RhythmRunner
{
    public class TutorialView : MonoBehaviour
    {
        [SerializeField] private GameObject _tutorialUserInterface = null;
        [SerializeField] private TutorialController _tutorialController = null;

        [SerializeField] private XRController _leftController = null;

        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI _text = null;
        [SerializeField] private GameObject _optionsCanvas = null;
        [SerializeField] private Slider _healthSlider = null;
        [SerializeField] private TextMeshProUGUI _scoreValue = null;

        [Header("Buttons")]
        [SerializeField] private Button _continueButton = null;
        [SerializeField] private Button _exitButton = null;
        [SerializeField] private Button _nextButton = null;
        [SerializeField] private Button _repeatButton = null;

        [Header("Durations")]
        [SerializeField] private float _sliderAnimationDurationFullValue = 2.5f;

        private bool _isActive = false;

        private void Awake()
        {
            _continueButton.onClick.AddListener(Continue);
            _exitButton.onClick.AddListener(Exit);
            _nextButton.onClick.AddListener(Next);
            _repeatButton.onClick.AddListener(Repeat);

            _tutorialController.OnExitTutorial += OnExitTutorial;
            _tutorialController.OnInitialize += OnInitialize;
            _tutorialController.OnPlayerHealthChanged += OnPlayerHealthChanged;
            _tutorialController.OnPlayerScoreChanged += OnPlayerScoreChanged;
        }

        private void OnDestroy()
        {
            _continueButton.onClick.RemoveListener(Continue);
            _exitButton.onClick.RemoveListener(Exit);
            _nextButton.onClick.RemoveListener(Next);
            _repeatButton.onClick.RemoveListener(Repeat);

            _tutorialController.OnExitTutorial -= OnExitTutorial;
            _tutorialController.OnInitialize -= OnInitialize;
            _tutorialController.OnPlayerHealthChanged -= OnPlayerHealthChanged;
            _tutorialController.OnPlayerScoreChanged -= OnPlayerScoreChanged;
        }

        private void Update()
        {
            if (_isActive)
            {
                if (_leftController.inputDevice.TryGetFeatureValue(CommonUsages.menuButton, out bool isPressed) && isPressed)
                {
                    Pause();
                }
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Pause();
                }
            }
        }

        private void OnExitTutorial()
        {
            _isActive = false;
            _tutorialUserInterface.SetActive(false);
        }

        private void OnInitialize()
        {
            _isActive = true;
            _tutorialUserInterface.SetActive(true);
            _optionsCanvas.SetActive(false);
            _nextButton.gameObject.SetActive(true);
            _exitButton.gameObject.SetActive(false);

            OnPlayerHealthChanged();
            OnPlayerScoreChanged();
            Repeat();
        }

        private void OnPlayerHealthChanged()
        {
            float healthValue = (float)_tutorialController.GetPlayerHealth() / (float)_tutorialController.GetPlayerMaximumHealth();
            float animationDurationFactor = _healthSlider.value - healthValue;
            _healthSlider.DOKill();
            _healthSlider.DOValue(healthValue, _sliderAnimationDurationFullValue * animationDurationFactor)
                .SetEase(Ease.Linear);
        }

        private void OnPlayerScoreChanged()
        {
            _scoreValue.text = _tutorialController.GetPlayerScore().ToString();
        }

        private void Continue()
        {
            _optionsCanvas.SetActive(false);
        }

        private void Pause()
        {
            _optionsCanvas.SetActive(true);
        }

        private void Exit()
        {
            _tutorialController.ReturnToMenu();
        }

        private void Next()
        {        
            _tutorialController.NextTutorialPart();

            if (_tutorialController.IsOver)
            {
                _text.text = "YOU ARE READY TO PLAY";
                _nextButton.gameObject.SetActive(false);
                _exitButton.gameObject.SetActive(true);
            }
            else
            {
                _text.text = _tutorialController.StartTutorialPart();
            }
        }

        private void Repeat()
        {
            if (_tutorialController.IsOver)
            {
                _tutorialController.InitializeTutorial();
            }
            else
            {
                _text.text = _tutorialController.StartTutorialPart();
            }
        }
    }
}