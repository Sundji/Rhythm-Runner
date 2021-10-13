using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Divit.RhythmRunner
{
    public class MenuView : MonoBehaviour
    {
        [SerializeField] private GameObject _menuViewUserInterface = null;
        [SerializeField] private MenuController _menuController = null;

        [Header("Level Buttons")]
        [SerializeField] private LevelButton _levelButtonPrefab = null;
        [SerializeField] private Transform _levelButtonPanel = null;

        [Header("Level Details Left UI Elements")]
        [SerializeField] private GameObject _titleLeftPanel = null;
        [SerializeField] private GameObject _levelDetailsLeftPanel = null;
        [SerializeField] private TextMeshProUGUI _highscoreText = null;

        [Header("Level Details Right UI Elements")]
        [SerializeField] private GameObject _titleRightPanel = null;
        [SerializeField] private GameObject _levelDetailsRightPanel = null;
        [SerializeField] private TextMeshProUGUI _durationText = null;

        [Header("Options UI Elements")]
        [SerializeField] private Button _playButton = null;
        [SerializeField] private Button _quitButton = null;

        [Header("Tutorial Button")]
        [SerializeField] private Button _tutorialButton = null;

        private LevelButton _selectedLevelButton = null;

        private bool _levelButtonsCreated;

        private void Awake()
        {
            _menuController.OnInitialize += OnInitialize;
            _menuController.OnExitMenu += OnExitMenu;

            _playButton.onClick.AddListener(StartGame);
            _quitButton.onClick.AddListener(Quit);
            _tutorialButton.onClick.AddListener(StartTutorial);
        }

        private void OnDestroy()
        {
            _menuController.OnInitialize -= OnInitialize;
            _menuController.OnExitMenu -= OnExitMenu;

            _playButton.onClick.RemoveListener(StartGame);
            _quitButton.onClick.RemoveListener(Quit);
            _tutorialButton.onClick.RemoveListener(StartTutorial);
        }

        private void OnInitialize()
        {
            DisplayMenuView();
        }

        private void OnExitMenu()
        {
            HideMenuView();
        }

        private void OnLevelButtonClick(LevelButton levelButton)
        {
            if (_selectedLevelButton == null || _selectedLevelButton.Level != levelButton.Level)
            {
                _selectedLevelButton?.Deselect();
                _selectedLevelButton = levelButton;
                _selectedLevelButton.Select();
                _playButton.interactable = true;
                DisplayLevelInformation(levelButton.Level);
            }
        }

        private void DisplayLevelInformation(int level)
        {
            _titleLeftPanel.SetActive(false);
            _titleRightPanel.SetActive(false);
            _levelDetailsLeftPanel.SetActive(true);
            _levelDetailsRightPanel.SetActive(true);

            if (HighscoreDataManager.Manager.TryGetHighscoreEntry(level, out HighscoreEntry entry))
            {
                _highscoreText.text = entry.Highscore.ToString();
            }
            else
            {
                _highscoreText.text = "0";
            }

            if (Constants.LEVEL_AUDIO_DATA.TryGetValue(level, out LevelAudioData levelAudioData))
            {
                _durationText.text = levelAudioData.AudioClipLength / 60 + ":" + (levelAudioData.AudioClipLength % 60).ToString("00");
            }
            else
            {
                _durationText.text = "UNKNOWN";
            }
        }

        private void DisplayMenuView()
        {
            _menuViewUserInterface.SetActive(true);

            _titleLeftPanel.SetActive(true);
            _titleRightPanel.SetActive(true);
            _levelDetailsLeftPanel.SetActive(false);
            _levelDetailsRightPanel.SetActive(false);

            _selectedLevelButton?.Deselect();
            _playButton.interactable = false;

            if (!_levelButtonsCreated)
            {
                foreach (int level in _menuController.GetLevels())
                {
                    LevelButton levelButton = (LevelButton)Instantiate(_levelButtonPrefab, _levelButtonPanel);
                    levelButton.SetUpButton(level, OnLevelButtonClick);
                }
                _levelButtonsCreated = true;
            }
        }

        private void HideMenuView()
        {
            _menuViewUserInterface.SetActive(false);
        }

        private void StartGame()
        {
            if (_selectedLevelButton)
            {
                _menuController.StartGame(_selectedLevelButton.Level);
            }
        }

        private void StartTutorial()
        {
            _menuController.StartTutorial();
        }

        private void Quit()
        {
            _menuController.Quit();
        }
    }
}