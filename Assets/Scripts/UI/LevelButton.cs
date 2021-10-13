using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Divit.RhythmRunner
{
    [RequireComponent(typeof(Button))]
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _buttonText = null;

        [Header("Button Colors")]
        [SerializeField] private Color _normalColor = Color.white;
        [SerializeField] private Color _selectedColor = Color.cyan;

        private Action<LevelButton> _onClickCallback;

        private Button _button;

        public int Level
        {
            get;
            private set;
        }

        public void Deselect()
        {
            _button.ChangeColor(_normalColor);
        }

        public void Select()
        {
            _button.ChangeColor(_selectedColor);
        }

        public void SetUpButton(int level, Action<LevelButton> onClickCallback)
        {
            Level = level;
            _onClickCallback = onClickCallback;

            string text = "LEVEL " + Level.ToString();
            if (Constants.LEVEL_AUDIO_DATA.TryGetValue(level, out LevelAudioData levelAudioData))
            {
                text = levelAudioData.AudioClipName.ToUpper();
            }
            _buttonText.text = text;
        }

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            _onClickCallback?.Invoke(this);
        }
    }
}