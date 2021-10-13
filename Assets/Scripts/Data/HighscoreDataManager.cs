using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Divit.RhythmRunner
{
    public class HighscoreDataManager : MonoBehaviour
    {
        private static HighscoreDataManager _highscoreDataManager;

        private Dictionary<int, HighscoreEntry> _highscoreEntries = new Dictionary<int, HighscoreEntry>();

        public static HighscoreDataManager Manager
        {
            get
            {
                if (_highscoreDataManager == null)
                {
                    _highscoreDataManager = FindObjectOfType<HighscoreDataManager>();
                }
                return _highscoreDataManager;
            }
        }

        public bool CheckIfHighscore(int score, int levelIndex)
        {
            if (_highscoreEntries.ContainsKey(levelIndex) && _highscoreEntries[levelIndex].Highscore >= score)
            {
                return false;
            }
            SaveHighscoreEntry(levelIndex, new HighscoreEntry
            {
                LevelIndex = levelIndex,
                Highscore = score
            });
            return true;
        }

        public void SaveHighscoreEntry(int levelIndex, HighscoreEntry entry)
        {
            if (_highscoreEntries.ContainsKey(levelIndex))
            {
                _highscoreEntries[levelIndex] = entry;
            }
            else
            {
                _highscoreEntries.Add(levelIndex, entry);
            }
            SaveHighscoreEntries();
        }

        public bool TryGetHighscoreEntry(int levelIndex, out HighscoreEntry entry)
        {
            if (_highscoreEntries.ContainsKey(levelIndex))
            {
                entry = _highscoreEntries[levelIndex];
                return true;
            }
            entry = null;
            return false;
        }

        private void Awake()
        {
            TryLoadHighscoreEntries();
        }

        private void SaveHighscoreEntries()
        {
            if (_highscoreDataManager == null)
            {
                _highscoreDataManager = this;
            }
            else if (!_highscoreDataManager.Equals(this))
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);

            if (_highscoreEntries.Count > 0)
            {
                HighscoreData highscoreData = new HighscoreData
                {
                    HighscoreEntries = new List<HighscoreEntry>()
                };
                foreach (HighscoreEntry entry in _highscoreEntries.Values)
                {
                    highscoreData.HighscoreEntries.Add(entry);
                }
                DataReaderWriter.SaveHighscoreData(highscoreData);
            }
        }

        private void TryLoadHighscoreEntries()
        {
            if (DataReaderWriter.TryLoadHighscoreData(out HighscoreData highscoreData))
            {
                foreach (HighscoreEntry highscoreEntry in highscoreData.HighscoreEntries)
                {
                    _highscoreEntries.Add(highscoreEntry.LevelIndex, highscoreEntry);
                }
            }
        }
    }
}